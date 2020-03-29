using System.Collections.Generic;
using System.Linq;

namespace StellarisNameListGenerator.Models
{
    public sealed class WeaponNames
    {
        public List<NameGroup> All => Ranged
            .Concat(Melee)
            .ToList();

        public List<NameGroup> Ranged => Artillery
            .Concat(Guns)
            .Concat(Crossbows)
            .Concat(Bows)
            .Concat(Javelins)
            .ToList();

        public List<NameGroup> Melee => Swords
            .Concat(Daggers)
            .Concat(Polearms)
            .Concat(Axes)
            .Concat(Hammers)
            .ToList();

        public List<NameGroup> Artillery { get; set; }

        public List<NameGroup> Guns { get; set; }
        public List<NameGroup> Crossbows { get; set; }
        public List<NameGroup> Bows { get; set; }
        public List<NameGroup> Javelins { get; set; }

        public List<NameGroup> Swords { get; set; }
        public List<NameGroup> Daggers { get; set; }
        public List<NameGroup> Polearms { get; set; }
        public List<NameGroup> Axes { get; set; }
        public List<NameGroup> Hammers { get; set; }

        public WeaponNames()
        {
            Artillery = new List<NameGroup>();

            Guns = new List<NameGroup>();
            Crossbows = new List<NameGroup>();
            Bows = new List<NameGroup>();
            Javelins = new List<NameGroup>();

            Swords = new List<NameGroup>();
            Daggers = new List<NameGroup>();
            Polearms = new List<NameGroup>();
            Axes = new List<NameGroup>();
            Hammers = new List<NameGroup>();
        }
    }
}
