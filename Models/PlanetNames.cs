using System.Collections.Generic;
using System.Linq;

namespace StellarisNameListGenerator.Models
{
    public sealed class PlanetNames
    {
        public List<NameGroup> Generic { get; set; }
        public List<NameGroup> Desert { get; set; }
        public List<NameGroup> Arid { get; set; }
        public List<NameGroup> Tropical { get; set; }
        public List<NameGroup> Continental { get; set; }
        public List<NameGroup> Gaia { get; set; }
        public List<NameGroup> Ocean { get; set; }
        public List<NameGroup> Tundra { get; set; }
        public List<NameGroup> Arctic { get; set; }
        public List<NameGroup> Tomb { get; set; }
        public List<NameGroup> Savannah { get; set; }
        public List<NameGroup> Alpine { get; set; }
        public List<NameGroup> Molten { get; set; }
        public List<NameGroup> Barren { get; set; }
        public List<NameGroup> Asteroid { get; set; }

        public PlanetNames()
        {
            Generic = new List<NameGroup>();
            Desert = new List<NameGroup>();
            Arid = new List<NameGroup>();
            Tropical = new List<NameGroup>();
            Continental = new List<NameGroup>();
            Gaia = new List<NameGroup>();
            Ocean = new List<NameGroup>();
            Ocean = new List<NameGroup>();
            Tundra = new List<NameGroup>();
            Arctic = new List<NameGroup>();
            Tomb = new List<NameGroup>();
            Savannah = new List<NameGroup>();
            Alpine = new List<NameGroup>();
            Molten = new List<NameGroup>();
            Barren = new List<NameGroup>();
            Asteroid = new List<NameGroup>();
        }
    }
}
