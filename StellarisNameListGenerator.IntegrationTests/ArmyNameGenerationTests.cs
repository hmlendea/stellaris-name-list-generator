using System.Collections.Generic;
using System.Text.RegularExpressions;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the composition of the <c>army_names</c> category: the fixed set of army sub-blocks, the
    /// generated psionic and xenomorph variants, the explicit army names, and the sequential-name
    /// fallbacks used when a category has no values.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class ArmyNameGenerationTests
    {
        private const string OutputFileName = "armies.txt";
        private const string DisplayName = "Army List";

        private static readonly Regex SubBlockPattern = new(@"^ {8}(?<name>[a-z_0-9]+) = \{$");

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(ArmyNameGenerationTests));
            environment.WriteInput(NameListFixtures.ArmyCompositionDocument);

            output = environment.Generate(OutputFileName, DisplayName);
        }

        [Test]
        public void GivenAnArmyDocument_WhenGenerating_ThenEveryArmySubBlockIsEmittedInTheFixedOrder()
        {
            Assert.That(
                GetSubBlockNames(output),
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
        }

        [Test]
        public void GivenExplicitDefenceArmyNames_WhenGenerating_ThenTheyAppearUnderDefenseArmy()
        {
            Assert.That(output.SubBlockContains("army_names", "defense_army", NameListFixtures.DefenceArmyName), Is.True);
        }

        [Test]
        public void GivenMythologicalCreatures_WhenGenerating_ThenEveryPsionicArmyVariantIsGenerated()
        {
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Commando"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Covert Ops"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Division"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Elite Corps"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Legion"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Platoon"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "psionic_army", $"{NameListFixtures.MythologicalCreatureName} Squadron"), Is.True);
        }

        [Test]
        public void GivenPsionicArmyValues_WhenGenerating_ThenTheyAreMergedWithTheGeneratedVariants()
        {
            Assert.That(output.SubBlockContains("army_names", "psionic_army", NameListFixtures.PsionicArmyName), Is.True);
        }

        [Test]
        public void GivenDeathDeities_WhenGenerating_ThenXenomorphVariantsCombineEveryPair()
        {
            Assert.That(output.SubBlockContains("army_names", "xenomorph_army", $"{NameListFixtures.DeathDeityName}'s Abomination Brood"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "xenomorph_army", $"{NameListFixtures.DeathDeityName}'s Xenomorph Warband"), Is.True);
        }

        [Test]
        public void GivenExplicitXenomorphValues_WhenGenerating_ThenTheyAreMergedWithTheGeneratedVariants()
        {
            Assert.That(output.SubBlockContains("army_names", "xenomorph_army", NameListFixtures.XenomorphArmyName), Is.True);
        }

        [Test]
        public void GivenExplicitPrimitiveArmyValues_WhenGenerating_ThenTheyReplaceTheSequentialTemplate()
        {
            Assert.That(output.SubBlockContains("army_names", "primitive_army", NameListFixtures.PrimitiveArmyName), Is.True);
            Assert.That(output.SubBlockContains("army_names", "primitive_army", "sequential_name"), Is.False);
        }

        [Test]
        public void GivenNoIndustrialArmyValues_WhenGenerating_ThenTheCountedSequentialTemplateIsEmitted()
        {
            Assert.That(output.SubBlockContains("army_names", "industrial_army", "sequential_name = \"Industrial Army %C%\""), Is.True);
            Assert.That(output.SubBlockContains("army_names", "postatomic_army", "sequential_name = \"Post-Atomic Army %C%\""), Is.True);
        }

        [Test]
        public void GivenNoMachineArmyValues_WhenGenerating_ThenOrdinalSequentialTemplatesAreEmitted()
        {
            Assert.That(output.SubBlockContains("army_names", "machine_defense", "sequential_name = \"%O% Planetary Guard\""), Is.True);
            Assert.That(output.SubBlockContains("army_names", "machine_assault_1", "sequential_name = \"%O% Expeditionary Force\""), Is.True);
            Assert.That(output.SubBlockContains("army_names", "machine_assault_3", "sequential_name = \"%O% Expeditionary Force\""), Is.True);
        }

        [Test]
        public void GivenAnArmyDocument_WhenGenerating_ThenRandomNamesAreWrappedInTheirContainer()
        {
            Assert.That(output.SubBlockContains("army_names", "defense_army", "random_names = {"), Is.True);
            Assert.That(output.SubBlockContains("army_names", "xenomorph_army", "# >>> Abomination Broods - Deities"), Is.True);
        }

        private static IEnumerable<string> GetSubBlockNames(GeneratedNameList generatedNameList)
        {
            List<string> names = [];

            foreach (string line in generatedNameList.GetBlockLines("army_names"))
            {
                Match match = SubBlockPattern.Match(line);

                if (match.Success)
                {
                    names.Add(match.Groups["name"].Value);
                }
            }

            return names;
        }
    }
}