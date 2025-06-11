using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class ArmyNames
    {
        public string FleetSequentialName { get; set; }
        public List<NameGroup> Fleet { get; set; }

        public string DefenceArmySequentialName { get; set; }
        public List<NameGroup> DefenceArmy { get; set; }
        public string AssaultArmySequentialName { get; set; }
        public List<NameGroup> AssaultArmy { get; set; }
        public string OccupationArmySequentialName { get; set; }
        public List<NameGroup> OccupationArmy { get; set; }

        public string SlaveArmySequentialName { get; set; }
        public List<NameGroup> SlaveArmy { get; set; }
        public string CloneArmySequentialName { get; set; }
        public List<NameGroup> CloneArmy { get; set; }
        public string UndeadArmySequentialName { get; set; }
        public List<NameGroup> UndeadArmy { get; set; }

        public string RoboticDefenceArmySequentialName { get; set; }
        public List<NameGroup> RoboticDefenceArmy { get; set; }
        public string RoboticAssaultArmySequentialName { get; set; }
        public List<NameGroup> RoboticAssaultArmy { get; set; }
        public string RoboticOccupationArmySequentialName { get; set; }
        public List<NameGroup> RoboticOccupationArmy { get; set; }

        public string AndroidAssaultArmySequentialName { get; set; }
        public List<NameGroup> AndroidAssaultArmy { get; set; }
        public string AndroidDefenceArmySequentialName { get; set; }
        public List<NameGroup> AndroidDefenceArmy { get; set; }

        public string PsionicArmySequentialName { get; set; }
        public List<NameGroup> PsionicArmy { get; set; }
        public string XenomorphArmySequentialName { get; set; }
        public List<NameGroup> XenomorphArmy { get; set; }
        public string SuperSoldierArmySequentialName { get; set; }
        public List<NameGroup> SuperSoldierArmy { get; set; }

        public string PrimitiveArmySequentialName { get; set; }
        public List<NameGroup> PrimitiveArmy { get; set; }
        public string IndustrialArmySequentialName { get; set; }
        public List<NameGroup> IndustrialArmy { get; set; }
        public string PostAtomicArmySequentialName { get; set; }
        public List<NameGroup> PostAtomicArmy { get; set; }

        public ArmyNames()
        {
            FleetSequentialName = "%O% Fleet";
            Fleet = [];

            DefenceArmySequentialName = "%O% Planetary Guard";
            DefenceArmy = [];
            AssaultArmySequentialName = "%O% Expeditionary Force";
            AssaultArmy = [];
            OccupationArmySequentialName = "%O% Garrison Force";
            OccupationArmy = [];

            SlaveArmySequentialName = "%O% Indentured Rifles";
            SlaveArmy = [];
            CloneArmySequentialName = "%O% Clone Army";
            CloneArmy = [];
            UndeadArmySequentialName = "%O% Undead Army";
            UndeadArmy = [];

            RoboticDefenceArmySequentialName = "%O% Ground Defence Matrix";
            RoboticDefenceArmy = [];
            RoboticAssaultArmySequentialName = "%O% Hunter-Killer Group";
            RoboticAssaultArmy = [];
            RoboticOccupationArmySequentialName = "%O% Mechanised Garrison";
            RoboticOccupationArmy = [];

            AndroidDefenceArmySequentialName = "%O% Synthetic Sentinels";
            AndroidDefenceArmy = [];
            AndroidAssaultArmySequentialName = "%O% Synthetic Rangers";
            AndroidAssaultArmy = [];

            PsionicArmySequentialName = "%O% Psi Commando";
            PsionicArmy = [];
            XenomorphArmySequentialName = "%O% Bio-Warfare Division";
            XenomorphArmy = [];
            SuperSoldierArmySequentialName = "%O% Bio-Engineered Squadron";
            SuperSoldierArmy = [];

            PrimitiveArmySequentialName = "Primitive Army %C%";
            PrimitiveArmy = [];
            IndustrialArmySequentialName = "Industrial Army %C%";
            IndustrialArmy = [];
            PostAtomicArmySequentialName = "Post-Atomic Army %C%";
            PostAtomicArmy = [];
        }
    }
}
