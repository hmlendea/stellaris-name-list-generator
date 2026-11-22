using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the composition of the <c>ship_class_names</c> category: which manufacturer categories feed
    /// each hull class, the merging of generic ship classes, and the mapped station and ion-cannon class
    /// entries.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class ShipClassNameGenerationTests
    {
        private const string OutputFileName = "ship_classes.txt";
        private const string DisplayName = "Ship Class List";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(ShipClassNameGenerationTests));
            environment.WriteInput(NameListFixtures.ShipClassCompositionDocument);

            output = environment.Generate(OutputFileName, DisplayName);
        }

        [Test]
        public void GivenManufacturersAndDenonyms_WhenGenerating_ThenTheyAllFeedTheGenericClassCategory()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.CountryTwo), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.GenericShipClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.SpacecraftManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.AircraftManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.RocketDesignerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.ShipTypeName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "corvette", NameListFixtures.CorvetteShipClassName), Is.True);
        }

        [Test]
        public void GivenWeaponManufacturers_WhenGenerating_ThenTheyFeedEveryCombatClassCategory()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "corvette", NameListFixtures.WeaponManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "destroyer", NameListFixtures.WeaponManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "cruiser", NameListFixtures.WeaponManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "battleship", NameListFixtures.WeaponManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "titan", NameListFixtures.WeaponManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "colossus", NameListFixtures.WeaponManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "juggernaut", NameListFixtures.WeaponManufacturerName), Is.True);
        }

        [Test]
        public void GivenAutomotiveManufacturers_WhenGenerating_ThenTheyFeedTheConstructorAndTransportClasses()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "constructor", NameListFixtures.AutomotiveManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "coloniser", NameListFixtures.AutomotiveManufacturerName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "transport", NameListFixtures.AutomotiveManufacturerName), Is.True);
        }

        [Test]
        public void GivenResearchCompanies_WhenGenerating_ThenTheyFeedTheScienceClassCategory()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "science", NameListFixtures.ResearchCompanyName), Is.True);
        }

        [Test]
        public void GivenInvestmentCompanies_WhenGenerating_ThenTheyFeedOnlyTheSponsoredColoniserClassCategory()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "sponsored_coloniser", NameListFixtures.InvestmentCompanyName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "coloniser", NameListFixtures.InvestmentCompanyName), Is.False);
        }

        [Test]
        public void GivenStationClasses_WhenGenerating_ThenGenericStationsAccumulateIntoEachSizeCategory()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_small", NameListFixtures.GenericStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_small", NameListFixtures.SmallStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_medium", NameListFixtures.MediumStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_large", NameListFixtures.LargeStationClassName), Is.True);
        }

        [Test]
        public void GivenArtilleryAndIonCannonClasses_WhenGenerating_ThenTheyFeedTheIonCannonClassCategory()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "ion_cannon", NameListFixtures.IonCannonShipClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "ion_cannon", NameListFixtures.ArtilleryName), Is.True);
        }

        [Test]
        public void GivenNoClassValuesForACategory_WhenGenerating_ThenThatCategoryIsOmitted()
        {
            Assert.That(output.BlockContains("ship_class_names", "mining_station"), Is.False);
            Assert.That(output.BlockContains("ship_class_names", "observation_station"), Is.False);
        }

        [Test]
        public void GivenAllClassSources_WhenGenerating_ThenCategoriesAppearInTheirFixedOrder()
        {
            Assert.That(
                GetSubBlockNames(),
                Is.EqualTo(new[]
                {
                    "generic",
                    "corvette",
                    "destroyer",
                    "cruiser",
                    "battleship",
                    "titan",
                    "colossus",
                    "juggernaut",
                    "constructor",
                    "science",
                    "coloniser",
                    "sponsored_coloniser",
                    "transport",
                    "military_station_small",
                    "military_station_medium",
                    "military_station_large",
                    "ion_cannon",
                }));
        }

        [Test]
        public void GivenNoClassValuesAtAll_WhenGenerating_ThenTheCategoryIsOmittedEntirely()
        {
            using GeneratorEnvironment environment = new("no_classes");

            GeneratedNameList emptyOutput = GeneratedNameList.Parse(
                environment.GenerateFromDocument(
                    NameListFixtures.MinimalDocument,
                    OutputFileName,
                    DisplayName));

            Assert.That(emptyOutput.BlockContains("ship_class_names", "generic"), Is.False);
            Assert.That(emptyOutput.TopLevelBlocks, Does.Not.Contain("ship_class_names"));
        }

        private IReadOnlyList<string> GetSubBlockNames()
        {
            List<string> names = [];

            foreach (string line in output.GetBlockLines("ship_class_names"))
            {
                if (!line.StartsWith("        ", System.StringComparison.Ordinal) ||
                    line.StartsWith("            ", System.StringComparison.Ordinal))
                {
                    continue;
                }

                string trimmed = line.Trim();

                if (!trimmed.EndsWith("= {", System.StringComparison.Ordinal))
                {
                    continue;
                }

                names.Add(trimmed[..trimmed.IndexOf(" = {", System.StringComparison.Ordinal)]);
            }

            return names;
        }
    }
}
