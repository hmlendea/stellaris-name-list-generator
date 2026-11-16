using System;
using System.IO;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers character name generation: ordering by weight, empty groups, duplicate identifiers, and
    /// missing identifiers.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class CharacterNameGenerationTests
    {
        private const string OutputFileName = "characters.txt";
        private const string DisplayName = "Character List";

        private GeneratorEnvironment environment = null!;

        [SetUp]
        public void SetUp()
            => environment = new GeneratorEnvironment(nameof(CharacterNameGenerationTests));

        [TearDown]
        public void TearDown()
            => environment.Dispose();

        [Test]
        public void GivenMixedWeights_WhenGenerating_ThenCharactersAreDescendingByWeight()
        {
            string content = environment.GenerateFromDocument(
                NameListFixtures.CharacterDocument,
                OutputFileName,
                DisplayName);

            GeneratedNameList output = GeneratedNameList.Parse(content);

            int firstIndex = content.IndexOf($"        {NameListFixtures.HeavyCharacterId} = {{", StringComparison.Ordinal);
            int secondIndex = content.IndexOf($"        {NameListFixtures.LightCharacterId} = {{", StringComparison.Ordinal);

            Assert.That(firstIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(secondIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(firstIndex, Is.LessThan(secondIndex));
            Assert.That(output.Contains(NameListFixtures.FemaleFirstName), Is.True);
        }

        [Test]
        public void GivenGivenCharacterNames_WhenGenerating_ThenTheExpectedNameGroupsAppear()
        {
            string content = environment.GenerateFromDocument(
                NameListFixtures.CharacterDocument,
                OutputFileName,
                DisplayName);

            GeneratedNameList output = GeneratedNameList.Parse(content);

            Assert.That(output.Contains(NameListFixtures.FemaleFirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.HeavyCharacterId, NameListFixtures.ThirdSurname), Is.True);
        }

        [Test]
        public void GivenEmptyCharacterGroups_WhenGenerating_ThenTheyAreOmitted()
        {
            string content = environment.GenerateFromDocument(
                NameListFixtures.EmptyCharacterDocument,
                OutputFileName,
                DisplayName);

            GeneratedNameList output = GeneratedNameList.Parse(content);

            Assert.That(output.SubBlockContains("character_names", "empty", NameListFixtures.FemaleFirstName), Is.False);
            Assert.That(output.SubBlockContains("character_names", "populated", NameListFixtures.FemaleFirstName), Is.True);
        }

        [Test]
        public void GivenMultipleCharacterGroupsWithSameId_WhenGenerating_ThenTheFirstGroupWins()
        {
            string content = environment.GenerateFromDocument(
                NameListFixtures.DuplicateCharacterIdDocument,
                OutputFileName,
                DisplayName);

            GeneratedNameList output = GeneratedNameList.Parse(content);

            string block = string.Join('\n', output.GetBlockLines("character_names", NameListFixtures.LightCharacterId)).ToLowerInvariant();

            Assert.That(block, Does.Contain(NameListFixtures.FemaleFirstName.ToLowerInvariant()));
            Assert.That(block, Does.Not.Contain(NameListFixtures.HeavyCharacterFirstName.ToLowerInvariant()));
        }

        [Test]
        public void GivenRecordWithoutIdentifier_WhenGenerating_ThenGenerationFails()
        {
            environment.WriteInput(
                NameListXml.Document(
                    NameListXml.Record(
                        NameListXml.CharacterWithoutId(NameListFixtures.LightCharacterWeight, NameListXml.Section("FemaleFirstNames", NameListXml.Group("Female first names", NameListFixtures.FemaleFirstName))))));

            Assert.That(
                () => environment.Generate(OutputFileName, DisplayName),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenZeroRecords_WhenGenerating_ThenOutputIsStillWellFormed()
        {
            string content = environment.GenerateFromDocument(
                NameListFixtures.EmptyDocument,
                OutputFileName,
                DisplayName);

            GeneratedNameList output = GeneratedNameList.Parse(content);

            Assert.That(output.IsBalanced, Is.True);
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.False);
        }
    }
}
