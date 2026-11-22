using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the composition of the <c>fleet_names</c> category: the deterministic expansion of every
    /// fleet-name template, the inclusion of explicit fleet names, and the pluralisation applied to the
    /// generated category headers.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class FleetNameGenerationTests
    {
        private const string OutputFileName = "fleets.txt";
        private const string DisplayName = "Fleet List";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(FleetNameGenerationTests));
            environment.WriteInput(NameListFixtures.FleetCompositionDocument);

            output = environment.Generate(OutputFileName, DisplayName);
        }

        [Test]
        public void GivenExplicitFleetNames_WhenGenerating_ThenTheyAppearVerbatimUnderTheFleetCategory()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"\"{NameListFixtures.ExplicitFleetName}\""), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "# >>> Fleets"), Is.True);
        }

        [Test]
        public void GivenWeaponNames_WhenGenerating_ThenEveryFleetTemplateUsesThem()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"The {NameListFixtures.WeaponName} Armada"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"Strike Force {NameListFixtures.WeaponName}"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"{NameListFixtures.WeaponName} Starfleet"), Is.True);
        }

        [Test]
        public void GivenMilitaryUnitTypes_WhenGenerating_ThenEveryFleetTemplateUsesThem()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"The {NameListFixtures.MilitaryUnitName} Fleet"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"Task Force {NameListFixtures.MilitaryUnitName}"), Is.True);
        }

        [Test]
        public void GivenShipTypes_WhenGenerating_ThenEveryFleetTemplateUsesThem()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"The {NameListFixtures.ShipTypeName} Corps"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"{NameListFixtures.ShipTypeName} Star Order"), Is.True);
        }

        [Test]
        public void GivenMythologicalCreatures_WhenGenerating_ThenEveryFleetTemplateUsesThem()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"The {NameListFixtures.MythologicalCreatureName} Flotilla"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", $"{NameListFixtures.MythologicalCreatureName} Squadron"), Is.True);
        }

        [Test]
        public void GivenAShipTypeEndingInS_WhenGenerating_ThenItsCategoryHeaderIsPluralisedOnce()
        {
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "# >>> Fleets - Ship Types"), Is.True);
            Assert.That(output.SubBlockContains("fleet_names", "random_names", "# >>> Navys - Ship Types"), Is.True);
            Assert.That(output.Content, Does.Not.Contain("Fleets - Ship Typess"));
        }

        [Test]
        public void GivenTheFullTemplateSet_WhenGenerating_ThenEveryCategorisedTemplateHeaderIsEmitted()
        {
            IReadOnlyList<string> categoryHeaders = GetCategoryHeaders();

            Assert.That(categoryHeaders, Does.Contain("Fleets - Weapons"));
            Assert.That(categoryHeaders, Does.Contain("Task Forces - Mythological Creatures"));
            Assert.That(categoryHeaders, Does.Contain("Strike Teams - Military Unit Types"));
            Assert.That(categoryHeaders, Does.Contain("Star Corps - Ship Types"));
        }

        [Test]
        public void GivenNoFleetNames_WhenGenerating_ThenTheSequentialNameTemplateIsEmitted()
        {
            using GeneratorEnvironment environment = new("empty_fleets");

            GeneratedNameList emptyOutput = GeneratedNameList.Parse(
                environment.GenerateFromDocument(
                    NameListFixtures.MinimalDocument,
                    OutputFileName,
                    DisplayName));

            Assert.That(emptyOutput.BlockContains("fleet_names", "sequential_name = \"%O% Fleet\""), Is.True);
        }

        [Test]
        public void GivenFleetNames_WhenGenerating_ThenNoSequentialNameTemplateIsEmitted()
        {
            Assert.That(output.BlockContains("fleet_names", "sequential_name"), Is.False);
        }

        private IReadOnlyList<string> GetCategoryHeaders()
        {
            List<string> headers = [];

            foreach (string line in output.GetBlockLines("fleet_names", "random_names"))
            {
                string trimmed = line.Trim();

                if (trimmed.StartsWith("# >>>", System.StringComparison.Ordinal))
                {
                    headers.Add(trimmed["# >>> ".Length..]);
                }
            }

            return headers;
        }
    }
}
