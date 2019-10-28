using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class PlaceNames
    {
        public List<NameGroup> Countries { get; set; }

        public List<NameGroup> Cities { get; set; }

        public PlaceNames()
        {
            Countries = new List<NameGroup>();
            Cities = new List<NameGroup>();
        }
    }
}
