using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the composition of the <c>planet_names</c> category: the mapping of source categories to
    /// planet classes, the exclusion of class-specific names from the generic category, and the omission
    /// of unpopulated planet classes.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class PlanetNameGenerationTests
    {
        private const string OutputFileName = "planets.txt";
        private const string DisplayName = "Planet List";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(PlanetNameGenerationTests));
            environment.WriteInput(NameListFixtures.PlanetClassDocument);

            output = environment.Generate(OutputFileName, DisplayName);
        }

        [Test]
        public void GivenEveryPlanetClass_WhenGenerating_ThenEachClassIsMappedToItsStellarisIdentifier()
        {
            Assert.That(output.SubBlockContains("planet_names", "pc_desert", NameListFixtures.DesertPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_arid", NameListFixtures.AridPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_tropical", NameListFixtures.TropicalPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_continental", NameListFixtures.ContinentalPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_gaia", NameListFixtures.GaiaPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_ocean", NameListFixtures.OceanPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_tundra", NameListFixtures.TundraPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_arctic", NameListFixtures.ArcticPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_savannah", NameListFixtures.SavannahPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_alpine", NameListFixtures.AlpinePlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_molten", NameListFixtures.MoltenPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_barren", NameListFixtures.BarrenPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_asteroid", NameListFixtures.AsteroidPlanetName), Is.True);
        }

        [Test]
        public void GivenATombPlanet_WhenGenerating_ThenItIsMappedToTheNukedIdentifier()
        {
            Assert.That(output.SubBlockContains("planet_names", "pc_nuked", NameListFixtures.TombPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_tomb", NameListFixtures.TombPlanetName), Is.False);
        }

        [Test]
        public void GivenAClassSpecificPlanetName_WhenGenerating_ThenItIsExcludedFromTheGenericCategory()
        {
            Assert.That(output.SubBlockContains("planet_names", "generic", NameListFixtures.PlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "generic", NameListFixtures.DesertPlanetName), Is.False);
        }

        [Test]
        public void GivenAPlanetClassWithoutValues_WhenGenerating_ThenThatClassBlockIsOmitted()
        {
            using GeneratorEnvironment environment = new("sparse_planets");

            GeneratedNameList sparseOutput = GeneratedNameList.Parse(
                environment.GenerateFromDocument(
                    NameListFixtures.SparsePlanetDocument,
                    OutputFileName,
                    DisplayName));

            Assert.That(sparseOutput.BlockContains("planet_names", "pc_desert"), Is.True);
            Assert.That(sparseOutput.BlockContains("planet_names", "pc_gaia"), Is.False);
            Assert.That(sparseOutput.BlockContains("planet_names", "pc_asteroid"), Is.False);
        }

        [Test]
        public void GivenPlanetClassValues_WhenGenerating_ThenTheyAreNestedUnderTheNamesArray()
        {
            Assert.That(output.SubBlockContains("planet_names", "pc_desert", "names = {"), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "pc_desert", "# >>> Desert"), Is.True);
        }
    }
}
