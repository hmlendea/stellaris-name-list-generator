# Flow: Name List Generation

## Overview

The name list generation flow is the primary end-to-end pipeline that transforms input XML name lists into a Stellaris-compatible output file.

## Flow Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         NAME LIST GENERATION FLOW                           │
└─────────────────────────────────────────────────────────────────────────────┘

  CLI Arguments
       │
       ▼
┌──────────────────┐
│  Program.cs      │
│  (Entry Point)   │
└────────┬─────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│  DEPENDENCY COMPOSITION                                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│  CacheManager ◄──────────────────────────────────────────────────────────┐  │
│       │                                                                   │  │
│       ▼                                                                   │  │
│  FileDownloader ◄──────────────────────────────────────────────────────┐  │  │
│       │                                                                 │  │  │
│       ▼                                                                 │  │  │
│  NamesBuilders (6) ◄────────────────────────────────────────────────┐  │  │  │
│       │                                                             │  │  │  │
│       ▼                                                             │  │  │  │
│  FileContentBuilder ◄────────────────────────────────────────────┐  │  │  │  │
│       │                                                          │  │  │  │  │
│       ▼                                                          │  │  │  │  │
│  XmlRepository<NameList> (NuciDAL) ◄──────────────────────────┐  │  │  │  │  │
│       │                                                        │  │  │  │  │  │
│       ▼                                                        │  │  │  │  │  │
│  NameListGenerator ◄────────────────────────────────────────┐  │  │  │  │  │  │
└───────┬──────────────────────────────────────────────────────┘  │  │  │  │  │  │
        │                                                         │  │  │  │  │  │
        ▼                                                         │  │  │  │  │  │
┌─────────────────────────────────────────────────────────────────┘  │  │  │  │  │
│  GENERATION PIPELINE                                              │  │  │  │  │
├─────────────────────────────────────────────────────────────────────┘  │  │  │  │
│                                                                        │  │  │  │
│  1. LOAD & MERGE INPUT FILES                                          │  │  │  │
│     ┌─────────────────────────────────────────────────────────────┐   │  │  │  │
│     │ NameListGenerator.GetMergedNameList(inputPaths)             │   │  │  │  │
│     │                                                              │   │  │  │  │
│     │   For each input path:                                       │   │  │  │  │
│     │     nameList = XmlRepository.Load(path)                     │   │  │  │  │
│     │     merged.AddRange(nameList)                               │   │  │  │  │
│     │                                                              │   │  │  │  │
│     │   Returns: merged NameList with all collections concatenated│   │  │  │  │
│     └─────────────────────────────────────────────────────────────┘   │  │  │  │
│                          │                                             │  │  │  │
│                          ▼                                             │  │  │  │
│  2. INJECT METADATA                                                   │  │  │  │
│     ┌─────────────────────────────────────────────────────────────┐   │  │  │  │
│     │ merged.Id = Path.GetFileNameWithoutExtension(outputPath)    │   │  │  │  │
│     │ merged.Name = name (from CLI)                               │   │  │  │  │
│     │ merged.IsLocked = !randomized (from CLI)                    │   │  │  │  │
│     └─────────────────────────────────────────────────────────────┘   │  │  │  │
│                          │                                             │  │  │  │
│                          ▼                                             │  │  │  │
│  3. BUILD CONTENT                                                     │  │  │  │
│     ┌─────────────────────────────────────────────────────────────┐   │  │  │  │
│     │ FileContentBuilder.BuildContent(merged, leaders, ships,     │   │  │  │  │
│     │                                fleets, colonies)            │   │  │  │  │
│     │                                                              │   │  │  │  │
│     │   a) Header: id, name, randomized                           │   │  │  │  │
│     │   b) CLI categories: leader_names, ship_names,              │   │  │  │  │
│     │      fleet_names, colony_names                              │   │  │  │  │
│     │   c) Generated categories (fixed order):                    │   │  │  │  │
│     │        ShipNamesBuilder.Build(merged.Ships)                 │   │  │  │  │
│     │        ShipClassNamesBuilder.Build(merged.ShipClasses)      │   │  │  │  │
│     │        FleetNamesBuilder.Build(merged)                      │   │  │  │  │
│     │        ArmyNamesBuilder.Build(merged.Armies)                │   │  │  │  │
│     │        PlanetNamesBuilder.Build(merged.Planets)             │  │  │  │  │
│     │        CharacterNamesBuilder.Build(merged.Characters)       │  │  │  │  │
│     │                                                              │   │  │  │  │
│     │   Returns: Complete Paradox script content                  │   │  │  │  │
│     └─────────────────────────────────────────────────────────────┘   │  │  │  │
│                          │                                             │  │  │  │
│                          ▼                                             │  │  │  │
│  4. WRITE OUTPUT                                                      │  │  │  │
│     ┌─────────────────────────────────────────────────────────────┐   │  │  │  │
│     │ await File.WriteAllTextAsync(outputPath, content)           │   │  │  │  │
│     └─────────────────────────────────────────────────────────────┘   │  │  │  │
│                          │                                             │  │  │  │
│                          ▼                                             │  │  │  │
│                    Output File (.txt)                                 │  │  │  │
└────────────────────────────────────────────────────────────────────────┘  │  │  │
                                                                           │  │  │
                                                                           ▼  ▼  ▼
                                                                (Dependencies)
```

## Detailed Steps

### Step 1: Load & Merge Input Files

**Method**: `NameListGenerator.GetMergedNameList(string[] inputPaths)`

```csharp
private NameList GetMergedNameList(string[] inputPaths)
{
    var merged = new NameList();

    foreach (var path in inputPaths)
    {
        var nameList = _repository.Load(path);  // XmlRepository<NameList>.Load()
        merged.AddRange(nameList);               // Concatenates all collections
    }

    return merged;
}
```

**XmlRepository.Load()** (NuciDAL):
- Deserializes XML file to `NameList` object
- Uses parameterless constructors and public properties
- Throws `FileNotFoundException` if file doesn't exist
- Throws serialization exception on malformed XML

**NameList.AddRange()**:
- Concatenates all `List<NameGroup>` properties
- No deduplication (handled later in builders)
- `Characters`: `this.Characters.AddRange(other.Characters)`

### Step 2: Inject Metadata

**Source**: CLI arguments parsed by `Program.cs`

```csharp
merged.Id = Path.GetFileNameWithoutExtension(outputPath);
merged.Name = name;           // --name argument
merged.IsLocked = !randomized; // --randomized flag (default false)
```

**Output Effect**:
- `id = "filename"` in output
- `name = "Display Name"` in output
- `randomized = "no"` if locked, `"yes"` if randomized

### Step 3: Build Content

**Method**: `FileContentBuilder.BuildContent(NameList, string[], string[], string[], string[])`

#### 3a: Header Generation

```csharp
sb.AppendLine($"# Generated by Stellaris Name List Generator");
sb.AppendLine($"id = \"{nameList.Id}\"");
sb.AppendLine($"name = \"{nameList.Name}\"");
sb.AppendLine($"randomized = {(nameList.IsLocked ? "no" : "yes")}");
sb.AppendLine();
```

#### 3b: CLI Category Injection

```csharp
// leader_names
if (leaders.Length > 0) { ... }

// ship_names
if (ships.Length > 0) { ... }

// fleet_names
if (fleets.Length > 0) { ... }

// colony_names
if (colonies.Length > 0) { ... }
```

#### 3c: Generated Categories (Fixed Order)

| Order | Builder | Input Model | Output Categories |
|-------|---------|-------------|-------------------|
| 1 | `ShipNamesBuilder` | `ShipNames` | 13 ship type arrays |
| 2 | `ShipClassNamesBuilder` | `ShipClassNames` | 13 ship class arrays + 4 station class arrays |
| 3 | `FleetNamesBuilder` | `NameList` (full) | 14 fleet format arrays |
| 4 | `ArmyNamesBuilder` | `ArmyNames` | 20+ army type arrays |
| 5 | `PlanetNamesBuilder` | `PlanetNames` | 15 planet class arrays |
| 6 | `CharacterNamesBuilder` | `List<CharacterNames>` | Grouped by ID, ordered by Weight |

### Step 4: Write Output

```csharp
await File.WriteAllTextAsync(outputPath, content);
```

- Overwrites existing file
- UTF-8 encoding (default)
- No BOM

## Data Flow Details

### Input XML Structure

Each input file contains a `NameList` with all categories populated via `NameGroup` elements:

```xml
<NameList>
  <Denonyms>
    <NameGroup Name="Base">
      <ExplicitValues>
        <string>Human</string>
        <string>Terran</string>
      </ExplicitValues>
    </NameGroup>
  </Denonyms>
  <Places>
    <Countries>
      <NameGroup Name="Earth">
        <ExplicitValues>
          <string>United States</string>
          <string>China</string>
        </ExplicitValues>
      </NameGroup>
    </Countries>
    <!-- ... -->
  </Places>
  <!-- ... all other categories ... -->
</NameList>
```

### NameGroup URL Resolution

During `NameGroup.Values` access (lazy, during builder execution):

```csharp
public IEnumerable<string> Values => ExplicitValues.Concat(UrlValues);

public List<string> UrlValues
{
    get
    {
        if (string.IsNullOrEmpty(Url)) return [];

        var content = FileDownloader.TryDownloadStringAsync(Url).Result;
        if (content == null) return [];

        return content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList();
    }
}
```

**Cache Flow**:
1. `FileDownloader.TryDownloadStringAsync(url)` called
2. `CacheManager.GetNameList(url)` checks `.cache/` directory
3. If cached and < 8 hours old → return cached content
4. Else → `HttpClient.GetAsync(url)` → store in cache → return content

### Builder Processing

Each builder receives domain model, extracts names via `NameGroup.Values`, processes through:

1. `ProcessName()` - Unicode normalization (500+ char mappings)
2. `CleanGenericNames()` - Diacritic-insensitive deduplication
3. `BuildNameArray()` - Paradox script formatting

## Error Handling

| Stage | Error | Handling |
|-------|-------|----------|
| Load | File not found | `FileNotFoundException` bubbles up |
| Load | XML malformed | NuciDAL throws descriptive exception |
| Merge | Null collections | All initialized to empty lists in constructors |
| Download | Network error | `FileDownloader` returns `null`, builder skips |
| Download | Cache corrupted | `CacheManager` deletes expired/corrupt files |
| Build | Empty categories | Builder returns empty string for that category |
| Write | Permission denied | `IOException` bubbles up |

## Performance Characteristics

- **Input Loading**: O(n) where n = total names across all input files
- **Merging**: O(n) concatenation
- **URL Downloads**: Parallel per `NameGroup` (lazy, on first `Values` access)
- **Caching**: 8-hour TTL reduces repeated downloads
- **Building**: O(n log n) due to sorting in `BuildNameArray()`
- **Memory**: Streams content via `StringBuilder`, no intermediate large objects

## Output Format

Stellaris-compatible Paradox script (`.txt`):

```text
# Generated by Stellaris Name List Generator
id = "my_namelist"
name = "My Custom Name List"
randomized = no

leader_names = {
    "Leader One"
    "Leader Two"
}

ship_names = {
    "Ship Alpha"
    "Ship Beta"
}

ship_names_generic = {
    "Generic Ship 1"
    "Generic Ship 2"
}

ship_names_corvette = {
    "Corvette Alpha"
}

... (all generated categories)
```

## Testing

**Unit Tests** (`NameListGeneratorTests`):
- `GenerateAsync_MultipleInputFiles_MergesCorrectly`
- `GenerateAsync_SingleInputFile_UsesFileNameAsId`
- `GenerateAsync_EmptyRepository_CreatesEmptyOutput`
- `GenerateAsync_OutputPath_SetsIdFromFileName`

**Integration**: Tested via `FileContentBuilderTests` and end-to-end CLI execution.