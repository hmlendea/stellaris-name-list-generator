# Repository Structure

## Top-Level Layout

```
stellaris-name-list-generator/
├── ARCHITECTURE.md              # High-level architecture documentation
├── CODE_OF_CONDUCT.md           # Community code of conduct
├── CONTRIBUTING.md              # Contribution guidelines
├── PRIVACY.md                   # Data handling practices
├── README.md                    # Project introduction and quick start
├── SECURITY.md                  # Security policy
├── LICENSE                      # Project licence
├── StellarisNameListGenerator.slnx  # Solution file
├── exampleNameList.xml          # Example XML input
├── extractor.sh                 # Name extraction helper script
├── merge.sh                     # Name list merging helper script
├── docs/                        # This documentation set
├── StellarisNameListGenerator/                        # Main application project
├── StellarisNameListGenerator.ArchitectureTests/      # Architecture and fitness tests
├── StellarisNameListGenerator.IntegrationTests/       # End-to-end generation tests
└── StellarisNameListGenerator.UnitTests/              # Unit test project
```

## Main Project: `StellarisNameListGenerator/`

```
StellarisNameListGenerator/
├── Program.cs                   # Entry point, CLI parsing, dependency composition
├── ArgumentNormaliser.cs        # Expands short CLI aliases to canonical options
├── StellarisNameListGenerator.csproj
├── Models/                      # Domain entities (XML-serializable)
│   ├── NameList.cs              # Root aggregate: all name categories
│   ├── NameGroup.cs             # Container: explicit values + URL downloads
│   ├── ArmyNames.cs             # Army categories + sequential name templates
│   ├── BiosphereNames.cs        # Animals, mythological creatures
│   ├── CharacterNames.cs        # Character first/second/full names by gender
│   ├── CompanyNames.cs          # Company categories (manufacturers, research)
│   ├── GreatPeople.cs           # Historical figures + deity categories
│   ├── MilitaryStationNames.cs  # Military station sizes
│   ├── PlaceNames.cs            # Geographic place categories
│   ├── PlanetNames.cs           # Planet class categories
│   ├── ShipNames.cs             # Ship type categories
│   ├── StationNames.cs          # Station categories
│   └── WarfareNames.cs          # Weapons, military units, forts, battles
└── Service/                     # Generation logic
    ├── ICacheManager.cs         # Cache abstraction
    ├── CacheManager.cs          # File-based URL cache (8-hour TTL)
    ├── IFileDownloader.cs       # Download abstraction
    ├── FileDownloader.cs        # HTTP download with cache integration
    ├── IFileContentBuilder.cs   # Content building abstraction
    ├── FileContentBuilder.cs   # Orchestrates all names builders
    ├── INameListGenerator.cs    # Generation abstraction
    ├── NameListGenerator.cs    # Merges lists, writes output file
    ├── Text/                    # Text conversion
    │   └── NameTransliterator.cs  # Unicode-to-Stellaris character conversion
    └── NamesBuilders/           # Per-category output formatters
        ├── INamesBuilder.cs     # Random-name provider contract
        ├── NamesBuilder.cs      # Shared: arrays, dedup, category unification
        ├── NameSampleBuilder.cs # Comment-header name samples
        ├── IShipNamesBuilder.cs
        ├── ShipNamesBuilder.cs
        ├── IShipClassNamesBuilder.cs
        ├── ShipClassNamesBuilder.cs
        ├── IFleetNamesBuilder.cs
        ├── FleetNamesBuilder.cs
        ├── IArmyNamesBuilder.cs
        ├── ArmyNamesBuilder.cs
        ├── IPlanetNamesBuilder.cs
        ├── PlanetNamesBuilder.cs
        ├── ICharacterNamesBuilder.cs
        └── CharacterNamesBuilder.cs
```

## Test Project: `StellarisNameListGenerator.UnitTests/`

```
StellarisNameListGenerator.UnitTests/
├── StellarisNameListGenerator.UnitTests.csproj
├── Models/
│   ├── CharacterNamesTests.cs
│   ├── NameGroupTests.cs
│   ├── NameListTests.cs
│   ├── OtherModelTests.cs
│   └── PlaceNamesTests.cs
└── Service/
    ├── FileContentBuilderTests.cs
    ├── NameListGeneratorTests.cs
    └── NameTransliteratorTests.cs
```

## Integration Test Project: `StellarisNameListGenerator.IntegrationTests/`

```
StellarisNameListGenerator.IntegrationTests/
├── StellarisNameListGenerator.IntegrationTests.csproj
├── CommandLineTests.cs               # Argument aliases, lock flag, validation, failure modes
├── NameListGenerationTests.cs        # Envelope, encoding, identifier, ordering, overwrite
├── NameListFormattingTests.cs        # Per-builder grammar, quoting, sorting, exclusions
├── CharacterNameGenerationTests.cs   # Character ordering, empty groups, duplicate identifiers
├── NameTransliterationTests.cs       # Diacritic removal, transliterated dedup, quoting
├── RemoteNameResolutionTests.cs      # Unusable URL degradation, cache-directory contract
├── Fixtures/
│   └── NameListFixtures.cs           # Canonical XML inputs and shared expected values
└── Tools/
    ├── GeneratorEnvironment.cs       # Isolated temporary working directory per invocation
    ├── GeneratedNameList.cs          # Structured access to generated output
    ├── NameListGeneratorCli.cs       # In-process command line invocation
    ├── NameListXml.cs                # XML document construction helpers
    └── TemporaryDirectory.cs         # Portable temporary directory lifecycle
```

## Architecture Test Project: `StellarisNameListGenerator.ArchitectureTests/`

```
StellarisNameListGenerator.ArchitectureTests/
├── StellarisNameListGenerator.ArchitectureTests.csproj
├── DependencyRuleTests.cs            # Layer dependency boundaries
├── ProjectAndSolutionTests.cs        # Project, solution, and packaging conventions
├── SourceFileConventionTests.cs      # One type per file, naming, namespaces
└── Infrastructure/
    ├── SourceFileLoader.cs
    ├── SourceFileSyntax.cs
    └── SourcePathResolver.cs
```

## Shell Scripts

| Script | Purpose |
|--------|---------|
| `extractor.sh` | Extracts names from existing Stellaris name lists into XML |
| `merge.sh` | Merges multiple name list XML files |

## Naming Conventions

- **Models**: Plain C# classes with parameterless constructors (required for XML deserialization)
- **Interfaces**: `I`-prefixed, one per service implementation
- **Builders**: One builder per output category, all inheriting `NamesBuilder`
- **Tests**: Mirror the source structure; one test fixture per class under test
