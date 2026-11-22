using System;

using NUnit.Framework;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.UnitTests.Models
{
    /// <summary>
    /// Verifies that <see cref="NameList.AddRange"/> concatenates every recognised collection. Each case
    /// seeds the same collection on both operands with a distinct value and asserts that the aggregate
    /// holds both.
    /// </summary>
    [TestFixture]
    public sealed class NameListMergeCoverageTests
    {
        private const string SourceValue = "Nucilandia";
        private const string OtherValue = "Astora";
                [TestCase("Denonyms")]
        [TestCase("Places.Countries")]
        [TestCase("Places.Regions")]
        [TestCase("Places.Cities")]
        [TestCase("Places.Mountains")]
        [TestCase("Places.Forests")]
        [TestCase("Places.Deserts")]
        [TestCase("Places.Rivers")]
        [TestCase("Places.Lakes")]
        [TestCase("Places.Seas")]
        [TestCase("Places.Airports")]
        [TestCase("GreatPeople.Explorers")]
        [TestCase("GreatPeople.Pioneers")]
        [TestCase("GreatPeople.Scientists")]
        [TestCase("GreatPeople.LeadersTier1")]
        [TestCase("GreatPeople.LeadersTier2")]
        [TestCase("GreatPeople.LeadersTier3")]
        [TestCase("GreatPeople.FlyingAces")]
        [TestCase("GreatPeople.Heroes")]
        [TestCase("GreatPeople.Admirals")]
        [TestCase("GreatPeople.GeneralsTier1")]
        [TestCase("GreatPeople.GeneralsTier2")]
        [TestCase("GreatPeople.GeneralsTier3")]
        [TestCase("GreatPeople.PowerDeities")]
        [TestCase("GreatPeople.CreationDeities")]
        [TestCase("GreatPeople.DestructionDeities")]
        [TestCase("GreatPeople.PeaceDeities")]
        [TestCase("GreatPeople.WarDeities")]
        [TestCase("GreatPeople.VictoryDeities")]
        [TestCase("GreatPeople.DeathDeities")]
        [TestCase("GreatPeople.HatredDeities")]
        [TestCase("GreatPeople.FearDeities")]
        [TestCase("GreatPeople.SorrowDeities")]
        [TestCase("GreatPeople.BeastsDeities")]
        [TestCase("GreatPeople.TimeDeities")]
        [TestCase("GreatPeople.ProphecyDeities")]
        [TestCase("GreatPeople.JusticeDeities")]
        [TestCase("GreatPeople.ProtectionDeities")]
        [TestCase("GreatPeople.PunishmentDeities")]
        [TestCase("GreatPeople.LoyaltyDeities")]
        [TestCase("GreatPeople.DisloyaltyDeities")]
        [TestCase("GreatPeople.LabourDeities")]
        [TestCase("GreatPeople.NatureDeities")]
        [TestCase("GreatPeople.HealthDeities")]
        [TestCase("GreatPeople.LoveDeities")]
        [TestCase("GreatPeople.KnowledgeDeities")]
        [TestCase("GreatPeople.ArtDeities")]
        [TestCase("GreatPeople.FeastDeities")]
        [TestCase("GreatPeople.FortuneDeities")]
        [TestCase("GreatPeople.SleepDeities")]
        [TestCase("GreatPeople.DarknessDeities")]
        [TestCase("GreatPeople.LightDeities")]
        [TestCase("GreatPeople.SunDeities")]
        [TestCase("GreatPeople.SkyDeities")]
        [TestCase("GreatPeople.AirDeities")]
        [TestCase("GreatPeople.ColdDeities")]
        [TestCase("GreatPeople.WarmthDeities")]
        [TestCase("GreatPeople.WaterDeities")]
        [TestCase("GreatPeople.OtherDeities")]
        [TestCase("Companies.RobotManufacturers")]
        [TestCase("Companies.AutomotiveManufacturers")]
        [TestCase("Companies.AircraftManufacturers")]
        [TestCase("Companies.SpacecraftManufacturers")]
        [TestCase("Companies.WeaponManufacturers")]
        [TestCase("Companies.RocketDesigners")]
        [TestCase("Companies.ResearchCompanies")]
        [TestCase("Companies.InvestmentCompanies")]
        [TestCase("Warfare.Weapons.Artillery")]
        [TestCase("Warfare.Weapons.Guns")]
        [TestCase("Warfare.Weapons.Crossbows")]
        [TestCase("Warfare.Weapons.Bows")]
        [TestCase("Warfare.Weapons.Throwables")]
        [TestCase("Warfare.Weapons.Swords")]
        [TestCase("Warfare.Weapons.Daggers")]
        [TestCase("Warfare.Weapons.Polearms")]
        [TestCase("Warfare.Weapons.Axes")]
        [TestCase("Warfare.Weapons.Hammers")]
        [TestCase("Warfare.MilitaryUnitTypes")]
        [TestCase("Warfare.ShipTypes")]
        [TestCase("Warfare.Forts")]
        [TestCase("Warfare.BattleLocations")]
        [TestCase("BiosphereNames.Animals")]
        [TestCase("BiosphereNames.MythologicalCreatures")]
        [TestCase("Ships.Generic")]
        [TestCase("Ships.Corvette")]
        [TestCase("Ships.Destroyer")]
        [TestCase("Ships.Cruiser")]
        [TestCase("Ships.Battleship")]
        [TestCase("Ships.Titan")]
        [TestCase("Ships.Colossus")]
        [TestCase("Ships.Juggernaut")]
        [TestCase("Ships.Constructor")]
        [TestCase("Ships.Science")]
        [TestCase("Ships.Coloniser")]
        [TestCase("Ships.Transport")]
        [TestCase("Ships.IonCannon")]
        [TestCase("ShipClasses.Generic")]
        [TestCase("ShipClasses.Corvette")]
        [TestCase("ShipClasses.Destroyer")]
        [TestCase("ShipClasses.Cruiser")]
        [TestCase("ShipClasses.Battleship")]
        [TestCase("ShipClasses.Titan")]
        [TestCase("ShipClasses.Colossus")]
        [TestCase("ShipClasses.Juggernaut")]
        [TestCase("ShipClasses.Constructor")]
        [TestCase("ShipClasses.Science")]
        [TestCase("ShipClasses.Coloniser")]
        [TestCase("ShipClasses.Transport")]
        [TestCase("ShipClasses.IonCannon")]
        [TestCase("Stations.MilitaryStations.Generic")]
        [TestCase("Stations.MilitaryStations.Small")]
        [TestCase("Stations.MilitaryStations.Medium")]
        [TestCase("Stations.MilitaryStations.Large")]
        [TestCase("Stations.MiningStations")]
        [TestCase("Stations.ResearchStations")]
        [TestCase("Stations.ObservationStations")]
        [TestCase("Armies.Fleet")]
        [TestCase("Armies.DefenceArmy")]
        [TestCase("Armies.AssaultArmy")]
        [TestCase("Armies.OccupationArmy")]
        [TestCase("Armies.SlaveArmy")]
        [TestCase("Armies.CloneArmy")]
        [TestCase("Armies.RoboticDefenceArmy")]
        [TestCase("Armies.RoboticAssaultArmy")]
        [TestCase("Armies.RoboticOccupationArmy")]
        [TestCase("Armies.AndroidDefenceArmy")]
        [TestCase("Armies.AndroidAssaultArmy")]
        [TestCase("Armies.PsionicArmy")]
        [TestCase("Armies.XenomorphArmy")]
        [TestCase("Armies.SuperSoldierArmy")]
        [TestCase("Armies.PrimitiveArmy")]
        [TestCase("Armies.IndustrialArmy")]
        [TestCase("Armies.PostAtomicArmy")]
        [TestCase("Planets.Generic")]
        [TestCase("Planets.Desert")]
        [TestCase("Planets.Arid")]
        [TestCase("Planets.Tropical")]
        [TestCase("Planets.Continental")]
        [TestCase("Planets.Gaia")]
        [TestCase("Planets.Ocean")]
        [TestCase("Planets.Tundra")]
        [TestCase("Planets.Arctic")]
        [TestCase("Planets.Tomb")]
        [TestCase("Planets.Savannah")]
        [TestCase("Planets.Alpine")]
        [TestCase("Planets.Molten")]
        [TestCase("Planets.Barren")]
        [TestCase("Planets.Asteroid")]
        [TestCase("Characters")]
        public void GivenACollectionWithValues_WhenMerging_ThenBothOperandsContribute(string path)
        {
            NameList nameList = new();
            NameList other = new();

            Add(nameList, path, SourceValue);
            Add(other, path, OtherValue);
            nameList.AddRange(other);

            Assert.That(Count(nameList, path), Is.EqualTo(2));
        }

                [TestCase("Denonyms")]
        [TestCase("Places.Countries")]
        [TestCase("Places.Regions")]
        [TestCase("Places.Cities")]
        [TestCase("Places.Mountains")]
        [TestCase("Places.Forests")]
        [TestCase("Places.Deserts")]
        [TestCase("Places.Rivers")]
        [TestCase("Places.Lakes")]
        [TestCase("Places.Seas")]
        [TestCase("Places.Airports")]
        [TestCase("GreatPeople.Explorers")]
        [TestCase("GreatPeople.Pioneers")]
        [TestCase("GreatPeople.Scientists")]
        [TestCase("GreatPeople.LeadersTier1")]
        [TestCase("GreatPeople.LeadersTier2")]
        [TestCase("GreatPeople.LeadersTier3")]
        [TestCase("GreatPeople.FlyingAces")]
        [TestCase("GreatPeople.Heroes")]
        [TestCase("GreatPeople.Admirals")]
        [TestCase("GreatPeople.GeneralsTier1")]
        [TestCase("GreatPeople.GeneralsTier2")]
        [TestCase("GreatPeople.GeneralsTier3")]
        [TestCase("GreatPeople.PowerDeities")]
        [TestCase("GreatPeople.CreationDeities")]
        [TestCase("GreatPeople.DestructionDeities")]
        [TestCase("GreatPeople.PeaceDeities")]
        [TestCase("GreatPeople.WarDeities")]
        [TestCase("GreatPeople.VictoryDeities")]
        [TestCase("GreatPeople.DeathDeities")]
        [TestCase("GreatPeople.HatredDeities")]
        [TestCase("GreatPeople.FearDeities")]
        [TestCase("GreatPeople.SorrowDeities")]
        [TestCase("GreatPeople.BeastsDeities")]
        [TestCase("GreatPeople.TimeDeities")]
        [TestCase("GreatPeople.ProphecyDeities")]
        [TestCase("GreatPeople.JusticeDeities")]
        [TestCase("GreatPeople.ProtectionDeities")]
        [TestCase("GreatPeople.PunishmentDeities")]
        [TestCase("GreatPeople.LoyaltyDeities")]
        [TestCase("GreatPeople.DisloyaltyDeities")]
        [TestCase("GreatPeople.LabourDeities")]
        [TestCase("GreatPeople.NatureDeities")]
        [TestCase("GreatPeople.HealthDeities")]
        [TestCase("GreatPeople.LoveDeities")]
        [TestCase("GreatPeople.KnowledgeDeities")]
        [TestCase("GreatPeople.ArtDeities")]
        [TestCase("GreatPeople.FeastDeities")]
        [TestCase("GreatPeople.FortuneDeities")]
        [TestCase("GreatPeople.SleepDeities")]
        [TestCase("GreatPeople.DarknessDeities")]
        [TestCase("GreatPeople.LightDeities")]
        [TestCase("GreatPeople.SunDeities")]
        [TestCase("GreatPeople.SkyDeities")]
        [TestCase("GreatPeople.AirDeities")]
        [TestCase("GreatPeople.ColdDeities")]
        [TestCase("GreatPeople.WarmthDeities")]
        [TestCase("GreatPeople.WaterDeities")]
        [TestCase("GreatPeople.OtherDeities")]
        [TestCase("Companies.RobotManufacturers")]
        [TestCase("Companies.AutomotiveManufacturers")]
        [TestCase("Companies.AircraftManufacturers")]
        [TestCase("Companies.SpacecraftManufacturers")]
        [TestCase("Companies.WeaponManufacturers")]
        [TestCase("Companies.RocketDesigners")]
        [TestCase("Companies.ResearchCompanies")]
        [TestCase("Companies.InvestmentCompanies")]
        [TestCase("Warfare.Weapons.Artillery")]
        [TestCase("Warfare.Weapons.Guns")]
        [TestCase("Warfare.Weapons.Crossbows")]
        [TestCase("Warfare.Weapons.Bows")]
        [TestCase("Warfare.Weapons.Throwables")]
        [TestCase("Warfare.Weapons.Swords")]
        [TestCase("Warfare.Weapons.Daggers")]
        [TestCase("Warfare.Weapons.Polearms")]
        [TestCase("Warfare.Weapons.Axes")]
        [TestCase("Warfare.Weapons.Hammers")]
        [TestCase("Warfare.MilitaryUnitTypes")]
        [TestCase("Warfare.ShipTypes")]
        [TestCase("Warfare.Forts")]
        [TestCase("Warfare.BattleLocations")]
        [TestCase("BiosphereNames.Animals")]
        [TestCase("BiosphereNames.MythologicalCreatures")]
        [TestCase("Ships.Generic")]
        [TestCase("Ships.Corvette")]
        [TestCase("Ships.Destroyer")]
        [TestCase("Ships.Cruiser")]
        [TestCase("Ships.Battleship")]
        [TestCase("Ships.Titan")]
        [TestCase("Ships.Colossus")]
        [TestCase("Ships.Juggernaut")]
        [TestCase("Ships.Constructor")]
        [TestCase("Ships.Science")]
        [TestCase("Ships.Coloniser")]
        [TestCase("Ships.Transport")]
        [TestCase("Ships.IonCannon")]
        [TestCase("ShipClasses.Generic")]
        [TestCase("ShipClasses.Corvette")]
        [TestCase("ShipClasses.Destroyer")]
        [TestCase("ShipClasses.Cruiser")]
        [TestCase("ShipClasses.Battleship")]
        [TestCase("ShipClasses.Titan")]
        [TestCase("ShipClasses.Colossus")]
        [TestCase("ShipClasses.Juggernaut")]
        [TestCase("ShipClasses.Constructor")]
        [TestCase("ShipClasses.Science")]
        [TestCase("ShipClasses.Coloniser")]
        [TestCase("ShipClasses.Transport")]
        [TestCase("ShipClasses.IonCannon")]
        [TestCase("Stations.MilitaryStations.Generic")]
        [TestCase("Stations.MilitaryStations.Small")]
        [TestCase("Stations.MilitaryStations.Medium")]
        [TestCase("Stations.MilitaryStations.Large")]
        [TestCase("Stations.MiningStations")]
        [TestCase("Stations.ResearchStations")]
        [TestCase("Stations.ObservationStations")]
        [TestCase("Armies.Fleet")]
        [TestCase("Armies.DefenceArmy")]
        [TestCase("Armies.AssaultArmy")]
        [TestCase("Armies.OccupationArmy")]
        [TestCase("Armies.SlaveArmy")]
        [TestCase("Armies.CloneArmy")]
        [TestCase("Armies.RoboticDefenceArmy")]
        [TestCase("Armies.RoboticAssaultArmy")]
        [TestCase("Armies.RoboticOccupationArmy")]
        [TestCase("Armies.AndroidDefenceArmy")]
        [TestCase("Armies.AndroidAssaultArmy")]
        [TestCase("Armies.PsionicArmy")]
        [TestCase("Armies.XenomorphArmy")]
        [TestCase("Armies.SuperSoldierArmy")]
        [TestCase("Armies.PrimitiveArmy")]
        [TestCase("Armies.IndustrialArmy")]
        [TestCase("Armies.PostAtomicArmy")]
        [TestCase("Planets.Generic")]
        [TestCase("Planets.Desert")]
        [TestCase("Planets.Arid")]
        [TestCase("Planets.Tropical")]
        [TestCase("Planets.Continental")]
        [TestCase("Planets.Gaia")]
        [TestCase("Planets.Ocean")]
        [TestCase("Planets.Tundra")]
        [TestCase("Planets.Arctic")]
        [TestCase("Planets.Tomb")]
        [TestCase("Planets.Savannah")]
        [TestCase("Planets.Alpine")]
        [TestCase("Planets.Molten")]
        [TestCase("Planets.Barren")]
        [TestCase("Planets.Asteroid")]
        [TestCase("Characters")]
        public void GivenAnEmptyOtherCollection_WhenMerging_ThenTheCountIsUnchanged(string path)
        {
            NameList nameList = new();
            NameList other = new();

            Add(nameList, path, SourceValue);

            int before = Count(nameList, path);
            nameList.AddRange(other);

            Assert.That(Count(nameList, path), Is.EqualTo(before));
        }

                [TestCase("Denonyms")]
        [TestCase("Places.Countries")]
        [TestCase("Places.Regions")]
        [TestCase("Places.Cities")]
        [TestCase("Places.Mountains")]
        [TestCase("Places.Forests")]
        [TestCase("Places.Deserts")]
        [TestCase("Places.Rivers")]
        [TestCase("Places.Lakes")]
        [TestCase("Places.Seas")]
        [TestCase("Places.Airports")]
        [TestCase("GreatPeople.Explorers")]
        [TestCase("GreatPeople.Pioneers")]
        [TestCase("GreatPeople.Scientists")]
        [TestCase("GreatPeople.LeadersTier1")]
        [TestCase("GreatPeople.LeadersTier2")]
        [TestCase("GreatPeople.LeadersTier3")]
        [TestCase("GreatPeople.FlyingAces")]
        [TestCase("GreatPeople.Heroes")]
        [TestCase("GreatPeople.Admirals")]
        [TestCase("GreatPeople.GeneralsTier1")]
        [TestCase("GreatPeople.GeneralsTier2")]
        [TestCase("GreatPeople.GeneralsTier3")]
        [TestCase("GreatPeople.PowerDeities")]
        [TestCase("GreatPeople.CreationDeities")]
        [TestCase("GreatPeople.DestructionDeities")]
        [TestCase("GreatPeople.PeaceDeities")]
        [TestCase("GreatPeople.WarDeities")]
        [TestCase("GreatPeople.VictoryDeities")]
        [TestCase("GreatPeople.DeathDeities")]
        [TestCase("GreatPeople.HatredDeities")]
        [TestCase("GreatPeople.FearDeities")]
        [TestCase("GreatPeople.SorrowDeities")]
        [TestCase("GreatPeople.BeastsDeities")]
        [TestCase("GreatPeople.TimeDeities")]
        [TestCase("GreatPeople.ProphecyDeities")]
        [TestCase("GreatPeople.JusticeDeities")]
        [TestCase("GreatPeople.ProtectionDeities")]
        [TestCase("GreatPeople.PunishmentDeities")]
        [TestCase("GreatPeople.LoyaltyDeities")]
        [TestCase("GreatPeople.DisloyaltyDeities")]
        [TestCase("GreatPeople.LabourDeities")]
        [TestCase("GreatPeople.NatureDeities")]
        [TestCase("GreatPeople.HealthDeities")]
        [TestCase("GreatPeople.LoveDeities")]
        [TestCase("GreatPeople.KnowledgeDeities")]
        [TestCase("GreatPeople.ArtDeities")]
        [TestCase("GreatPeople.FeastDeities")]
        [TestCase("GreatPeople.FortuneDeities")]
        [TestCase("GreatPeople.SleepDeities")]
        [TestCase("GreatPeople.DarknessDeities")]
        [TestCase("GreatPeople.LightDeities")]
        [TestCase("GreatPeople.SunDeities")]
        [TestCase("GreatPeople.SkyDeities")]
        [TestCase("GreatPeople.AirDeities")]
        [TestCase("GreatPeople.ColdDeities")]
        [TestCase("GreatPeople.WarmthDeities")]
        [TestCase("GreatPeople.WaterDeities")]
        [TestCase("GreatPeople.OtherDeities")]
        [TestCase("Companies.RobotManufacturers")]
        [TestCase("Companies.AutomotiveManufacturers")]
        [TestCase("Companies.AircraftManufacturers")]
        [TestCase("Companies.SpacecraftManufacturers")]
        [TestCase("Companies.WeaponManufacturers")]
        [TestCase("Companies.RocketDesigners")]
        [TestCase("Companies.ResearchCompanies")]
        [TestCase("Companies.InvestmentCompanies")]
        [TestCase("Warfare.Weapons.Artillery")]
        [TestCase("Warfare.Weapons.Guns")]
        [TestCase("Warfare.Weapons.Crossbows")]
        [TestCase("Warfare.Weapons.Bows")]
        [TestCase("Warfare.Weapons.Throwables")]
        [TestCase("Warfare.Weapons.Swords")]
        [TestCase("Warfare.Weapons.Daggers")]
        [TestCase("Warfare.Weapons.Polearms")]
        [TestCase("Warfare.Weapons.Axes")]
        [TestCase("Warfare.Weapons.Hammers")]
        [TestCase("Warfare.MilitaryUnitTypes")]
        [TestCase("Warfare.ShipTypes")]
        [TestCase("Warfare.Forts")]
        [TestCase("Warfare.BattleLocations")]
        [TestCase("BiosphereNames.Animals")]
        [TestCase("BiosphereNames.MythologicalCreatures")]
        [TestCase("Ships.Generic")]
        [TestCase("Ships.Corvette")]
        [TestCase("Ships.Destroyer")]
        [TestCase("Ships.Cruiser")]
        [TestCase("Ships.Battleship")]
        [TestCase("Ships.Titan")]
        [TestCase("Ships.Colossus")]
        [TestCase("Ships.Juggernaut")]
        [TestCase("Ships.Constructor")]
        [TestCase("Ships.Science")]
        [TestCase("Ships.Coloniser")]
        [TestCase("Ships.Transport")]
        [TestCase("Ships.IonCannon")]
        [TestCase("ShipClasses.Generic")]
        [TestCase("ShipClasses.Corvette")]
        [TestCase("ShipClasses.Destroyer")]
        [TestCase("ShipClasses.Cruiser")]
        [TestCase("ShipClasses.Battleship")]
        [TestCase("ShipClasses.Titan")]
        [TestCase("ShipClasses.Colossus")]
        [TestCase("ShipClasses.Juggernaut")]
        [TestCase("ShipClasses.Constructor")]
        [TestCase("ShipClasses.Science")]
        [TestCase("ShipClasses.Coloniser")]
        [TestCase("ShipClasses.Transport")]
        [TestCase("ShipClasses.IonCannon")]
        [TestCase("Stations.MilitaryStations.Generic")]
        [TestCase("Stations.MilitaryStations.Small")]
        [TestCase("Stations.MilitaryStations.Medium")]
        [TestCase("Stations.MilitaryStations.Large")]
        [TestCase("Stations.MiningStations")]
        [TestCase("Stations.ResearchStations")]
        [TestCase("Stations.ObservationStations")]
        [TestCase("Armies.Fleet")]
        [TestCase("Armies.DefenceArmy")]
        [TestCase("Armies.AssaultArmy")]
        [TestCase("Armies.OccupationArmy")]
        [TestCase("Armies.SlaveArmy")]
        [TestCase("Armies.CloneArmy")]
        [TestCase("Armies.RoboticDefenceArmy")]
        [TestCase("Armies.RoboticAssaultArmy")]
        [TestCase("Armies.RoboticOccupationArmy")]
        [TestCase("Armies.AndroidDefenceArmy")]
        [TestCase("Armies.AndroidAssaultArmy")]
        [TestCase("Armies.PsionicArmy")]
        [TestCase("Armies.XenomorphArmy")]
        [TestCase("Armies.SuperSoldierArmy")]
        [TestCase("Armies.PrimitiveArmy")]
        [TestCase("Armies.IndustrialArmy")]
        [TestCase("Armies.PostAtomicArmy")]
        [TestCase("Planets.Generic")]
        [TestCase("Planets.Desert")]
        [TestCase("Planets.Arid")]
        [TestCase("Planets.Tropical")]
        [TestCase("Planets.Continental")]
        [TestCase("Planets.Gaia")]
        [TestCase("Planets.Ocean")]
        [TestCase("Planets.Tundra")]
        [TestCase("Planets.Arctic")]
        [TestCase("Planets.Tomb")]
        [TestCase("Planets.Savannah")]
        [TestCase("Planets.Alpine")]
        [TestCase("Planets.Molten")]
        [TestCase("Planets.Barren")]
        [TestCase("Planets.Asteroid")]
        [TestCase("Characters")]
        public void GivenAnEmptyTargetCollection_WhenMerging_ThenTheOtherValuesAreCopied(string path)
        {
            NameList nameList = new();
            NameList other = new();

            Add(other, path, OtherValue);
            nameList.AddRange(other);

            Assert.That(Count(nameList, path), Is.EqualTo(1));
        }

        private static NameGroup Group(string value)
            => new() { Name = value, ExplicitValues = [value] };

        private static void Add(NameList nameList, string path, string value)
        {
            switch (path)
            {
                case "Denonyms": nameList.Denonyms.Add(Group(value)); break;
                case "Places.Countries": nameList.Places.Countries.Add(Group(value)); break;
                case "Places.Regions": nameList.Places.Regions.Add(Group(value)); break;
                case "Places.Cities": nameList.Places.Cities.Add(Group(value)); break;
                case "Places.Mountains": nameList.Places.Mountains.Add(Group(value)); break;
                case "Places.Forests": nameList.Places.Forests.Add(Group(value)); break;
                case "Places.Deserts": nameList.Places.Deserts.Add(Group(value)); break;
                case "Places.Rivers": nameList.Places.Rivers.Add(Group(value)); break;
                case "Places.Lakes": nameList.Places.Lakes.Add(Group(value)); break;
                case "Places.Seas": nameList.Places.Seas.Add(Group(value)); break;
                case "Places.Airports": nameList.Places.Airports.Add(Group(value)); break;
                case "GreatPeople.Explorers": nameList.GreatPeople.Explorers.Add(Group(value)); break;
                case "GreatPeople.Pioneers": nameList.GreatPeople.Pioneers.Add(Group(value)); break;
                case "GreatPeople.Scientists": nameList.GreatPeople.Scientists.Add(Group(value)); break;
                case "GreatPeople.LeadersTier1": nameList.GreatPeople.LeadersTier1.Add(Group(value)); break;
                case "GreatPeople.LeadersTier2": nameList.GreatPeople.LeadersTier2.Add(Group(value)); break;
                case "GreatPeople.LeadersTier3": nameList.GreatPeople.LeadersTier3.Add(Group(value)); break;
                case "GreatPeople.FlyingAces": nameList.GreatPeople.FlyingAces.Add(Group(value)); break;
                case "GreatPeople.Heroes": nameList.GreatPeople.Heroes.Add(Group(value)); break;
                case "GreatPeople.Admirals": nameList.GreatPeople.Admirals.Add(Group(value)); break;
                case "GreatPeople.GeneralsTier1": nameList.GreatPeople.GeneralsTier1.Add(Group(value)); break;
                case "GreatPeople.GeneralsTier2": nameList.GreatPeople.GeneralsTier2.Add(Group(value)); break;
                case "GreatPeople.GeneralsTier3": nameList.GreatPeople.GeneralsTier3.Add(Group(value)); break;
                case "GreatPeople.PowerDeities": nameList.GreatPeople.PowerDeities.Add(Group(value)); break;
                case "GreatPeople.CreationDeities": nameList.GreatPeople.CreationDeities.Add(Group(value)); break;
                case "GreatPeople.DestructionDeities": nameList.GreatPeople.DestructionDeities.Add(Group(value)); break;
                case "GreatPeople.PeaceDeities": nameList.GreatPeople.PeaceDeities.Add(Group(value)); break;
                case "GreatPeople.WarDeities": nameList.GreatPeople.WarDeities.Add(Group(value)); break;
                case "GreatPeople.VictoryDeities": nameList.GreatPeople.VictoryDeities.Add(Group(value)); break;
                case "GreatPeople.DeathDeities": nameList.GreatPeople.DeathDeities.Add(Group(value)); break;
                case "GreatPeople.HatredDeities": nameList.GreatPeople.HatredDeities.Add(Group(value)); break;
                case "GreatPeople.FearDeities": nameList.GreatPeople.FearDeities.Add(Group(value)); break;
                case "GreatPeople.SorrowDeities": nameList.GreatPeople.SorrowDeities.Add(Group(value)); break;
                case "GreatPeople.BeastsDeities": nameList.GreatPeople.BeastsDeities.Add(Group(value)); break;
                case "GreatPeople.TimeDeities": nameList.GreatPeople.TimeDeities.Add(Group(value)); break;
                case "GreatPeople.ProphecyDeities": nameList.GreatPeople.ProphecyDeities.Add(Group(value)); break;
                case "GreatPeople.JusticeDeities": nameList.GreatPeople.JusticeDeities.Add(Group(value)); break;
                case "GreatPeople.ProtectionDeities": nameList.GreatPeople.ProtectionDeities.Add(Group(value)); break;
                case "GreatPeople.PunishmentDeities": nameList.GreatPeople.PunishmentDeities.Add(Group(value)); break;
                case "GreatPeople.LoyaltyDeities": nameList.GreatPeople.LoyaltyDeities.Add(Group(value)); break;
                case "GreatPeople.DisloyaltyDeities": nameList.GreatPeople.DisloyaltyDeities.Add(Group(value)); break;
                case "GreatPeople.LabourDeities": nameList.GreatPeople.LabourDeities.Add(Group(value)); break;
                case "GreatPeople.NatureDeities": nameList.GreatPeople.NatureDeities.Add(Group(value)); break;
                case "GreatPeople.HealthDeities": nameList.GreatPeople.HealthDeities.Add(Group(value)); break;
                case "GreatPeople.LoveDeities": nameList.GreatPeople.LoveDeities.Add(Group(value)); break;
                case "GreatPeople.KnowledgeDeities": nameList.GreatPeople.KnowledgeDeities.Add(Group(value)); break;
                case "GreatPeople.ArtDeities": nameList.GreatPeople.ArtDeities.Add(Group(value)); break;
                case "GreatPeople.FeastDeities": nameList.GreatPeople.FeastDeities.Add(Group(value)); break;
                case "GreatPeople.FortuneDeities": nameList.GreatPeople.FortuneDeities.Add(Group(value)); break;
                case "GreatPeople.SleepDeities": nameList.GreatPeople.SleepDeities.Add(Group(value)); break;
                case "GreatPeople.DarknessDeities": nameList.GreatPeople.DarknessDeities.Add(Group(value)); break;
                case "GreatPeople.LightDeities": nameList.GreatPeople.LightDeities.Add(Group(value)); break;
                case "GreatPeople.SunDeities": nameList.GreatPeople.SunDeities.Add(Group(value)); break;
                case "GreatPeople.SkyDeities": nameList.GreatPeople.SkyDeities.Add(Group(value)); break;
                case "GreatPeople.AirDeities": nameList.GreatPeople.AirDeities.Add(Group(value)); break;
                case "GreatPeople.ColdDeities": nameList.GreatPeople.ColdDeities.Add(Group(value)); break;
                case "GreatPeople.WarmthDeities": nameList.GreatPeople.WarmthDeities.Add(Group(value)); break;
                case "GreatPeople.WaterDeities": nameList.GreatPeople.WaterDeities.Add(Group(value)); break;
                case "GreatPeople.OtherDeities": nameList.GreatPeople.OtherDeities.Add(Group(value)); break;
                case "Companies.RobotManufacturers": nameList.Companies.RobotManufacturers.Add(Group(value)); break;
                case "Companies.AutomotiveManufacturers": nameList.Companies.AutomotiveManufacturers.Add(Group(value)); break;
                case "Companies.AircraftManufacturers": nameList.Companies.AircraftManufacturers.Add(Group(value)); break;
                case "Companies.SpacecraftManufacturers": nameList.Companies.SpacecraftManufacturers.Add(Group(value)); break;
                case "Companies.WeaponManufacturers": nameList.Companies.WeaponManufacturers.Add(Group(value)); break;
                case "Companies.RocketDesigners": nameList.Companies.RocketDesigners.Add(Group(value)); break;
                case "Companies.ResearchCompanies": nameList.Companies.ResearchCompanies.Add(Group(value)); break;
                case "Companies.InvestmentCompanies": nameList.Companies.InvestmentCompanies.Add(Group(value)); break;
                case "Warfare.Weapons.Artillery": nameList.Warfare.Weapons.Artillery.Add(Group(value)); break;
                case "Warfare.Weapons.Guns": nameList.Warfare.Weapons.Guns.Add(Group(value)); break;
                case "Warfare.Weapons.Crossbows": nameList.Warfare.Weapons.Crossbows.Add(Group(value)); break;
                case "Warfare.Weapons.Bows": nameList.Warfare.Weapons.Bows.Add(Group(value)); break;
                case "Warfare.Weapons.Throwables": nameList.Warfare.Weapons.Throwables.Add(Group(value)); break;
                case "Warfare.Weapons.Swords": nameList.Warfare.Weapons.Swords.Add(Group(value)); break;
                case "Warfare.Weapons.Daggers": nameList.Warfare.Weapons.Daggers.Add(Group(value)); break;
                case "Warfare.Weapons.Polearms": nameList.Warfare.Weapons.Polearms.Add(Group(value)); break;
                case "Warfare.Weapons.Axes": nameList.Warfare.Weapons.Axes.Add(Group(value)); break;
                case "Warfare.Weapons.Hammers": nameList.Warfare.Weapons.Hammers.Add(Group(value)); break;
                case "Warfare.MilitaryUnitTypes": nameList.Warfare.MilitaryUnitTypes.Add(Group(value)); break;
                case "Warfare.ShipTypes": nameList.Warfare.ShipTypes.Add(Group(value)); break;
                case "Warfare.Forts": nameList.Warfare.Forts.Add(Group(value)); break;
                case "Warfare.BattleLocations": nameList.Warfare.BattleLocations.Add(Group(value)); break;
                case "BiosphereNames.Animals": nameList.BiosphereNames.Animals.Add(Group(value)); break;
                case "BiosphereNames.MythologicalCreatures": nameList.BiosphereNames.MythologicalCreatures.Add(Group(value)); break;
                case "Ships.Generic": nameList.Ships.Generic.Add(Group(value)); break;
                case "Ships.Corvette": nameList.Ships.Corvette.Add(Group(value)); break;
                case "Ships.Destroyer": nameList.Ships.Destroyer.Add(Group(value)); break;
                case "Ships.Cruiser": nameList.Ships.Cruiser.Add(Group(value)); break;
                case "Ships.Battleship": nameList.Ships.Battleship.Add(Group(value)); break;
                case "Ships.Titan": nameList.Ships.Titan.Add(Group(value)); break;
                case "Ships.Colossus": nameList.Ships.Colossus.Add(Group(value)); break;
                case "Ships.Juggernaut": nameList.Ships.Juggernaut.Add(Group(value)); break;
                case "Ships.Constructor": nameList.Ships.Constructor.Add(Group(value)); break;
                case "Ships.Science": nameList.Ships.Science.Add(Group(value)); break;
                case "Ships.Coloniser": nameList.Ships.Coloniser.Add(Group(value)); break;
                case "Ships.Transport": nameList.Ships.Transport.Add(Group(value)); break;
                case "Ships.IonCannon": nameList.Ships.IonCannon.Add(Group(value)); break;
                case "ShipClasses.Generic": nameList.ShipClasses.Generic.Add(Group(value)); break;
                case "ShipClasses.Corvette": nameList.ShipClasses.Corvette.Add(Group(value)); break;
                case "ShipClasses.Destroyer": nameList.ShipClasses.Destroyer.Add(Group(value)); break;
                case "ShipClasses.Cruiser": nameList.ShipClasses.Cruiser.Add(Group(value)); break;
                case "ShipClasses.Battleship": nameList.ShipClasses.Battleship.Add(Group(value)); break;
                case "ShipClasses.Titan": nameList.ShipClasses.Titan.Add(Group(value)); break;
                case "ShipClasses.Colossus": nameList.ShipClasses.Colossus.Add(Group(value)); break;
                case "ShipClasses.Juggernaut": nameList.ShipClasses.Juggernaut.Add(Group(value)); break;
                case "ShipClasses.Constructor": nameList.ShipClasses.Constructor.Add(Group(value)); break;
                case "ShipClasses.Science": nameList.ShipClasses.Science.Add(Group(value)); break;
                case "ShipClasses.Coloniser": nameList.ShipClasses.Coloniser.Add(Group(value)); break;
                case "ShipClasses.Transport": nameList.ShipClasses.Transport.Add(Group(value)); break;
                case "ShipClasses.IonCannon": nameList.ShipClasses.IonCannon.Add(Group(value)); break;
                case "Stations.MilitaryStations.Generic": nameList.Stations.MilitaryStations.Generic.Add(Group(value)); break;
                case "Stations.MilitaryStations.Small": nameList.Stations.MilitaryStations.Small.Add(Group(value)); break;
                case "Stations.MilitaryStations.Medium": nameList.Stations.MilitaryStations.Medium.Add(Group(value)); break;
                case "Stations.MilitaryStations.Large": nameList.Stations.MilitaryStations.Large.Add(Group(value)); break;
                case "Stations.MiningStations": nameList.Stations.MiningStations.Add(Group(value)); break;
                case "Stations.ResearchStations": nameList.Stations.ResearchStations.Add(Group(value)); break;
                case "Stations.ObservationStations": nameList.Stations.ObservationStations.Add(Group(value)); break;
                case "Armies.Fleet": nameList.Armies.Fleet.Add(Group(value)); break;
                case "Armies.DefenceArmy": nameList.Armies.DefenceArmy.Add(Group(value)); break;
                case "Armies.AssaultArmy": nameList.Armies.AssaultArmy.Add(Group(value)); break;
                case "Armies.OccupationArmy": nameList.Armies.OccupationArmy.Add(Group(value)); break;
                case "Armies.SlaveArmy": nameList.Armies.SlaveArmy.Add(Group(value)); break;
                case "Armies.CloneArmy": nameList.Armies.CloneArmy.Add(Group(value)); break;
                case "Armies.RoboticDefenceArmy": nameList.Armies.RoboticDefenceArmy.Add(Group(value)); break;
                case "Armies.RoboticAssaultArmy": nameList.Armies.RoboticAssaultArmy.Add(Group(value)); break;
                case "Armies.RoboticOccupationArmy": nameList.Armies.RoboticOccupationArmy.Add(Group(value)); break;
                case "Armies.AndroidDefenceArmy": nameList.Armies.AndroidDefenceArmy.Add(Group(value)); break;
                case "Armies.AndroidAssaultArmy": nameList.Armies.AndroidAssaultArmy.Add(Group(value)); break;
                case "Armies.PsionicArmy": nameList.Armies.PsionicArmy.Add(Group(value)); break;
                case "Armies.XenomorphArmy": nameList.Armies.XenomorphArmy.Add(Group(value)); break;
                case "Armies.SuperSoldierArmy": nameList.Armies.SuperSoldierArmy.Add(Group(value)); break;
                case "Armies.PrimitiveArmy": nameList.Armies.PrimitiveArmy.Add(Group(value)); break;
                case "Armies.IndustrialArmy": nameList.Armies.IndustrialArmy.Add(Group(value)); break;
                case "Armies.PostAtomicArmy": nameList.Armies.PostAtomicArmy.Add(Group(value)); break;
                case "Planets.Generic": nameList.Planets.Generic.Add(Group(value)); break;
                case "Planets.Desert": nameList.Planets.Desert.Add(Group(value)); break;
                case "Planets.Arid": nameList.Planets.Arid.Add(Group(value)); break;
                case "Planets.Tropical": nameList.Planets.Tropical.Add(Group(value)); break;
                case "Planets.Continental": nameList.Planets.Continental.Add(Group(value)); break;
                case "Planets.Gaia": nameList.Planets.Gaia.Add(Group(value)); break;
                case "Planets.Ocean": nameList.Planets.Ocean.Add(Group(value)); break;
                case "Planets.Tundra": nameList.Planets.Tundra.Add(Group(value)); break;
                case "Planets.Arctic": nameList.Planets.Arctic.Add(Group(value)); break;
                case "Planets.Tomb": nameList.Planets.Tomb.Add(Group(value)); break;
                case "Planets.Savannah": nameList.Planets.Savannah.Add(Group(value)); break;
                case "Planets.Alpine": nameList.Planets.Alpine.Add(Group(value)); break;
                case "Planets.Molten": nameList.Planets.Molten.Add(Group(value)); break;
                case "Planets.Barren": nameList.Planets.Barren.Add(Group(value)); break;
                case "Planets.Asteroid": nameList.Planets.Asteroid.Add(Group(value)); break;
                case "Characters": nameList.Characters.Add(new CharacterNames { Id = value, Weight = 1 }); break;
                default: throw new ArgumentOutOfRangeException(nameof(path), path, "Unknown collection");
            }
        }

        private static int Count(NameList nameList, string path)
            => path switch
            {
                "Denonyms" => nameList.Denonyms.Count,
                "Places.Countries" => nameList.Places.Countries.Count,
                "Places.Regions" => nameList.Places.Regions.Count,
                "Places.Cities" => nameList.Places.Cities.Count,
                "Places.Mountains" => nameList.Places.Mountains.Count,
                "Places.Forests" => nameList.Places.Forests.Count,
                "Places.Deserts" => nameList.Places.Deserts.Count,
                "Places.Rivers" => nameList.Places.Rivers.Count,
                "Places.Lakes" => nameList.Places.Lakes.Count,
                "Places.Seas" => nameList.Places.Seas.Count,
                "Places.Airports" => nameList.Places.Airports.Count,
                "GreatPeople.Explorers" => nameList.GreatPeople.Explorers.Count,
                "GreatPeople.Pioneers" => nameList.GreatPeople.Pioneers.Count,
                "GreatPeople.Scientists" => nameList.GreatPeople.Scientists.Count,
                "GreatPeople.LeadersTier1" => nameList.GreatPeople.LeadersTier1.Count,
                "GreatPeople.LeadersTier2" => nameList.GreatPeople.LeadersTier2.Count,
                "GreatPeople.LeadersTier3" => nameList.GreatPeople.LeadersTier3.Count,
                "GreatPeople.FlyingAces" => nameList.GreatPeople.FlyingAces.Count,
                "GreatPeople.Heroes" => nameList.GreatPeople.Heroes.Count,
                "GreatPeople.Admirals" => nameList.GreatPeople.Admirals.Count,
                "GreatPeople.GeneralsTier1" => nameList.GreatPeople.GeneralsTier1.Count,
                "GreatPeople.GeneralsTier2" => nameList.GreatPeople.GeneralsTier2.Count,
                "GreatPeople.GeneralsTier3" => nameList.GreatPeople.GeneralsTier3.Count,
                "GreatPeople.PowerDeities" => nameList.GreatPeople.PowerDeities.Count,
                "GreatPeople.CreationDeities" => nameList.GreatPeople.CreationDeities.Count,
                "GreatPeople.DestructionDeities" => nameList.GreatPeople.DestructionDeities.Count,
                "GreatPeople.PeaceDeities" => nameList.GreatPeople.PeaceDeities.Count,
                "GreatPeople.WarDeities" => nameList.GreatPeople.WarDeities.Count,
                "GreatPeople.VictoryDeities" => nameList.GreatPeople.VictoryDeities.Count,
                "GreatPeople.DeathDeities" => nameList.GreatPeople.DeathDeities.Count,
                "GreatPeople.HatredDeities" => nameList.GreatPeople.HatredDeities.Count,
                "GreatPeople.FearDeities" => nameList.GreatPeople.FearDeities.Count,
                "GreatPeople.SorrowDeities" => nameList.GreatPeople.SorrowDeities.Count,
                "GreatPeople.BeastsDeities" => nameList.GreatPeople.BeastsDeities.Count,
                "GreatPeople.TimeDeities" => nameList.GreatPeople.TimeDeities.Count,
                "GreatPeople.ProphecyDeities" => nameList.GreatPeople.ProphecyDeities.Count,
                "GreatPeople.JusticeDeities" => nameList.GreatPeople.JusticeDeities.Count,
                "GreatPeople.ProtectionDeities" => nameList.GreatPeople.ProtectionDeities.Count,
                "GreatPeople.PunishmentDeities" => nameList.GreatPeople.PunishmentDeities.Count,
                "GreatPeople.LoyaltyDeities" => nameList.GreatPeople.LoyaltyDeities.Count,
                "GreatPeople.DisloyaltyDeities" => nameList.GreatPeople.DisloyaltyDeities.Count,
                "GreatPeople.LabourDeities" => nameList.GreatPeople.LabourDeities.Count,
                "GreatPeople.NatureDeities" => nameList.GreatPeople.NatureDeities.Count,
                "GreatPeople.HealthDeities" => nameList.GreatPeople.HealthDeities.Count,
                "GreatPeople.LoveDeities" => nameList.GreatPeople.LoveDeities.Count,
                "GreatPeople.KnowledgeDeities" => nameList.GreatPeople.KnowledgeDeities.Count,
                "GreatPeople.ArtDeities" => nameList.GreatPeople.ArtDeities.Count,
                "GreatPeople.FeastDeities" => nameList.GreatPeople.FeastDeities.Count,
                "GreatPeople.FortuneDeities" => nameList.GreatPeople.FortuneDeities.Count,
                "GreatPeople.SleepDeities" => nameList.GreatPeople.SleepDeities.Count,
                "GreatPeople.DarknessDeities" => nameList.GreatPeople.DarknessDeities.Count,
                "GreatPeople.LightDeities" => nameList.GreatPeople.LightDeities.Count,
                "GreatPeople.SunDeities" => nameList.GreatPeople.SunDeities.Count,
                "GreatPeople.SkyDeities" => nameList.GreatPeople.SkyDeities.Count,
                "GreatPeople.AirDeities" => nameList.GreatPeople.AirDeities.Count,
                "GreatPeople.ColdDeities" => nameList.GreatPeople.ColdDeities.Count,
                "GreatPeople.WarmthDeities" => nameList.GreatPeople.WarmthDeities.Count,
                "GreatPeople.WaterDeities" => nameList.GreatPeople.WaterDeities.Count,
                "GreatPeople.OtherDeities" => nameList.GreatPeople.OtherDeities.Count,
                "Companies.RobotManufacturers" => nameList.Companies.RobotManufacturers.Count,
                "Companies.AutomotiveManufacturers" => nameList.Companies.AutomotiveManufacturers.Count,
                "Companies.AircraftManufacturers" => nameList.Companies.AircraftManufacturers.Count,
                "Companies.SpacecraftManufacturers" => nameList.Companies.SpacecraftManufacturers.Count,
                "Companies.WeaponManufacturers" => nameList.Companies.WeaponManufacturers.Count,
                "Companies.RocketDesigners" => nameList.Companies.RocketDesigners.Count,
                "Companies.ResearchCompanies" => nameList.Companies.ResearchCompanies.Count,
                "Companies.InvestmentCompanies" => nameList.Companies.InvestmentCompanies.Count,
                "Warfare.Weapons.Artillery" => nameList.Warfare.Weapons.Artillery.Count,
                "Warfare.Weapons.Guns" => nameList.Warfare.Weapons.Guns.Count,
                "Warfare.Weapons.Crossbows" => nameList.Warfare.Weapons.Crossbows.Count,
                "Warfare.Weapons.Bows" => nameList.Warfare.Weapons.Bows.Count,
                "Warfare.Weapons.Throwables" => nameList.Warfare.Weapons.Throwables.Count,
                "Warfare.Weapons.Swords" => nameList.Warfare.Weapons.Swords.Count,
                "Warfare.Weapons.Daggers" => nameList.Warfare.Weapons.Daggers.Count,
                "Warfare.Weapons.Polearms" => nameList.Warfare.Weapons.Polearms.Count,
                "Warfare.Weapons.Axes" => nameList.Warfare.Weapons.Axes.Count,
                "Warfare.Weapons.Hammers" => nameList.Warfare.Weapons.Hammers.Count,
                "Warfare.MilitaryUnitTypes" => nameList.Warfare.MilitaryUnitTypes.Count,
                "Warfare.ShipTypes" => nameList.Warfare.ShipTypes.Count,
                "Warfare.Forts" => nameList.Warfare.Forts.Count,
                "Warfare.BattleLocations" => nameList.Warfare.BattleLocations.Count,
                "BiosphereNames.Animals" => nameList.BiosphereNames.Animals.Count,
                "BiosphereNames.MythologicalCreatures" => nameList.BiosphereNames.MythologicalCreatures.Count,
                "Ships.Generic" => nameList.Ships.Generic.Count,
                "Ships.Corvette" => nameList.Ships.Corvette.Count,
                "Ships.Destroyer" => nameList.Ships.Destroyer.Count,
                "Ships.Cruiser" => nameList.Ships.Cruiser.Count,
                "Ships.Battleship" => nameList.Ships.Battleship.Count,
                "Ships.Titan" => nameList.Ships.Titan.Count,
                "Ships.Colossus" => nameList.Ships.Colossus.Count,
                "Ships.Juggernaut" => nameList.Ships.Juggernaut.Count,
                "Ships.Constructor" => nameList.Ships.Constructor.Count,
                "Ships.Science" => nameList.Ships.Science.Count,
                "Ships.Coloniser" => nameList.Ships.Coloniser.Count,
                "Ships.Transport" => nameList.Ships.Transport.Count,
                "Ships.IonCannon" => nameList.Ships.IonCannon.Count,
                "ShipClasses.Generic" => nameList.ShipClasses.Generic.Count,
                "ShipClasses.Corvette" => nameList.ShipClasses.Corvette.Count,
                "ShipClasses.Destroyer" => nameList.ShipClasses.Destroyer.Count,
                "ShipClasses.Cruiser" => nameList.ShipClasses.Cruiser.Count,
                "ShipClasses.Battleship" => nameList.ShipClasses.Battleship.Count,
                "ShipClasses.Titan" => nameList.ShipClasses.Titan.Count,
                "ShipClasses.Colossus" => nameList.ShipClasses.Colossus.Count,
                "ShipClasses.Juggernaut" => nameList.ShipClasses.Juggernaut.Count,
                "ShipClasses.Constructor" => nameList.ShipClasses.Constructor.Count,
                "ShipClasses.Science" => nameList.ShipClasses.Science.Count,
                "ShipClasses.Coloniser" => nameList.ShipClasses.Coloniser.Count,
                "ShipClasses.Transport" => nameList.ShipClasses.Transport.Count,
                "ShipClasses.IonCannon" => nameList.ShipClasses.IonCannon.Count,
                "Stations.MilitaryStations.Generic" => nameList.Stations.MilitaryStations.Generic.Count,
                "Stations.MilitaryStations.Small" => nameList.Stations.MilitaryStations.Small.Count,
                "Stations.MilitaryStations.Medium" => nameList.Stations.MilitaryStations.Medium.Count,
                "Stations.MilitaryStations.Large" => nameList.Stations.MilitaryStations.Large.Count,
                "Stations.MiningStations" => nameList.Stations.MiningStations.Count,
                "Stations.ResearchStations" => nameList.Stations.ResearchStations.Count,
                "Stations.ObservationStations" => nameList.Stations.ObservationStations.Count,
                "Armies.Fleet" => nameList.Armies.Fleet.Count,
                "Armies.DefenceArmy" => nameList.Armies.DefenceArmy.Count,
                "Armies.AssaultArmy" => nameList.Armies.AssaultArmy.Count,
                "Armies.OccupationArmy" => nameList.Armies.OccupationArmy.Count,
                "Armies.SlaveArmy" => nameList.Armies.SlaveArmy.Count,
                "Armies.CloneArmy" => nameList.Armies.CloneArmy.Count,
                "Armies.RoboticDefenceArmy" => nameList.Armies.RoboticDefenceArmy.Count,
                "Armies.RoboticAssaultArmy" => nameList.Armies.RoboticAssaultArmy.Count,
                "Armies.RoboticOccupationArmy" => nameList.Armies.RoboticOccupationArmy.Count,
                "Armies.AndroidDefenceArmy" => nameList.Armies.AndroidDefenceArmy.Count,
                "Armies.AndroidAssaultArmy" => nameList.Armies.AndroidAssaultArmy.Count,
                "Armies.PsionicArmy" => nameList.Armies.PsionicArmy.Count,
                "Armies.XenomorphArmy" => nameList.Armies.XenomorphArmy.Count,
                "Armies.SuperSoldierArmy" => nameList.Armies.SuperSoldierArmy.Count,
                "Armies.PrimitiveArmy" => nameList.Armies.PrimitiveArmy.Count,
                "Armies.IndustrialArmy" => nameList.Armies.IndustrialArmy.Count,
                "Armies.PostAtomicArmy" => nameList.Armies.PostAtomicArmy.Count,
                "Planets.Generic" => nameList.Planets.Generic.Count,
                "Planets.Desert" => nameList.Planets.Desert.Count,
                "Planets.Arid" => nameList.Planets.Arid.Count,
                "Planets.Tropical" => nameList.Planets.Tropical.Count,
                "Planets.Continental" => nameList.Planets.Continental.Count,
                "Planets.Gaia" => nameList.Planets.Gaia.Count,
                "Planets.Ocean" => nameList.Planets.Ocean.Count,
                "Planets.Tundra" => nameList.Planets.Tundra.Count,
                "Planets.Arctic" => nameList.Planets.Arctic.Count,
                "Planets.Tomb" => nameList.Planets.Tomb.Count,
                "Planets.Savannah" => nameList.Planets.Savannah.Count,
                "Planets.Alpine" => nameList.Planets.Alpine.Count,
                "Planets.Molten" => nameList.Planets.Molten.Count,
                "Planets.Barren" => nameList.Planets.Barren.Count,
                "Planets.Asteroid" => nameList.Planets.Asteroid.Count,
                "Characters" => nameList.Characters.Count,
                _ => throw new ArgumentOutOfRangeException(nameof(path), path, "Unknown collection"),
            };
    }
}
