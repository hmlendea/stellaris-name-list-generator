using System.Collections.Generic;

using NuciDAL.DataObjects;

namespace StellarisNameListGenerator.Models
{
    public class NameList : EntityBase
    {
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        
        public List<NameGroup> Denonyms { get; set; }
        public PlaceNames Places { get; set; }
        public GreatPeople GreatPeople { get; set; }

        public CompanyNames Companies { get; set; }
        public WarfareNames Warfare { get; set; }
        public BiosphereNames BiosphereNames { get; set; }

        public ShipNames Ships { get; set; }
        public ShipNames ShipClasses { get; set; }

        public StationNames Stations { get; set; }
        public StationNames StationClasses { get; set; }

        public ArmyNames Armies { get; set; }

        public PlanetNames Planets { get; set; }

        public List<CharacterNames> Characters { get; set; }

        public NameList()
        {
            Name = string.Empty;
            IsLocked = true;

            Denonyms = new List<NameGroup>();
            Places = new PlaceNames();
            GreatPeople = new GreatPeople();

            Companies = new CompanyNames();
            Warfare = new WarfareNames();
            BiosphereNames = new BiosphereNames();

            Ships = new ShipNames();
            ShipClasses = new ShipNames();

            Stations = new StationNames();
            StationClasses = new StationNames();

            Armies = new ArmyNames();

            Planets = new PlanetNames();

            Characters = new List<CharacterNames>();
        }

        public void AddRange(NameList other)
        {
            Denonyms.AddRange(other.Denonyms);

            Places.Countries.AddRange(other.Places.Countries);
            Places.Regions.AddRange(other.Places.Regions);
            Places.Cities.AddRange(other.Places.Cities);

            Places.Mountains.AddRange(other.Places.Mountains);
            Places.Forests.AddRange(other.Places.Forests);
            Places.Deserts.AddRange(other.Places.Deserts);

            Places.Rivers.AddRange(other.Places.Rivers);
            Places.Lakes.AddRange(other.Places.Lakes);
            Places.Seas.AddRange(other.Places.Seas);

            GreatPeople.Explorers.AddRange(other.GreatPeople.Explorers);
            GreatPeople.Pioneers.AddRange(other.GreatPeople.Pioneers);
            GreatPeople.Scientists.AddRange(other.GreatPeople.Scientists);

            GreatPeople.LeadersTier1.AddRange(other.GreatPeople.LeadersTier1);
            GreatPeople.LeadersTier2.AddRange(other.GreatPeople.LeadersTier2);
            GreatPeople.LeadersTier3.AddRange(other.GreatPeople.LeadersTier3);

            GreatPeople.FlyingAces.AddRange(other.GreatPeople.FlyingAces);
            GreatPeople.Heroes.AddRange(other.GreatPeople.Heroes);
            GreatPeople.Admirals.AddRange(other.GreatPeople.Admirals);

            GreatPeople.GeneralsTier1.AddRange(other.GreatPeople.GeneralsTier1);
            GreatPeople.GeneralsTier2.AddRange(other.GreatPeople.GeneralsTier2);
            GreatPeople.GeneralsTier3.AddRange(other.GreatPeople.GeneralsTier3);

            GreatPeople.Deities.AddRange(other.GreatPeople.Deities);
            GreatPeople.WarDeities.AddRange(other.GreatPeople.WarDeities);
            GreatPeople.DeathDeities.AddRange(other.GreatPeople.DeathDeities);
            GreatPeople.FearDeities.AddRange(other.GreatPeople.FearDeities);
            GreatPeople.LabourDeities.AddRange(other.GreatPeople.LabourDeities);
            GreatPeople.NatureDeities.AddRange(other.GreatPeople.NatureDeities);
            GreatPeople.HealthDeities.AddRange(other.GreatPeople.HealthDeities);
            GreatPeople.FeastDeities.AddRange(other.GreatPeople.FeastDeities);
            GreatPeople.WealthDeities.AddRange(other.GreatPeople.WealthDeities);
            GreatPeople.SunDeities.AddRange(other.GreatPeople.SunDeities);
            GreatPeople.SkyDeities.AddRange(other.GreatPeople.SkyDeities);
            GreatPeople.AirDeities.AddRange(other.GreatPeople.AirDeities);
            GreatPeople.ColdDeities.AddRange(other.GreatPeople.ColdDeities);
            GreatPeople.WaterDeities.AddRange(other.GreatPeople.WaterDeities);

            Companies.AutomotiveManufacturers.AddRange(other.Companies.AutomotiveManufacturers);
            Companies.AircraftManufacturers.AddRange(other.Companies.AircraftManufacturers);
            Companies.SpacecraftManufacturers.AddRange(other.Companies.SpacecraftManufacturers);
            Companies.RocketDesigners.AddRange(other.Companies.RocketDesigners);

            Companies.ResearchCompanies.AddRange(other.Companies.ResearchCompanies);
            Companies.InvestmentCompanies.AddRange(other.Companies.InvestmentCompanies);
            
            Warfare.Weapons.Artillery.AddRange(other.Warfare.Weapons.Artillery);
            Warfare.Weapons.Guns.AddRange(other.Warfare.Weapons.Guns);
            Warfare.Weapons.Crossbows.AddRange(other.Warfare.Weapons.Crossbows);
            Warfare.Weapons.Bows.AddRange(other.Warfare.Weapons.Bows);
            Warfare.Weapons.Javelins.AddRange(other.Warfare.Weapons.Javelins);
            Warfare.Weapons.Swords.AddRange(other.Warfare.Weapons.Swords);
            Warfare.Weapons.Daggers.AddRange(other.Warfare.Weapons.Daggers);
            Warfare.Weapons.Polearms.AddRange(other.Warfare.Weapons.Polearms);
            Warfare.Weapons.Axes.AddRange(other.Warfare.Weapons.Axes);
            Warfare.Weapons.Hammers.AddRange(other.Warfare.Weapons.Hammers);

            Warfare.MilitaryUnitTypes.AddRange(other.Warfare.MilitaryUnitTypes);
            Warfare.ShipTypes.AddRange(other.Warfare.ShipTypes);

            Warfare.Forts.AddRange(other.Warfare.Forts);
            Warfare.BattleLocations.AddRange(other.Warfare.BattleLocations);
            
            BiosphereNames.Animals.AddRange(other.BiosphereNames.Animals);
            BiosphereNames.MythologicalCreatures.AddRange(other.BiosphereNames.MythologicalCreatures);
            
            Ships.Generic.AddRange(other.Ships.Generic);
            Ships.Corvette.AddRange(other.Ships.Corvette);
            Ships.Destroyer.AddRange(other.Ships.Destroyer);
            Ships.Cruiser.AddRange(other.Ships.Cruiser);
            Ships.Battleship.AddRange(other.Ships.Battleship);
            Ships.Titan.AddRange(other.Ships.Titan);
            Ships.Colossus.AddRange(other.Ships.Colossus);
            Ships.Juggernaut.AddRange(other.Ships.Juggernaut);
            Ships.Constructor.AddRange(other.Ships.Constructor);
            Ships.Science.AddRange(other.Ships.Science);
            Ships.Coloniser.AddRange(other.Ships.Coloniser);
            Ships.Transport.AddRange(other.Ships.Transport);
            Ships.IonCannon.AddRange(other.Ships.IonCannon);

            ShipClasses.Generic.AddRange(other.ShipClasses.Generic);
            ShipClasses.Corvette.AddRange(other.ShipClasses.Corvette);
            ShipClasses.Destroyer.AddRange(other.ShipClasses.Destroyer);
            ShipClasses.Cruiser.AddRange(other.ShipClasses.Cruiser);
            ShipClasses.Battleship.AddRange(other.ShipClasses.Battleship);
            ShipClasses.Titan.AddRange(other.ShipClasses.Titan);
            ShipClasses.Colossus.AddRange(other.ShipClasses.Colossus);
            ShipClasses.Juggernaut.AddRange(other.ShipClasses.Juggernaut);
            ShipClasses.Constructor.AddRange(other.ShipClasses.Constructor);
            ShipClasses.Science.AddRange(other.ShipClasses.Science);
            ShipClasses.Coloniser.AddRange(other.ShipClasses.Coloniser);
            ShipClasses.Transport.AddRange(other.ShipClasses.Transport);
            ShipClasses.IonCannon.AddRange(other.ShipClasses.IonCannon);

            Stations.MilitaryStations.Generic.AddRange(other.Stations.MilitaryStations.Generic);
            Stations.MilitaryStations.Small.AddRange(other.Stations.MilitaryStations.Small);
            Stations.MilitaryStations.Medium.AddRange(other.Stations.MilitaryStations.Medium);
            Stations.MilitaryStations.Large.AddRange(other.Stations.MilitaryStations.Large);
            Stations.Starbases.Generic.AddRange(other.Stations.Starbases.Generic);
            Stations.Starbases.Outposts.AddRange(other.Stations.Starbases.Outposts);
            Stations.Starbases.Starports.AddRange(other.Stations.Starbases.Starports);
            Stations.Starbases.Starholds.AddRange(other.Stations.Starbases.Starholds);
            Stations.Starbases.Starfortresses.AddRange(other.Stations.Starbases.Starfortresses);
            Stations.Starbases.Citadels.AddRange(other.Stations.Starbases.Citadels);
            Stations.MiningStations.AddRange(other.Stations.MiningStations);
            Stations.ResearchStations.AddRange(other.Stations.ResearchStations);
            Stations.ObservationStations.AddRange(other.Stations.ObservationStations);

            Armies.Fleet.AddRange(other.Armies.Fleet);
            Armies.DefenceArmy.AddRange(other.Armies.DefenceArmy);
            Armies.AssaultArmy.AddRange(other.Armies.AssaultArmy);
            Armies.OccupationArmy.AddRange(other.Armies.OccupationArmy);
            Armies.SlaveArmy.AddRange(other.Armies.SlaveArmy);
            Armies.CloneArmy.AddRange(other.Armies.CloneArmy);
            Armies.RoboticDefenceArmy.AddRange(other.Armies.RoboticDefenceArmy);
            Armies.RoboticAssaultArmy.AddRange(other.Armies.RoboticAssaultArmy);
            Armies.RoboticOccupationArmy.AddRange(other.Armies.RoboticOccupationArmy);
            Armies.AndroidDefenceArmy.AddRange(other.Armies.AndroidDefenceArmy);
            Armies.AndroidAssaultArmy.AddRange(other.Armies.AndroidAssaultArmy);
            Armies.PsionicArmy.AddRange(other.Armies.PsionicArmy);
            Armies.XenomorphArmy.AddRange(other.Armies.XenomorphArmy);
            Armies.SuperSoldierArmy.AddRange(other.Armies.SuperSoldierArmy);
            Armies.PrimitiveArmy.AddRange(other.Armies.PrimitiveArmy);
            Armies.IndustrialArmy.AddRange(other.Armies.IndustrialArmy);
            Armies.PostAtomicArmy.AddRange(other.Armies.PostAtomicArmy);

            Planets.Generic.AddRange(other.Planets.Generic);
            Planets.Desert.AddRange(other.Planets.Desert);
            Planets.Arid.AddRange(other.Planets.Arid);
            Planets.Tropical.AddRange(other.Planets.Tropical);
            Planets.Continental.AddRange(other.Planets.Continental);
            Planets.Gaia.AddRange(other.Planets.Gaia);
            Planets.Ocean.AddRange(other.Planets.Ocean);
            Planets.Tundra.AddRange(other.Planets.Tundra);
            Planets.Arctic.AddRange(other.Planets.Arctic);
            Planets.Tomb.AddRange(other.Planets.Tomb);
            Planets.Savannah.AddRange(other.Planets.Savannah);
            Planets.Alpine.AddRange(other.Planets.Alpine);
            Planets.Molten.AddRange(other.Planets.Molten);
            Planets.Barren.AddRange(other.Planets.Barren);
            Planets.Asteroid.AddRange(other.Planets.Asteroid);

            Characters.AddRange(other.Characters);
        }
    }
}
