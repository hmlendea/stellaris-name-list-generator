using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Asserts, for every source category that reaches the output, the exact set of blocks into which its
    /// value is expanded. This is the inverse view of the block composition matrix and was captured from
    /// the same fully-populated coverage document.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class SourceRoutingTests
    {
        private const string OutputFileName = "routing.txt";
        private const string DisplayName = "Routing";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(SourceRoutingTests));
            environment.WriteInput(CoverageDocument.Build());

            environment.Generate(OutputFileName, DisplayName, isLocked: true);
            output = GeneratedNameList.Parse(environment.ReadText(OutputFileName));
        }

        [TestCaseSource(nameof(RoutingCases))]
        public void GivenEverySourceCategory_WhenGenerating_ThenItsValueReachesExactlyItsMappedBlocks(string source, string expectedBlocks)
        {
            string token = CoverageDocument.Token(source);
            HashSet<string> expected = [.. expectedBlocks.Split('|')];
            HashSet<string> actual = [];

            foreach (string block in expected)
            {
                (string section, string subBlock) = Resolve(block);
                string joined = string.Join('\n', output.GetBlockLines(section, subBlock));

                if (joined.Contains(token, StringComparison.Ordinal))
                {
                    actual.Add(block);
                }
            }

            foreach (string block in AllBlocks())
            {
                if (expected.Contains(block))
                {
                    continue;
                }

                (string section, string subBlock) = Resolve(block);
                string joined = string.Join('\n', output.GetBlockLines(section, subBlock));

                Assert.That(joined, Does.Not.Contain(token), $"Source '{source}' unexpectedly reached '{block}'");
            }

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void GivenSourcesThatReachNoBlock_WhenGenerating_ThenTheirTokensIsAbsentFromEveryBlock()
        {
            using GeneratorEnvironment environment = new($"{nameof(SourceRoutingTests)}_unmapped");
            environment.WriteInput(CoverageDocument.Build());

            environment.Generate("unmapped.txt", DisplayName, isLocked: true);
            GeneratedNameList unmapped = GeneratedNameList.Parse(environment.ReadText("unmapped.txt"));

            string blockContent = string.Join(
                '\n',
                unmapped.GetBlockLines("ship_names")
                    .Concat(unmapped.GetBlockLines("ship_class_names"))
                    .Concat(unmapped.GetBlockLines("planet_names"))
                    .Concat(unmapped.GetBlockLines("army_names")));

            foreach (string source in new[] { "Companies.RobotManufacturers", "Places.Airports", "ShipClasses.Transport" })
            {
                Assert.That(blockContent, Does.Not.Contain(CoverageDocument.Token(source)), source);
            }
        }

        private static IEnumerable<string> AllBlocks()
            => AllBlockNames;

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

        private static readonly string[] AllBlockNames =
        [
            "army:android_army",
            "army:android_defense_army",
            "army:assault_army",
            "army:clone_army",
            "army:defense_army",
            "army:gene_warrior_army",
            "army:industrial_army",
            "army:machine_assault_1",
            "army:machine_assault_2",
            "army:machine_assault_3",
            "army:machine_defense",
            "army:occupation_army",
            "army:perfected_clone_army",
            "army:postatomic_army",
            "army:primitive_army",
            "army:psionic_army",
            "army:robotic_army",
            "army:robotic_defense_army",
            "army:robotic_occupation_army",
            "army:slave_army",
            "army:undead_army",
            "army:xenomorph_army",
            "class:battleship",
            "class:coloniser",
            "class:colossus",
            "class:constructor",
            "class:corvette",
            "class:cruiser",
            "class:destroyer",
            "class:generic",
            "class:ion_cannon",
            "class:juggernaut",
            "class:military_station_large",
            "class:military_station_medium",
            "class:military_station_small",
            "class:mining_station",
            "class:observation_station",
            "class:research_station",
            "class:science",
            "class:sponsored_coloniser",
            "class:titan",
            "class:transport",
            "planet:generic",
            "planet:pc_alpine",
            "planet:pc_arctic",
            "planet:pc_arid",
            "planet:pc_asteroid",
            "planet:pc_barren",
            "planet:pc_continental",
            "planet:pc_desert",
            "planet:pc_gaia",
            "planet:pc_molten",
            "planet:pc_nuked",
            "planet:pc_ocean",
            "planet:pc_savannah",
            "planet:pc_tropical",
            "planet:pc_tundra",
            "ship:battleship",
            "ship:colonizer",
            "ship:colossus",
            "ship:constructor",
            "ship:corvette",
            "ship:cruiser",
            "ship:destroyer",
            "ship:generic",
            "ship:ion_cannon",
            "ship:juggernaut",
            "ship:military_station_large",
            "ship:military_station_medium",
            "ship:military_station_small",
            "ship:mining_station",
            "ship:observation_station",
            "ship:research_station",
            "ship:science",
            "ship:sponsored_colonizer",
            "ship:titan",
            "ship:transport",
        ];

        private static readonly object[][] RoutingCases =
        [
            new object[] { "Armies.AndroidAssaultArmy", "army:android_army" },
            new object[] { "Armies.AndroidDefenceArmy", "army:android_defense_army" },
            new object[] { "Armies.AssaultArmy", "army:assault_army" },
            new object[] { "Armies.CloneArmy", "army:clone_army" },
            new object[] { "Armies.DefenceArmy", "army:defense_army" },
            new object[] { "Armies.IndustrialArmy", "army:industrial_army" },
            new object[] { "Armies.OccupationArmy", "army:occupation_army" },
            new object[] { "Armies.PerfectedCloneArmy", "army:perfected_clone_army" },
            new object[] { "Armies.PostAtomicArmy", "army:postatomic_army" },
            new object[] { "Armies.PrimitiveArmy", "army:primitive_army" },
            new object[] { "Armies.PsionicArmy", "army:psionic_army" },
            new object[] { "Armies.RoboticAssaultArmy", "army:robotic_army" },
            new object[] { "Armies.RoboticDefenceArmy", "army:robotic_defense_army" },
            new object[] { "Armies.RoboticOccupationArmy", "army:robotic_occupation_army" },
            new object[] { "Armies.SlaveArmy", "army:slave_army" },
            new object[] { "Armies.SuperSoldierArmy", "army:gene_warrior_army" },
            new object[] { "Armies.UndeadArmy", "army:undead_army" },
            new object[] { "Armies.XenomorphArmy", "army:xenomorph_army" },
            new object[] { "BattleLocations", "planet:pc_nuked|ship:battleship|ship:cruiser|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Biosphere.Animals", "ship:corvette" },
            new object[] { "Biosphere.MythologicalCreatures", "army:psionic_army|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "Companies.AircraftManufacturers", "class:coloniser|class:constructor|class:generic|class:sponsored_coloniser|class:transport" },
            new object[] { "Companies.AutomotiveManufacturers", "class:coloniser|class:constructor|class:sponsored_coloniser|class:transport" },
            new object[] { "Companies.InvestmentCompanies", "class:sponsored_coloniser" },
            new object[] { "Companies.ResearchCompanies", "class:science|ship:research_station" },
            new object[] { "Companies.RocketDesigners", "class:generic" },
            new object[] { "Companies.SpacecraftManufacturers", "class:generic" },
            new object[] { "Companies.WeaponManufacturers", "class:battleship|class:colossus|class:corvette|class:cruiser|class:destroyer|class:juggernaut|class:titan" },
            new object[] { "Denonyms", "class:generic|ship:generic" },
            new object[] { "Forts", "ship:cruiser|ship:destroyer|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "GreatPeople.Admirals", "ship:battleship|ship:cruiser|ship:military_station_large|ship:military_station_medium|ship:military_station_small" },
            new object[] { "GreatPeople.AirDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.ArtDeities", "planet:pc_gaia" },
            new object[] { "GreatPeople.BeastsDeities", "army:xenomorph_army|planet:pc_nuked|ship:titan|ship:transport" },
            new object[] { "GreatPeople.ColdDeities", "planet:pc_alpine|planet:pc_arctic|planet:pc_tundra|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.CreationDeities", "planet:pc_gaia|ship:colossus|ship:military_station_large|ship:titan|ship:transport" },
            new object[] { "GreatPeople.DarknessDeities", "army:xenomorph_army|planet:pc_asteroid|planet:pc_barren|planet:pc_nuked|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer|ship:titan|ship:transport" },
            new object[] { "GreatPeople.DeathDeities", "army:xenomorph_army|planet:pc_barren|planet:pc_nuked|ship:colossus|ship:military_station_large|ship:titan" },
            new object[] { "GreatPeople.DestructionDeities", "planet:pc_molten|planet:pc_nuked|ship:colossus|ship:military_station_large|ship:transport" },
            new object[] { "GreatPeople.DisloyaltyDeities", "planet:pc_arctic|planet:pc_asteroid|planet:pc_barren|planet:pc_desert|planet:pc_molten|planet:pc_nuked|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.Explorers", "ship:research_station|ship:science" },
            new object[] { "GreatPeople.FearDeities", "army:xenomorph_army|planet:pc_asteroid|planet:pc_barren|planet:pc_nuked|ship:battleship|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:titan|ship:transport" },
            new object[] { "GreatPeople.FeastDeities", "planet:pc_gaia|ship:colonizer|ship:sponsored_colonizer" },
            new object[] { "GreatPeople.FlyingAces", "ship:corvette" },
            new object[] { "GreatPeople.FortuneDeities", "planet:pc_gaia|ship:colonizer|ship:sponsored_colonizer" },
            new object[] { "GreatPeople.GeneralsTier1", "ship:corvette|ship:military_station_small" },
            new object[] { "GreatPeople.GeneralsTier2", "ship:cruiser|ship:military_station_medium|ship:military_station_small" },
            new object[] { "GreatPeople.GeneralsTier3", "ship:battleship|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:titan" },
            new object[] { "GreatPeople.HatredDeities", "army:xenomorph_army|planet:pc_asteroid|planet:pc_barren|planet:pc_nuked|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:titan|ship:transport" },
            new object[] { "GreatPeople.HealthDeities", "planet:pc_gaia|ship:colonizer|ship:research_station|ship:science|ship:sponsored_colonizer" },
            new object[] { "GreatPeople.Heroes", "ship:corvette|ship:transport" },
            new object[] { "GreatPeople.JusticeDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.KnowledgeDeities", "planet:generic|ship:observation_station|ship:research_station|ship:science" },
            new object[] { "GreatPeople.LabourDeities", "planet:pc_molten|ship:constructor|ship:mining_station" },
            new object[] { "GreatPeople.LeadersTier1", "ship:corvette|ship:military_station_small" },
            new object[] { "GreatPeople.LeadersTier2", "ship:cruiser|ship:military_station_medium|ship:military_station_small" },
            new object[] { "GreatPeople.LeadersTier3", "ship:battleship|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:titan" },
            new object[] { "GreatPeople.LightDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.LoveDeities", "planet:pc_gaia|ship:colonizer|ship:sponsored_colonizer" },
            new object[] { "GreatPeople.LoyaltyDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.NatureDeities", "planet:pc_gaia|planet:pc_tropical|ship:research_station|ship:science" },
            new object[] { "GreatPeople.OtherDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer|ship:titan" },
            new object[] { "GreatPeople.PeaceDeities", "planet:pc_continental|planet:pc_gaia|ship:colonizer|ship:military_station_medium|ship:sponsored_colonizer" },
            new object[] { "GreatPeople.Pioneers", "ship:research_station|ship:science" },
            new object[] { "GreatPeople.PowerDeities", "planet:generic|ship:battleship|ship:colossus|ship:cruiser|ship:military_station_large|ship:military_station_medium|ship:titan|ship:transport" },
            new object[] { "GreatPeople.ProphecyDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer|ship:transport" },
            new object[] { "GreatPeople.ProtectionDeities", "planet:pc_continental|planet:pc_gaia|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.PunishmentDeities", "planet:pc_alpine|planet:pc_arctic|planet:pc_asteroid|planet:pc_barren|planet:pc_desert|planet:pc_molten|planet:pc_nuked|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.Scientists", "ship:research_station|ship:science" },
            new object[] { "GreatPeople.SkyDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.SleepDeities", "planet:generic|ship:battleship|ship:colonizer|ship:corvette|ship:cruiser|ship:destroyer|ship:sponsored_colonizer" },
            new object[] { "GreatPeople.SorrowDeities", "army:xenomorph_army|planet:pc_asteroid|planet:pc_barren|planet:pc_nuked|ship:battleship|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:titan" },
            new object[] { "GreatPeople.SunDeities", "planet:pc_arid|planet:pc_desert|planet:pc_savannah|ship:battleship|ship:colossus|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.TimeDeities", "planet:generic|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer|ship:transport" },
            new object[] { "GreatPeople.VictoryDeities", "planet:pc_continental|planet:pc_gaia|ship:colossus|ship:military_station_large|ship:titan|ship:transport" },
            new object[] { "GreatPeople.WarDeities", "planet:pc_molten|planet:pc_nuked|ship:battleship|ship:military_station_large|ship:titan|ship:transport" },
            new object[] { "GreatPeople.WarmthDeities", "planet:pc_arid|planet:pc_desert|planet:pc_molten|planet:pc_savannah|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "GreatPeople.WaterDeities", "planet:pc_ocean|ship:battleship|ship:corvette|ship:cruiser|ship:destroyer" },
            new object[] { "MilitaryUnitTypes", "ship:battleship|ship:corvette|ship:cruiser|ship:destroyer|ship:transport" },
            new object[] { "Places.Cities", "ship:colonizer|ship:constructor|ship:destroyer|ship:sponsored_colonizer" },
            new object[] { "Places.Countries", "ship:battleship|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Places.Deserts", "planet:pc_arid|planet:pc_desert|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Places.Forests", "planet:pc_tropical|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Places.Lakes", "planet:pc_ocean|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Places.Mountains", "planet:pc_alpine|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Places.Regions", "ship:colonizer|ship:constructor|ship:cruiser|ship:sponsored_colonizer" },
            new object[] { "Places.Rivers", "planet:pc_ocean|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Places.Seas", "planet:pc_ocean|ship:colonizer|ship:constructor|ship:sponsored_colonizer" },
            new object[] { "Planets.Alpine", "planet:pc_alpine" },
            new object[] { "Planets.Arctic", "planet:pc_arctic" },
            new object[] { "Planets.Arid", "planet:pc_arid" },
            new object[] { "Planets.Asteroid", "planet:pc_asteroid" },
            new object[] { "Planets.Barren", "planet:pc_barren" },
            new object[] { "Planets.Continental", "planet:pc_continental" },
            new object[] { "Planets.Desert", "planet:pc_desert" },
            new object[] { "Planets.Gaia", "planet:pc_gaia" },
            new object[] { "Planets.Generic", "planet:generic" },
            new object[] { "Planets.Molten", "planet:pc_molten" },
            new object[] { "Planets.Ocean", "planet:pc_ocean" },
            new object[] { "Planets.Savannah", "planet:pc_savannah" },
            new object[] { "Planets.Tomb", "planet:pc_nuked" },
            new object[] { "Planets.Tropical", "planet:pc_tropical" },
            new object[] { "Planets.Tundra", "planet:pc_tundra" },
            new object[] { "ShipClasses.Battleship", "class:battleship" },
            new object[] { "ShipClasses.Coloniser", "class:coloniser|class:sponsored_coloniser|class:transport" },
            new object[] { "ShipClasses.Colossus", "class:colossus" },
            new object[] { "ShipClasses.Constructor", "class:constructor" },
            new object[] { "ShipClasses.Corvette", "class:corvette" },
            new object[] { "ShipClasses.Cruiser", "class:cruiser" },
            new object[] { "ShipClasses.Destroyer", "class:destroyer" },
            new object[] { "ShipClasses.Generic", "class:generic" },
            new object[] { "ShipClasses.IonCannon", "class:ion_cannon" },
            new object[] { "ShipClasses.Juggernaut", "class:juggernaut" },
            new object[] { "ShipClasses.Science", "class:science" },
            new object[] { "ShipClasses.Titan", "class:titan" },
            new object[] { "ShipTypes", "class:generic" },
            new object[] { "Ships.Battleship", "ship:battleship" },
            new object[] { "Ships.Coloniser", "ship:colonizer|ship:sponsored_colonizer" },
            new object[] { "Ships.Colossus", "ship:colossus" },
            new object[] { "Ships.Constructor", "ship:constructor" },
            new object[] { "Ships.Corvette", "ship:corvette" },
            new object[] { "Ships.Cruiser", "ship:cruiser" },
            new object[] { "Ships.Destroyer", "ship:destroyer" },
            new object[] { "Ships.Generic", "ship:generic" },
            new object[] { "Ships.IonCannon", "ship:ion_cannon" },
            new object[] { "Ships.Juggernaut", "ship:juggernaut" },
            new object[] { "Ships.Science", "ship:research_station|ship:science" },
            new object[] { "Ships.Titan", "ship:titan" },
            new object[] { "Ships.Transport", "ship:transport" },
            new object[] { "StationClasses.MilitaryStations.Generic", "class:military_station_large|class:military_station_medium|class:military_station_small" },
            new object[] { "StationClasses.MilitaryStations.Large", "class:military_station_large" },
            new object[] { "StationClasses.MilitaryStations.Medium", "class:military_station_medium" },
            new object[] { "StationClasses.MilitaryStations.Small", "class:military_station_small" },
            new object[] { "StationClasses.MiningStations", "class:mining_station" },
            new object[] { "StationClasses.ObservationStations", "class:observation_station" },
            new object[] { "StationClasses.ResearchStations", "class:research_station" },
            new object[] { "Stations.MilitaryStations.Generic", "ship:military_station_large|ship:military_station_medium|ship:military_station_small" },
            new object[] { "Stations.MilitaryStations.Large", "ship:military_station_large" },
            new object[] { "Stations.MilitaryStations.Medium", "ship:military_station_medium" },
            new object[] { "Stations.MilitaryStations.Small", "ship:military_station_small" },
            new object[] { "Stations.MiningStations", "ship:mining_station" },
            new object[] { "Stations.ObservationStations", "ship:observation_station" },
            new object[] { "Stations.ResearchStations", "ship:research_station" },
            new object[] { "Weapons.Artillery", "class:ion_cannon|ship:corvette|ship:ion_cannon|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Axes", "ship:corvette|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Bows", "ship:corvette|ship:ion_cannon|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Crossbows", "ship:corvette|ship:ion_cannon|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Daggers", "ship:corvette|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Guns", "ship:corvette|ship:ion_cannon|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Hammers", "ship:corvette|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Polearms", "ship:corvette|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Swords", "ship:corvette|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
            new object[] { "Weapons.Throwables", "ship:corvette|ship:ion_cannon|ship:military_station_large|ship:military_station_medium|ship:military_station_small|ship:transport" },
        ]
        ;
    }
}
