using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class MilitaryStationNames
    {
        public List<NameGroup> Generic { get; set; }

        public List<NameGroup> Small { get; set; }

        public List<NameGroup> Medium { get; set; }
        
        public List<NameGroup> Large { get; set; }

        public MilitaryStationNames()
        {
            Generic = new List<NameGroup>();
            Small = new List<NameGroup>();
            Medium = new List<NameGroup>();
            Large = new List<NameGroup>();
        }
    }
}
