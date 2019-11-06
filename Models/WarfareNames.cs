using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class WarfareNames
    {
        public List<NameGroup> Weapons { get; set; }

        public List<NameGroup> MilitaryUnitTypes { get; set; }
        
        public List<NameGroup> BattleLocations { get; set; }
        
        public List<NameGroup> MilitaryPeopleTier1 { get; set; }
        public List<NameGroup> MilitaryPeopleTier2 { get; set; }
        public List<NameGroup> MilitaryPeopleTier3 { get; set; }

        public WarfareNames()
        {
            Weapons = new List<NameGroup>();
            MilitaryUnitTypes = new List<NameGroup>();
            BattleLocations = new List<NameGroup>();

            MilitaryPeopleTier1 = new List<NameGroup>();
            MilitaryPeopleTier2 = new List<NameGroup>();
            MilitaryPeopleTier3 = new List<NameGroup>();
        }
    }
}
