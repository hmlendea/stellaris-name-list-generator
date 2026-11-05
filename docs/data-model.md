# Data Model

## Overview

The domain model represents all Stellaris name categories as a hierarchy of plain C# classes. The root aggregate is `NameList`, which contains all category models. Each category model contains `List<NameGroup>` properties for its subcategories.

All models have parameterless constructors (required for XML deserialization via `NuciDAL`) and initialize all collections to empty lists.

## Root Aggregate: `NameList`

```csharp
public sealed class NameList
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsLocked { get; set; } = true;

    public List<NameGroup> Denonyms { get; set; }
    public PlaceNames Places { get; set; }
    public GreatPeople GreatPeople { get; set; }
    public CompanyNames Companies { get; set; }
    public WarfareNames Warfare { get; set; }
    public BiosphereNames BiosphereNames { get; set; }
    public ShipNames Ships { get; set; }
    public ShipClassNames ShipClasses { get; set; }
    public StationNames Stations { get; set; }
    public StationClassNames StationClasses { get; set; }
    public ArmyNames Armies { get; set; }
    public PlanetNames Planets { get; set; }
    public List<CharacterNames> Characters { get; set; }

    public void AddRange(NameList other) { ... }
}
```

**Properties:**
- `Id`: File name without extension (set from output path)
- `Name`: Display name (from CLI argument)
- `IsLocked`: Whether `randomized = no` in output (from CLI argument)
- `Denonyms`: Base name groups for species demonyms
- `Places`: Geographic place names
- `GreatPeople`: Historical figures and deity categories
- `Companies`: Company and manufacturer names
- `Warfare`: Weapons, military units, forts, battles
- `BiosphereNames`: Animals and mythological creatures
- `Ships`: Ship names by type
- `ShipClasses`: Ship class names by type
- `Stations`: Station names by type
- `StationClasses`: Station class names by type
- `Armies`: Army names by type + sequential templates
- `Planets`: Planet names by class
- `Characters`: Character name sets (multiple per name list)

**Methods:**
- `AddRange(NameList other)`: Merges another name list into this one by concatenating all collections

## Core Building Block: `NameGroup`

```csharp
public sealed class NameGroup
{
    public string Name { get; set; }
    public List<string> ExplicitValues { get; set; }
    public string Url { get; set; }

    public IEnumerable<string> Values => ExplicitValues.Concat(UrlValues);
    public bool IsEmpty => !Values.Any();
    public List<string> UrlValues { get; } // Downloads from Url on first access
}
```

**Properties:**
- `Name`: Category label (used as comment in output)
- `ExplicitValues`: Names provided directly in XML
- `Url`: Optional URL to download additional names from
- `Values`: Combined explicit + downloaded names (lazy evaluation)
- `IsEmpty`: True if no names available
- `UrlValues`: Downloads content from `Url` on first access, splits by newline

**Behavior:**
- `UrlValues` uses static `FileDownloader` with `CacheManager` (8-hour TTL)
- Download happens on first access to `Values` or `UrlValues`
- Downloaded content is split by newline into individual names

## Category Models

### `PlaceNames`

```csharp
public sealed class PlaceNames
{
    public List<NameGroup> Countries { get; set; }
    public List<NameGroup> Regions { get; set; }
    public List<NameGroup> Cities { get; set; }
    public List<NameGroup> Mountains { get; set; }
    public List<NameGroup> Forests { get; set; }
    public List<NameGroup> Deserts { get; set; }
    public List<NameGroup> Rivers { get; set; }
    public List<NameGroup> Lakes { get; set; }
    public List<NameGroup> Seas { get; set; }
    public List<NameGroup> Airports { get; set; }

    public IEnumerable<NameGroup> WaterBodies => Rivers.Concat(Lakes).Concat(Seas);
    public IEnumerable<NameGroup> GeographicalPlaces => Mountains.Concat(Forests).Concat(Deserts).Concat(Rivers).Concat(Lakes).Concat(Seas);
}
```

### `GreatPeople`

```csharp
public sealed class GreatPeople
{
    // Historical figures
    public List<NameGroup> Explorers { get; set; }
    public List<NameGroup> Pioneers { get; set; }
    public List<NameGroup> Scientists { get; set; }
    public List<NameGroup> LeadersTier1 { get; set; }
    public List<NameGroup> LeadersTier2 { get; set; }
    public List<NameGroup> LeadersTier3 { get; set; }
    public List<NameGroup> FlyingAces { get; set; }
    public List<NameGroup> Heroes { get; set; }
    public List<NameGroup> Admirals { get; set; }
    public List<NameGroup> GeneralsTier1 { get; set; }
    public List<NameGroup> GeneralsTier2 { get; set; }
    public List<NameGroup> GeneralsTier3 { get; set; }

    // Deity categories
    public List<NameGroup> PowerDeities { get; set; }
    public List<NameGroup> CreationDeities { get; set; }
    public List<NameGroup> DestructionDeities { get; set; }
    public List<NameGroup> PeaceDeities { get; set; }
    public List<NameGroup> WarDeities { get; set; }
    public List<NameGroup> VictoryDeities { get; set; }
    public List<NameGroup> DeathDeities { get; set; }
    public List<NameGroup> HatredDeities { get; set; }
    public List<NameGroup> FearDeities { get; set; }
    public List<NameGroup> SorrowDeities { get; set; }
    public List<NameGroup> BeastsDeities { get; set; }
    public List<NameGroup> DarknessDeities { get; set; }
    public List<NameGroup> LightDeities { get; set; }
    public List<NameGroup> SunDeities { get; set; }
    public List<NameGroup> SkyDeities { get; set; }
    public List<NameGroup> AirDeities { get; set; }
    public List<NameGroup> ColdDeities { get; set; }
    public List<NameGroup> WarmthDeities { get; set; }
    public List<NameGroup> WaterDeities { get; set; }
    public List<NameGroup> NatureDeities { get; set; }
    public List<NameGroup> HealthDeities { get; set; }
    public List<NameGroup> LoveDeities { get; set; }
    public List<NameGroup> ArtDeities { get; set; }
    public List<NameGroup> FeastDeities { get; set; }
    public List<NameGroup> FortuneDeities { get; set; }
    public List<NameGroup> SleepDeities { get; set; }
    public List<NameGroup> TimeDeities { get; set; }
    public List<NameGroup> ProphecyDeities { get; set; }
    public List<NameGroup> JusticeDeities { get; set; }
    public List<NameGroup> ProtectionDeities { get; set; }
    public List<NameGroup> PunishmentDeities { get; set; }
    public List<NameGroup> LoyaltyDeities { get; set; }
    public List<NameGroup> DisloyaltyDeities { get; set; }
    public List<NameGroup> KnowledgeDeities { get; set; }
    public List<NameGroup> LabourDeities { get; set; }
    public List<NameGroup> OtherDeities { get; set; }

    // Concatenation properties
    public IEnumerable<NameGroup> AllLeaders => LeadersTier1.Concat(LeadersTier2).Concat(LeadersTier3);
    public IEnumerable<NameGroup> AllGenerals => GeneralsTier1.Concat(GeneralsTier2).Concat(GeneralsTier3);
    public IEnumerable<NameGroup> AllDeities => PowerDeities.Concat(CreationDeities).Concat(DestructionDeities).Concat(PeaceDeities).Concat(WarDeities).Concat(VictoryDeities).Concat(DeathDeities).Concat(HatredDeities).Concat(FearDeities).Concat(SorrowDeities).Concat(BeastsDeities).Concat(DarknessDeities).Concat(LightDeities).Concat(SunDeities).Concat(SkyDeities).Concat(AirDeities).Concat(ColdDeities).Concat(WarmthDeities).Concat(WaterDeities).Concat(NatureDeities).Concat(HealthDeities).Concat(LoveDeities).Concat(ArtDeities).Concat(FeastDeities).Concat(FortuneDeities).Concat(SleepDeities).Concat(TimeDeities).Concat(ProphecyDeities).Concat(JusticeDeities).Concat(ProtectionDeities).Concat(PunishmentDeities).Concat(LoyaltyDeities).Concat(DisloyaltyDeities).Concat(KnowledgeDeities).Concat(LabourDeities).Concat(OtherDeities);
}
```

### `CompanyNames`

```csharp
public sealed class CompanyNames
{
    public List<NameGroup> RobotManufacturers { get; set; }
    public List<NameGroup> AutomotiveManufacturers { get; set; }
    public List<NameGroup> AircraftManufacturers { get; set; }
    public List<NameGroup> SpacecraftManufacturers { get; set; }
    public List<NameGroup> WeaponManufacturers { get; set; }
    public List<NameGroup> RocketDesigners { get; set; }
    public List<NameGroup> ResearchCompanies { get; set; }
    public List<NameGroup> InvestmentCompanies { get; set; }
}
```

### `WarfareNames`

```csharp
public sealed class WarfareNames
{
    public WeaponNames Weapons { get; set; }
    public List<NameGroup> MilitaryUnitTypes { get; set; }
    public List<NameGroup> ShipTypes { get; set; }
    public List<NameGroup> Forts { get; set; }
    public List<NameGroup> BattleLocations { get; set; }
}
```

### `WeaponNames`

```csharp
public sealed class WeaponNames
{
    public List<NameGroup> All { get; set; }
    public List<NameGroup> Ranged { get; set; }
    public List<NameGroup> Artillery { get; set; }
}
```

### `BiosphereNames`

```csharp
public sealed class BiosphereNames
{
    public List<NameGroup> Animals { get; set; }
    public List<NameGroup> MythologicalCreatures { get; set; }
}
```

### `ShipNames`

```csharp
public sealed class ShipNames
{
    public List<NameGroup> Generic { get; set; }
    public List<NameGroup> Corvette { get; set; }
    public List<NameGroup> Destroyer { get; set; }
    public List<NameGroup> Cruiser { get; set; }
    public List<NameGroup> Battleship { get; set; }
    public List<NameGroup> Titan { get; set; }
    public List<NameGroup> Colossus { get; set; }
    public List<NameGroup> Juggernaut { get; set; }
    public List<NameGroup> Constructor { get; set; }
    public List<NameGroup> Science { get; set; }
    public List<NameGroup> Coloniser { get; set; }
    public List<NameGroup> Transport { get; set; }
    public List<NameGroup> IonCannon { get; set; }
}
```

### `ShipClassNames`

```csharp
public sealed class ShipClassNames
{
    public List<NameGroup> Generic { get; set; }
    public List<NameGroup> Corvette { get; set; }
    public List<NameGroup> Destroyer { get; set; }
    public List<NameGroup> Cruiser { get; set; }
    public List<NameGroup> Battleship { get; set; }
    public List<NameGroup> Titan { get; set; }
    public List<NameGroup> Colossus { get; set; }
    public List<NameGroup> Juggernaut { get; set; }
    public List<NameGroup> Constructor { get; set; }
    public List<NameGroup> Science { get; set; }
    public List<NameGroup> Coloniser { get; set; }
    public List<NameGroup> Transport { get; set; }
    public List<NameGroup> IonCannon { get; set; }
}
```

### `StationNames`

```csharp
public sealed class StationNames
{
    public MilitaryStationNames MilitaryStations { get; set; }
    public List<NameGroup> MiningStations { get; set; }
    public List<NameGroup> ResearchStations { get; set; }
    public List<NameGroup> ObservationStations { get; set; }
}
```

### `MilitaryStationNames`

```csharp
public sealed class MilitaryStationNames
{
    public List<NameGroup> Generic { get; set; }
    public List<NameGroup> Small { get; set; }
    public List<NameGroup> Medium { get; set; }
    public List<NameGroup> Large { get; set; }
}
```

### `StationClassNames`

```csharp
public sealed class StationClassNames
{
    public MilitaryStationNames MilitaryStations { get; set; }
    public List<NameGroup> MiningStations { get; set; }
    public List<NameGroup> ResearchStations { get; set; }
    public List<NameGroup> ObservationStations { get; set; }
}
```

### `ArmyNames`

```csharp
public sealed class ArmyNames
{
    // Sequential name templates (used when no explicit names)
    public string FleetSequentialName { get; set; } = "%O% Fleet";
    public string DefenceArmySequentialName { get; set; } = "%O% Planetary Guard";
    public string AssaultArmySequentialName { get; set; } = "%O% Expeditionary Force";
    public string OccupationArmySequentialName { get; set; } = "%O% Garrison Force";
    public string SlaveArmySequentialName { get; set; } = "%O% Indentured Rifles";
    public string CloneArmySequentialName { get; set; } = "%O% Clone Army";
    public string PerfectedCloneArmySequentialName { get; set; } = "%O% Clone Army";
    public string UndeadArmySequentialName { get; set; } = "%O% Undead Army";
    public string RoboticDefenceArmySequentialName { get; set; } = "%O% Ground Defence Matrix";
    public string RoboticAssaultArmySequentialName { get; set; } = "%O% Hunter-Killer Group";
    public string RoboticOccupationArmySequentialName { get; set; } = "%O% Mechanised Garrison";
    public string AndroidDefenceArmySequentialName { get; set; } = "%O% Synthetic Sentinels";
    public string AndroidAssaultArmySequentialName { get; set; } = "%O% Synthetic Rangers";
    public string PsionicArmySequentialName { get; set; } = "%O% Psi Commando";
    public string XenomorphArmySequentialName { get; set; } = "%O% Bio-Warfare Division";
    public string SuperSoldierArmySequentialName { get; set; } = "%O% Bio-Engineered Squadron";
    public string PrimitiveArmySequentialName { get; set; } = "Primitive Army %C%";
    public string IndustrialArmySequentialName { get; set; } = "Industrial Army %C%";
    public string PostAtomicArmySequentialName { get; set; } = "Post-Atomic Army %C%";

    // Explicit name groups
    public List<NameGroup> Fleet { get; set; }
    public List<NameGroup> DefenceArmy { get; set; }
    public List<NameGroup> AssaultArmy { get; set; }
    public List<NameGroup> OccupationArmy { get; set; }
    public List<NameGroup> SlaveArmy { get; set; }
    public List<NameGroup> CloneArmy { get; set; }
    public List<NameGroup> PerfectedCloneArmy { get; set; }
    public List<NameGroup> UndeadArmy { get; set; }
    public List<NameGroup> RoboticDefenceArmy { get; set; }
    public List<NameGroup> RoboticAssaultArmy { get; set; }
    public List<NameGroup> RoboticOccupationArmy { get; set; }
    public List<NameGroup> AndroidDefenceArmy { get; set; }
    public List<NameGroup> AndroidAssaultArmy { get; set; }
    public List<NameGroup> PsionicArmy { get; set; }
    public List<NameGroup> XenomorphArmy { get; set; }
    public List<NameGroup> SuperSoldierArmy { get; set; }
    public List<NameGroup> PrimitiveArmy { get; set; }
    public List<NameGroup> IndustrialArmy { get; set; }
    public List<NameGroup> PostAtomicArmy { get; set; }
}
```

### `PlanetNames`

```csharp
public sealed class PlanetNames
{
    public List<NameGroup> Generic { get; set; }
    public List<NameGroup> Desert { get; set; }
    public List<NameGroup> Arid { get; set; }
    public List<NameGroup> Tropical { get; set; }
    public List<NameGroup> Continental { get; set; }
    public List<NameGroup> Gaia { get; set; }
    public List<NameGroup> Ocean { get; set; }
    public List<NameGroup> Tundra { get; set; }
    public List<NameGroup> Arctic { get; set; }
    public List<NameGroup> Tomb { get; set; }
    public List<NameGroup> Savannah { get; set; }
    public List<NameGroup> Alpine { get; set; }
    public List<NameGroup> Molten { get; set; }
    public List<NameGroup> Barren { get; set; }
    public List<NameGroup> Asteroid { get; set; }
}
```

### `CharacterNames`

```csharp
public sealed class CharacterNames
{
    public string Id { get; set; }
    public int Weight { get; set; }

    public List<NameGroup> FullNames { get; set; }
    public List<NameGroup> FirstNames { get; set; }
    public List<NameGroup> RoyalFirstNames { get; set; }

    public List<NameGroup> MaleFullNames { get; set; }
    public List<NameGroup> MaleFirstNames { get; set; }
    public List<NameGroup> MaleRoyalFirstNames { get; set; }

    public List<NameGroup> FemaleFullNames { get; set; }
    public List<NameGroup> FemaleFirstNames { get; set; }
    public List<NameGroup> FemaleRoyalFirstNames { get; set; }

    public List<NameGroup> SecondNames { get; set; }
    public List<NameGroup> RoyalSecondNames { get; set; }

    public bool IsEmpty => FullNames.All(x => x.IsEmpty) &&
        FirstNames.All(x => x.IsEmpty) &&
        RoyalFirstNames.All(x => x.IsEmpty) &&
        MaleFullNames.All(x => x.IsEmpty) &&
        MaleFirstNames.All(x => x.IsEmpty) &&
        MaleRoyalFirstNames.All(x => x.IsEmpty) &&
        FemaleFullNames.All(x => x.IsEmpty) &&
        FemaleFirstNames.All(x => x.IsEmpty) &&
        FemaleRoyalFirstNames.All(x => x.IsEmpty) &&
        SecondNames.All(x => x.IsEmpty) &&
        RoyalSecondNames.All(x => x.IsEmpty);
}
```

## Relationships

```
NameList (root)
├── Denonyms: List<NameGroup>
├── Places: PlaceNames
│   ├── Countries, Regions, Cities, Mountains, Forests, Deserts, Rivers, Lakes, Seas, Airports
│   ├── WaterBodies (concat): Rivers + Lakes + Seas
│   └── GeographicalPlaces (concat): Mountains + Forests + Deserts + Rivers + Lakes + Seas
├── GreatPeople: GreatPeople
│   ├── Historical figures (Explorers, Pioneers, Scientists, Leaders, FlyingAces, Heroes, Admirals, Generals)
│   ├── Deity categories (Power, Creation, Destruction, Peace, War, Victory, Death, Hatred, Fear, Sorrow, Beasts, Darkness, Light, Sun, Sky, Air, Cold, Warmth, Water, Nature, Health, Love, Art, Feast, Fortune, Sleep, Time, Prophecy, Justice, Protection, Punishment, Loyalty, Disloyalty, Knowledge, Labour, Other)
│   ├── AllLeaders (concat): LeadersTier1 + LeadersTier2 + LeadersTier3
│   ├── AllGenerals (concat): GeneralsTier1 + GeneralsTier2 + GeneralsTier3
│   └── AllDeities (concat): All deity categories
├── Companies: CompanyNames
│   ├── RobotManufacturers, AutomotiveManufacturers, AircraftManufacturers, SpacecraftManufacturers
│   ├── WeaponManufacturers, RocketDesigners, ResearchCompanies, InvestmentCompanies
├── Warfare: WarfareNames
│   ├── Weapons: WeaponNames (All, Ranged, Artillery)
│   ├── MilitaryUnitTypes, ShipTypes, Forts, BattleLocations
├── BiosphereNames: BiosphereNames
│   ├── Animals, MythologicalCreatures
├── Ships: ShipNames
│   ├── Generic, Corvette, Destroyer, Cruiser, Battleship, Titan, Colossus, Juggernaut, Constructor, Science, Coloniser, Transport, IonCannon
├── ShipClasses: ShipClassNames
│   ├── Same structure as ShipNames
├── Stations: StationNames
│   ├── MilitaryStations: MilitaryStationNames (Generic, Small, Medium, Large)
│   ├── MiningStations, ResearchStations, ObservationStations
├── StationClasses: StationClassNames
│   ├── Same structure as StationNames
├── Armies: ArmyNames
│   ├── Sequential templates (Fleet, Defence, Assault, Occupation, Slave, Clone, PerfectedClone, Undead, RoboticDefence, RoboticAssault, RoboticOccupation, AndroidDefence, AndroidAssault, Psionic, Xenomorph, SuperSoldier, Primitive, Industrial, PostAtomic)
│   └── Explicit name groups for each army type
├── Planets: PlanetNames
│   ├── Generic, Desert, Arid, Tropical, Continental, Gaia, Ocean, Tundra, Arctic, Tomb, Savannah, Alpine, Molten, Barren, Asteroid
└── Characters: List<CharacterNames>
    ├── Id, Weight
    ├── FullNames, FirstNames, RoyalFirstNames
    ├── MaleFullNames, MaleFirstNames, MaleRoyalFirstNames
    ├── FemaleFullNames, FemaleFirstNames, FemaleRoyalFirstNames
    ├── SecondNames, RoyalSecondNames
    └── IsEmpty (computed)
```

## Serialization

All models are XML-serializable via `NuciDAL`. Requirements:
- Parameterless constructors (all models have them)
- Public get/set properties
- Collections initialized to empty lists (not null)

The `NameGroup.Values` property is computed and not serialized directly; only `ExplicitValues` and `Url` are persisted.

## Merging Behavior

`NameList.AddRange(NameList other)` concatenates all collections:
- `Denonyms`: `this.Denonyms.AddRange(other.Denonyms)`
- `Places`: Each subcategory concatenated
- `GreatPeople`: Each subcategory concatenated
- `Companies`: Each subcategory concatenated
- `Warfare`: Each subcategory concatenated
- `BiosphereNames`: Each subcategory concatenated
- `Ships`: Each subcategory concatenated
- `ShipClasses`: Each subcategory concatenated
- `Stations`: Each subcategory concatenated
- `StationClasses`: Each subcategory concatenated
- `Armies`: Each subcategory concatenated
- `Planets`: Each subcategory concatenated
- `Characters`: `this.Characters.AddRange(other.Characters)`

No deduplication occurs during merge; deduplication happens later in `NamesBuilder.GetFormattedNameCollection()`.