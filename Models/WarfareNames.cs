using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class WarfareNames
    {
        public List<NameGroup> Weapons { get; set; }
        public List<NameGroup> MilitaryUnitTypes { get; set; }
        
        public List<NameGroup> Forts { get; set; }
        public List<NameGroup> BattleLocations { get; set; }

        public WarfareNames()
        {
            Weapons = new List<NameGroup>();
            MilitaryUnitTypes = new List<NameGroup>();

            Forts = new List<NameGroup>();
            BattleLocations = new List<NameGroup>();
        }
    }
}
