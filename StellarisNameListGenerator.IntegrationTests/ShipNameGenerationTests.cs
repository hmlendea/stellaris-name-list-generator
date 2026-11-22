using System.Collections.Generic;
using System.Text.RegularExpressions;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the composition of the <c>ship_names</c> category: which source categories feed each
    /// hull type, the exclusion of denonyms from the specialised categories, and the derived
    /// station and ion-cannon entries.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class ShipNameGenerationTests
    {
        private const string OutputFileName = "ships.txt";
        private const string DisplayName = "Ship List";

        private static readonly Regex SubBlockPattern = new(@"^ {8}(?<name>[a-z_0-9]+) = \{$");

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(ShipNameGenerationTests));
            environment.WriteInput(NameListFixtures.ShipCompositionDocument);

            output = environment.Generate(OutputFileName, DisplayName);
        }

        [Test]
        public void GivenGenericShipsAndDenonyms_WhenGenerating_ThenBothAppearUnderTheGenericCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.GenericShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenSharedFighterNames_WhenGenerating_ThenWeaponsLeadersAndCreaturesShareTheCorvetteCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.MilitaryUnitName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.WeaponName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.ArtilleryName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.CorvetteShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.LeaderName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.AnimalName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.MythologicalCreatureName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.PunishmentDeityName), Is.True);
        }

        [Test]
        public void GivenCrossCategoryDuplicates_WhenGenerating_ThenASharedNameAppearsInOnlyOneCorvetteEntry()
        {
            IReadOnlyList<string> corvetteLines = output.GetBlockLines("ship_names", "corvette");
            int occurrences = 0;

            foreach (string line in corvetteLines)
            {
                occurrences += CountOccurrences(line, NameListFixtures.MythologicalCreatureName);
            }

            Assert.That(occurrences, Is.EqualTo(1));
        }

        [Test]
        public void GivenCitiesAndForts_WhenGenerating_ThenTheyFeedTheDestroyerCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "destroyer", NameListFixtures.CityOne), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "destroyer", NameListFixtures.CityTwo), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "destroyer", NameListFixtures.DestroyerShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "destroyer", NameListFixtures.FortName), Is.True);
        }

        [Test]
        public void GivenRegionsAndAdmirals_WhenGenerating_ThenTheyFeedTheCruiserCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "cruiser", NameListFixtures.CruiserShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "cruiser", NameListFixtures.RegionName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "cruiser", NameListFixtures.AdmiralName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "cruiser", NameListFixtures.TierTwoLeaderName), Is.False);
            Assert.That(output.SubBlockContains("ship_names", "cruiser", NameListFixtures.TransliteratedTierTwoLeaderName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "cruiser", NameListFixtures.PowerDeityName), Is.True);
        }

        [Test]
        public void GivenCountriesAndWarDeities_WhenGenerating_ThenTheyFeedTheBattleshipCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "battleship", NameListFixtures.BattleshipShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "battleship", NameListFixtures.CountryOne), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "battleship", NameListFixtures.TierThreeLeaderName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "battleship", NameListFixtures.WarDeityName), Is.True);
        }

        [Test]
        public void GivenCreationAndDeathDeities_WhenGenerating_ThenTheyFeedTheTitanAndColossusCategories()
        {
            Assert.That(output.SubBlockContains("ship_names", "titan", NameListFixtures.TitanShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "titan", NameListFixtures.CreationDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "titan", NameListFixtures.DeathDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "colossus", NameListFixtures.ColossusShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "colossus", NameListFixtures.CreationDeityName), Is.True);
        }

        [Test]
        public void GivenDedicatedJuggernautNames_WhenGenerating_ThenTheyAppearWithoutAnyGeneratedAdditions()
        {
            Assert.That(output.SubBlockContains("ship_names", "juggernaut", NameListFixtures.JuggernautShipName), Is.True);
            Assert.That(output.GetBlockLines("ship_names", "juggernaut"), Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenGeographicalPlaces_WhenGenerating_ThenTheyFeedTheConstructorCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "constructor", NameListFixtures.ConstructorShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "constructor", NameListFixtures.MountainName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "constructor", NameListFixtures.DesertName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "constructor", NameListFixtures.SeaName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "constructor", NameListFixtures.LabourDeityName), Is.True);
        }

        [Test]
        public void GivenExplorersAndKnowledgeDeities_WhenGenerating_ThenTheyFeedTheScienceCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "science", NameListFixtures.ScienceShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "science", NameListFixtures.ExplorerName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "science", NameListFixtures.KnowledgeDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "science", NameListFixtures.HealthDeityName), Is.True);
        }

        [Test]
        public void GivenColoniserNames_WhenGenerating_ThenTheSponsoredColoniserMirrorsTheColoniserCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "colonizer", NameListFixtures.ColoniserShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "colonizer", NameListFixtures.PeaceDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "sponsored_colonizer", NameListFixtures.ColoniserShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "sponsored_colonizer", NameListFixtures.PeaceDeityName), Is.True);
        }

        [Test]
        public void GivenTransportNames_WhenGenerating_ThenTheyCombineWeaponsUnitsAndForts()
        {
            Assert.That(output.SubBlockContains("ship_names", "transport", NameListFixtures.TransportShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "transport", NameListFixtures.ArtilleryName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "transport", NameListFixtures.FortName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "transport", NameListFixtures.MilitaryUnitName), Is.True);
        }

        [Test]
        public void GivenStationNames_WhenGenerating_ThenEachStationCategoryReceivesItsOwnEntries()
        {
            Assert.That(output.SubBlockContains("ship_names", "mining_station", NameListFixtures.LabourDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "research_station", NameListFixtures.ResearchCompanyName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "observation_station", NameListFixtures.KnowledgeDeityName), Is.True);
        }

        [Test]
        public void GivenMilitaryStationNames_WhenGenerating_ThenSizeCategoriesAccumulateLeadersAndWeapons()
        {
            Assert.That(output.SubBlockContains("ship_names", "military_station_small", NameListFixtures.LeaderName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_small", NameListFixtures.TierThreeLeaderName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_medium", NameListFixtures.PowerDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_large", NameListFixtures.WarDeityName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_large", NameListFixtures.CreationDeityName), Is.True);
        }

        [Test]
        public void GivenRangedWeapons_WhenGenerating_ThenOnlyRangedWeaponsFeedTheIonCannonCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "ion_cannon", NameListFixtures.ArtilleryName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "ion_cannon", NameListFixtures.IonCannonShipName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "ion_cannon", NameListFixtures.WeaponName), Is.False);
        }

        [Test]
        public void GivenDenonyms_WhenGenerating_ThenTheyAreAbsentFromTheWeaponDrivenCategories()
        {
            Assert.That(output.SubBlockContains("ship_names", "corvette", NameListFixtures.CountryTwo), Is.False);
            Assert.That(output.SubBlockContains("ship_names", "destroyer", NameListFixtures.CountryTwo), Is.False);
            Assert.That(output.SubBlockContains("ship_names", "constructor", NameListFixtures.CountryTwo), Is.False);
        }

        [Test]
        public void GivenMultiWordUnitTypes_WhenGenerating_ThenTheyAreQuotedInTheCorvetteCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "corvette", $"\"{NameListFixtures.MilitaryUnitName}\""), Is.True);
        }

        [Test]
        public void GivenPopulatedCategories_WhenGenerating_ThenShipSubBlocksAppearInTheirFixedOrder()
        {
            List<string> subBlocks = [];

            foreach (string line in output.GetBlockLines("ship_names"))
            {
                Match match = SubBlockPattern.Match(line);

                if (match.Success)
                {
                    subBlocks.Add(match.Groups["name"].Value);
                }
            }

            Assert.That(subBlocks, Does.Contain("generic"));
            Assert.That(subBlocks.IndexOf("generic"), Is.LessThan(subBlocks.IndexOf("corvette")));
            Assert.That(subBlocks.IndexOf("corvette"), Is.LessThan(subBlocks.IndexOf("destroyer")));
            Assert.That(subBlocks.IndexOf("destroyer"), Is.LessThan(subBlocks.IndexOf("cruiser")));
            Assert.That(subBlocks.IndexOf("cruiser"), Is.LessThan(subBlocks.IndexOf("battleship")));
            Assert.That(subBlocks.IndexOf("battleship"), Is.LessThan(subBlocks.IndexOf("titan")));
            Assert.That(subBlocks.IndexOf("titan"), Is.LessThan(subBlocks.IndexOf("colossus")));
            Assert.That(subBlocks.IndexOf("colossus"), Is.LessThan(subBlocks.IndexOf("juggernaut")));
        }

        [Test]
        public void GivenColoniserSources_WhenGenerating_ThenTheSponsoredColoniserDirectlyFollowsTheColoniser()
        {
            int coloniserIndex = output.Content.IndexOf("        colonizer = {", System.StringComparison.Ordinal);
            int sponsoredIndex = output.Content.IndexOf("        sponsored_colonizer = {", System.StringComparison.Ordinal);

            Assert.That(coloniserIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(sponsoredIndex, Is.GreaterThan(coloniserIndex));
        }

        private static int CountOccurrences(string text, string value)
            => text.Split(value).Length - 1;
    }
}
