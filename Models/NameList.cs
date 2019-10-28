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

        public void AddRange(NameList other, string discriminator)
        {
            Nationalities.AddRange(ImportNames(other.Nationalities, discriminator));
            Weapons.AddRange(ImportNames(other.Weapons, discriminator));

            Places.Countries.AddRange(ImportNames(other.Places.Countries, discriminator));
            Places.Cities.AddRange(ImportNames(other.Places.Cities, discriminator));

            Ships.Generic.AddRange(ImportNames(other.Ships.Generic, discriminator));
            Ships.Corvette.AddRange(ImportNames(other.Ships.Corvette, discriminator));
            Ships.Destroyer.AddRange(ImportNames(other.Ships.Destroyer, discriminator));
            Ships.Cruiser.AddRange(ImportNames(other.Ships.Cruiser, discriminator));
            Ships.Battleship.AddRange(ImportNames(other.Ships.Battleship, discriminator));
            Ships.Titan.AddRange(ImportNames(other.Ships.Titan, discriminator));
            Ships.Colossus.AddRange(ImportNames(other.Ships.Colossus, discriminator));
            Ships.Constructor.AddRange(ImportNames(other.Ships.Constructor, discriminator));
            Ships.Science.AddRange(ImportNames(other.Ships.Science, discriminator));
            Ships.Coloniser.AddRange(ImportNames(other.Ships.Coloniser, discriminator));
            Ships.Transport.AddRange(ImportNames(other.Ships.Transport, discriminator));
            Ships.IonCannon.AddRange(ImportNames(other.Ships.IonCannon, discriminator));

            ShipClasses.Generic.AddRange(ImportNames(other.ShipClasses.Generic, discriminator));
            ShipClasses.Corvette.AddRange(ImportNames(other.ShipClasses.Corvette, discriminator));
            ShipClasses.Destroyer.AddRange(ImportNames(other.ShipClasses.Destroyer, discriminator));
            ShipClasses.Cruiser.AddRange(ImportNames(other.ShipClasses.Cruiser, discriminator));
            ShipClasses.Battleship.AddRange(ImportNames(other.ShipClasses.Battleship, discriminator));
            ShipClasses.Titan.AddRange(ImportNames(other.ShipClasses.Titan, discriminator));
            ShipClasses.Colossus.AddRange(ImportNames(other.ShipClasses.Colossus, discriminator));
            ShipClasses.Constructor.AddRange(ImportNames(other.ShipClasses.Constructor, discriminator));
            ShipClasses.Science.AddRange(ImportNames(other.ShipClasses.Science, discriminator));
            ShipClasses.Coloniser.AddRange(ImportNames(other.ShipClasses.Coloniser, discriminator));
            ShipClasses.Transport.AddRange(ImportNames(other.ShipClasses.Transport, discriminator));
            ShipClasses.IonCannon.AddRange(ImportNames(other.ShipClasses.IonCannon, discriminator));

            Stations.MilitaryStations.Generic.AddRange(ImportNames(other.Stations.MilitaryStations.Generic, discriminator));
            Stations.MilitaryStations.Small.AddRange(ImportNames(other.Stations.MilitaryStations.Small, discriminator));
            Stations.MilitaryStations.Medium.AddRange(ImportNames(other.Stations.MilitaryStations.Medium, discriminator));
            Stations.MilitaryStations.Large.AddRange(ImportNames(other.Stations.MilitaryStations.Large, discriminator));
            Stations.StarbaseNames.Generic.AddRange(ImportNames(other.Stations.StarbaseNames.Generic, discriminator));
            Stations.StarbaseNames.Outposts.AddRange(ImportNames(other.Stations.StarbaseNames.Outposts, discriminator));
            Stations.StarbaseNames.Starports.AddRange(ImportNames(other.Stations.StarbaseNames.Starports, discriminator));
            Stations.StarbaseNames.Starholds.AddRange(ImportNames(other.Stations.StarbaseNames.Starholds, discriminator));
            Stations.StarbaseNames.Starfortresses.AddRange(ImportNames(other.Stations.StarbaseNames.Starfortresses, discriminator));
            Stations.StarbaseNames.Citadels.AddRange(ImportNames(other.Stations.StarbaseNames.Citadels, discriminator));
            Stations.MiningStations.AddRange(ImportNames(other.Stations.MiningStations, discriminator));
            Stations.ResearchStations.AddRange(ImportNames(other.Stations.ResearchStations, discriminator));
            Stations.ObservationStations.AddRange(ImportNames(other.Stations.ObservationStations, discriminator));

            Armies.Fleet.AddRange(ImportNames(other.Armies.Fleet, discriminator));
            Armies.DefenceArmy.AddRange(ImportNames(other.Armies.DefenceArmy, discriminator));
            Armies.AssaultArmy.AddRange(ImportNames(other.Armies.AssaultArmy, discriminator));
            Armies.OccupationArmy.AddRange(ImportNames(other.Armies.OccupationArmy, discriminator));
            Armies.SlaveArmy.AddRange(ImportNames(other.Armies.SlaveArmy, discriminator));
            Armies.CloneArmy.AddRange(ImportNames(other.Armies.CloneArmy, discriminator));
            Armies.RoboticDefenceArmy.AddRange(ImportNames(other.Armies.RoboticDefenceArmy, discriminator));
            Armies.RoboticAssaultArmy.AddRange(ImportNames(other.Armies.RoboticAssaultArmy, discriminator));
            Armies.RoboticOccupationArmy.AddRange(ImportNames(other.Armies.RoboticOccupationArmy, discriminator));
            Armies.AndroidAssaultArmy.AddRange(ImportNames(other.Armies.AndroidAssaultArmy, discriminator));
            Armies.AndroidDefenceArmy.AddRange(ImportNames(other.Armies.AndroidDefenceArmy, discriminator));
            Armies.PsionicArmy.AddRange(ImportNames(other.Armies.PsionicArmy, discriminator));
            Armies.XenomorphArmy.AddRange(ImportNames(other.Armies.XenomorphArmy, discriminator));
            Armies.SuperSoldierArmy.AddRange(ImportNames(other.Armies.SuperSoldierArmy, discriminator));

            Planets.Generic.AddRange(ImportNames(other.Planets.Generic, discriminator));
            Planets.Desert.AddRange(ImportNames(other.Planets.Desert, discriminator));
            Planets.Arid.AddRange(ImportNames(other.Planets.Arid, discriminator));
            Planets.Tropical.AddRange(ImportNames(other.Planets.Tropical, discriminator));
            Planets.Continental.AddRange(ImportNames(other.Planets.Continental, discriminator));
            Planets.Gaia.AddRange(ImportNames(other.Planets.Gaia, discriminator));
            Planets.Ocean.AddRange(ImportNames(other.Planets.Ocean, discriminator));
            Planets.Tundra.AddRange(ImportNames(other.Planets.Tundra, discriminator));
            Planets.Arctic.AddRange(ImportNames(other.Planets.Arctic, discriminator));
            Planets.Tomb.AddRange(ImportNames(other.Planets.Tomb, discriminator));
            Planets.Savannah.AddRange(ImportNames(other.Planets.Savannah, discriminator));
            Planets.Alpine.AddRange(ImportNames(other.Planets.Alpine, discriminator));
            Planets.Molten.AddRange(ImportNames(other.Planets.Molten, discriminator));
            Planets.Barren.AddRange(ImportNames(other.Planets.Barren, discriminator));
            Planets.Asteroid.AddRange(ImportNames(other.Planets.Asteroid, discriminator));

            Characters.AddRange(other.Characters);
        }

        List<NameGroup> ImportNames(List<NameGroup> names, string discriminator)
        {
            return names
                .Select(x => new NameGroup
                {
                    Name = string.Join(" ", $"[{discriminator}]", x.Name),
                    Values = x.Values.ToList()
                })
                .ToList();
        }
    }
}
