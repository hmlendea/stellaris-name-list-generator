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
            Generic = [];
            Small = [];
            Medium = [];
            Large = [];
        }
    }
}
