# Dependencies

## Overview

The project targets **.NET 10.0** (`net10.0`) and uses four NuGet packages from the `Nuci` ecosystem, all authored by the same publisher.

## NuGet Packages

### `NuciCLI` (3.0.1)

**Purpose**: Command-line argument parsing and validation.

**Usage**: `Program.cs` - Parses CLI arguments into typed objects.

```csharp
var args = Cli.Parse<CliArguments>(args);
```

**Features**:
- Attribute-based argument definition
- Short/long aliases (`-i` / `--input`)
- Required/optional validation
- Automatic help/usage generation
- Type conversion (string, bool, arrays, enums)

**License**: MIT (assumed, typical for Nuci packages)

### `NuciCLI.Arguments` (1.0.1)

**Purpose**: Argument model definitions and validation attributes for `NuciCLI`.

**Usage**: Defines the `CliArguments` class with `[Argument]` attributes.

```csharp
public sealed class CliArguments
{
    [Argument('i', "input", Required = true)]
    public string Input { get; set; }

    [Argument('o', "output", Required = true)]
    public string Output { get; set; }

    [Argument('n', "name", Required = true)]
    public string Name { get; set; }

    [Argument('l', "leaders")]
    public string Leaders { get; set; }

    [Argument('s', "ships")]
    public string Ships { get; set; }

    [Argument('f', "fleets")]
    public string Fleets { get; set; }

    [Argument('c', "colonies")]
    public string Colonies { get; set; }

    [Argument('r', "randomized")]
    public bool Randomized { get; set; }
}
```

**Relationship**: Companion package to `NuciCLI`; provides argument attributes.

### `NuciDAL` (3.0.1)

**Purpose**: Data Access Layer for XML repository pattern. Provides `XmlRepository<T>` for deserializing XML files to objects.

**Usage**: `NameListGenerator` - Loads input XML files.

```csharp
private readonly IFileRepository<NameList> _repository;

// In constructor:
_repository = new XmlRepository<NameList>();

// In GetMergedNameList:
var nameList = _repository.Load(path);
```

**Features**:
- Generic `IFileRepository<T>` interface
- `XmlRepository<T>` implementation
- Automatic XML deserialization using `System.Xml.Serialization`
- Requires parameterless constructors and public get/set properties
- Throws descriptive exceptions on file not found or malformed XML

**Interface**:
```csharp
public interface IFileRepository<T>
{
    T Load(string path);
}
```

### `NuciExtensions` (5.3.1)

**Purpose**: Extension methods for collections, strings, and text processing.

**Usage**: `NamesBuilder` base class - Uses `RemoveDiacritics()` for diacritic-insensitive comparison.

```csharp
// In NamesBuilder.DoNamesMatch():
return string.Equals(
    a.RemoveDiacritics(),
    b.RemoveDiacritics(),
    StringComparison.OrdinalIgnoreCase
);
```

**Key Extensions Used**:
- `string.RemoveDiacritics()` - Removes diacritical marks (accents, umlauts, etc.)
- Collection extensions (likely `DistinctBy`, `Chunk`, etc.)
- Text processing utilities

**Other Available Extensions** (not directly used but available):
- `IEnumerable<T>` extensions
- `StringBuilder` extensions
- Date/time formatting
- Numeric formatting

## Dependency Graph

```
StellarisNameListGenerator (net10.0)
├── NuciCLI (3.0.1)
│   └── NuciCLI.Arguments (1.0.1)
├── NuciDAL (3.0.1)
│   └── (System.Xml, System.IO)
└── NuciExtensions (5.3.1)
    └── (System.Text, System.Collections)

StellarisNameListGenerator.UnitTests (net10.0)
├── NUnit (3.14.0)
├── Moq (4.20.72)
├── NuciCLI (3.0.1)
├── NuciDAL (3.0.1)
└── NuciExtensions (5.3.1)
    └── StellarisNameListGenerator (project reference)
```

## Framework Dependencies

### .NET 10.0 (net10.0)

**Target Framework**: `net10.0` in all `.csproj` files.

**Implicit Packages** (from .NET SDK):
- `System.Runtime`
- `System.Collections`
- `System.Linq`
- `System.Text.Json` (not used directly)
- `System.Xml` (used by NuciDAL)
- `System.Net.Http` (used by `HttpClient` in `FileDownloader`)
- `System.Security.Cryptography` (used by `SHA256` in `CacheManager`)
- `System.IO` (file system operations)

## Version Policy

- **Nuci packages**: Pinned to specific versions (3.0.1, 1.0.1, 5.3.1)
- **Test packages**: Pinned (NUnit 3.14.0, Moq 4.20.72)
- **No floating versions** (`*`) used
- **No central package management** (Directory.Packages.props) - versions in each `.csproj`

## Transitive Dependencies

| Package | Transitive Deps | Notes |
|---------|-----------------|-------|
| NuciCLI | NuciCLI.Arguments, System.CommandLine (likely) | Modern CLI parsing |
| NuciDAL | System.Xml, System.IO | XML serialization |
| NuciExtensions | System.Text, System.Collections | Extension methods |
| NUnit | NUnit.Analyzers | Test framework |
| Moq | Castle.Core | Mocking framework |

## Security

- **No known vulnerabilities** in current versions (as of project creation)
- **All packages from same publisher** (Nuci ecosystem) - consistent trust boundary
- **No runtime code generation** or reflection-heavy libraries
- **HttpClient** uses system SSL/TLS validation

## License Compliance

All Nuci packages assumed MIT-compatible. Verify via:
- `dotnet nuget list package --source nuget.org` for license URLs
- Package `licenseUrl` or `license` metadata

## Updating Dependencies

### Check for Updates

```bash
dotnet list package --outdated
```

### Update Nuci Packages

Update all three Nuci packages together (they're version-locked as a set):

```xml
<!-- StellarisNameListGenerator.csproj -->
<PackageReference Include="NuciCLI" Version="3.0.1" />
<PackageReference Include="NuciCLI.Arguments" Version="1.0.1" />
<PackageReference Include="NuciDAL" Version="3.0.1" />
<PackageReference Include="NuciExtensions" Version="5.3.1" />
```

### Update Test Packages

```xml
<!-- StellarisNameListGenerator.UnitTests.csproj -->
<PackageReference Include="NUnit" Version="3.14.0" />
<PackageReference Include="Moq" Version="4.20.72" />
```

### Validation After Update

1. `dotnet build` - Compile
2. `dotnet test` - Run tests
3. Manual CLI test - Verify generation works

## Internal Dependencies

### Project References

```
StellarisNameListGenerator.UnitTests
    └── StellarisNameListGenerator (project reference)
```

### Namespace Organization

| Namespace | Assembly | Purpose |
|-----------|----------|---------|
| `StellarisNameListGenerator` | Main | Entry point, composition |
| `StellarisNameListGenerator.Models` | Main | Domain entities |
| `StellarisNameListGenerator.Service` | Main | Application services |
| `StellarisNameListGenerator.Service.NamesBuilders` | Main | Category formatters |
| `StellarisNameListGenerator.UnitTests.Models` | Tests | Model tests |
| `StellarisNameListGenerator.UnitTests.Service` | Tests | Service tests |

## Dependency Rules

1. **Models** - No external dependencies (except NuciExtensions for `RemoveDiacritics` in `NameGroup`? No, only in `NamesBuilder`)
2. **Services** - Depend on Models, NuciDAL, NuciExtensions
3. **NamesBuilders** - Depend on Models, NuciExtensions, base `NamesBuilder`
4. **Program** - Depends on all Services, NuciCLI
5. **Tests** - Depend on Main project, NUnit, Moq

## Missing/Implicit Dependencies

### Not Used But Common

- **Logging**: No `Microsoft.Extensions.Logging` or `Serilog` - uses `Console.WriteLine` if needed
- **DI Container**: No `Microsoft.Extensions.DependencyInjection` - manual composition in `Program.cs`
- **Configuration**: No `Microsoft.Extensions.Configuration` - CLI only
- **JSON**: No `System.Text.Json` or `Newtonsoft.Json` - XML only via NuciDAL
- **YAML**: No YAML support
- **Database**: No database access

### Potential Future Dependencies

- `Microsoft.Extensions.Hosting` - For hosted service pattern
- `System.CommandLine` - If migrating from NuciCLI
- `Polly` - For retry policies on downloads
- `FluentValidation` - For input validation