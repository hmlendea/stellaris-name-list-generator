using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Asserts the exact source composition of every generated block. The expected sets were captured by
    /// running the generator over a document in which each source category carries a unique traceable
    /// token, then reading back which tokens reached each block.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class BlockCompositionTests
    {
        private const string OutputFileName = "composition.txt";
        private const string DisplayName = "Composition";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(BlockCompositionTests));
            environment.WriteInput(CoverageDocument.Build());

            environment.Generate(OutputFileName, DisplayName, isLocked: true);
            output = GeneratedNameList.Parse(environment.ReadText(OutputFileName));
        }

        [TestCaseSource(nameof(CompositionCases))]
        public void GivenEverySourceCategory_WhenGenerating_ThenTheBlockContainsExactlyItsMappedSources(string block, string expectedSources)
        {
            (string section, string subBlock) = Resolve(block);

            HashSet<string> expected = [.. expectedSources.Split('|').Select(CoverageDocument.Token)];
            HashSet<string> actual = CollectTokens(output.GetBlockLines(section, subBlock));

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenTheGenericShipCategoryExcludesSpecialisedSources()
        {
            HashSet<string> actual = CollectTokens(output.GetBlockLines("ship_names", "generic"));

            Assert.That(actual, Is.EquivalentTo(new[] { CoverageDocument.Token("Denonyms"), CoverageDocument.Token("Ships.Generic") }));
        }

        [Test]
        public void GivenOnlyJuggernautValues_WhenGenerating_ThenTheJuggernautCategoryHasNoGeneratedAdditions()
        {
            HashSet<string> actual = CollectTokens(output.GetBlockLines("ship_names", "juggernaut"));

            Assert.That(actual, Is.EquivalentTo(new[] { CoverageDocument.Token("Ships.Juggernaut") }));
        }

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenArmyBlocksWithoutValuesFallBackToSequentialTemplates()
        {
            foreach (string block in new[] { "machine_defense", "machine_assault_1", "machine_assault_2", "machine_assault_3" })
            {
                Assert.That(output.SubBlockContains("army_names", block, "sequential_name"), Is.True, block);
            }
        }

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenArmyBlocksWithValuesDoNotFallBack()
        {
            foreach (string block in new[] { "industrial_army", "postatomic_army", "defense_army", "psionic_army" })
            {
                Assert.That(output.SubBlockContains("army_names", block, "sequential_name"), Is.False, block);
            }
        }

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenEveryMappedBlockIsEmitted()
        {
            List<string> emitted = [];

            foreach (string section in new[] { "ship_names", "ship_class_names", "planet_names", "army_names" })
            {
                emitted.AddRange(BlocksIn(output.GetBlockLines(section)).Select(name => $"{section}:{name}"));
            }

            Assert.That(emitted, Has.Count.EqualTo(77));
        }

        private static (string Section, string SubBlock) Resolve(string block)
        {
            string[] parts = block.Split(':');

            return parts[0] switch
            {
                "ship" => ("ship_names", parts[1]),
                "class" => ("ship_class_names", parts[1]),
                "planet" => ("planet_names", parts[1]),
                _ => ("army_names", parts[1]),
            };
        }

        private static HashSet<string> CollectTokens(IReadOnlyList<string> lines)
        {
            HashSet<string> tokens = [];
            string joined = string.Join('\n', lines);

            foreach (string path in CoverageDocument.AllPaths)
            {
                string token = CoverageDocument.Token(path);

                if (joined.Contains(token, StringComparison.Ordinal))
                {
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        private static IEnumerable<string> BlocksIn(IReadOnlyList<string> lines)
        {
            foreach (string line in lines)
            {
                if (!line.StartsWith("        ", StringComparison.Ordinal) ||
                    line.StartsWith("            ", StringComparison.Ordinal))
                {
                    continue;
                }

                string trimmed = line.Trim();

                if (trimmed.EndsWith("= {", StringComparison.Ordinal))
                {
                    yield return trimmed[..trimmed.IndexOf(" = {", StringComparison.Ordinal)];
                }
            }
        }

        private static readonly object[][] CompositionCases =
        [
            new object[] { "army:android_army", "Armies.AndroidAssaultArmy" },
            new object[] { "army:android_defense_army", "Armies.AndroidDefenceArmy" },
            new object[] { "army:assault_army", "Armies.AssaultArmy" },
            new object[] { "army:clone_army", "Armies.CloneArmy" },
            new object[] { "army:defense_army", "Armies.DefenceArmy" },
            new object[] { "army:gene_warrior_army", "Armies.SuperSoldierArmy" },
            new object[] { "army:industrial_army", "Armies.IndustrialArmy" },
            new object[] { "army:occupation_army", "Armies.OccupationArmy" },
            new object[] { "army:perfected_clone_army", "Armies.PerfectedCloneArmy" },
            new object[] { "army:postatomic_army", "Armies.PostAtomicArmy" },
            new object[] { "army:primitive_army", "Armies.PrimitiveArmy" },
            new object[] { "army:psionic_army", "Armies.PsionicArmy|Biosphere.MythologicalCreatures" },
            new object[] { "army:robotic_army", "Armies.RoboticAssaultArmy" },
            new object[] { "army:robotic_defense_army", "Armies.RoboticDefenceArmy" },
            new object[] { "army:robotic_occupation_army", "Armies.RoboticOccupationArmy" },
            new object[] { "army:slave_army", "Armies.SlaveArmy" },
            new object[] { "army:undead_army", "Armies.UndeadArmy" },
            new object[] { "army:xenomorph_army", "Armies.XenomorphArmy|GreatPeople.BeastsDeities|GreatPeople.DarknessDeities|GreatPeople.DeathDeities|GreatPeople.FearDeities|GreatPeople.HatredDeities|GreatPeople.SorrowDeities" },
            new object[] { "class:battleship", "Companies.WeaponManufacturers|ShipClasses.Battleship" },
            new object[] { "class:coloniser", "Companies.AircraftManufacturers|Companies.AutomotiveManufacturers|ShipClasses.Coloniser" },
            new object[] { "class:colossus", "Companies.WeaponManufacturers|ShipClasses.Colossus" },
            new object[] { "class:constructor", "Companies.AircraftManufacturers|Companies.AutomotiveManufacturers|ShipClasses.Constructor" },
            new object[] { "class:corvette", "Companies.WeaponManufacturers|ShipClasses.Corvette" },
            new object[] { "class:cruiser", "Companies.WeaponManufacturers|ShipClasses.Cruiser" },
            new object[] { "class:destroyer", "Companies.WeaponManufacturers|ShipClasses.Destroyer" },
            new object[] { "class:generic", "Companies.AircraftManufacturers|Companies.RocketDesigners|Companies.SpacecraftManufacturers|Denonyms|ShipClasses.Generic|ShipTypes" },
            new object[] { "class:ion_cannon", "ShipClasses.IonCannon|Weapons.Artillery" },
            new object[] { "class:juggernaut", "Companies.WeaponManufacturers|ShipClasses.Juggernaut" },
            new object[] { "class:military_station_large", "StationClasses.MilitaryStations.Generic|StationClasses.MilitaryStations.Large" },
            new object[] { "class:military_station_medium", "StationClasses.MilitaryStations.Generic|StationClasses.MilitaryStations.Medium" },
            new object[] { "class:military_station_small", "StationClasses.MilitaryStations.Generic|StationClasses.MilitaryStations.Small" },
            new object[] { "class:mining_station", "StationClasses.MiningStations" },
            new object[] { "class:observation_station", "StationClasses.ObservationStations" },
            new object[] { "class:research_station", "StationClasses.ResearchStations" },
            new object[] { "class:science", "Companies.ResearchCompanies|ShipClasses.Science" },
            new object[] { "class:sponsored_coloniser", "Companies.AircraftManufacturers|Companies.AutomotiveManufacturers|Companies.InvestmentCompanies|ShipClasses.Coloniser" },
            new object[] { "class:titan", "Companies.WeaponManufacturers|ShipClasses.Titan" },
            new object[] { "class:transport", "Companies.AircraftManufacturers|Companies.AutomotiveManufacturers|ShipClasses.Coloniser" },
            new object[] { "planet:generic", "GreatPeople.AirDeities|GreatPeople.JusticeDeities|GreatPeople.KnowledgeDeities|GreatPeople.LightDeities|GreatPeople.LoyaltyDeities|GreatPeople.OtherDeities|GreatPeople.PowerDeities|GreatPeople.ProphecyDeities|GreatPeople.SkyDeities|GreatPeople.SleepDeities|GreatPeople.TimeDeities|Planets.Generic" },
            new object[] { "planet:pc_alpine", "GreatPeople.ColdDeities|GreatPeople.PunishmentDeities|Places.Mountains|Planets.Alpine" },
            new object[] { "planet:pc_arctic", "GreatPeople.ColdDeities|GreatPeople.DisloyaltyDeities|GreatPeople.PunishmentDeities|Planets.Arctic" },
            new object[] { "planet:pc_arid", "GreatPeople.SunDeities|GreatPeople.WarmthDeities|Places.Deserts|Planets.Arid" },
            new object[] { "planet:pc_asteroid", "GreatPeople.DarknessDeities|GreatPeople.DisloyaltyDeities|GreatPeople.FearDeities|GreatPeople.HatredDeities|GreatPeople.PunishmentDeities|GreatPeople.SorrowDeities|Planets.Asteroid" },
            new object[] { "planet:pc_barren", "GreatPeople.DarknessDeities|GreatPeople.DeathDeities|GreatPeople.DisloyaltyDeities|GreatPeople.FearDeities|GreatPeople.HatredDeities|GreatPeople.PunishmentDeities|GreatPeople.SorrowDeities|Planets.Barren" },
            new object[] { "planet:pc_continental", "GreatPeople.PeaceDeities|GreatPeople.ProtectionDeities|GreatPeople.VictoryDeities|Planets.Continental" },
            new object[] { "planet:pc_desert", "GreatPeople.DisloyaltyDeities|GreatPeople.PunishmentDeities|GreatPeople.SunDeities|GreatPeople.WarmthDeities|Places.Deserts|Planets.Desert" },
            new object[] { "planet:pc_gaia", "GreatPeople.ArtDeities|GreatPeople.CreationDeities|GreatPeople.FeastDeities|GreatPeople.FortuneDeities|GreatPeople.HealthDeities|GreatPeople.LoveDeities|GreatPeople.NatureDeities|GreatPeople.PeaceDeities|GreatPeople.ProtectionDeities|GreatPeople.VictoryDeities|Planets.Gaia" },
            new object[] { "planet:pc_molten", "GreatPeople.DestructionDeities|GreatPeople.DisloyaltyDeities|GreatPeople.LabourDeities|GreatPeople.PunishmentDeities|GreatPeople.WarDeities|GreatPeople.WarmthDeities|Planets.Molten" },
            new object[] { "planet:pc_nuked", "BattleLocations|GreatPeople.BeastsDeities|GreatPeople.DarknessDeities|GreatPeople.DeathDeities|GreatPeople.DestructionDeities|GreatPeople.DisloyaltyDeities|GreatPeople.FearDeities|GreatPeople.HatredDeities|GreatPeople.PunishmentDeities|GreatPeople.SorrowDeities|GreatPeople.WarDeities|Planets.Tomb" },
            new object[] { "planet:pc_ocean", "GreatPeople.WaterDeities|Places.Lakes|Places.Rivers|Places.Seas|Planets.Ocean" },
            new object[] { "planet:pc_savannah", "GreatPeople.SunDeities|GreatPeople.WarmthDeities|Planets.Savannah" },
            new object[] { "planet:pc_tropical", "GreatPeople.NatureDeities|Places.Forests|Planets.Tropical" },
            new object[] { "planet:pc_tundra", "GreatPeople.ColdDeities|Planets.Tundra" },
            new object[] { "ship:battleship", "BattleLocations|Biosphere.MythologicalCreatures|GreatPeople.Admirals|GreatPeople.AirDeities|GreatPeople.ColdDeities|GreatPeople.DarknessDeities|GreatPeople.DisloyaltyDeities|GreatPeople.FearDeities|GreatPeople.GeneralsTier3|GreatPeople.HatredDeities|GreatPeople.JusticeDeities|GreatPeople.LeadersTier3|GreatPeople.LightDeities|GreatPeople.LoyaltyDeities|GreatPeople.OtherDeities|GreatPeople.PowerDeities|GreatPeople.ProphecyDeities|GreatPeople.ProtectionDeities|GreatPeople.PunishmentDeities|GreatPeople.SkyDeities|GreatPeople.SleepDeities|GreatPeople.SorrowDeities|GreatPeople.SunDeities|GreatPeople.TimeDeities|GreatPeople.WarDeities|GreatPeople.WarmthDeities|GreatPeople.WaterDeities|MilitaryUnitTypes|Places.Countries|Ships.Battleship" },
            new object[] { "ship:colonizer", "GreatPeople.FeastDeities|GreatPeople.FortuneDeities|GreatPeople.HealthDeities|GreatPeople.LoveDeities|GreatPeople.PeaceDeities|GreatPeople.SleepDeities|Places.Cities|Places.Countries|Places.Deserts|Places.Forests|Places.Lakes|Places.Mountains|Places.Regions|Places.Rivers|Places.Seas|Ships.Coloniser" },
            new object[] { "ship:colossus", "GreatPeople.CreationDeities|GreatPeople.DeathDeities|GreatPeople.DestructionDeities|GreatPeople.PowerDeities|GreatPeople.SunDeities|GreatPeople.VictoryDeities|Ships.Colossus" },
            new object[] { "ship:constructor", "GreatPeople.LabourDeities|Places.Cities|Places.Countries|Places.Deserts|Places.Forests|Places.Lakes|Places.Mountains|Places.Regions|Places.Rivers|Places.Seas|Ships.Constructor" },
            new object[] { "ship:corvette", "Biosphere.Animals|Biosphere.MythologicalCreatures|GreatPeople.AirDeities|GreatPeople.ColdDeities|GreatPeople.DarknessDeities|GreatPeople.DisloyaltyDeities|GreatPeople.FlyingAces|GreatPeople.GeneralsTier1|GreatPeople.HatredDeities|GreatPeople.Heroes|GreatPeople.JusticeDeities|GreatPeople.LeadersTier1|GreatPeople.LightDeities|GreatPeople.LoyaltyDeities|GreatPeople.OtherDeities|GreatPeople.ProphecyDeities|GreatPeople.ProtectionDeities|GreatPeople.PunishmentDeities|GreatPeople.SkyDeities|GreatPeople.SleepDeities|GreatPeople.SunDeities|GreatPeople.TimeDeities|GreatPeople.WarmthDeities|GreatPeople.WaterDeities|MilitaryUnitTypes|Ships.Corvette|Weapons.Artillery|Weapons.Axes|Weapons.Bows|Weapons.Crossbows|Weapons.Daggers|Weapons.Guns|Weapons.Hammers|Weapons.Polearms|Weapons.Swords|Weapons.Throwables" },
            new object[] { "ship:cruiser", "BattleLocations|Biosphere.MythologicalCreatures|Forts|GreatPeople.Admirals|GreatPeople.AirDeities|GreatPeople.ColdDeities|GreatPeople.DarknessDeities|GreatPeople.DisloyaltyDeities|GreatPeople.GeneralsTier2|GreatPeople.HatredDeities|GreatPeople.JusticeDeities|GreatPeople.LeadersTier2|GreatPeople.LightDeities|GreatPeople.LoyaltyDeities|GreatPeople.OtherDeities|GreatPeople.PowerDeities|GreatPeople.ProphecyDeities|GreatPeople.ProtectionDeities|GreatPeople.PunishmentDeities|GreatPeople.SkyDeities|GreatPeople.SleepDeities|GreatPeople.SunDeities|GreatPeople.TimeDeities|GreatPeople.WarmthDeities|GreatPeople.WaterDeities|MilitaryUnitTypes|Places.Regions|Ships.Cruiser" },
            new object[] { "ship:destroyer", "Biosphere.MythologicalCreatures|Forts|GreatPeople.AirDeities|GreatPeople.ColdDeities|GreatPeople.DarknessDeities|GreatPeople.DisloyaltyDeities|GreatPeople.HatredDeities|GreatPeople.JusticeDeities|GreatPeople.LightDeities|GreatPeople.LoyaltyDeities|GreatPeople.OtherDeities|GreatPeople.ProphecyDeities|GreatPeople.ProtectionDeities|GreatPeople.PunishmentDeities|GreatPeople.SkyDeities|GreatPeople.SleepDeities|GreatPeople.SunDeities|GreatPeople.TimeDeities|GreatPeople.WarmthDeities|GreatPeople.WaterDeities|MilitaryUnitTypes|Places.Cities|Ships.Destroyer" },
            new object[] { "ship:generic", "Denonyms|Ships.Generic" },
            new object[] { "ship:ion_cannon", "Ships.IonCannon|Weapons.Artillery|Weapons.Bows|Weapons.Crossbows|Weapons.Guns|Weapons.Throwables" },
            new object[] { "ship:juggernaut", "Ships.Juggernaut" },
            new object[] { "ship:military_station_large", "BattleLocations|Forts|GreatPeople.Admirals|GreatPeople.CreationDeities|GreatPeople.DeathDeities|GreatPeople.DestructionDeities|GreatPeople.FearDeities|GreatPeople.GeneralsTier3|GreatPeople.HatredDeities|GreatPeople.LeadersTier3|GreatPeople.PowerDeities|GreatPeople.SorrowDeities|GreatPeople.VictoryDeities|GreatPeople.WarDeities|Stations.MilitaryStations.Generic|Stations.MilitaryStations.Large|Weapons.Artillery|Weapons.Axes|Weapons.Bows|Weapons.Crossbows|Weapons.Daggers|Weapons.Guns|Weapons.Hammers|Weapons.Polearms|Weapons.Swords|Weapons.Throwables" },
            new object[] { "ship:military_station_medium", "BattleLocations|Forts|GreatPeople.Admirals|GreatPeople.FearDeities|GreatPeople.GeneralsTier2|GreatPeople.GeneralsTier3|GreatPeople.HatredDeities|GreatPeople.LeadersTier2|GreatPeople.LeadersTier3|GreatPeople.PeaceDeities|GreatPeople.PowerDeities|GreatPeople.SorrowDeities|Stations.MilitaryStations.Generic|Stations.MilitaryStations.Medium|Weapons.Artillery|Weapons.Axes|Weapons.Bows|Weapons.Crossbows|Weapons.Daggers|Weapons.Guns|Weapons.Hammers|Weapons.Polearms|Weapons.Swords|Weapons.Throwables" },
            new object[] { "ship:military_station_small", "BattleLocations|Forts|GreatPeople.Admirals|GreatPeople.FearDeities|GreatPeople.GeneralsTier1|GreatPeople.GeneralsTier2|GreatPeople.GeneralsTier3|GreatPeople.HatredDeities|GreatPeople.LeadersTier1|GreatPeople.LeadersTier2|GreatPeople.LeadersTier3|GreatPeople.SorrowDeities|Stations.MilitaryStations.Generic|Stations.MilitaryStations.Small|Weapons.Artillery|Weapons.Axes|Weapons.Bows|Weapons.Crossbows|Weapons.Daggers|Weapons.Guns|Weapons.Hammers|Weapons.Polearms|Weapons.Swords|Weapons.Throwables" },
            new object[] { "ship:mining_station", "GreatPeople.LabourDeities|Stations.MiningStations" },
            new object[] { "ship:observation_station", "GreatPeople.KnowledgeDeities|Stations.ObservationStations" },
            new object[] { "ship:research_station", "Companies.ResearchCompanies|GreatPeople.Explorers|GreatPeople.HealthDeities|GreatPeople.KnowledgeDeities|GreatPeople.NatureDeities|GreatPeople.Pioneers|GreatPeople.Scientists|Ships.Science|Stations.ResearchStations" },
            new object[] { "ship:science", "GreatPeople.Explorers|GreatPeople.HealthDeities|GreatPeople.KnowledgeDeities|GreatPeople.NatureDeities|GreatPeople.Pioneers|GreatPeople.Scientists|Ships.Science" },
            new object[] { "ship:sponsored_colonizer", "GreatPeople.FeastDeities|GreatPeople.FortuneDeities|GreatPeople.HealthDeities|GreatPeople.LoveDeities|GreatPeople.PeaceDeities|GreatPeople.SleepDeities|Places.Cities|Places.Countries|Places.Deserts|Places.Forests|Places.Lakes|Places.Mountains|Places.Regions|Places.Rivers|Places.Seas|Ships.Coloniser" },
            new object[] { "ship:titan", "GreatPeople.BeastsDeities|GreatPeople.CreationDeities|GreatPeople.DarknessDeities|GreatPeople.DeathDeities|GreatPeople.FearDeities|GreatPeople.GeneralsTier3|GreatPeople.HatredDeities|GreatPeople.LeadersTier3|GreatPeople.OtherDeities|GreatPeople.PowerDeities|GreatPeople.SorrowDeities|GreatPeople.VictoryDeities|GreatPeople.WarDeities|Ships.Titan" },
            new object[] { "ship:transport", "BattleLocations|Forts|GreatPeople.BeastsDeities|GreatPeople.CreationDeities|GreatPeople.DarknessDeities|GreatPeople.DestructionDeities|GreatPeople.FearDeities|GreatPeople.HatredDeities|GreatPeople.Heroes|GreatPeople.PowerDeities|GreatPeople.ProphecyDeities|GreatPeople.TimeDeities|GreatPeople.VictoryDeities|GreatPeople.WarDeities|MilitaryUnitTypes|Ships.Transport|Weapons.Artillery|Weapons.Axes|Weapons.Bows|Weapons.Crossbows|Weapons.Daggers|Weapons.Guns|Weapons.Hammers|Weapons.Polearms|Weapons.Swords|Weapons.Throwables" },
        ]
        ;
    }
}
