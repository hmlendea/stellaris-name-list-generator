using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class BiosphereNames
    {
        public List<NameGroup> Animals { get; set; }
        public List<NameGroup> MythologicalCreatures { get; set; }

        public BiosphereNames()
        {
            Animals = [];
            MythologicalCreatures = [];
        }
    }
}
