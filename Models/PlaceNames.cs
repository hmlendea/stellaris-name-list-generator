using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class PlaceNames
    {
        public List<NameGroup> Countries { get; set; }
        public List<NameGroup> States { get; set; }
        public List<NameGroup> Cities { get; set; }

        public List<NameGroup> Rivers { get; set; }
        public List<NameGroup> Lakes { get; set; }
        public List<NameGroup> Seas { get; set; }

        public PlaceNames()
        {
            Countries = new List<NameGroup>();
            States = new List<NameGroup>();
            Cities = new List<NameGroup>();

            Rivers = new List<NameGroup>();
            Lakes = new List<NameGroup>();
            Seas = new List<NameGroup>();
        }
    }
}
