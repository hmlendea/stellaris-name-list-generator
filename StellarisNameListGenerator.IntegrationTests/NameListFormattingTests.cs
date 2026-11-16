using System;
using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the generated Paradox script structure produced by each category builder, including
    /// quoting, transliteration, exclusions, derived names, and the sequential-name fallbacks.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class NameListFormattingTests
    {
        private const string OutputFileName = "nucilandia.txt";

        private GeneratorEnvironment environment = null!;
        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            environment = new GeneratorEnvironment(nameof(NameListFormattingTests));
            environment.WriteInput(NameListFixtures.RichDocument);
            environment.Generate(OutputFileName, NameListFixtures.RichRecordName);

            output = GeneratedNameList.Parse(environment.ReadText(OutputFileName));
        }

        [TearDown]
        public void TearDown()
            => environment.Dispose();

        [Test]
        public void GivenMultiWordNames_WhenGenerating_ThenTheyAreQuotedInTheShipNames()
        {
            Assert.That(output.BlockContains("ship_names", "\"Mehal Sefari\""), Is.True);
        }

        [Test]
        public void GivenUnlockedGeneration_WhenGenerating_ThenRandomisationIsEnabledInTheOutput()
        {
            Assert.That(output.BlockContains("ship_names", "randomized = yes"), Is.False);
            Assert.That(output.Content, Does.Contain("randomized = yes"));
        }

        [Test]
        public void GivenNamesWithSpaces_WhenGenerating_ThenTheSpacesArePreservedInsideQuotes()
        {
            Assert.That(output.BlockContains("ship_names", "\"Mehal Sefari\""), Is.True);
        }

        [Test]
        public void GivenDenonyms_WhenGenerating_ThenTheyAreExcludedFromTheGenericShipCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.False);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenCityValues_WhenGenerating_ThenTheyAreSortedAndSpaceSeparated()
        {
            string[] cityLines = GetBlockValues("ship_names", "destroyer");

            Assert.That(cityLines, Does.Contain($"{NameListFixtures.CityOne} {NameListFixtures.CityTwo}"));
        }

        [Test]
        public void GivenCategoryHeaders_WhenGenerating_ThenTheyAreEmittedAsComments()
        {
            Assert.That(output.SubBlockContains("ship_names", "generic", "# >>> Generic"), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "generic", "# >>> Denonyms"), Is.True);
        }

        [Test]
        public void GivenShipClasses_WhenGenerating_ThenManufacturerCompaniesAreIncluded()
        {
            Assert.That(output.SubBlockContains("ship_class_names", "generic", $"\"{NameListFixtures.ShipClassName}\""), Is.True);
        }

        [Test]
        public void GivenFleetNames_WhenGenerating_ThenTemplatesAreExpandedWithUnitTypesAndWeapons()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "The Mehal Sefari Fleet"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "The Shotel Armada"), Is.True);
        }

        [Test]
        public void GivenFleetNames_WhenGenerating_ThenExplicitFleetNamesAreIncludedVerbatim()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"\"{NameListFixtures.FleetName}\""), Is.True);
        }

        [Test]
        public void GivenMythologicalCreatures_WhenGenerating_ThenPsionicArmyVariantsAreGenerated()
        {
            Assert.That(output.SubBlockContains("army_names", "psionic_army", "Griffin Commando"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", "Griffin Legion"), Is.True);
        }

        [Test]
        public void GivenDeathDeities_WhenGenerating_ThenXenomorphArmyCombinationsAreGenerated()
        {
            Assert.That(output.SubBlockContains("army_names", "xenomorph_army", "Mors's Abomination Brood"), Is.True);
        }

        [Test]
        public void GivenPlanetValues_WhenGenerating_ThenTheyAreEmittedUnderTheMatchingPlanetClass()
        {
            Assert.That(output.SubBlockContains("planet_names", "pc_desert", NameListFixtures.DesertPlanetName), Is.True);
            Assert.That(output.SubBlockContains("planet_names", "generic", NameListFixtures.PlanetName), Is.True);
        }

        [Test]
        public void GivenEmptyPlanetCategories_WhenGenerating_ThenOnlyPopulatedPlanetClassesAreEmitted()
        {
            IReadOnlyList<string> planetLines = output.GetBlockLines("planet_names");
            string joinedLines = string.Join('\n', planetLines);

            Assert.That(joinedLines, Does.Contain("pc_desert"));
            Assert.That(joinedLines, Does.Not.Contain("pc_gaia"));
        }

        [Test]
        public void GivenCharacterNames_WhenGenerating_ThenCharacterIdsAreEmittedAsBlocks()
        {
            Assert.That(output.BlockContains("character_names", NameListFixtures.HeavyCharacterId), Is.True);
            Assert.That(output.BlockContains("character_names", NameListFixtures.LightCharacterId), Is.True);
        }

        [Test]
        public void GivenSequentialNames_WhenGenerating_ThenUnpopulatedArmyCategoriesFallBackToTheTemplate()
        {
            Assert.That(output.SubBlockContains("army_names", "primitive_army", "sequential_name = \"Primitive Army %C%\""), Is.True);
            Assert.That(output.SubBlockContains("army_names", "machine_defense", "sequential_name = \"%O% Planetary Guard\""), Is.True);
        }

        [Test]
        public void GivenRandomisedNames_WhenGenerating_ThenTheyAreWrappedInTheRandomNamesContainer()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "The Mehal Sefari Fleet"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "# >>> Fleets - Military Unit Types"), Is.True);
        }

        [Test]
        public void GivenGenericShipNames_WhenGenerating_ThenTheyAreEmittedUnderTheGenericCategory()
        {
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.GenericShipName), Is.True);
        }

        private string[] GetBlockValues(string blockName, string subBlockName)
        {
            List<string> values = [];

            foreach (string line in output.GetBlockLines(blockName, subBlockName))
            {
                string trimmed = line.Trim();

                if (trimmed.Length == 0 || trimmed.StartsWith('#') || trimmed.EndsWith("{", StringComparison.Ordinal) || trimmed.Equals("}", StringComparison.Ordinal))
                {
                    continue;
                }

                values.Add(trimmed);
            }

            return [.. values];
        }
    }
}
