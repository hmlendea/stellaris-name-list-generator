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
            .Concat(Throwables)
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
        public List<NameGroup> Throwables { get; set; }

        public List<NameGroup> Swords { get; set; }
        public List<NameGroup> Daggers { get; set; }
        public List<NameGroup> Polearms { get; set; }
        public List<NameGroup> Axes { get; set; }
        public List<NameGroup> Hammers { get; set; }

        public WeaponNames()
        {
            Artillery = [];

            Guns = [];
            Crossbows = [];
            Bows = [];
            Throwables = [];

            Swords = [];
            Daggers = [];
            Polearms = [];
            Axes = [];
            Hammers = [];
        }
    }
}
