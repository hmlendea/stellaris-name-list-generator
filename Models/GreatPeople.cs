using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<NameGroup> AllLeaders => LeadersTier1
            .Concat(LeadersTier2)
            .Concat(LeadersTier3);

        public List<NameGroup> FlyingAces { get; set; }
        public List<NameGroup> Heroes { get; set; }
        public List<NameGroup> Admirals { get; set; }

        public List<NameGroup> GeneralsTier1 { get; set; }
        public List<NameGroup> GeneralsTier2 { get; set; }
        public List<NameGroup> GeneralsTier3 { get; set; }
        public IEnumerable<NameGroup> AllGenerals => GeneralsTier1
            .Concat(GeneralsTier2)
            .Concat(GeneralsTier3);

        public List<NameGroup> PowerDeities { get; set; }
        public List<NameGroup> CreationDeities { get; set; }
        public List<NameGroup> DestructionDeities { get; set; }
        public List<NameGroup> PeaceDeities { get; set; }
        public List<NameGroup> WarDeities { get; set; }
        public List<NameGroup> VictoryDeities { get; set; }
        public List<NameGroup> DeathDeities { get; set; }
        public List<NameGroup> HatredDeities { get; set; }
        public List<NameGroup> FearDeities { get; set; }
        public List<NameGroup> SorrowDeities { get; set; }
        public List<NameGroup> BeastsDeities { get; set; }
        public List<NameGroup> TimeDeities { get; set; }
        public List<NameGroup> ProphecyDeities { get; set; }
        public List<NameGroup> JusticeDeities { get; set; }
        public List<NameGroup> ProtectionDeities { get; set; }
        public List<NameGroup> PunishmentDeities { get; set; }
        public List<NameGroup> LoyaltyDeities { get; set; }
        public List<NameGroup> DisloyaltyDeities { get; set; }
        public List<NameGroup> LabourDeities { get; set; }
        public List<NameGroup> NatureDeities { get; set; }
        public List<NameGroup> HealthDeities { get; set; }
        public List<NameGroup> LoveDeities { get; set; }
        public List<NameGroup> KnowledgeDeities { get; set; }
        public List<NameGroup> ArtDeities { get; set; }
        public List<NameGroup> FeastDeities { get; set; }
        public List<NameGroup> FortuneDeities { get; set; }
        public List<NameGroup> SleepDeities { get; set; }
        public List<NameGroup> DarknessDeities { get; set; }
        public List<NameGroup> LightDeities { get; set; }
        public List<NameGroup> SunDeities { get; set; }
        public List<NameGroup> SkyDeities { get; set; }
        public List<NameGroup> AirDeities { get; set; }
        public List<NameGroup> ColdDeities { get; set; }
        public List<NameGroup> WarmthDeities { get; set; }
        public List<NameGroup> WaterDeities { get; set; }
        public List<NameGroup> OtherDeities { get; set; }

        public IEnumerable<NameGroup> AllDeities => OtherDeities
            .Concat(PowerDeities)
            .Concat(CreationDeities)
            .Concat(DestructionDeities)
            .Concat(PeaceDeities)
            .Concat(WarDeities)
            .Concat(VictoryDeities)
            .Concat(DeathDeities)
            .Concat(HatredDeities)
            .Concat(FearDeities)
            .Concat(SorrowDeities)
            .Concat(BeastsDeities)
            .Concat(TimeDeities)
            .Concat(ProphecyDeities)
            .Concat(JusticeDeities)
            .Concat(ProtectionDeities)
            .Concat(PunishmentDeities)
            .Concat(LoyaltyDeities)
            .Concat(DisloyaltyDeities)
            .Concat(LabourDeities)
            .Concat(NatureDeities)
            .Concat(HealthDeities)
            .Concat(LoveDeities)
            .Concat(KnowledgeDeities)
            .Concat(ArtDeities)
            .Concat(FeastDeities)
            .Concat(FortuneDeities)
            .Concat(SleepDeities)
            .Concat(DarknessDeities)
            .Concat(LightDeities)
            .Concat(SunDeities)
            .Concat(SkyDeities)
            .Concat(AirDeities)
            .Concat(ColdDeities)
            .Concat(WarmthDeities)
            .Concat(WaterDeities)
            .Concat(OtherDeities);

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

            PowerDeities = new List<NameGroup>();
            CreationDeities = new List<NameGroup>();
            DestructionDeities = new List<NameGroup>();
            PeaceDeities = new List<NameGroup>();
            WarDeities = new List<NameGroup>();
            VictoryDeities = new List<NameGroup>();
            DeathDeities = new List<NameGroup>();
            HatredDeities = new List<NameGroup>();
            FearDeities = new List<NameGroup>();
            SorrowDeities = new List<NameGroup>();
            BeastsDeities = new List<NameGroup>();
            TimeDeities = new List<NameGroup>();
            ProphecyDeities = new List<NameGroup>();
            JusticeDeities = new List<NameGroup>();
            ProtectionDeities = new List<NameGroup>();
            PunishmentDeities = new List<NameGroup>();
            LoyaltyDeities = new List<NameGroup>();
            DisloyaltyDeities = new List<NameGroup>();
            LabourDeities = new List<NameGroup>();
            NatureDeities = new List<NameGroup>();
            HealthDeities = new List<NameGroup>();
            LoveDeities = new List<NameGroup>();
            KnowledgeDeities = new List<NameGroup>();
            ArtDeities = new List<NameGroup>();
            FeastDeities = new List<NameGroup>();
            FortuneDeities = new List<NameGroup>();
            SleepDeities = new List<NameGroup>();
            DarknessDeities = new List<NameGroup>();
            LightDeities = new List<NameGroup>();
            SunDeities = new List<NameGroup>();
            SkyDeities = new List<NameGroup>();
            AirDeities = new List<NameGroup>();
            ColdDeities = new List<NameGroup>();
            WarmthDeities = new List<NameGroup>();
            WaterDeities = new List<NameGroup>();
            OtherDeities = new List<NameGroup>();
        }
    }
}
