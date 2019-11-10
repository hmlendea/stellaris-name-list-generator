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
            FullNames.Sum(x => x.Values.Count) == 0 &&
            FirstNames.Sum(x => x.Values.Count) == 0 &&
            RoyalFirstNames.Sum(x => x.Values.Count) == 0 &&
            MaleFullNames.Sum(x => x.Values.Count) == 0 &&
            MaleFirstNames.Sum(x => x.Values.Count) == 0 &&
            MaleRoyalFirstNames.Sum(x => x.Values.Count) == 0 &&
            FemaleFullNames.Sum(x => x.Values.Count) == 0 &&
            FemaleFirstNames.Sum(x => x.Values.Count) == 0 &&
            FemaleRoyalFirstNames.Sum(x => x.Values.Count) == 0 &&
            SecondNames.Sum(x => x.Values.Count) == 0 &&
            RoyalSecondNames.Sum(x => x.Values.Count) == 0;

        public CharacterNames()
        {
            FullNames = new List<NameGroup>();
            FirstNames = new List<NameGroup>();
            RoyalFirstNames = new List<NameGroup>();

            MaleFullNames = new List<NameGroup>();
            MaleFirstNames = new List<NameGroup>();
            MaleRoyalFirstNames = new List<NameGroup>();

            FemaleFullNames = new List<NameGroup>();
            FemaleFirstNames = new List<NameGroup>();
            FemaleRoyalFirstNames = new List<NameGroup>();

            SecondNames = new List<NameGroup>();
            RoyalSecondNames = new List<NameGroup>();
        }
    }
}
