using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.UnitTests.Models
{
    [TestFixture]
    public sealed class OtherModelTests
    {
        [Test]
        public void GivenNewGreatPeople_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            GreatPeople greatPeople = new();

            Assert.That(greatPeople.Explorers, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.Pioneers, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.Scientists, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.LeadersTier1, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.LeadersTier2, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.LeadersTier3, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.FlyingAces, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.Heroes, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.Admirals, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.GeneralsTier1, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.GeneralsTier2, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.GeneralsTier3, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.PowerDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.CreationDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.DestructionDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.PeaceDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.WarDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.VictoryDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.DeathDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.HatredDeities, Is.Not.Null.And.Empty);
            Assert.That(greatPeople.FearDeities, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewCompanyNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            CompanyNames companies = new();

            Assert.That(companies.RobotManufacturers, Is.Not.Null.And.Empty);
            Assert.That(companies.AutomotiveManufacturers, Is.Not.Null.And.Empty);
            Assert.That(companies.AircraftManufacturers, Is.Not.Null.And.Empty);
            Assert.That(companies.SpacecraftManufacturers, Is.Not.Null.And.Empty);
            Assert.That(companies.WeaponManufacturers, Is.Not.Null.And.Empty);
            Assert.That(companies.RocketDesigners, Is.Not.Null.And.Empty);
            Assert.That(companies.ResearchCompanies, Is.Not.Null.And.Empty);
            Assert.That(companies.InvestmentCompanies, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewWarfareNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            WarfareNames warfare = new();

            Assert.That(warfare.Weapons, Is.Not.Null);
            Assert.That(warfare.MilitaryUnitTypes, Is.Not.Null.And.Empty);
            Assert.That(warfare.ShipTypes, Is.Not.Null.And.Empty);
            Assert.That(warfare.Forts, Is.Not.Null.And.Empty);
            Assert.That(warfare.BattleLocations, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewBiosphereNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            BiosphereNames biosphere = new();

            Assert.That(biosphere.Animals, Is.Not.Null.And.Empty);
            Assert.That(biosphere.MythologicalCreatures, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewShipNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            ShipNames ships = new();

            Assert.That(ships.Generic, Is.Not.Null.And.Empty);
            Assert.That(ships.Corvette, Is.Not.Null.And.Empty);
            Assert.That(ships.Destroyer, Is.Not.Null.And.Empty);
            Assert.That(ships.Cruiser, Is.Not.Null.And.Empty);
            Assert.That(ships.Battleship, Is.Not.Null.And.Empty);
            Assert.That(ships.Titan, Is.Not.Null.And.Empty);
            Assert.That(ships.Colossus, Is.Not.Null.And.Empty);
            Assert.That(ships.Juggernaut, Is.Not.Null.And.Empty);
            Assert.That(ships.Constructor, Is.Not.Null.And.Empty);
            Assert.That(ships.Science, Is.Not.Null.And.Empty);
            Assert.That(ships.Coloniser, Is.Not.Null.And.Empty);
            Assert.That(ships.Transport, Is.Not.Null.And.Empty);
            Assert.That(ships.IonCannon, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewStationNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            StationNames stations = new();

            Assert.That(stations.MilitaryStations, Is.Not.Null);
            Assert.That(stations.MiningStations, Is.Not.Null.And.Empty);
            Assert.That(stations.ResearchStations, Is.Not.Null.And.Empty);
            Assert.That(stations.ObservationStations, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewArmyNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            ArmyNames armies = new();

            Assert.That(armies.FleetSequentialName, Is.Not.Null);
            Assert.That(armies.Fleet, Is.Not.Null.And.Empty);
            Assert.That(armies.DefenceArmySequentialName, Is.Not.Null);
            Assert.That(armies.DefenceArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.AssaultArmySequentialName, Is.Not.Null);
            Assert.That(armies.AssaultArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.OccupationArmySequentialName, Is.Not.Null);
            Assert.That(armies.OccupationArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.SlaveArmySequentialName, Is.Not.Null);
            Assert.That(armies.SlaveArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.CloneArmySequentialName, Is.Not.Null);
            Assert.That(armies.CloneArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.PerfectedCloneArmySequentialName, Is.Not.Null);
            Assert.That(armies.PerfectedCloneArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.UndeadArmySequentialName, Is.Not.Null);
            Assert.That(armies.UndeadArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.RoboticDefenceArmySequentialName, Is.Not.Null);
            Assert.That(armies.RoboticDefenceArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.RoboticAssaultArmySequentialName, Is.Not.Null);
            Assert.That(armies.RoboticAssaultArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.RoboticOccupationArmySequentialName, Is.Not.Null);
            Assert.That(armies.RoboticOccupationArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.AndroidAssaultArmySequentialName, Is.Not.Null);
            Assert.That(armies.AndroidAssaultArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.AndroidDefenceArmySequentialName, Is.Not.Null);
            Assert.That(armies.AndroidDefenceArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.PsionicArmySequentialName, Is.Not.Null);
            Assert.That(armies.PsionicArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.XenomorphArmySequentialName, Is.Not.Null);
            Assert.That(armies.XenomorphArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.SuperSoldierArmySequentialName, Is.Not.Null);
            Assert.That(armies.SuperSoldierArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.PrimitiveArmySequentialName, Is.Not.Null);
            Assert.That(armies.PrimitiveArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.IndustrialArmySequentialName, Is.Not.Null);
            Assert.That(armies.IndustrialArmy, Is.Not.Null.And.Empty);
            Assert.That(armies.PostAtomicArmySequentialName, Is.Not.Null);
            Assert.That(armies.PostAtomicArmy, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNewPlanetNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            PlanetNames planets = new();

            Assert.That(planets.Generic, Is.Not.Null.And.Empty);
            Assert.That(planets.Desert, Is.Not.Null.And.Empty);
            Assert.That(planets.Arid, Is.Not.Null.And.Empty);
            Assert.That(planets.Tropical, Is.Not.Null.And.Empty);
            Assert.That(planets.Continental, Is.Not.Null.And.Empty);
            Assert.That(planets.Gaia, Is.Not.Null.And.Empty);
            Assert.That(planets.Ocean, Is.Not.Null.And.Empty);
            Assert.That(planets.Tundra, Is.Not.Null.And.Empty);
            Assert.That(planets.Arctic, Is.Not.Null.And.Empty);
            Assert.That(planets.Tomb, Is.Not.Null.And.Empty);
            Assert.That(planets.Savannah, Is.Not.Null.And.Empty);
            Assert.That(planets.Alpine, Is.Not.Null.And.Empty);
            Assert.That(planets.Molten, Is.Not.Null.And.Empty);
            Assert.That(planets.Barren, Is.Not.Null.And.Empty);
            Assert.That(planets.Asteroid, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenGreatPeopleWithValues_WhenAddingValues_ThenValuesAreStored()
        {
            GreatPeople greatPeople = new()
            {
                Explorers = [new NameGroup { ExplicitValues = ["Explorer1"] }],
                Scientists = [new NameGroup { ExplicitValues = ["Scientist1"] }]
            };

            Assert.That(greatPeople.Explorers, Has.Count.EqualTo(1));
            Assert.That(greatPeople.Scientists, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenCompanyNamesWithValues_WhenAddingValues_ThenValuesAreStored()
        {
            CompanyNames companies = new()
            {
                RobotManufacturers = [new NameGroup { ExplicitValues = ["RobotCorp1"] }],
                ResearchCompanies = [new NameGroup { ExplicitValues = ["Research1"] }]
            };

            Assert.That(companies.RobotManufacturers, Has.Count.EqualTo(1));
            Assert.That(companies.ResearchCompanies, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenShipNamesWithValues_WhenAddingValues_ThenValuesAreStored()
        {
            ShipNames ships = new()
            {
                Corvette = [new NameGroup { ExplicitValues = ["Corvette1"] }],
                Destroyer = [new NameGroup { ExplicitValues = ["Destroyer1"] }]
            };

            Assert.That(ships.Corvette, Has.Count.EqualTo(1));
            Assert.That(ships.Destroyer, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenStationNamesWithValues_WhenAddingValues_ThenValuesAreStored()
        {
            StationNames stations = new()
            {
                MiningStations = [new NameGroup { ExplicitValues = ["Mining1"] }],
                ResearchStations = [new NameGroup { ExplicitValues = ["Research1"] }]
            };

            Assert.That(stations.MiningStations, Has.Count.EqualTo(1));
            Assert.That(stations.ResearchStations, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenArmyNamesWithValues_WhenAddingValues_ThenValuesAreStored()
        {
            ArmyNames armies = new()
            {
                Fleet = [new NameGroup { ExplicitValues = ["Fleet1"] }],
                DefenceArmy = [new NameGroup { ExplicitValues = ["Defence1"] }]
            };

            Assert.That(armies.Fleet, Has.Count.EqualTo(1));
            Assert.That(armies.DefenceArmy, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenPlanetNamesWithValues_WhenAddingValues_ThenValuesAreStored()
        {
            PlanetNames planets = new()
            {
                Generic = [new NameGroup { ExplicitValues = ["Generic1"] }],
                Desert = [new NameGroup { ExplicitValues = ["Desert1"] }]
            };

            Assert.That(planets.Generic, Has.Count.EqualTo(1));
            Assert.That(planets.Desert, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenGreatPeopleWithAllLeaders_WhenAccessingAllLeaders_ThenReturnsCombinedLeaders()
        {
            GreatPeople greatPeople = new()
            {
                LeadersTier1 = [new NameGroup { ExplicitValues = ["Leader1"] }],
                LeadersTier2 = [new NameGroup { ExplicitValues = ["Leader2"] }],
                LeadersTier3 = [new NameGroup { ExplicitValues = ["Leader3"] }]
            };

            IEnumerable<NameGroup> allLeaders = greatPeople.AllLeaders;

            Assert.That(allLeaders.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenGreatPeopleWithAllGenerals_WhenAccessingAllGenerals_ThenReturnsCombinedGenerals()
        {
            GreatPeople greatPeople = new()
            {
                GeneralsTier1 = [new NameGroup { ExplicitValues = ["General1"] }],
                GeneralsTier2 = [new NameGroup { ExplicitValues = ["General2"] }],
                GeneralsTier3 = [new NameGroup { ExplicitValues = ["General3"] }]
            };

            IEnumerable<NameGroup> allGenerals = greatPeople.AllGenerals;

            Assert.That(allGenerals.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenGreatPeopleWithAllDeities_WhenAccessingAllDeities_ThenReturnsCombinedDeities()
        {
            GreatPeople greatPeople = new()
            {
                PowerDeities = [new NameGroup { ExplicitValues = ["Power1"] }],
                CreationDeities = [new NameGroup { ExplicitValues = ["Creation1"] }],
                WarDeities = [new NameGroup { ExplicitValues = ["War1"] }]
            };

            IEnumerable<NameGroup> allDeities = greatPeople.AllDeities;

            Assert.That(allDeities.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenGreatPeopleWithMultipleDeityCategories_WhenAccessingAllDeities_ThenIncludesAllCategories()
        {
            GreatPeople greatPeople = new()
            {
                PowerDeities = [new NameGroup { ExplicitValues = ["Power1"] }],
                CreationDeities = [new NameGroup { ExplicitValues = ["Creation1"] }],
                DestructionDeities = [new NameGroup { ExplicitValues = ["Destruction1"] }],
                PeaceDeities = [new NameGroup { ExplicitValues = ["Peace1"] }],
                WarDeities = [new NameGroup { ExplicitValues = ["War1"] }],
                VictoryDeities = [new NameGroup { ExplicitValues = ["Victory1"] }],
                DeathDeities = [new NameGroup { ExplicitValues = ["Death1"] }],
                HatredDeities = [new NameGroup { ExplicitValues = ["Hatred1"] }],
                FearDeities = [new NameGroup { ExplicitValues = ["Fear1"] }],
                SorrowDeities = [new NameGroup { ExplicitValues = ["Sorrow1"] }],
                BeastsDeities = [new NameGroup { ExplicitValues = ["Beasts1"] }],
                TimeDeities = [new NameGroup { ExplicitValues = ["Time1"] }],
                ProphecyDeities = [new NameGroup { ExplicitValues = ["Prophecy1"] }],
                JusticeDeities = [new NameGroup { ExplicitValues = ["Justice1"] }],
                ProtectionDeities = [new NameGroup { ExplicitValues = ["Protection1"] }],
                PunishmentDeities = [new NameGroup { ExplicitValues = ["Punishment1"] }],
                LoyaltyDeities = [new NameGroup { ExplicitValues = ["Loyalty1"] }],
                DisloyaltyDeities = [new NameGroup { ExplicitValues = ["Disloyalty1"] }],
                LabourDeities = [new NameGroup { ExplicitValues = ["Labour1"] }],
                NatureDeities = [new NameGroup { ExplicitValues = ["Nature1"] }],
                HealthDeities = [new NameGroup { ExplicitValues = ["Health1"] }],
                LoveDeities = [new NameGroup { ExplicitValues = ["Love1"] }],
                KnowledgeDeities = [new NameGroup { ExplicitValues = ["Knowledge1"] }],
                ArtDeities = [new NameGroup { ExplicitValues = ["Art1"] }],
                FeastDeities = [new NameGroup { ExplicitValues = ["Feast1"] }],
                FortuneDeities = [new NameGroup { ExplicitValues = ["Fortune1"] }],
                SleepDeities = [new NameGroup { ExplicitValues = ["Sleep1"] }],
                DarknessDeities = [new NameGroup { ExplicitValues = ["Darkness1"] }],
                LightDeities = [new NameGroup { ExplicitValues = ["Light1"] }],
                SunDeities = [new NameGroup { ExplicitValues = ["Sun1"] }],
                SkyDeities = [new NameGroup { ExplicitValues = ["Sky1"] }],
                AirDeities = [new NameGroup { ExplicitValues = ["Air1"] }],
                ColdDeities = [new NameGroup { ExplicitValues = ["Cold1"] }],
                WarmthDeities = [new NameGroup { ExplicitValues = ["Warmth1"] }],
                WaterDeities = [new NameGroup { ExplicitValues = ["Water1"] }],
                OtherDeities = [new NameGroup { ExplicitValues = ["Other1"] }]
            };

            IEnumerable<NameGroup> allDeities = greatPeople.AllDeities;

            Assert.That(allDeities.Count(), Is.EqualTo(35));
        }

        [Test]
        public void GivenCompanyNamesWithAllCategories_WhenAddingValues_ThenAllCategoriesArePopulated()
        {
            CompanyNames companies = new()
            {
                RobotManufacturers = [new NameGroup { ExplicitValues = ["Robot1"] }],
                AutomotiveManufacturers = [new NameGroup { ExplicitValues = ["Auto1"] }],
                AircraftManufacturers = [new NameGroup { ExplicitValues = ["Aircraft1"] }],
                SpacecraftManufacturers = [new NameGroup { ExplicitValues = ["Space1"] }],
                WeaponManufacturers = [new NameGroup { ExplicitValues = ["Weapon1"] }],
                RocketDesigners = [new NameGroup { ExplicitValues = ["Rocket1"] }],
                ResearchCompanies = [new NameGroup { ExplicitValues = ["Research1"] }],
                InvestmentCompanies = [new NameGroup { ExplicitValues = ["Investment1"] }]
            };

            Assert.That(companies.RobotManufacturers, Has.Count.EqualTo(1));
            Assert.That(companies.AutomotiveManufacturers, Has.Count.EqualTo(1));
            Assert.That(companies.AircraftManufacturers, Has.Count.EqualTo(1));
            Assert.That(companies.SpacecraftManufacturers, Has.Count.EqualTo(1));
            Assert.That(companies.WeaponManufacturers, Has.Count.EqualTo(1));
            Assert.That(companies.RocketDesigners, Has.Count.EqualTo(1));
            Assert.That(companies.ResearchCompanies, Has.Count.EqualTo(1));
            Assert.That(companies.InvestmentCompanies, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenWarfareNamesWithWeapons_WhenAddingWeaponNames_ThenWeaponsAreStored()
        {
            WarfareNames warfare = new()
            {
                Weapons = new WeaponNames
                {
                    Artillery = [new NameGroup { ExplicitValues = ["Artillery1"] }],
                    Guns = [new NameGroup { ExplicitValues = ["Gun1"] }],
                    Swords = [new NameGroup { ExplicitValues = ["Sword1"] }]
                }
            };

            Assert.That(warfare.Weapons.Artillery, Has.Count.EqualTo(1));
            Assert.That(warfare.Weapons.Guns, Has.Count.EqualTo(1));
            Assert.That(warfare.Weapons.Swords, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenWarfareNamesWithAllCategories_WhenAddingValues_ThenAllCategoriesArePopulated()
        {
            WarfareNames warfare = new()
            {
                MilitaryUnitTypes = [new NameGroup { ExplicitValues = ["Unit1"] }],
                ShipTypes = [new NameGroup { ExplicitValues = ["Ship1"] }],
                Forts = [new NameGroup { ExplicitValues = ["Fort1"] }],
                BattleLocations = [new NameGroup { ExplicitValues = ["Battle1"] }]
            };

            Assert.That(warfare.MilitaryUnitTypes, Has.Count.EqualTo(1));
            Assert.That(warfare.ShipTypes, Has.Count.EqualTo(1));
            Assert.That(warfare.Forts, Has.Count.EqualTo(1));
            Assert.That(warfare.BattleLocations, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenBiosphereNamesWithBothCategories_WhenAddingValues_ThenBothCategoriesArePopulated()
        {
            BiosphereNames biosphere = new()
            {
                Animals = [new NameGroup { ExplicitValues = ["Animal1"] }],
                MythologicalCreatures = [new NameGroup { ExplicitValues = ["Creature1"] }]
            };

            Assert.That(biosphere.Animals, Has.Count.EqualTo(1));
            Assert.That(biosphere.MythologicalCreatures, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenShipNamesWithAllShipTypes_WhenAddingValues_ThenAllShipTypesArePopulated()
        {
            ShipNames ships = new()
            {
                Generic = [new NameGroup { ExplicitValues = ["Generic1"] }],
                Corvette = [new NameGroup { ExplicitValues = ["Corvette1"] }],
                Destroyer = [new NameGroup { ExplicitValues = ["Destroyer1"] }],
                Cruiser = [new NameGroup { ExplicitValues = ["Cruiser1"] }],
                Battleship = [new NameGroup { ExplicitValues = ["Battleship1"] }],
                Titan = [new NameGroup { ExplicitValues = ["Titan1"] }],
                Colossus = [new NameGroup { ExplicitValues = ["Colossus1"] }],
                Juggernaut = [new NameGroup { ExplicitValues = ["Juggernaut1"] }],
                Constructor = [new NameGroup { ExplicitValues = ["Constructor1"] }],
                Science = [new NameGroup { ExplicitValues = ["Science1"] }],
                Coloniser = [new NameGroup { ExplicitValues = ["Coloniser1"] }],
                Transport = [new NameGroup { ExplicitValues = ["Transport1"] }],
                IonCannon = [new NameGroup { ExplicitValues = ["IonCannon1"] }]
            };

            Assert.That(ships.Generic, Has.Count.EqualTo(1));
            Assert.That(ships.Corvette, Has.Count.EqualTo(1));
            Assert.That(ships.Destroyer, Has.Count.EqualTo(1));
            Assert.That(ships.Cruiser, Has.Count.EqualTo(1));
            Assert.That(ships.Battleship, Has.Count.EqualTo(1));
            Assert.That(ships.Titan, Has.Count.EqualTo(1));
            Assert.That(ships.Colossus, Has.Count.EqualTo(1));
            Assert.That(ships.Juggernaut, Has.Count.EqualTo(1));
            Assert.That(ships.Constructor, Has.Count.EqualTo(1));
            Assert.That(ships.Science, Has.Count.EqualTo(1));
            Assert.That(ships.Coloniser, Has.Count.EqualTo(1));
            Assert.That(ships.Transport, Has.Count.EqualTo(1));
            Assert.That(ships.IonCannon, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenStationNamesWithAllCategories_WhenAddingValues_ThenAllCategoriesArePopulated()
        {
            StationNames stations = new()
            {
                MilitaryStations = new MilitaryStationNames
                {
                    Generic = [new NameGroup { ExplicitValues = ["Military1"] }],
                    Small = [new NameGroup { ExplicitValues = ["Small1"] }],
                    Medium = [new NameGroup { ExplicitValues = ["Medium1"] }],
                    Large = [new NameGroup { ExplicitValues = ["Large1"] }]
                },
                MiningStations = [new NameGroup { ExplicitValues = ["Mining1"] }],
                ResearchStations = [new NameGroup { ExplicitValues = ["Research1"] }],
                ObservationStations = [new NameGroup { ExplicitValues = ["Observation1"] }]
            };

            Assert.That(stations.MilitaryStations.Generic, Has.Count.EqualTo(1));
            Assert.That(stations.MilitaryStations.Small, Has.Count.EqualTo(1));
            Assert.That(stations.MilitaryStations.Medium, Has.Count.EqualTo(1));
            Assert.That(stations.MilitaryStations.Large, Has.Count.EqualTo(1));
            Assert.That(stations.MiningStations, Has.Count.EqualTo(1));
            Assert.That(stations.ResearchStations, Has.Count.EqualTo(1));
            Assert.That(stations.ObservationStations, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenArmyNamesWithAllArmyTypes_WhenAddingValues_ThenAllArmyTypesArePopulated()
        {
            ArmyNames armies = new()
            {
                Fleet = [new NameGroup { ExplicitValues = ["Fleet1"] }],
                DefenceArmy = [new NameGroup { ExplicitValues = ["Defence1"] }],
                AssaultArmy = [new NameGroup { ExplicitValues = ["Assault1"] }],
                OccupationArmy = [new NameGroup { ExplicitValues = ["Occupation1"] }],
                SlaveArmy = [new NameGroup { ExplicitValues = ["Slave1"] }],
                CloneArmy = [new NameGroup { ExplicitValues = ["Clone1"] }],
                PerfectedCloneArmy = [new NameGroup { ExplicitValues = ["PerfectedClone1"] }],
                UndeadArmy = [new NameGroup { ExplicitValues = ["Undead1"] }],
                RoboticDefenceArmy = [new NameGroup { ExplicitValues = ["RoboticDefence1"] }],
                RoboticAssaultArmy = [new NameGroup { ExplicitValues = ["RoboticAssault1"] }],
                RoboticOccupationArmy = [new NameGroup { ExplicitValues = ["RoboticOccupation1"] }],
                AndroidAssaultArmy = [new NameGroup { ExplicitValues = ["AndroidAssault1"] }],
                AndroidDefenceArmy = [new NameGroup { ExplicitValues = ["AndroidDefence1"] }],
                PsionicArmy = [new NameGroup { ExplicitValues = ["Psionic1"] }],
                XenomorphArmy = [new NameGroup { ExplicitValues = ["Xenomorph1"] }],
                SuperSoldierArmy = [new NameGroup { ExplicitValues = ["SuperSoldier1"] }],
                PrimitiveArmy = [new NameGroup { ExplicitValues = ["Primitive1"] }],
                IndustrialArmy = [new NameGroup { ExplicitValues = ["Industrial1"] }],
                PostAtomicArmy = [new NameGroup { ExplicitValues = ["PostAtomic1"] }]
            };

            Assert.That(armies.Fleet, Has.Count.EqualTo(1));
            Assert.That(armies.DefenceArmy, Has.Count.EqualTo(1));
            Assert.That(armies.AssaultArmy, Has.Count.EqualTo(1));
            Assert.That(armies.OccupationArmy, Has.Count.EqualTo(1));
            Assert.That(armies.SlaveArmy, Has.Count.EqualTo(1));
            Assert.That(armies.CloneArmy, Has.Count.EqualTo(1));
            Assert.That(armies.PerfectedCloneArmy, Has.Count.EqualTo(1));
            Assert.That(armies.UndeadArmy, Has.Count.EqualTo(1));
            Assert.That(armies.RoboticDefenceArmy, Has.Count.EqualTo(1));
            Assert.That(armies.RoboticAssaultArmy, Has.Count.EqualTo(1));
            Assert.That(armies.RoboticOccupationArmy, Has.Count.EqualTo(1));
            Assert.That(armies.AndroidAssaultArmy, Has.Count.EqualTo(1));
            Assert.That(armies.AndroidDefenceArmy, Has.Count.EqualTo(1));
            Assert.That(armies.PsionicArmy, Has.Count.EqualTo(1));
            Assert.That(armies.XenomorphArmy, Has.Count.EqualTo(1));
            Assert.That(armies.SuperSoldierArmy, Has.Count.EqualTo(1));
            Assert.That(armies.PrimitiveArmy, Has.Count.EqualTo(1));
            Assert.That(armies.IndustrialArmy, Has.Count.EqualTo(1));
            Assert.That(armies.PostAtomicArmy, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenArmyNames_WhenCheckingSequentialNames_ThenDefaultValuesAreSet()
        {
            ArmyNames armies = new();

            Assert.That(armies.FleetSequentialName, Is.EqualTo("%O% Fleet"));
            Assert.That(armies.DefenceArmySequentialName, Is.EqualTo("%O% Planetary Guard"));
            Assert.That(armies.AssaultArmySequentialName, Is.EqualTo("%O% Expeditionary Force"));
            Assert.That(armies.OccupationArmySequentialName, Is.EqualTo("%O% Garrison Force"));
            Assert.That(armies.SlaveArmySequentialName, Is.EqualTo("%O% Indentured Rifles"));
            Assert.That(armies.CloneArmySequentialName, Is.EqualTo("%O% Clone Army"));
            Assert.That(armies.PerfectedCloneArmySequentialName, Is.EqualTo("%O% Clone Army"));
            Assert.That(armies.UndeadArmySequentialName, Is.EqualTo("%O% Undead Army"));
            Assert.That(armies.RoboticDefenceArmySequentialName, Is.EqualTo("%O% Ground Defence Matrix"));
            Assert.That(armies.RoboticAssaultArmySequentialName, Is.EqualTo("%O% Hunter-Killer Group"));
            Assert.That(armies.RoboticOccupationArmySequentialName, Is.EqualTo("%O% Mechanised Garrison"));
            Assert.That(armies.AndroidDefenceArmySequentialName, Is.EqualTo("%O% Synthetic Sentinels"));
            Assert.That(armies.AndroidAssaultArmySequentialName, Is.EqualTo("%O% Synthetic Rangers"));
            Assert.That(armies.PsionicArmySequentialName, Is.EqualTo("%O% Psi Commando"));
            Assert.That(armies.XenomorphArmySequentialName, Is.EqualTo("%O% Bio-Warfare Division"));
            Assert.That(armies.SuperSoldierArmySequentialName, Is.EqualTo("%O% Bio-Engineered Squadron"));
            Assert.That(armies.PrimitiveArmySequentialName, Is.EqualTo("Primitive Army %C%"));
            Assert.That(armies.IndustrialArmySequentialName, Is.EqualTo("Industrial Army %C%"));
            Assert.That(armies.PostAtomicArmySequentialName, Is.EqualTo("Post-Atomic Army %C%"));
        }

        [Test]
        public void GivenPlanetNamesWithAllPlanetTypes_WhenAddingValues_ThenAllPlanetTypesArePopulated()
        {
            PlanetNames planets = new()
            {
                Generic = [new NameGroup { ExplicitValues = ["Generic1"] }],
                Desert = [new NameGroup { ExplicitValues = ["Desert1"] }],
                Arid = [new NameGroup { ExplicitValues = ["Arid1"] }],
                Tropical = [new NameGroup { ExplicitValues = ["Tropical1"] }],
                Continental = [new NameGroup { ExplicitValues = ["Continental1"] }],
                Gaia = [new NameGroup { ExplicitValues = ["Gaia1"] }],
                Ocean = [new NameGroup { ExplicitValues = ["Ocean1"] }],
                Tundra = [new NameGroup { ExplicitValues = ["Tundra1"] }],
                Arctic = [new NameGroup { ExplicitValues = ["Arctic1"] }],
                Tomb = [new NameGroup { ExplicitValues = ["Tomb1"] }],
                Savannah = [new NameGroup { ExplicitValues = ["Savannah1"] }],
                Alpine = [new NameGroup { ExplicitValues = ["Alpine1"] }],
                Molten = [new NameGroup { ExplicitValues = ["Molten1"] }],
                Barren = [new NameGroup { ExplicitValues = ["Barren1"] }],
                Asteroid = [new NameGroup { ExplicitValues = ["Asteroid1"] }]
            };

            Assert.That(planets.Generic, Has.Count.EqualTo(1));
            Assert.That(planets.Desert, Has.Count.EqualTo(1));
            Assert.That(planets.Arid, Has.Count.EqualTo(1));
            Assert.That(planets.Tropical, Has.Count.EqualTo(1));
            Assert.That(planets.Continental, Has.Count.EqualTo(1));
            Assert.That(planets.Gaia, Has.Count.EqualTo(1));
            Assert.That(planets.Ocean, Has.Count.EqualTo(1));
            Assert.That(planets.Tundra, Has.Count.EqualTo(1));
            Assert.That(planets.Arctic, Has.Count.EqualTo(1));
            Assert.That(planets.Tomb, Has.Count.EqualTo(1));
            Assert.That(planets.Savannah, Has.Count.EqualTo(1));
            Assert.That(planets.Alpine, Has.Count.EqualTo(1));
            Assert.That(planets.Molten, Has.Count.EqualTo(1));
            Assert.That(planets.Barren, Has.Count.EqualTo(1));
            Assert.That(planets.Asteroid, Has.Count.EqualTo(1));
        }
    }
}