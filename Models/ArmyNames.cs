using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class ArmyNames
    {
        public List<NameGroup> Fleet { get; set; }

        public List<NameGroup> DefenceArmy { get; set; }
        public List<NameGroup> AssaultArmy { get; set; }
        public List<NameGroup> OccupationArmy { get; set; }

        public List<NameGroup> SlaveArmy { get; set; }
        public List<NameGroup> CloneArmy { get; set; }
        
        public List<NameGroup> RoboticDefenceArmy { get; set; }
        public List<NameGroup> RoboticAssaultArmy { get; set; }
        public List<NameGroup> RoboticOccupationArmy { get; set; }

        public List<NameGroup> AndroidAssaultArmy { get; set; }
        public List<NameGroup> AndroidDefenceArmy { get; set; }

        public List<NameGroup> PsionicArmy { get; set; }
        public List<NameGroup> XenomorphArmy { get; set; }
        public List<NameGroup> SuperSoldierArmy { get; set; }

        public ArmyNames()
        {
            Fleet = new List<NameGroup>();

            DefenceArmy = new List<NameGroup>();
            AssaultArmy = new List<NameGroup>();
            OccupationArmy = new List<NameGroup>();

            SlaveArmy = new List<NameGroup>();
            CloneArmy = new List<NameGroup>();

            RoboticDefenceArmy = new List<NameGroup>();
            RoboticAssaultArmy = new List<NameGroup>();
            RoboticOccupationArmy = new List<NameGroup>();

            AndroidAssaultArmy = new List<NameGroup>();
            AndroidDefenceArmy = new List<NameGroup>();

            PsionicArmy = new List<NameGroup>();
            XenomorphArmy = new List<NameGroup>();
            SuperSoldierArmy = new List<NameGroup>();
        }
    }
}
