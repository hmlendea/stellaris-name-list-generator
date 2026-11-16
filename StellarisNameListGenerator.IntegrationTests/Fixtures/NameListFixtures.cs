using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests.Fixtures
{
    /// <summary>
    /// Supplies the canonical input documents used by the integration tests.
    /// </summary>
    internal static class NameListFixtures
    {
        internal static string RichRecordId => "nucilandia";

        internal static string RichRecordName => "Nucilandia";

        internal static string DiacriticName => "Çupișan";

        internal static string TransliteratedDiacriticName => "Çupisan";

        internal static string DuplicateDiacriticName => "Liliana";

        internal static string DuplicateDiacriticSourceName => "Lílíana";

        internal static string SpacedName => "Ionuț Karr";

        internal static string TransliteratedSpacedName => "\"Ionut Karr\"";

        internal static string CountryOne => "Astora";

        internal static string CountryTwo => "Nucilandia";

        internal static string CountryThree => "Romania";

        internal static string CityOne => "Cluj-Napoca";

        internal static string CityTwo => "Oradea";

        internal static string DesertName => "Wahiba Sands";

        internal static string SeaName => "Pontic Sea";

        internal static string WeaponName => "Shotel";

        internal static string ArtilleryName => "Ballista";

        internal static string MilitaryUnitName => "Mehal Sefari";

        internal static string AnimalName => "Wyvern";

        internal static string MythologicalCreatureName => "Griffin";

        internal static string LeaderName => "Bobert";

        internal static string GenericShipName => "Zephyr";

        internal static string CorvetteShipName => "Wasp";

        internal static string ShipClassName => "NuciSoft Yard";

        internal static string FleetName => "Home Fleet";

        internal static string PlanetName => "Solara";

        internal static string DesertPlanetName => "Dune";

        internal static string FemaleFirstName => "Elodia";

        internal static string MaleFirstName => "Bobert";

        internal static string HeavyCharacterFirstName => "Ioana";

        internal static string SecondSurname => "Pintilie";

        internal static string ThirdSurname => "Karr";

        internal static string DeathDeityName => "Mors";

        internal static string HeavyCharacterId => "beta";

        internal static string LightCharacterId => "alpha";

        internal static int HeavyCharacterWeight => 873;

        internal static int LightCharacterWeight => 42;

        internal static string EmptyDocument => NameListXml.Document();

        internal static string MinimalDocument => NameListXml.Document(MinimalRecord());

        internal static string RichDocument => NameListXml.Document(RichRecord());

        internal static string MergedDocument => NameListXml.Document(RichRecord(), AdditionalRecord());

        internal static string DiacriticDocument => NameListXml.Document(DiacriticRecord());

        internal static string CharacterDocument => NameListXml.Document(CharacterRecord());

        internal static string EmptyCharacterDocument => NameListXml.Document(EmptyCharacterRecord());

        internal static string DuplicateCharacterIdDocument => NameListXml.Document(DuplicateCharacterIdRecord());

        internal static string UnknownElementDocument => NameListXml.Document(UnknownElementRecord());

        internal static string MalformedDocument => "<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOfNameList><NameList>";

        internal static string UrlDocument(string url) => NameListXml.Document(UrlRecord(url));

        private static string MinimalRecord()
            => NameListXml.Record($"<Id>{RichRecordId}</Id>");

        private static string AdditionalRecord()
            => NameListXml.Record(
                "<Id>additional</Id>",
                NameListXml.Section("Places", NameListXml.Section("Countries", NameListXml.Group("Countries", CountryThree))),
                NameListXml.Section("Denonyms", NameListXml.Group("Denonyms", CountryTwo)));

        private static string RichRecord()
            => NameListXml.Record(
                $"<Id>{RichRecordId}</Id>",
                NameListXml.Section("Denonyms", NameListXml.Group("Denonyms", CountryTwo)),
                NameListXml.Section("Places", BuildPlaces()),
                NameListXml.Section("GreatPeople", BuildGreatPeople()),
                NameListXml.Section("Companies", NameListXml.Section("ResearchCompanies", NameListXml.Group("Research companies", "Nucilandian Softworks"))),
                NameListXml.Section("Warfare", BuildWarfare()),
                NameListXml.Section("BiosphereNames", BuildBiosphere()),
                NameListXml.Section("Ships", BuildShips()),
                NameListXml.Section("ShipClasses", NameListXml.Section("Generic", NameListXml.Group("Spacecraft manufacturers", ShipClassName))),
                NameListXml.Section("Armies", NameListXml.Section("Fleet", NameListXml.Group("Fleet", FleetName))),
                NameListXml.Section("Planets", BuildPlanets()),
                NameListXml.Section("Characters", BuildCharacters()));

        private static string BuildGreatPeople()
            => string.Concat(
                NameListXml.Section("LeadersTier1", NameListXml.Group("Leaders", LeaderName)),
                NameListXml.Section("DeathDeities", NameListXml.Group("Deities", DeathDeityName)));

        private static string BuildPlaces()
            => string.Concat(
                NameListXml.Section("Countries", NameListXml.Group("Countries", CountryTwo, CountryOne)),
                NameListXml.Section("Regions", NameListXml.Group("Regions", "Talmeria")),
                NameListXml.Section("Cities", NameListXml.Group("Cities", CityOne, CityTwo)),
                NameListXml.Section("Mountains", NameListXml.Group("Mountains", "Dacian Peak")),
                NameListXml.Section("Deserts", NameListXml.Group("Deserts", DesertName)),
                NameListXml.Section("Seas", NameListXml.Group("Seas", SeaName)));

        private static string BuildWarfare()
            => string.Concat(
                NameListXml.Section("Weapons", string.Concat(
                    NameListXml.Section("Swords", NameListXml.Group("Swords", WeaponName)),
                    NameListXml.Section("Artillery", NameListXml.Group("Artillery", ArtilleryName)))),
                NameListXml.Section("MilitaryUnitTypes", NameListXml.Group("Military unit types", MilitaryUnitName)),
                NameListXml.Section("BattleLocations", NameListXml.Group("Battle locations", "Fields of Blood")));

        private static string BuildBiosphere()
            => string.Concat(
                NameListXml.Section("Animals", NameListXml.Group("Animals", AnimalName)),
                NameListXml.Section("MythologicalCreatures", NameListXml.Group("Mythological creatures", MythologicalCreatureName)));

        private static string BuildShips()
            => string.Concat(
                NameListXml.Section("Generic", NameListXml.Group("Generic", GenericShipName)),
                NameListXml.Section("Corvette", NameListXml.Group("Corvette", CorvetteShipName)));

        private static string BuildPlanets()
            => string.Concat(
                NameListXml.Section("Generic", NameListXml.Group("Generic", PlanetName)),
                NameListXml.Section("Desert", NameListXml.Group("Desert", DesertPlanetName)));

        private static string BuildCharacters()
            => string.Concat(
                NameListXml.Character(
                    HeavyCharacterId,
                    HeavyCharacterWeight,
                    NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", FemaleFirstName)),
                    NameListXml.Section("SecondNames", NameListXml.Group("Second names", SecondSurname))),
                NameListXml.Character(
                    LightCharacterId,
                    LightCharacterWeight,
                    NameListXml.Section("MaleFirstNames", NameListXml.Group("Male first names", MaleFirstName)),
                    NameListXml.Section("SecondNames", NameListXml.Group("Second names", ThirdSurname))));

        private static string DiacriticRecord()
            => NameListXml.Record(
                "<Id>diacritics</Id>",
                NameListXml.Section("Denonyms", NameListXml.Group(
                    "Denonyms",
                    DiacriticName,
                    DuplicateDiacriticSourceName,
                    DuplicateDiacriticName,
                    SpacedName)));

        private static string CharacterRecord()
            => NameListXml.Record(
                "<Id>characters</Id>",
                NameListXml.Section("Characters", BuildWeightedCharacters()));

        private static string BuildWeightedCharacters()
            => string.Concat(
                NameListXml.Character(
                    LightCharacterId,
                    LightCharacterWeight,
                    NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", FemaleFirstName)),
                    NameListXml.Section("SecondNames", NameListXml.Group("Second names", SecondSurname))),
                NameListXml.Character(
                    HeavyCharacterId,
                    HeavyCharacterWeight,
                    NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", HeavyCharacterFirstName)),
                    NameListXml.Section("SecondNames", NameListXml.Group("Second names", ThirdSurname))));

        private static string EmptyCharacterRecord()
            => NameListXml.Record(
                "<Id>characters</Id>",
                NameListXml.Section("Characters", string.Concat(
                    NameListXml.Character("empty", LightCharacterWeight),
                    NameListXml.Character(
                        "populated",
                        HeavyCharacterWeight,
                        NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", FemaleFirstName))))));

        private static string DuplicateCharacterIdRecord()
            => NameListXml.Record(
                "<Id>characters</Id>",
                NameListXml.Section("Characters", string.Concat(
                    NameListXml.Character(
                        LightCharacterId,
                        LightCharacterWeight,
                        NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", FemaleFirstName))),
                    NameListXml.Character(
                        LightCharacterId,
                        HeavyCharacterWeight,
                        NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", "Ioana"))))));

        private static string UnknownElementRecord()
            => NameListXml.Record(
                "<Id>unknown</Id>",
                "<UnmappedSection><UnmappedValue>ignored</UnmappedValue></UnmappedSection>",
                NameListXml.Section("Denonyms", NameListXml.Group("Denonyms", CountryTwo)));

        private static string UrlRecord(string url)
            => NameListXml.Record(
                "<Id>remote</Id>",
                NameListXml.Section("Denonyms", NameListXml.GroupWithUrl("Denonyms", url, CountryTwo)));
    }
}
