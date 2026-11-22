# Testing

## Overview

The project uses **NUnit** with **Moq** for mocking. Tests are grouped into three projects: `StellarisNameListGenerator.UnitTests` (model and service behaviour in isolation), `StellarisNameListGenerator.IntegrationTests` (end-to-end generation through the command line entry point), and `StellarisNameListGenerator.ArchitectureTests` (dependency, project, solution, and source-file conventions).

## Test Structure

```
StellarisNameListGenerator.UnitTests/
├── StellarisNameListGenerator.UnitTests.csproj
├── ArgumentNormaliserTests.cs
├── Models/
│   ├── CharacterNamesTests.cs
│   ├── ModelDefaultInitialisationTests.cs
│   ├── NameGroupTests.cs
│   ├── NameGroupValueTests.cs
│   ├── NameListMergeCoverageTests.cs
│   ├── NameListTests.cs
│   ├── OtherModelTests.cs
│   └── PlaceNamesTests.cs
└── Service/
    ├── FileContentBuilderTests.cs
    ├── NameListGeneratorServiceTests.cs
    ├── NameListGeneratorTests.cs
    ├── NamesBuilderTests.cs
    ├── NameTransliteratorCompositionTests.cs
    ├── NameTransliteratorTableTests.cs
    └── NameTransliteratorTests.cs

StellarisNameListGenerator.IntegrationTests/
├── StellarisNameListGenerator.IntegrationTests.csproj
├── BlockCompositionTests.cs
├── BlockOrderingTests.cs
├── CharacterCategoryMappingTests.cs
├── CharacterNameGenerationTests.cs
├── CommandLineScenarioTests.cs
├── CommandLineTests.cs
├── FleetNameGenerationTests.cs
├── GenerationDeterminismTests.cs
├── GenerationMatrixTests.cs
├── InputDocumentToleranceTests.cs
├── ArmyNameGenerationTests.cs
├── NameListFormattingTests.cs
├── NameListGenerationTests.cs
├── NameListHeaderTests.cs
├── NameListValueHandlingTests.cs
├── NameTransliterationTests.cs
├── PlanetNameGenerationTests.cs
├── RemoteNameResolutionTests.cs
├── ShipClassNameGenerationTests.cs
├── ShipNameGenerationTests.cs
├── SourceRoutingTests.cs
├── StationNameGenerationTests.cs
├── Fixtures/
│   ├── CoverageDocument.cs
│   └── NameListFixtures.cs
└── Tools/
    ├── GeneratorEnvironment.cs
    ├── GeneratedNameList.cs
    ├── NameListGeneratorCli.cs
    ├── NameListXml.cs
    └── TemporaryDirectory.cs

StellarisNameListGenerator.ArchitectureTests/
├── StellarisNameListGenerator.ArchitectureTests.csproj
├── DependencyRuleTests.cs
├── ProjectAndSolutionTests.cs
├── SourceFileConventionTests.cs
└── Infrastructure/
```

## Running Tests

### Command Line

```bash
# Everything in the solution
dotnet test StellarisNameListGenerator.slnx --verbosity normal

# A single project
dotnet test StellarisNameListGenerator.IntegrationTests/StellarisNameListGenerator.IntegrationTests.csproj --verbosity normal
```

### VS Code Tasks

| Task | Command | Purpose |
|------|---------|---------|
| `test` | `dotnet test` | Run every test project in the solution |
| `test-unit` | `dotnet test` | Run the unit tests only |
| `test-integration` | `dotnet test` | Run the integration tests only |

### CI Workflow

GitHub Actions `dotnet.yml` workflow runs:
```yaml
run: dotnet test --no-build --verbosity normal
```

## Test Categories

The suite contains 1914 tests across the three projects: 1387 unit, 506 integration, and 21 architecture.

### Suite Overview

| Suite | Project | Coverage |
|-------|---------|----------|
| `NameTransliteratorTableTests` | Unit | 335 verified single-character transliteration mappings, one per replacement branch |
| `NameListMergeCoverageTests` | Unit | All 149 merged collections, each in three merge scenarios |
| `ModelDefaultInitialisationTests` | Unit | Every collection property on every model: non-null, empty, mutable, stable |
| `ArgumentNormaliserTests` | Unit | Every option alias, bare and valued lock forms, validation failures |
| `NamesBuilderTests` | Unit | Each category builder: envelope, exclusions, quoting, derived names, random selection |
| `NameTransliteratorCompositionTests` | Unit | Multi-character names, idempotence, permitted characters, script mixing |
| `NameGroupValueTests` | Unit | Value resolution, caching semantics, URL degradation, emptiness |
| `NameListGeneratorServiceTests` | Unit | Identity, lock propagation, merged-list selection, encoding, replacement |
| `BlockCompositionTests` | Integration | Exact source composition of all 73 populated blocks |
| `SourceRoutingTests` | Integration | Exact destination blocks of all 153 routed source categories |
| `BlockOrderingTests` | Integration | Declaration order of every sub-block in every category |
| `GenerationMatrixTests` | Integration | 12 documents × 9 invariant classes, including all lock forms |
| Per-category suites | Integration | Ships, ship classes, fleets, armies, planets, stations, characters |
| `NameListValueHandlingTests` | Integration | Sorting, distinctness, trimming, entity decoding, wrapping |
| `GenerationDeterminismTests` | Integration | Locked reproducibility and lock-flag isolation |
| `InputDocumentToleranceTests` | Integration | Declaration, byte-order mark, whitespace, unknown root |

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

#### `NameListMergeCoverageTests` (447 tests)

Every collection merged by `NameList.AddRange` is exercised in three scenarios: both operands populated
(two items), the other operand empty (count unchanged), and the target empty (one item). This proves that
no collection was omitted from the merge implementation.

#### `ModelDefaultInitialisationTests` (298 tests)

Every collection property on every model is checked for non-nullness, emptiness, mutability, and stable
identity across reads, so deserialisation cannot encounter a null collection.

#### `NameGroupTests` (6 tests)

| Test | Description |
|------|-------------|
| `UrlValues_EmptyUrl_ReturnsEmptyList` | Null/empty URL returns empty |
| `UrlValues_ValidUrl_ReturnsDownloadedNames` | Valid URL downloads and returns names |
| `UrlValues_NetworkError_ReturnsEmptyList` | Network failure returns empty |
| `Values_CombinesExplicitAndUrlValues` | Explicit + URL values combined |
| `IsEmpty_NoValues_ReturnsTrue` | No names → IsEmpty true |
| `IsEmpty_WithValues_ReturnsFalse` | Has names → IsEmpty false |

#### `NameGroupValueTests` (21 tests)

Value resolution ordering, cache identity, cache invalidation on list replacement, conservative cache
serving on in-place mutation, URL degradation for unreachable and malformed URLs, and whitespace URLs.

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
| Models | Exhaustive | Every model collection is asserted for default initialisation, and every `AddRange` collection is asserted in three merge scenarios |
| Transliteration | Exhaustive | 335 verified per-character mappings plus composition, idempotence, and permitted-character tests |
| Services | High | Merge, build, write, identity, lock propagation, and encoding tested; live download not directly tested |
| Builders | High | Each builder is unit-tested directly and every generated block is asserted for exact source composition |
| Presentation | High | `CommandLineTests`, `CommandLineScenarioTests`, and `ArgumentNormaliserTests` cover parsing, validation, aliases, and failure modes |
| Encoding and grammar | High | The UTF-8 preamble, line endings, identifier derivation, and category ordering are asserted across 12 document shapes |
| Determinism | High | `GenerationDeterminismTests` and `GenerationMatrixTests` assert reproducible locked output and lock-flag isolation |
| Value handling | High | `NameListValueHandlingTests` asserts sorting, distinctness, trimming, entity decoding, and wrapping |
| Block composition | Exhaustive | 73 block compositions and 153 source routings are asserted against captured ground truth |

### Gaps

1. **Golden files**: No comparison against a captured Stellaris reference output
2. **Live remote download**: `FileDownloader`/`CacheManager` cannot be stubbed from the command line boundary; the merge success path is covered offline through a pre-populated cache entry, while the live network path is verified only through its degradation contract
3. **Stellaris acceptance**: Game ingestion is verified manually, not automatically
4. **Performance**: No performance benchmarks or load tests
5. **Header samples**: The sample lines in the comment header draw random values, so byte-identical comparison of a full file requires normalising those lines

### Recommended Additional Tests

```csharp
// Golden-file comparison
[Test]
public void GivenRichDocument_WhenGenerating_ThenOutputMatchesTheGoldenFile()
{
    Assert.That(
        environment.GenerateRichDocument("nucilandia.txt"),
        Is.EqualTo(expectedGoldenContent));
}

// Live remote download (requires a stubbable downloader)
[Test]
public void GivenReachableUrl_WhenGenerating_ThenDownloadedNamesAreMerged()
{
    Assert.That(output.BlockContains("ship_names", "generic", "Downloaded"), Is.True);
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

The unit tests construct in-memory model objects. The integration tests build XML documents through `Tools/NameListXml.cs` and the canonical inputs in `Fixtures/NameListFixtures.cs`, writing each document into a private temporary directory. `Fixtures/CoverageDocument.cs` builds a single document in which every recognised source category carries a unique traceable token (`NucQVnnn`), which is what makes the composition and routing matrices exact. No external data file is required, so the suite is cross-platform, environment-independent, and machine-independent.

### Captured Ground Truth

The composition, routing, and transliteration tables were produced by running the generator over purpose-built documents and reading back the result, not by transcribing the implementation. Any behavioural change that alters a mapping therefore fails a test rather than silently agreeing with itself.

## Test Execution Time

| Command | Time |
|---------|------|
| `dotnet test StellarisNameListGenerator.slnx` | ~5 seconds |
| `dotnet test ...IntegrationTests.csproj` | ~4 seconds |
| `dotnet test ...UnitTests.csproj` | ~1 second |
| `dotnet test ...ArchitectureTests.csproj` | ~1 second |

## Test Maintenance

When adding new models or builders:
1. Add model tests for new model classes
2. Add builder tests if new builder is added
3. Extend the matching per-category generation suite when the category grammar changes
4. Extend `CoverageDocument` when a new source category is added, then regenerate the composition and routing expectations by running the generator
5. Update `FileContentBuilderTests` if builder order changes
6. Update `NameListGeneratorTests` if merge behavior changes
7. Update `OtherModelTests` and `ModelDefaultInitialisationTests` if new model needs default initialization test
8. Update `NameListMergeCoverageTests` when a collection is added to `NameList.AddRange`
9. Extend `Fixtures/NameListFixtures.cs` and the matching integration fixture if the generated grammar changes