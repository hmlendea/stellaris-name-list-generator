# Flow: Name Download & Caching

## Overview

The name download flow handles URL-based name acquisition for `NameGroup` elements. When a `NameGroup` has a `Url` property, its content is downloaded, cached, and merged with explicit values on first access.

## Flow Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         NAME DOWNLOAD & CACHING FLOW                        │
└─────────────────────────────────────────────────────────────────────────────┘

  NameGroup.Values Accessed (Lazy)
       │
       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│  NameGroup.UrlValues Property Getter                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  if (string.IsNullOrEmpty(Url))                                             │
│      return empty list                                                      │
│                                                                             │
│  content = FileDownloader.TryDownloadStringAsync(Url).Result                │
│                                                                             │
│  if (content == null)                                                       │
│      return empty list                                                      │
│                                                                             │
│  return content.Split('\n')                                                 │
│      .Select(s => s.Trim())                                                 │
│      .Where(s => !string.IsNullOrEmpty(s))                                  │
│      .ToList()                                                              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
       │
       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│  FileDownloader.TryDownloadStringAsync(url)                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  // 1. CHECK CACHE                                                          │
│  cached = CacheManager.GetNameList(url)                                     │
│  if (cached != null)                                                        │
│      return cached                                                          │
│                                                                             │
│  // 2. DOWNLOAD FROM NETWORK                                                │
│  try                                                                        │
│  {                                                                          │
│      response = await HttpClient.GetAsync(url)                              │
│      response.EnsureSuccessStatusCode()                                     │
│      content = await response.Content.ReadAsStringAsync()                   │
│                                                                             │
│      // 3. POPULATE CACHE                                                   │
│      CacheManager.StoreNameList(url, content)                               │
│                                                                             │
│      return content                                                         │
│  }                                                                          │
│  catch (HttpRequestException)                                               │
│  {                                                                          │
│      return null                                                            │
│  }                                                                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
       │
       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│  CacheManager.GetNameList(url)                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  fileName = SHA256(url) + ".txt"                                            │
│  filePath = Path.Combine(cacheDir, fileName)                                │
│                                                                             │
│  if (!File.Exists(filePath))                                                │
│      return null                                                            │
│                                                                             │
│  // CHECK TTL (8 hours)                                                     │
│  creationTime = File.GetCreationTimeUtc(filePath)                           │
│  if (DateTime.UtcNow - creationTime > TimeSpan.FromHours(8))                │
│  {                                                                          │
│      File.Delete(filePath)                                                  │
│      return null                                                            │
│  }                                                                          │
│                                                                             │
│  return File.ReadAllText(filePath)                                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
       │
       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│  CacheManager.StoreNameList(url, content)                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  fileName = SHA256(url) + ".txt"                                            │
│  filePath = Path.Combine(cacheDir, fileName)                                │
│                                                                             │
│  Directory.CreateDirectory(cacheDir)                                        │
│  File.WriteAllText(filePath, content)                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Components

### `NameGroup` - Lazy URL Resolution

```csharp
public sealed class NameGroup
{
    public string Name { get; set; }
    public List<string> ExplicitValues { get; set; }
    public string Url { get; set; }

    public IEnumerable<string> Values => ExplicitValues.Concat(UrlValues);

    public List<string> UrlValues
    {
        get
        {
            if (string.IsNullOrEmpty(Url))
            {
                return [];
            }

            // Static FileDownloader with CacheManager
            var content = FileDownloader.TryDownloadStringAsync(Url).Result;
            if (content == null)
            {
                return [];
            }

            return content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
        }
    }

    public bool IsEmpty => !Values.Any();
}
```

**Key Points**:
- `UrlValues` is a **computed property** - downloads on every access (no instance caching)
- Uses **static** `FileDownloader` instance (shared across all `NameGroup` instances)
- Blocks on `.Result` (synchronous) - called during builder execution
- Splits by newline, trims whitespace, filters empty lines

### `FileDownloader` - Cache-First Download

```csharp
public sealed class FileDownloader : IFileDownloader
{
    private static readonly HttpClient _httpClient = new();
    private readonly ICacheManager _cacheManager;

    public FileDownloader(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    public async Task<string?> TryDownloadStringAsync(string url)
    {
        // 1. Check cache
        var cached = _cacheManager.GetNameList(url);
        if (cached != null)
        {
            return cached;
        }

        // 2. Download
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // 3. Store in cache
            _cacheManager.StoreNameList(url, content);

            return content;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
```

**Key Points**:
- **Static `HttpClient`** - Connection pooling, reused across calls
- **Cache-first** - Checks `ICacheManager` before network request
- **Graceful failure** - Returns `null` on any network error
- **Cache population** - Stores successful downloads automatically

### `CacheManager` - File-Based TTL Cache

```csharp
public sealed class CacheManager : ICacheManager
{
    private readonly string _cacheDirectory;
    private readonly TimeSpan _ttl = TimeSpan.FromHours(8);

    public CacheManager()
    {
        _cacheDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StellarisNameListGenerator",
            "cache"
        );
        Directory.CreateDirectory(_cacheDirectory);
    }

    public void StoreNameList(string url, string content)
    {
        var fileName = GetCacheFileName(url);
        var filePath = Path.Combine(_cacheDirectory, fileName);
        File.WriteAllText(filePath, content);
    }

    public string? GetNameList(string url)
    {
        var fileName = GetCacheFileName(url);
        var filePath = Path.Combine(_cacheDirectory, fileName);

        if (!File.Exists(filePath))
        {
            return null;
        }

        // Check TTL
        var creationTime = File.GetCreationTimeUtc(filePath);
        if (DateTime.UtcNow - creationTime > _ttl)
        {
            File.Delete(filePath);
            return null;
        }

        return File.ReadAllText(filePath);
    }

    private string GetCacheFileName(string url)
    {
        var bytes = Encoding.UTF8.GetBytes(url);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash) + ".txt";
    }
}
```

**Key Points**:
- **Location**: `%LOCALAPPDATA%/StellarisNameListGenerator/cache/` (Linux: `~/.local/share/StellarisNameListGenerator/cache/`)
- **Key**: SHA256 hash of URL → hex string + `.txt`
- **TTL**: 8 hours from file creation time
- **Cleanup**: Expired files deleted on access (lazy expiration)
- **No size limit** - Relies on OS disk pressure

## Integration with Generation Flow

### When Downloads Occur

Downloads happen **during builder execution** when `NameGroup.Values` is accessed:

```csharp
// In NamesBuilder.BuildNameArray()
var names = nameGroups.SelectMany(g => g.Values)  // <-- Triggers download
    .Distinct(DoNamesMatch)
    .OrderBy(n => n);
```

### Execution Order

1. `FileContentBuilder.BuildContent()` called
2. Each builder calls `BuildNameArray()` for its categories
3. `BuildNameArray()` calls `nameGroups.SelectMany(g => g.Values)`
4. `NameGroup.Values` getter accesses `UrlValues`
5. `UrlValues` calls `FileDownloader.TryDownloadStringAsync(url).Result`
6. Download/cache check happens **synchronously** (blocks builder)

### Concurrency

- **Sequential per builder** - Each builder processes its categories in order
- **Parallel across NameGroups** - Not parallelized; each `UrlValues` access blocks
- **Static HttpClient** - Thread-safe for concurrent downloads (if parallelized later)

## Error Handling

| Scenario | Behavior |
|----------|----------|
| URL empty/null | Returns empty list, no download attempted |
| Cache hit (valid) | Returns cached content immediately |
| Cache hit (expired) | Deletes cache file, proceeds to download |
| Network success | Stores in cache, returns content |
| Network failure (DNS, timeout, 4xx, 5xx) | Returns `null`, builder treats as empty |
| Cache write failure | Exception propagates (rare, disk full/permissions) |
| Cache read failure | Exception propagates (corrupt file) |

## Cache Invalidation

- **Automatic**: TTL-based (8 hours), checked on every `GetNameList()`
- **Manual**: Delete files in cache directory
- **No programmatic invalidation** - No API to force refresh

## Security Considerations

- **No authentication** - Downloads from public URLs only
- **No SSL validation override** - Uses system trust store
- **URLs from XML** - Trusted input (user-provided name list files)
- **Content not executed** - Treated as plain text, split by newline

## Testing

**Unit Tests** (`NameGroupTests`):
- `UrlValues_EmptyUrl_ReturnsEmptyList`
- `UrlValues_ValidUrl_ReturnsDownloadedNames`
- `UrlValues_NetworkError_ReturnsEmptyList`
- `Values_CombinesExplicitAndUrlValues`
- `IsEmpty_NoValues_ReturnsTrue`
- `IsEmpty_WithValues_ReturnsFalse`

**Integration**: Tested via `FileContentBuilderTests` with URL-based name groups.

## Performance

- **First access**: Network latency + download time (or cache read)
- **Subsequent accesses (within 8h)**: Cache read only (~1-5ms)
- **Memory**: Content held in `StringBuilder` during build, then released
- **Disk**: One file per unique URL, ~few KB each

## Configuration

No runtime configuration. Constants in code:
- Cache directory: `LocalApplicationData/StellarisNameListGenerator/cache`
- TTL: 8 hours (`TimeSpan.FromHours(8)`)
- Hash algorithm: SHA256

To modify: Change `CacheManager` constructor and `_ttl` field.