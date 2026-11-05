# Presentation Layer: CLI Entry Point

## Overview

The presentation layer consists of `Program.cs` — the application entry point that handles CLI argument parsing, normalizes input, composes dependencies, and orchestrates the name list generation pipeline.

## `Program.cs`

### Responsibilities

1. **Argument Parsing**: Uses `NuciCLI` to parse command-line arguments
2. **Argument Normalization**: Converts CLI arguments to internal representation
3. **Dependency Composition**: Instantiates all services and builders
4. **Pipeline Execution**: Coordinates the generation flow

### CLI Arguments

| Argument | Type | Required | Description |
|----------|------|----------|-------------|
| `--input` / `-i` | string[] | Yes | Input XML file paths (one or more) |
| `--output` / `-o` | string | Yes | Output file path |
| `--name` / `-n` | string | Yes | Name list display name |
| `--leaders` / `-l` | string | No | Leader names (comma-separated) |
| `--ships` / `-s` | string | No | Ship names (comma-separated) |
| `--fleets` / `-f` | string | No | Fleet names (comma-separated) |
| `--colonies` / `-c` | string | No | Colony names (comma-separated) |
| `--randomized` / `-r` | bool | No | Whether names are randomized (default: false) |

### Argument Normalization

```csharp
// Input paths: split by comma, trim, filter empty
var inputPaths = args.Input.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(p => p.Trim())
    .ToArray();

// Output path: use as-is
var outputPath = args.Output;

// Name: use as-is
var name = args.Name;

// Leaders/Ships/Fleets/Colonies: split by comma, trim, filter empty
var leaders = args.Leaders?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToArray() ?? [];

var ships = args.Ships?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToArray() ?? [];

var fleets = args.Fleets?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToArray() ?? [];

var colonies = args.Colonies?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToArray() ?? [];

// Randomized: boolean flag
var randomized = args.Randomized;
```

### Dependency Composition

```csharp
// Cache and download
var cacheManager = new CacheManager();
var fileDownloader = new FileDownloader(cacheManager);

// Names builders (category-specific formatters)
var shipNamesBuilder = new ShipNamesBuilder();
var shipClassNamesBuilder = new ShipClassNamesBuilder();
var fleetNamesBuilder = new FleetNamesBuilder();
var armyNamesBuilder = new ArmyNamesBuilder();
var planetNamesBuilder = new PlanetNamesBuilder();
var characterNamesBuilder = new CharacterNamesBuilder();

// Content builder (orchestrates all names builders)
var fileContentBuilder = new FileContentBuilder(
    shipNamesBuilder,
    shipClassNamesBuilder,
    fleetNamesBuilder,
    armyNamesBuilder,
    planetNamesBuilder,
    characterNamesBuilder
);

// Repository for reading input XML files
var repository = new XmlRepository<NameList>();

// Generator (merges input, writes output)
var generator = new NameListGenerator(fileContentBuilder, repository);
```

### Pipeline Execution

```csharp
// Generate the name list
await generator.GenerateAsync(
    inputPaths,
    outputPath,
    name,
    leaders,
    ships,
    fleets,
    colonies,
    randomized
);
```

## `NuciCLI` Integration

The application uses `NuciCLI` (v3.0.1) for argument parsing:

```csharp
var args = Cli.Parse<CliArguments>(args);
```

Where `CliArguments` is a record/class with properties matching the CLI options.

## Error Handling

- Missing required arguments: `NuciCLI` throws with usage message
- File not found: `XmlRepository` throws `FileNotFoundException`
- XML deserialization errors: `NuciDAL` throws descriptive exceptions
- Network errors during download: `FileDownloader` propagates `HttpRequestException`

## Output

On success, writes a Stellaris-compatible name list file to the output path. The file format is Paradox script (`.txt` extension typically used).

## Testing

The presentation layer is not unit tested directly (integration tested via `NameListGeneratorTests`). CLI argument parsing is validated by `NuciCLI` at runtime.