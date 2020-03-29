using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class ShipNames
    {
        public List<NameGroup> Generic { get; set; }

        public List<NameGroup> Corvette { get; set; }
        public List<NameGroup> Destroyer { get; set; }
        public List<NameGroup> Cruiser { get; set; }
        public List<NameGroup> Battleship { get; set; }

        public List<NameGroup> Titan { get; set; }
        public List<NameGroup> Colossus { get; set; }
        public List<NameGroup> Juggernaut { get; set; }

        public List<NameGroup> Constructor { get; set; }
        public List<NameGroup> Science { get; set; }
        public List<NameGroup> Coloniser { get; set; }
        public List<NameGroup> Transport { get; set; }

        public List<NameGroup> IonCannon { get; set; }

        public ShipNames()
        {
            Generic = new List<NameGroup>();

            Corvette = new List<NameGroup>();
            Destroyer = new List<NameGroup>();
            Cruiser = new List<NameGroup>();
            Battleship = new List<NameGroup>();

            Titan = new List<NameGroup>();
            Colossus = new List<NameGroup>();
            Juggernaut = new List<NameGroup>();

            Constructor = new List<NameGroup>();
            Science = new List<NameGroup>();
            Coloniser = new List<NameGroup>();
            Transport = new List<NameGroup>();

            IonCannon = new List<NameGroup>();
        }
    }
}
