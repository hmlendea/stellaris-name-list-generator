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
        public List<NameGroup> Admirals { get; set; }

        public List<NameGroup> GeneralsTier1 { get; set; }
        public List<NameGroup> GeneralsTier2 { get; set; }
        public List<NameGroup> GeneralsTier3 { get; set; }

        public GreatPeople()
        {
            Explorers = new List<NameGroup>();
            Scientists = new List<NameGroup>();
            Pioneers = new List<NameGroup>();

            LeadersTier1 = new List<NameGroup>();
            LeadersTier2 = new List<NameGroup>();
            LeadersTier3 = new List<NameGroup>();

            FlyingAces = new List<NameGroup>();
            Admirals = new List<NameGroup>();

            GeneralsTier1 = new List<NameGroup>();
            GeneralsTier2 = new List<NameGroup>();
            GeneralsTier3 = new List<NameGroup>();
        }
    }
}
