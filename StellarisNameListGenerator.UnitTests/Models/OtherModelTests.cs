using System.Collections.Generic;

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
    }
}