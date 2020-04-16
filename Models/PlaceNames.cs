using System.Collections.Generic;
using System.Linq;

namespace StellarisNameListGenerator.Models
{
    public sealed class PlaceNames
    {
        public List<NameGroup> Countries { get; set; }
        public List<NameGroup> Regions { get; set; }
        public List<NameGroup> Cities { get; set; }

        public List<NameGroup> Mountains { get; set; }
        public List<NameGroup> Forests { get; set; }
        public List<NameGroup> Deserts { get; set; }

        public List<NameGroup> Rivers { get; set; }
        public List<NameGroup> Lakes { get; set; }
        public List<NameGroup> Seas { get; set; }
        public IEnumerable<NameGroup> WaterBodies => Rivers
            .Concat(Lakes)
            .Concat(Seas);

        public IEnumerable<NameGroup> GeographicalPlaces => Mountains
            .Concat(Forests)
            .Concat(Deserts)
            .Concat(WaterBodies);

        public PlaceNames()
        {
            Countries = new List<NameGroup>();
            Regions = new List<NameGroup>();
            Cities = new List<NameGroup>();

            Mountains = new List<NameGroup>();
            Forests = new List<NameGroup>();
            Deserts = new List<NameGroup>();

            Rivers = new List<NameGroup>();
            Lakes = new List<NameGroup>();
            Seas = new List<NameGroup>();
        }
    }
}
