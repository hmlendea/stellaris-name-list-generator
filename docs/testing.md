# Testing

## Overview

The project uses **NUnit 3.14.0** with **Moq 4.20.72** for mocking. Tests are organized by layer: model tests in `Models/` and service tests in `Service/`.

## Test Structure

```
StellarisNameListGenerator.UnitTests/
├── StellarisNameListGenerator.UnitTests.csproj
├── Models/
│   ├── CharacterNamesTests.cs      # 5 tests
│   ├── NameGroupTests.cs           # 6 tests
│   ├── NameListTests.cs            # 7 tests
│   ├── OtherModelTests.cs          # 10 tests (all models)
│   └── PlaceNamesTests.cs          # 2 tests
└── Service/
    ├── FileContentBuilderTests.cs  # 5 tests
    └── NameListGeneratorTests.cs    # 4 tests
```

## Running Tests

### Command Line

```bash
dotnet test StellarisNameListGenerator.UnitTests/StellarisNameListGenerator.UnitTests.csproj --verbosity normal
```

### VS Code Tasks

| Task | Command | Purpose |
|------|---------|---------|
| `test` | `dotnet test --no-build -c Release` | Run all tests |
| `test-unit` | `dotnet test --no-build -c Release --filter "FullyQualifiedName~Unit"` | Unit tests only |

### CI Workflow

GitHub Actions `dotnet.yml` workflow runs:
```yaml
run: dotnet test --no-build --verbosity normal
```

## Test Categories

### Model Tests (`Models/`)

Test domain model behavior: defaults, collections, merging, URL behavior, concatenation properties.

#### `NameListTests` (7 tests)

| Test | Description |
|------|-------------|
| `AddRange_MultipleInputFiles_MergesCorrectly` | Merges two name lists with different categories |
| `AddRange_SingleInputFile_UsesFileNameAsId` | Single file sets ID from output path |
| `AddRange_EmptyRepository_CreatesEmptyOutput` | Empty input produces empty name list |
| `AddRange_PreservesAllCategories` | All categories preserved after merge |
| `AddRange_CharactersConcatenated` | Characters list concatenated |
| `AddRange_DenonymsConcatenated` | Denonyms list concatenated |
| `AddRange_PlacesConcatenated` | Places subcategories concatenated |

#### `NameGroupTests` (6 tests)

| Test | Description |
|------|-------------|
| `UrlValues_EmptyUrl_ReturnsEmptyList` | Null/empty URL returns empty |
| `UrlValues_ValidUrl_ReturnsDownloadedNames` | Valid URL downloads and returns names |
| `UrlValues_NetworkError_ReturnsEmptyList` | Network failure returns empty |
| `Values_CombinesExplicitAndUrlValues` | Explicit + URL values combined |
| `IsEmpty_NoValues_ReturnsTrue` | No names → IsEmpty true |
| `IsEmpty_WithValues_ReturnsFalse` | Has names → IsEmpty false |

#### `CharacterNamesTests` (5 tests)

| Test | Description |
|------|-------------|
| `IsEmpty_AllEmpty_ReturnsTrue` | All categories empty → IsEmpty true |
| `IsEmpty_MixedEmpty_ReturnsFalse` | Some categories have names → IsEmpty false |
| `MaleFirstNames_Empty_ReturnsTrue` | Male first names all empty |
| `FemaleFirstNames_Empty_ReturnsTrue` | Female first names all empty |
| `RoyalSecondNames_Empty_ReturnsTrue` | Royal second names all empty |

#### `PlaceNamesTests` (2 tests)

| Test | Description |
|------|-------------|
| `WaterBodies_ContainsRiversLakesSeas` | WaterBodies includes rivers, lakes, seas |
| `GeographicalPlaces_ContainsMountainsForestsDeserts` | GeographicalPlaces includes all terrain types |

#### `OtherModelTests` (10 tests)

Tests all model default initializations:
- `ArmyNames_DefaultsInitialized`
- `BiosphereNames_DefaultsInitialized`
- `CompanyNames_DefaultsInitialized`
- `GreatPeople_DefaultsInitialized`
- `MilitaryStationNames_DefaultsInitialized`
- `PlanetNames_DefaultsInitialized`
- `ShipNames_DefaultsInitialized`
- `StationNames_DefaultsInitialized`
- `StationClassNames_DefaultsInitialized`
- `WarfareNames_DefaultsInitialized`

### Service Tests (`Service/`)

Test application services: merging, content building, file writing.

#### `NameListGeneratorTests` (4 tests)

| Test | Description |
|------|-------------|
| `GenerateAsync_MultipleInputFiles_MergesCorrectly` | Multiple files merged correctly |
| `GenerateAsync_SingleInputFile_UsesFileNameAsId` | Single file uses filename as ID |
| `GenerateAsync_EmptyRepository_CreatesEmptyOutput` | Empty repo creates empty output |
| `GenerateAsync_OutputPath_SetsIdFromFileName` | Output path determines ID |

#### `FileContentBuilderTests` (5 tests)

| Test | Description |
|------|-------------|
| `BuildContent_GeneratesHeader` | Header metadata generated correctly |
| `BuildContent_CLICategoriesInjected` | CLI leader/ship/fleet/colony injected |
| `BuildContent_GeneratedCategoriesPresent` | All 6 builder categories present |
| `BuildContent_MergeBehavior` | Merge behavior reflected in output |
| `BuildContent_RandomizedFlag` | randomized flag set correctly |

## Test Patterns

### Model Tests

```csharp
[Test]
public void AddRange_MultipleInputFiles_MergesCorrectly()
{
    var list1 = new NameList();
    list1.Denonyms.Add(new NameGroup { Name = "Base", ExplicitValues = new List<string> { "Human" } });

    var list2 = new NameList();
    list2.Denonyms.Add(new NameGroup { Name = "Base", ExplicitValues = new List<string> { "Terran" } });

    list1.AddRange(list2);

    Assert.That(list1.Denonyms.Count, Is.EqualTo(2));
}
```

### Service Tests

```csharp
[Test]
public async Task GenerateAsync_SingleInputFile_UsesFileNameAsId()
{
    var generator = new NameListGenerator(
        new FileContentBuilder(...),
        new XmlRepository<NameList>()
    );

    await generator.GenerateAsync(
        new[] { "input.xml" },
        "output/test.txt",
        "Test Name",
        Array.Empty<string>(),
        Array.Empty<string>(),
        Array.Empty<string>(),
        Array.Empty<string>(),
        false
    );

    // Verify output file exists and has correct ID
    var content = await File.ReadAllTextAsync("output/test.txt");
    Assert.That(content, Does.Contain("id = \"test\""));
}
```

### Mocking

Moq is used for mocking dependencies where needed:

```csharp
// Example mock setup
var mockBuilder = new Mock<IFileContentBuilder>();
mockBuilder.Setup(b => b.BuildContent(It.IsAny<NameList>(), It.IsAny<string[]>(), ...))
    .Returns("expected content");
```

Note: Most tests use real implementations (not mocks) since the services are simple and deterministic.

## Coverage Assessment

### What's Covered

| Layer | Coverage | Notes |
|-------|----------|-------|
| Models | High | All models tested for defaults, merging, URL behavior |
| Services | Medium | Merge, build, write tested; download not directly tested |
| Builders | Low | Indirectly tested via FileContentBuilder tests |
| Presentation | None | Not unit tested (integration tested via CLI) |

### Gaps

1. **No integration tests**: End-to-end generation from CLI to file not tested
2. **No UI tests**: No GUI or manual verification procedures
3. **Builder edge cases**: Exhaustive testing of Unicode normalization, dedup logic not done
4. **Download flow**: CacheManager and FileDownloader not directly unit tested
5. **Error paths**: Exception handling for malformed XML, network errors not tested
6. **Performance**: No performance benchmarks or load tests

### Recommended Additional Tests

```csharp
// Integration test example
[Test]
public async Task GenerateAsync_EndToEnd_ProducesValidOutput()
{
    // Create temp input XML
    // Run generator
    // Validate output format (Paradox script syntax)
}

// Builder edge case
[Test]
public void ProcessName_SpecialCharacters_Normalized()
{
    var builder = new ShipNamesBuilder();
    var result = builder.ProcessName("Café résumé naïve");
    Assert.That(result, Does.Not.Contain("é")); // Should be normalized
}
```

## Test Configuration

### NUnit Attributes

```csharp
using NUnit.Framework;

[TestFixture]
public class NameListTests
{
    [Test]
    public void AddRange_MultipleInputFiles_MergesCorrectly() { ... }
}
```

### Assertions

- `Assert.That(...)` with `Is`, `Does`, `Contains`, `EqualTo`
- `Assert.Throws<Exception>(...)` for exception testing
- `Assert.Pass()` / `Assert.Fail()` for pass/fail markers

### Test Discovery

NUnit discovers tests via:
- `[Test]` attribute on methods
- `[TestCase]` for parameterized tests
- `[TestFixture]` on classes
- `dotnet test` auto-discovers via `Microsoft.NET.Test.Sdk`

## CI Integration

GitHub Actions `dotnet.yml` workflow:
```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal
```

Test results uploaded as artifact:
```yaml
- name: Upload test results
  uses: actions/upload-artifact@v4
  if: always()
  with:
    name: test-results
    path: **/test_results.trx
```

## Test Data

No external test data files. Tests create in-memory objects and use real file I/O for output verification. The `exampleNameList.xml` in the repository root serves as documentation but is not used as test fixture.

## Test Execution Time

| Command | Time |
|---------|------|
| `dotnet test` | ~2-5 seconds |
| `dotnet test --filter "FullyQualifiedName~Unit"` | ~1-3 seconds |
| `dotnet test --filter "FullyQualifiedName~Models"` | ~1-2 seconds |
| `dotnet test --filter "FullyQualifiedName~Service"` | ~1-2 seconds |

## Test Maintenance

When adding new models or builders:
1. Add model tests for new model classes
2. Add builder tests if new builder is added
3. Update `FileContentBuilderTests` if builder order changes
4. Update `NameListGeneratorTests` if merge behavior changes
5. Update `OtherModelTests` if new model needs default initialization test