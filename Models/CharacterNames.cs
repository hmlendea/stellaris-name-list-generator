using System.Collections.Generic;

namespace StellarisNameListGenerator.Models
{
    public sealed class CharacterNames
    {
        public string Id { get; set; }

        public int Weight { get; set; }

        public List<NameGroup> MaleFirstNames { get; set; }
        public List<NameGroup> MaleRoyalFirstNames { get; set; }

        public List<NameGroup> FemaleFirstNames { get; set; }
        public List<NameGroup> FemaleRoyalFirstNames { get; set; }
        
        public List<NameGroup> SecondNames { get; set; }
        public List<NameGroup> RoyalSecondNames { get; set; }

        public CharacterNames()
        {
            MaleFirstNames = new List<NameGroup>();
            MaleRoyalFirstNames = new List<NameGroup>();

            FemaleFirstNames = new List<NameGroup>();
            FemaleRoyalFirstNames = new List<NameGroup>();

            SecondNames = new List<NameGroup>();
            RoyalSecondNames = new List<NameGroup>();
        }
    }
}
