# Configuration

## Overview

Configuration is entirely driven by command-line arguments. There are no configuration files, environment variables, or settings files. All runtime behavior is controlled via CLI arguments parsed by `NuciCLI`.

## CLI Arguments

### Required Arguments

| Argument | Short | Type | Description |
|----------|-------|------|-------------|
| `--input` | `-i` | `string[]` | Input XML file paths (comma-separated) |
| `--output` | `-o` | `string` | Output file path |
| `--name` | `-n` | `string` | Display name for the name list |

### Optional Arguments

| Argument | Short | Type | Default | Description |
|----------|-------|------|---------|-------------|
| `--leaders` | `-l` | `string[]` | `[]` | Leader names (comma-separated) |
| `--ships` | `-s` | `string[]` | `[]` | Ship names (comma-separated) |
| `--fleets` | `-f` | `string[]` | `[]` | Fleet names (comma-separated) |
| `--colonies` | `-c` | `string[]` | `[]` | Colony names (comma-separated) |
| `--randomized` | `-r` | `bool` | `false` | Whether names are randomized |

## Argument Parsing

### NuciCLI Integration

```csharp
// Program.cs
var args = Cli.Parse<CliArguments>(args);
```

The `CliArguments` class (record or class) defines properties matching the CLI options. `NuciCLI` handles:
- Short/long form aliases (`-i` / `--input`)
- Required argument validation
- Type conversion (string, bool, arrays)
- Usage/help generation

### Input Path Processing

```csharp
// Comma-separated, trimmed, empty entries removed
var inputPaths = args.Input
    .Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(p => p.Trim())
    .ToArray();
```

**Examples**:
- `--input file1.xml,file2.xml` → `["file1.xml", "file2.xml"]`
- `--input "file1.xml, file2.xml ,file3.xml"` → `["file1.xml", "file2.xml", "file3.xml"]`
- `-i file1.xml -i file2.xml` → Not supported (single `--input` with comma-separated values)

### Output Path

Used as-is. The file name (without extension) becomes the `Id` in the output:

```csharp
merged.Id = Path.GetFileNameWithoutExtension(outputPath);
```

### Name

Used directly as the `name` field in output:

```csharp
merged.Name = name;
```

### Randomized Flag

Controls the `randomized` field in output:

```csharp
merged.IsLocked = !randomized;
// Output: randomized = "no" (if locked) or "yes" (if randomized)
```

Default: `false` → `randomized = "no"`

### Category Arrays (Leaders, Ships, Fleets, Colonies)

All processed identically:

```csharp
var leaders = args.Leaders?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToArray() ?? [];
```

Injected into output as Paradox script arrays:

```text
leader_names = {
    "Leader One"
    "Leader Two"
}
```

## Argument Validation

### NuciCLI Validation

- Missing required arguments → Throws with usage message
- Unknown arguments → Throws with usage message
- Type mismatch → Throws with error message

### Application-Level Validation

| Check | Location | Behavior |
|-------|----------|----------|
| Input files exist | `XmlRepository.Load()` | `FileNotFoundException` |
| Input files valid XML | `XmlRepository.Load()` | Serialization exception |
| Output directory writable | `File.WriteAllTextAsync()` | `IOException` |
| At least one input file | `Program.cs` (implicit) | Empty merge if none |

## Environment Variables

**None used**. The application does not read any environment variables for configuration.

## Configuration Files

**None used**. No JSON, XML, YAML, or INI configuration files are read.

## Defaults

| Setting | Default | Override |
|---------|---------|----------|
| Randomized | `false` | `--randomized` / `-r` |
| Leaders | `[]` | `--leaders` / `-l` |
| Ships | `[]` | `--ships` / `-s` |
| Fleets | `[]` | `--fleets` / `-f` |
| Colonies | `[]` | `--colonies` / `-c` |
| Cache TTL | 8 hours | Code only (`CacheManager._ttl`) |
| Cache Location | `%LOCALAPPDATA%/StellarisNameListGenerator/cache` | Code only |
| HTTP Timeout | Default `HttpClient` (100s) | Code only |

## Example Invocations

### Minimal

```bash
dotnet run -- -i input.xml -o output.txt -n "My Name List"
```

### Full

```bash
dotnet run -- \
  -i "base.xml,custom.xml" \
  -o "my_namelist.txt" \
  -n "My Custom Name List" \
  -l "Leader One,Leader Two" \
  -s "Ship Alpha,Ship Beta" \
  -f "Fleet Prime,Fleet Secundus" \
  -c "Colony Alpha,Colony Beta" \
  -r
```

### With Randomized Names

```bash
dotnet run -i input.xml -o output.txt -n "Random Names" -r
```
Output: `randomized = yes`

## Argument Precedence

No precedence rules - all arguments are independent. Multiple input files are merged (concatenated).

## Future Extensibility

Potential configuration additions (not implemented):
- Configuration file support (JSON/YAML)
- Environment variable overrides
- Per-category enable/disable flags
- Output format options (JSON, binary)
- Parallel download configuration
- Cache TTL/directory configuration
- Logging verbosity