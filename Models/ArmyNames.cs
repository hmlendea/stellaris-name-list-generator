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
            Fleet = new List<NameGroup>();

            DefenceArmySequentialName = "%O% Planetary Guard";
            DefenceArmy = new List<NameGroup>();
            AssaultArmySequentialName = "%O% Expeditionary Force";
            AssaultArmy = new List<NameGroup>();
            OccupationArmySequentialName = "%O% Garrison Force";
            OccupationArmy = new List<NameGroup>();

            SlaveArmySequentialName = "%O% Indentured Rifles";
            SlaveArmy = new List<NameGroup>();
            CloneArmySequentialName = "%O% Clone Army";
            CloneArmy = new List<NameGroup>();

            RoboticDefenceArmySequentialName = "%O% Ground Defence Matrix";
            RoboticDefenceArmy = new List<NameGroup>();
            RoboticAssaultArmySequentialName = "%O% Hunter-Killer Group";
            RoboticAssaultArmy = new List<NameGroup>();
            RoboticOccupationArmySequentialName = "%O% Mechanised Garrison";
            RoboticOccupationArmy = new List<NameGroup>();

            AndroidDefenceArmySequentialName = "%O% Synthetic Sentinels";
            AndroidDefenceArmy = new List<NameGroup>();
            AndroidAssaultArmySequentialName = "%O% Synthetic Rangers";
            AndroidAssaultArmy = new List<NameGroup>();

            PsionicArmySequentialName = "%O% Psi Commando";
            PsionicArmy = new List<NameGroup>();
            XenomorphArmySequentialName = "%O% Bio-Warfare Division";
            XenomorphArmy = new List<NameGroup>();
            SuperSoldierArmySequentialName = "%O% Bio-Engineered Squadron";
            SuperSoldierArmy = new List<NameGroup>();

            PrimitiveArmySequentialName = "Primitive Army %C%";
            PrimitiveArmy = new List<NameGroup>();
            IndustrialArmySequentialName = "Primitive Army %C%";
            IndustrialArmy = new List<NameGroup>();
            PostAtomicArmySequentialName = "Primitive Army %C%";
            PostAtomicArmy = new List<NameGroup>();
        }
    }
}
