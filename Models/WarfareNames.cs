using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class WarfareNames
    {
        public WeaponNames Weapons { get; set; }
        public List<NameGroup> MilitaryUnitTypes { get; set; }
        public List<NameGroup> ShipTypes { get; set; }

        public List<NameGroup> Forts { get; set; }
        public List<NameGroup> BattleLocations { get; set; }

        public WarfareNames()
        {
            Weapons = new WeaponNames();
            MilitaryUnitTypes = [];
            ShipTypes = [];

            Forts = [];
            BattleLocations = [];
        }
    }
}
