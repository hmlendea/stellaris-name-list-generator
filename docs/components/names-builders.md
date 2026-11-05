# Names Builders

## Overview

The names builders layer contains category-specific formatters that convert domain models into Stellaris-compatible Paradox script format. Each builder handles one name category and inherits shared formatting logic from the `NamesBuilder` base class.

## Architecture

```
INamesBuilder (interface)
    │
    ├──► NamesBuilder (abstract base class)
    │       │
    │       ├──► ShipNamesBuilder
    │       ├──► ShipClassNamesBuilder
    │       ├──► FleetNamesBuilder
    │       ├──► ArmyNamesBuilder
    │       ├──► PlanetNamesBuilder
    │       └──► CharacterNamesBuilder
    │
    └──► (Injected into FileContentBuilder)
```

## Base Class: `NamesBuilder`

### Purpose

Provides shared formatting, cleaning, and Unicode normalization logic used by all derived builders.

### Key Methods

#### `BuildNameArray(string categoryName, IEnumerable<string> names)`

Formats a collection of names as a Paradox script array:

```csharp
protected string BuildNameArray(string categoryName, IEnumerable<string> names)
{
    var formatted = names
        .Select(n => $"\"{ProcessName(n)}\"")
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(n => n, StringComparer.OrdinalIgnoreCase);

    if (!formatted.Any())
    {
        return string.Empty;
    }

    var sb = new StringBuilder();
    sb.AppendLine($"{categoryName} = {{");
    foreach (var name in formatted)
    {
        sb.AppendLine($"    {name}");
    }
    sb.AppendLine($"}}");
    sb.AppendLine();
    return sb.ToString();
}
```

#### `CleanGenericNames(IEnumerable<string> names)`

Removes duplicates with diacritic-insensitive comparison:

```csharp
protected IEnumerable<string> CleanGenericNames(IEnumerable<string> names)
{
    return names
        .Where(n => !string.IsNullOrWhiteSpace(n))
        .Select(n => n.Trim())
        .Distinct(DoNamesMatch)
        .OrderBy(n => n, StringComparer.OrdinalIgnoreCase);
}
```

#### `ProcessName(string name)`

Normalizes Unicode characters (500+ mappings):

```csharp
protected string ProcessName(string name)
{
    // Extensive character replacement mapping
    // Examples:
    // "æ" → "ae", "ø" → "o", "ß" → "ss"
    // "α" → "a", "β" → "b", "γ" → "g"
    // Full-width → half-width, smart quotes → straight quotes
    // ... 500+ mappings
}
```

#### `DoNamesMatch(string a, string b)`

Diacritic-insensitive equality comparison:

```csharp
protected bool DoNamesMatch(string a, string b)
{
    return string.Equals(
        a.RemoveDiacritics(),
        b.RemoveDiacritics(),
        StringComparison.OrdinalIgnoreCase
    );
}
```

### Dependencies

- `NuciExtensions.RemoveDiacritics()` - Diacritic removal

## `IShipNamesBuilder` / `ShipNamesBuilder`

### Input

`ShipNames` model with 14 categories:
- Generic, Corvette, Destroyer, Cruiser, Battleship, Titan, Colossus, Juggernaut, Constructor, Science, Coloniser, Transport, IonCannon

### Output

Generates ship name arrays by composing multiple source categories:

```csharp
public string Build(ShipNames ships)
{
    var sb = new StringBuilder();

    // Generic: combines Generic + Corvette + Destroyer + Cruiser + Battleship + Titan + Colossus + Juggernaut
    sb.Append(BuildNameArray("ship_names_generic", GetGenericNames(ships)));

    // Corvette: Corvette + Generic
    sb.Append(BuildNameArray("ship_names_corvette", GetCorvetteNames(ships)));

    // Destroyer: Destroyer + Generic
    sb.Append(BuildNameArray("ship_names_destroyer", GetDestroyerNames(ships)));

    // Cruiser: Cruiser + Generic
    sb.Append(BuildNameArray("ship_names_cruiser", GetCruiserNames(ships)));

    // Battleship: Battleship + Generic
    sb.Append(BuildNameArray("ship_names_battleship", GetBattleshipNames(ships)));

    // Titan: Titan + Generic
    sb.Append(BuildNameArray("ship_names_titan", GetTitanNames(ships)));

    // Colossus: Colossus + Generic
    sb.Append(BuildNameArray("ship_names_colossus", GetColossusNames(ships)));

    // Juggernaut: Juggernaut + Generic
    sb.Append(BuildNameArray("ship_names_juggernaut", GetJuggernautNames(ships)));

    // Constructor: Constructor + Generic
    sb.Append(BuildNameArray("ship_names_constructor", GetConstructorNames(ships)));

    // Science: Science + Generic
    sb.Append(BuildNameArray("ship_names_science", GetScienceNames(ships)));

    // Coloniser: Coloniser + Generic
    sb.Append(BuildNameArray("ship_names_coloniser", GetColoniserNames(ships)));

    // Transport: Transport + Generic
    sb.Append(BuildNameArray("ship_names_transport", GetTransportNames(ships)));

    // Ion Cannon: IonCannon + Generic
    sb.Append(BuildNameArray("ship_names_ion_cannon", GetIonCannonNames(ships)));

    // Mining/Research/Observation/Military Stations: from Stations model
    // (handled in ShipClassNamesBuilder for classes, but stations have names here too)

    return sb.ToString();
}
```

### Composition Logic

Each ship type combines its specific category + Generic names for fallback variety.

## `IShipClassNamesBuilder` / `ShipClassNamesBuilder`

### Input

`ShipClassNames` model with 14 categories (same structure as `ShipNames`)

### Output

Generates ship class name arrays, sourcing from Companies:

```csharp
public string Build(ShipClassNames shipClasses)
{
    var sb = new StringBuilder();

    // Generic: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_generic", GetGenericNames(shipClasses)));

    // Corvette: SpacecraftManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_corvette", GetCorvetteNames(shipClasses)));

    // Destroyer: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_destroyer", GetDestroyerNames(shipClasses)));

    // Cruiser: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_cruiser", GetCruiserNames(shipClasses)));

    // Battleship: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_battleship", GetBattleshipNames(shipClasses)));

    // Titan: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_titan", GetTitanNames(shipClasses)));

    // Colossus: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_colossus", GetColossusNames(shipClasses)));

    // Juggernaut: SpacecraftManufacturers + WeaponManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_juggernaut", GetJuggernautNames(shipClasses)));

    // Constructor: SpacecraftManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_constructor", GetConstructorNames(shipClasses)));

    // Science: ResearchCompanies + Generic
    sb.Append(BuildNameArray("ship_class_names_science", GetScienceNames(shipClasses)));

    // Coloniser: SpacecraftManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_coloniser", GetColoniserNames(shipClasses)));

    // Transport: SpacecraftManufacturers + Generic
    sb.Append(BuildNameArray("ship_class_names_transport", GetTransportNames(shipClasses)));

    // Station classes: from StationClassNames model
    // Mining: RobotManufacturers + AutomotiveManufacturers
    // Research: ResearchCompanies
    // Observation: ResearchCompanies
    // Military: WeaponManufacturers

    return sb.ToString();
}
```

### Composition Logic

Ship classes are primarily sourced from `CompanyNames` categories:
- Spacecraft manufacturers → most military classes
- Weapon manufacturers → combat classes
- Research companies → science/observation
- Robot/Automotive → mining/construction

## `IFleetNamesBuilder` / `FleetNamesBuilder`

### Input

Full `NameList` (accesses multiple categories)

### Output

Generates 14 fleet name format categories with template patterns:

```csharp
public string Build(NameList nameList)
{
    var sb = new StringBuilder();

    // 14 fleet name formats:
    // 1. fleet_names_generic: "{Adjective} {Noun} Fleet"
    // 2. fleet_names_weapon: "{Weapon} Fleet"
    // 3. fleet_names_military_unit: "{MilitaryUnit} Fleet"
    // 4. fleet_names_ship_type: "{ShipType} Fleet"
    // 5. fleet_names_mythological_creature: "{Creature} Fleet"
    // 6. fleet_names_leader: "{Leader} Fleet"
    // 7. fleet_names_admiral: "{Admiral} Fleet"
    // 8. fleet_names_general: "{General} Fleet"
    // 9. fleet_names_scientist: "{Scientist} Fleet"
    // 10. fleet_names_explorer: "{Explorer} Fleet"
    // 11. fleet_names_pioneer: "{Pioneer} Fleet"
    // 12. fleet_names_hero: "{Hero} Fleet"
    // 13. fleet_names_flying_ace: "{FlyingAce} Fleet"
    // 14. fleet_names_deity: "{Deity} Fleet"

    // Each format uses GenerateFleetNames() with specific source categories
    sb.Append(GenerateFleetNames("fleet_names_generic",
        nameList.GreatPeople.AllLeaders,
        nameList.Warfare.MilitaryUnitTypes,
        nameList.Warfare.ShipTypes));

    // ... 13 more formats

    return sb.ToString();
}
```

### Format Templates

Each fleet category uses a specific naming pattern:
- Generic: `"{Adjective} {Noun} Fleet"`
- Weapon: `"{Weapon} Fleet"`
- Military Unit: `"{Unit} Fleet"`
- Ship Type: `"{ShipType} Fleet"`
- Mythological Creature: `"{Creature} Fleet"`
- Leader/Admiral/General/Scientist/Explorer/Pioneer/Hero/Flying Ace/Deity: `"{Name} Fleet"`

### Source Categories

- `GreatPeople.AllLeaders`, `AllGenerals`, `AllDeities`
- `Warfare.Weapons.All`, `MilitaryUnitTypes`, `ShipTypes`
- `BiosphereNames.MythologicalCreatures`

## `IArmyNamesBuilder` / `ArmyNamesBuilder`

### Input

`ArmyNames` model with 20+ army types (sequential templates + explicit name groups)

### Output

Generates army name arrays including special formatting for psionic/xenomorph:

```csharp
public string Build(ArmyNames armies)
{
    var sb = new StringBuilder();

    // Standard armies (use sequential template if no explicit names)
    sb.Append(BuildArmyNames("army_names_fleet", armies.Fleet, armies.FleetSequentialName));
    sb.Append(BuildArmyNames("army_names_defence_army", armies.DefenceArmy, armies.DefenceArmySequentialName));
    sb.Append(BuildArmyNames("army_names_assault_army", armies.AssaultArmy, armies.AssaultArmySequentialName));
    sb.Append(BuildArmyNames("army_names_occupation_army", armies.OccupationArmy, armies.OccupationArmySequentialName));
    sb.Append(BuildArmyNames("army_names_slave_army", armies.SlaveArmy, armies.SlaveArmySequentialName));
    sb.Append(BuildArmyNames("army_names_clone_army", armies.CloneArmy, armies.CloneArmySequentialName));
    sb.Append(BuildArmyNames("army_names_perfected_clone_army", armies.PerfectedCloneArmy, armies.PerfectedCloneArmySequentialName));
    sb.Append(BuildArmyNames("army_names_undead_army", armies.UndeadArmy, armies.UndeadArmySequentialName));
    sb.Append(BuildArmyNames("army_names_robotic_defence_army", armies.RoboticDefenceArmy, armies.RoboticDefenceArmySequentialName));
    sb.Append(BuildArmyNames("army_names_robotic_assault_army", armies.RoboticAssaultArmy, armies.RoboticAssaultArmySequentialName));
    sb.Append(BuildArmyNames("army_names_robotic_occupation_army", armies.RoboticOccupationArmy, armies.RoboticOccupationArmySequentialName));
    sb.Append(BuildArmyNames("army_names_android_defence_army", armies.AndroidDefenceArmy, armies.AndroidDefenceArmySequentialName));
    sb.Append(BuildArmyNames("army_names_android_assault_army", armies.AndroidAssaultArmy, armies.AndroidAssaultArmySequentialName));

    // Special armies with custom formatting
    sb.Append(GeneratePsionicArmyNames(armies));
    sb.Append(GenerateXenomorphArmyNames(armies));

    sb.Append(BuildArmyNames("army_names_super_soldier_army", armies.SuperSoldierArmy, armies.SuperSoldierArmySequentialName));
    sb.Append(BuildArmyNames("army_names_primitive_army", armies.PrimitiveArmy, armies.PrimitiveArmySequentialName));
    sb.Append(BuildArmyNames("army_names_industrial_army", armies.IndustrialArmy, armies.IndustrialArmySequentialName));
    sb.Append(BuildArmyNames("army_names_post_atomic_army", armies.PostAtomicArmy, armies.PostAtomicArmySequentialName));

    return sb.ToString();
}
```

### Sequential Name Templates

Each army type has a default sequential template (used when no explicit names):
- `%O%` = Origin (species/empire name)
- `%C%` = Counter (number)

Examples:
- Fleet: `"%O% Fleet"`
- Defence: `"%O% Planetary Guard"`
- Assault: `"%O% Expeditionary Force"`
- Robotic Defence: `"%O% Ground Defence Matrix"`
- Psionic: `"%O% Psi Commando"`
- Xenomorph: `"%O% Bio-Warfare Division"`

### Special Formatting

**Psionic Armies**: Uses `GeneratePsionicArmyNames()` with custom format
**Xenomorph Armies**: Uses `GenerateXenomorphArmyNames()` with custom format

## `IPlanetNamesBuilder` / `PlanetNamesBuilder`

### Input

`PlanetNames` model with 15 planet classes

### Output

Generates nested planet name structure by class:

```csharp
public string Build(PlanetNames planets)
{
    var sb = new StringBuilder();

    // 15 planet classes
    sb.Append(BuildPlanetNameArray("planet_names_generic", planets.Generic));
    sb.Append(BuildPlanetNameArray("planet_names_desert", planets.Desert));
    sb.Append(BuildPlanetNameArray("planet_names_arid", planets.Arid));
    sb.Append(BuildPlanetNameArray("planet_names_tropical", planets.Tropical));
    sb.Append(BuildPlanetNameArray("planet_names_continental", planets.Continental));
    sb.Append(BuildPlanetNameArray("planet_names_gaia", planets.Gaia));
    sb.Append(BuildPlanetNameArray("planet_names_ocean", planets.Ocean));
    sb.Append(BuildPlanetNameArray("planet_names_tundra", planets.Tundra));
    sb.Append(BuildPlanetNameArray("planet_names_arctic", planets.Arctic));
    sb.Append(BuildPlanetNameArray("planet_names_tomb", planets.Tomb));
    sb.Append(BuildPlanetNameArray("planet_names_savannah", planets.Savannah));
    sb.Append(BuildPlanetNameArray("planet_names_alpine", planets.Alpine));
    sb.Append(BuildPlanetNameArray("planet_names_molten", planets.Molten));
    sb.Append(BuildPlanetNameArray("planet_names_barren", planets.Barren));
    sb.Append(BuildPlanetNameArray("planet_names_asteroid", planets.Asteroid));

    return sb.ToString();
}
```

### Nested Structure

`BuildPlanetNameArray()` creates Paradox script with nested arrays:

```csharp
protected string BuildPlanetNameArray(string categoryName, List<NameGroup> nameGroups)
{
    var names = nameGroups.SelectMany(g => g.Values).Distinct().OrderBy(n => n);

    if (!names.Any())
    {
        return string.Empty;
    }

    var sb = new StringBuilder();
    sb.AppendLine($"{categoryName} = {{");
    foreach (var name in names)
    {
        sb.AppendLine($"    \"{ProcessName(name)}\"");
    }
    sb.AppendLine($"}}");
    sb.AppendLine();
    return sb.ToString();
}
```

## `ICharacterNamesBuilder` / `CharacterNamesBuilder`

### Input

`List<CharacterNames>` - multiple character name sets, each with:
- `Id`: Identifier
- `Weight`: Probability weight
- 11 name group categories (FullNames, FirstNames, RoyalFirstNames, gendered variants, SecondNames, RoyalSecondNames)

### Output

Generates character name arrays grouped by ID, ordered by Weight:

```csharp
public string Build(List<CharacterNames> characters)
{
    var sb = new StringBuilder();

    // Group by Id, order by Weight
    var grouped = characters
        .Where(c => !c.IsEmpty)
        .GroupBy(c => c.Id)
        .OrderBy(g => g.First().Weight);

    foreach (var group in grouped)
    {
        var id = group.Key;
        var charNames = group.First();

        sb.AppendLine($"character_names_{id} = {{");

        // Full names
        sb.Append(BuildCharacterNamesArray("full_names", charNames.FullNames));
        sb.Append(BuildCharacterNamesArray("first_names", charNames.FirstNames));
        sb.Append(BuildCharacterNamesArray("royal_first_names", charNames.RoyalFirstNames));

        // Male
        sb.Append(BuildCharacterNamesArray("male_full_names", charNames.MaleFullNames));
        sb.Append(BuildCharacterNamesArray("male_first_names", charNames.MaleFirstNames));
        sb.Append(BuildCharacterNamesArray("male_royal_first_names", charNames.MaleRoyalFirstNames));

        // Female
        sb.Append(BuildCharacterNamesArray("female_full_names", charNames.FemaleFullNames));
        sb.Append(BuildCharacterNamesArray("female_first_names", charNames.FemaleFirstNames));
        sb.Append(BuildCharacterNamesArray("female_royal_first_names", charNames.FemaleRoyalFirstNames));

        // Second names
        sb.Append(BuildCharacterNamesArray("second_names", charNames.SecondNames));
        sb.Append(BuildCharacterNamesArray("royal_second_names", charNames.RoyalSecondNames));

        sb.AppendLine($"}}");
        sb.AppendLine();
    }

    return sb.ToString();
}
```

### Grouping Logic

- Multiple `CharacterNames` with same `Id` are merged
- Groups ordered by `Weight` (lower = higher priority)
- Empty character sets (`IsEmpty == true`) are skipped

## Builder Execution Order

In `FileContentBuilder.BuildContent()`, builders execute in fixed order:

1. **ShipNamesBuilder** - Ship names by type
2. **ShipClassNamesBuilder** - Ship class names by type
3. **FleetNamesBuilder** - Fleet names (14 formats)
4. **ArmyNamesBuilder** - Army names (20+ types)
5. **PlanetNamesBuilder** - Planet names (15 classes)
6. **CharacterNamesBuilder** - Character names (grouped by ID)

## Testing

No direct unit tests for builders. Tested indirectly via:
- `FileContentBuilderTests` - Full content generation
- `NameListGeneratorTests` - End-to-end generation