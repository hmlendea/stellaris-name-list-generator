using System.Collections.Generic;

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
        }
    }
}
