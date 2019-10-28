using System.Collections.Generic;
using System.Linq;

using NuciDAL.DataObjects;

namespace StellarisNameListGenerator.Models
{
    public class NameList : EntityBase
    {
        public string Name { get; set; }
        
        public List<NameGroup> Nationalities { get; set; }
        public List<NameGroup> Weapons { get; set; }
        public PlaceNames Places { get; set; }

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

            Nationalities = new List<NameGroup>();
            Weapons = new List<NameGroup>();
            Places = new PlaceNames();

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
            Nationalities.AddRange(ImportNames(other.Nationalities, other.Name));
            Weapons.AddRange(ImportNames(other.Weapons, other.Name));

            Places.Countries.AddRange(ImportNames(other.Places.Countries, other.Name));
            Places.Cities.AddRange(ImportNames(other.Places.Cities, other.Name));

            Ships.Generic.AddRange(ImportNames(other.Ships.Generic, other.Name));
            Ships.Corvette.AddRange(ImportNames(other.Ships.Corvette, other.Name));
            Ships.Destroyer.AddRange(ImportNames(other.Ships.Destroyer, other.Name));
            Ships.Cruiser.AddRange(ImportNames(other.Ships.Cruiser, other.Name));
            Ships.Battleship.AddRange(ImportNames(other.Ships.Battleship, other.Name));
            Ships.Titan.AddRange(ImportNames(other.Ships.Titan, other.Name));
            Ships.Colossus.AddRange(ImportNames(other.Ships.Colossus, other.Name));
            Ships.Constructor.AddRange(ImportNames(other.Ships.Constructor, other.Name));
            Ships.Science.AddRange(ImportNames(other.Ships.Science, other.Name));
            Ships.Coloniser.AddRange(ImportNames(other.Ships.Coloniser, other.Name));
            Ships.Transport.AddRange(ImportNames(other.Ships.Transport, other.Name));
            Ships.IonCannon.AddRange(ImportNames(other.Ships.IonCannon, other.Name));

            ShipClasses.Generic.AddRange(ImportNames(other.ShipClasses.Generic, other.Name));
            ShipClasses.Corvette.AddRange(ImportNames(other.ShipClasses.Corvette, other.Name));
            ShipClasses.Destroyer.AddRange(ImportNames(other.ShipClasses.Destroyer, other.Name));
            ShipClasses.Cruiser.AddRange(ImportNames(other.ShipClasses.Cruiser, other.Name));
            ShipClasses.Battleship.AddRange(ImportNames(other.ShipClasses.Battleship, other.Name));
            ShipClasses.Titan.AddRange(ImportNames(other.ShipClasses.Titan, other.Name));
            ShipClasses.Colossus.AddRange(ImportNames(other.ShipClasses.Colossus, other.Name));
            ShipClasses.Constructor.AddRange(ImportNames(other.ShipClasses.Constructor, other.Name));
            ShipClasses.Science.AddRange(ImportNames(other.ShipClasses.Science, other.Name));
            ShipClasses.Coloniser.AddRange(ImportNames(other.ShipClasses.Coloniser, other.Name));
            ShipClasses.Transport.AddRange(ImportNames(other.ShipClasses.Transport, other.Name));
            ShipClasses.IonCannon.AddRange(ImportNames(other.ShipClasses.IonCannon, other.Name));

            Stations.MilitaryStations.Generic.AddRange(ImportNames(other.Stations.MilitaryStations.Generic, other.Name));
            Stations.MilitaryStations.Small.AddRange(ImportNames(other.Stations.MilitaryStations.Small, other.Name));
            Stations.MilitaryStations.Medium.AddRange(ImportNames(other.Stations.MilitaryStations.Medium, other.Name));
            Stations.MilitaryStations.Large.AddRange(ImportNames(other.Stations.MilitaryStations.Large, other.Name));
            Stations.StarbaseNames.Generic.AddRange(ImportNames(other.Stations.StarbaseNames.Generic, other.Name));
            Stations.StarbaseNames.Outposts.AddRange(ImportNames(other.Stations.StarbaseNames.Outposts, other.Name));
            Stations.StarbaseNames.Starports.AddRange(ImportNames(other.Stations.StarbaseNames.Starports, other.Name));
            Stations.StarbaseNames.Starholds.AddRange(ImportNames(other.Stations.StarbaseNames.Starholds, other.Name));
            Stations.StarbaseNames.Starfortresses.AddRange(ImportNames(other.Stations.StarbaseNames.Starfortresses, other.Name));
            Stations.StarbaseNames.Citadels.AddRange(ImportNames(other.Stations.StarbaseNames.Citadels, other.Name));
            Stations.MiningStations.AddRange(ImportNames(other.Stations.MiningStations, other.Name));
            Stations.ResearchStations.AddRange(ImportNames(other.Stations.ResearchStations, other.Name));
            Stations.ObservationStations.AddRange(ImportNames(other.Stations.ObservationStations, other.Name));

            Armies.Fleet.AddRange(ImportNames(other.Armies.Fleet, other.Name));
            Armies.DefenceArmy.AddRange(ImportNames(other.Armies.DefenceArmy, other.Name));
            Armies.AssaultArmy.AddRange(ImportNames(other.Armies.AssaultArmy, other.Name));
            Armies.OccupationArmy.AddRange(ImportNames(other.Armies.OccupationArmy, other.Name));
            Armies.SlaveArmy.AddRange(ImportNames(other.Armies.SlaveArmy, other.Name));
            Armies.CloneArmy.AddRange(ImportNames(other.Armies.CloneArmy, other.Name));
            Armies.RoboticDefenceArmy.AddRange(ImportNames(other.Armies.RoboticDefenceArmy, other.Name));
            Armies.RoboticAssaultArmy.AddRange(ImportNames(other.Armies.RoboticAssaultArmy, other.Name));
            Armies.RoboticOccupationArmy.AddRange(ImportNames(other.Armies.RoboticOccupationArmy, other.Name));
            Armies.AndroidAssaultArmy.AddRange(ImportNames(other.Armies.AndroidAssaultArmy, other.Name));
            Armies.AndroidDefenceArmy.AddRange(ImportNames(other.Armies.AndroidDefenceArmy, other.Name));
            Armies.PsionicArmy.AddRange(ImportNames(other.Armies.PsionicArmy, other.Name));
            Armies.XenomorphArmy.AddRange(ImportNames(other.Armies.XenomorphArmy, other.Name));
            Armies.SuperSoldierArmy.AddRange(ImportNames(other.Armies.SuperSoldierArmy, other.Name));

            Planets.Generic.AddRange(ImportNames(other.Planets.Generic, other.Name));
            Planets.Desert.AddRange(ImportNames(other.Planets.Desert, other.Name));
            Planets.Arid.AddRange(ImportNames(other.Planets.Arid, other.Name));
            Planets.Tropical.AddRange(ImportNames(other.Planets.Tropical, other.Name));
            Planets.Continental.AddRange(ImportNames(other.Planets.Continental, other.Name));
            Planets.Gaia.AddRange(ImportNames(other.Planets.Gaia, other.Name));
            Planets.Ocean.AddRange(ImportNames(other.Planets.Ocean, other.Name));
            Planets.Tundra.AddRange(ImportNames(other.Planets.Tundra, other.Name));
            Planets.Arctic.AddRange(ImportNames(other.Planets.Arctic, other.Name));
            Planets.Tomb.AddRange(ImportNames(other.Planets.Tomb, other.Name));
            Planets.Savannah.AddRange(ImportNames(other.Planets.Savannah, other.Name));
            Planets.Alpine.AddRange(ImportNames(other.Planets.Alpine, other.Name));
            Planets.Molten.AddRange(ImportNames(other.Planets.Molten, other.Name));
            Planets.Barren.AddRange(ImportNames(other.Planets.Barren, other.Name));
            Planets.Asteroid.AddRange(ImportNames(other.Planets.Asteroid, other.Name));

            Characters.AddRange(other.Characters);
        }

        List<NameGroup> ImportNames(List<NameGroup> names, string nameListName)
        {
            return names
                .Select(x => new NameGroup
                {
                    Name = string.Join(" ", $"[{nameListName}]", x.Name),
                    Values = x.Values.ToList()
                })
                .ToList();
        }
    }
}
