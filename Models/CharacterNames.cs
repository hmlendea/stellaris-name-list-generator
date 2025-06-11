using System.Collections.Generic;
using System.Linq;

namespace StellarisNameListGenerator.Models
{
    public sealed class CharacterNames
    {
        public string Id { get; set; }

        public int Weight { get; set; }

        public List<NameGroup> FullNames { get; set; }
        public List<NameGroup> FirstNames { get; set; }
        public List<NameGroup> RoyalFirstNames { get; set; }

        public List<NameGroup> MaleFullNames { get; set; }
        public List<NameGroup> MaleFirstNames { get; set; }
        public List<NameGroup> MaleRoyalFirstNames { get; set; }

        public List<NameGroup> FemaleFullNames { get; set; }
        public List<NameGroup> FemaleFirstNames { get; set; }
        public List<NameGroup> FemaleRoyalFirstNames { get; set; }

        public List<NameGroup> SecondNames { get; set; }
        public List<NameGroup> RoyalSecondNames { get; set; }

        public bool IsEmpty =>
            FullNames.All(x => x.IsEmpty) &&
            FirstNames.All(x => x.IsEmpty) &&
            RoyalFirstNames.All(x => x.IsEmpty) &&
            MaleFullNames.All(x => x.IsEmpty) &&
            MaleFirstNames.All(x => x.IsEmpty) &&
            MaleRoyalFirstNames.All(x => x.IsEmpty) &&
            FemaleFullNames.All(x => x.IsEmpty) &&
            FemaleFirstNames.All(x => x.IsEmpty) &&
            FemaleRoyalFirstNames.All(x => x.IsEmpty) &&
            SecondNames.All(x => x.IsEmpty) &&
            RoyalSecondNames.All(x => x.IsEmpty);

        public CharacterNames()
        {
            FullNames = [];
            FirstNames = [];
            RoyalFirstNames = [];

            MaleFullNames = [];
            MaleFirstNames = [];
            MaleRoyalFirstNames = [];

            FemaleFullNames = [];
            FemaleFirstNames = [];
            FemaleRoyalFirstNames = [];

            SecondNames = [];
            RoyalSecondNames = [];
        }
    }
}
