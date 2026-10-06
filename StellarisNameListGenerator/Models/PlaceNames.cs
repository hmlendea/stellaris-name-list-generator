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

        public List<NameGroup> Airports { get; set; }

        public PlaceNames()
        {
            Countries = [];
            Regions = [];
            Cities = [];

            Mountains = [];
            Forests = [];
            Deserts = [];

            Rivers = [];
            Lakes = [];
            Seas = [];

            Airports = [];
        }
    }
}
