using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class GreatPeople
    {
        public List<NameGroup> Explorers { get; set; }
        public List<NameGroup> Pioneers { get; set; }
        public List<NameGroup> Scientists { get; set; }

        public List<NameGroup> LeadersTier1 { get; set; }
        public List<NameGroup> LeadersTier2 { get; set; }
        public List<NameGroup> LeadersTier3 { get; set; }

        public List<NameGroup> FlyingAces { get; set; }
        public List<NameGroup> Heroes { get; set; }
        public List<NameGroup> Admirals { get; set; }

        public List<NameGroup> GeneralsTier1 { get; set; }
        public List<NameGroup> GeneralsTier2 { get; set; }
        public List<NameGroup> GeneralsTier3 { get; set; }

        public List<NameGroup> Deities { get; set; }
        public List<NameGroup> WarDeities { get; set; }
        public List<NameGroup> DeathDeities { get; set; }
        public List<NameGroup> LabourDeities { get; set; }
        public List<NameGroup> NatureDeities { get; set; }
        public List<NameGroup> HealthDeities { get; set; }
        public List<NameGroup> FeastDeities { get; set; }
        public List<NameGroup> WealthDeities { get; set; }
        public List<NameGroup> SunDeities { get; set; }
        public List<NameGroup> SkyDeities { get; set; }
        public List<NameGroup> ColdDeities { get; set; }
        public List<NameGroup> WaterDeities { get; set; }

        public GreatPeople()
        {
            Explorers = new List<NameGroup>();
            Scientists = new List<NameGroup>();
            Pioneers = new List<NameGroup>();

            LeadersTier1 = new List<NameGroup>();
            LeadersTier2 = new List<NameGroup>();
            LeadersTier3 = new List<NameGroup>();

            FlyingAces = new List<NameGroup>();
            Heroes = new List<NameGroup>();
            Admirals = new List<NameGroup>();

            GeneralsTier1 = new List<NameGroup>();
            GeneralsTier2 = new List<NameGroup>();
            GeneralsTier3 = new List<NameGroup>();

            Deities = new List<NameGroup>();
            WarDeities = new List<NameGroup>();
            DeathDeities = new List<NameGroup>();
            LabourDeities = new List<NameGroup>();
            NatureDeities = new List<NameGroup>();
            HealthDeities = new List<NameGroup>();
            FeastDeities = new List<NameGroup>();
            WealthDeities = new List<NameGroup>();
            SunDeities = new List<NameGroup>();
            SkyDeities = new List<NameGroup>();
            ColdDeities = new List<NameGroup>();
            WaterDeities = new List<NameGroup>();
        }
    }
}
