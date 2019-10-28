using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class StarbaseNames
    {
        public List<NameGroup> Generic { get; set; }

        public List<NameGroup> Outposts { get; set; }

        public List<NameGroup> Starports { get; set; }

        public List<NameGroup> Starholds { get; set; }

        public List<NameGroup> Starfortresses { get; set; }
        
        public List<NameGroup> Citadels { get; set; }

        public StarbaseNames()
        {
            Generic = new List<NameGroup>();
            Outposts = new List<NameGroup>();
            Starports = new List<NameGroup>();
            Starholds = new List<NameGroup>();
            Starfortresses = new List<NameGroup>();
            Citadels = new List<NameGroup>();
        }
    }
}
