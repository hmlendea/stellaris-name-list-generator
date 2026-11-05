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
├── release.sh                   # Release helper script
├── docs/                        # This documentation set
├── StellarisNameListGenerator/         # Main application project
└── StellarisNameListGenerator.UnitTests/  # Unit test project
```

## Main Project: `StellarisNameListGenerator/`

```
StellarisNameListGenerator/
├── Program.cs                   # Entry point, CLI parsing, dependency composition
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
    └── NamesBuilders/           # Per-category output formatters
        ├── INamesBuilder.cs     # Base builder contract
        ├── NamesBuilder.cs      # Shared: arrays, dedup, Unicode mapping
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
    └── NameListGeneratorTests.cs
```

## Shell Scripts

| Script | Purpose |
|--------|---------|
| `extractor.sh` | Extracts names from existing Stellaris name lists into XML |
| `merge.sh` | Merges multiple name list XML files |
| `release.sh` | Automates release packaging |

## Naming Conventions

- **Models**: Plain C# classes with parameterless constructors (required for XML deserialization)
- **Interfaces**: `I`-prefixed, one per service implementation
- **Builders**: One builder per output category, all inheriting `NamesBuilder`
- **Tests**: Mirror the source structure; one test fixture per class under test
