using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Asserts the declaration order of every generated sub-block within each populated category. The
    /// expected orders were captured from a document that populates every source category.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class BlockOrderingTests
    {
        private const string OutputFileName = "ordering.txt";
        private const string DisplayName = "Ordering";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(BlockOrderingTests));
            environment.WriteInput(CoverageDocument.Build());

            environment.Generate(OutputFileName, DisplayName, isLocked: true);
            output = GeneratedNameList.Parse(environment.ReadText(OutputFileName));
        }

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenShipNameBlocksFollowTheDeclaredOrder()
            => Assert.That(
                GetSubBlocks("ship_names"),
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
                    "colonizer",
                    "sponsored_colonizer",
                    "transport",
                    "mining_station",
                    "research_station",
                    "observation_station",
                    "military_station_small",
                    "military_station_medium",
                    "military_station_large",
                    "ion_cannon",
                }));

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenShipClassBlocksFollowTheDeclaredOrder()
            => Assert.That(
                GetSubBlocks("ship_class_names"),
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
                    "mining_station",
                    "research_station",
                    "observation_station",
                    "military_station_small",
                    "military_station_medium",
                    "military_station_large",
                    "ion_cannon",
                }));

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenPlanetClassBlocksFollowTheDeclaredOrder()
            => Assert.That(
                GetSubBlocks("planet_names"),
                Is.EqualTo(new[]
                {
                    "generic",
                    "pc_desert",
                    "pc_arid",
                    "pc_tropical",
                    "pc_continental",
                    "pc_gaia",
                    "pc_ocean",
                    "pc_tundra",
                    "pc_arctic",
                    "pc_nuked",
                    "pc_savannah",
                    "pc_alpine",
                    "pc_molten",
                    "pc_barren",
                    "pc_asteroid",
                }));

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenArmyBlocksFollowTheDeclaredOrder()
            => Assert.That(
                GetSubBlocks("army_names"),
                Is.EqualTo(new[]
                {
                    "machine_defense",
                    "machine_assault_1",
                    "machine_assault_2",
                    "machine_assault_3",
                    "defense_army",
                    "assault_army",
                    "occupation_army",
                    "slave_army",
                    "clone_army",
                    "perfected_clone_army",
                    "undead_army",
                    "robotic_defense_army",
                    "robotic_army",
                    "robotic_occupation_army",
                    "android_defense_army",
                    "android_army",
                    "psionic_army",
                    "xenomorph_army",
                    "gene_warrior_army",
                    "primitive_army",
                    "industrial_army",
                    "postatomic_army",
                }));

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenTopLevelCategoriesFollowTheDeclaredOrder()
            => Assert.That(
                output.TopLevelBlocks,
                Is.EqualTo(new[]
                {
                    "ship_names",
                    "ship_class_names",
                    "fleet_names",
                    "army_names",
                    "planet_names",
                    "character_names",
                }));

        [Test]
        public void GivenEverySourceCategory_WhenGenerating_ThenTheCharacterBlockPrecedesTheAssignmentClose()
        {
            int characterIndex = output.Content.IndexOf("        cid = {", System.StringComparison.Ordinal);
            int closingIndex = output.Content.LastIndexOf("\n}", System.StringComparison.Ordinal);

            Assert.That(characterIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(characterIndex, Is.LessThan(closingIndex));
        }

        private IReadOnlyList<string> GetSubBlocks(string section)
        {
            List<string> names = [];

            foreach (string line in output.GetBlockLines(section))
            {
                if (!line.StartsWith("        ", System.StringComparison.Ordinal) ||
                    line.StartsWith("            ", System.StringComparison.Ordinal))
                {
                    continue;
                }

                string trimmed = line.Trim();

                if (trimmed.EndsWith("= {", System.StringComparison.Ordinal))
                {
                    names.Add(trimmed[..trimmed.IndexOf(" = {", System.StringComparison.Ordinal)]);
                }
            }

            return names;
        }
    }
}
