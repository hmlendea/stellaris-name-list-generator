using System.Collections.Generic;
using System.Linq;

using NuciDAL.DataObjects;

namespace StellarisNameListGenerator.Models
{
    public class NameList : EntityBase
    {
        public string Name { get; set; }
        
        public List<NameGroup> Denonyms { get; set; }
        public PlaceNames Places { get; set; }

        public WarfareNames Warfare { get; set; }
        public List<NameGroup> MythologicalCreatures { get; set; }

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

            Denonyms = new List<NameGroup>();
            Places = new PlaceNames();

            Warfare = new WarfareNames();
            MythologicalCreatures = new List<NameGroup>();

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
            Places.Deserts.AddRange(other.Places.Deserts);

            Places.Rivers.AddRange(other.Places.Rivers);
            Places.Lakes.AddRange(other.Places.Lakes);
            Places.Seas.AddRange(other.Places.Seas);
            
            Warfare.Weapons.AddRange(other.Warfare.Weapons);
            Warfare.MilitaryUnitTypes.AddRange(other.Warfare.MilitaryUnitTypes);
            Warfare.BattleLocations.AddRange(other.Warfare.BattleLocations);

            Warfare.MilitaryPeopleTier1.AddRange(other.Warfare.MilitaryPeopleTier1);
            Warfare.MilitaryPeopleTier2.AddRange(other.Warfare.MilitaryPeopleTier2);
            Warfare.MilitaryPeopleTier3.AddRange(other.Warfare.MilitaryPeopleTier3);
            
            MythologicalCreatures.AddRange(other.MythologicalCreatures);
            
            Ships.Generic.AddRange(other.Ships.Generic);
            Ships.Corvette.AddRange(other.Ships.Corvette);
            Ships.Destroyer.AddRange(other.Ships.Destroyer);
            Ships.Cruiser.AddRange(other.Ships.Cruiser);
            Ships.Battleship.AddRange(other.Ships.Battleship);
            Ships.Titan.AddRange(other.Ships.Titan);
            Ships.Colossus.AddRange(other.Ships.Colossus);
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
            Armies.AndroidAssaultArmy.AddRange(other.Armies.AndroidAssaultArmy);
            Armies.AndroidDefenceArmy.AddRange(other.Armies.AndroidDefenceArmy);
            Armies.PsionicArmy.AddRange(other.Armies.PsionicArmy);
            Armies.XenomorphArmy.AddRange(other.Armies.XenomorphArmy);
            Armies.SuperSoldierArmy.AddRange(other.Armies.SuperSoldierArmy);

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
