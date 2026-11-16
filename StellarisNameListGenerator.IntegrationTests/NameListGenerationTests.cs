using System;
using System.IO;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers end-to-end generation from XML input to the Stellaris-compatible output file, including
    /// the file envelope, the encoding contract, and the fixed ordering of output categories.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class NameListGenerationTests
    {
        private const string OutputFileName = "nucilandia.txt";
        private const string DisplayName = "Nucilandia Integration";

        private static readonly byte[] Utf8Preamble = [0xEF, 0xBB, 0xBF];

        private GeneratorEnvironment environment = null!;

        [SetUp]
        public void SetUp()
            => environment = new GeneratorEnvironment(nameof(NameListGenerationTests));

        [TearDown]
        public void TearDown()
            => environment.Dispose();

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenFileStartsWithUtf8ByteOrderMark()
        {
            environment.GenerateRichDocument(OutputFileName);

            environment.ReadBytes(OutputFileName)
                .Take(Utf8Preamble.Length)
                .Should()
                .BeEquivalentTo(Utf8Preamble);
        }

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenEveryLineUsesLineFeedEndingsOnly()
        {
            string content = environment.GenerateRichDocument(OutputFileName);

            Assert.That(content, Does.Not.Contain("\r"));
        }

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenHeadersPrecedeTheAssignmentBlock()
        {
            GeneratedNameList output = GeneratedNameList.Parse(environment.GenerateRichDocument(OutputFileName));

            Assert.That(output.HeaderLines, Has.Count.EqualTo(6));
            Assert.That(output.HeaderLines[0], Is.EqualTo($"### {NameListFixtures.RichRecordId}"));
            Assert.That(output.HeaderLines[1], Is.EqualTo($"### {NameListFixtures.RichRecordName}"));
            Assert.That(output.HeaderLines[2], Does.StartWith("### Leaders: "));
            Assert.That(output.HeaderLines[3], Does.StartWith("### Ships: "));
            Assert.That(output.HeaderLines[4], Does.StartWith("### Fleets: "));
            Assert.That(output.HeaderLines[5], Does.StartWith("### Colonies: "));
        }

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenIdentifierIsDerivedFromOutputFileName()
        {
            GeneratedNameList output = GeneratedNameList.Parse(environment.GenerateRichDocument("custom_identifier.txt"));

            Assert.That(output.AssignmentLine, Is.EqualTo("custom_identifier = {"));
            Assert.That(output.HeaderLines[0], Is.EqualTo("### custom_identifier"));
        }

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenDisplayNameIsWrittenToTheHeader()
        {
            GeneratedNameList output = GeneratedNameList.Parse(environment.GenerateRichDocument(OutputFileName));

            Assert.That(output.HeaderLines[1], Is.EqualTo($"### {NameListFixtures.RichRecordName}"));
        }

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenCategoriesAppearInFixedOrder()
        {
            GeneratedNameList output = GeneratedNameList.Parse(environment.GenerateRichDocument(OutputFileName));

            Assert.That(
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
        }

        [Test]
        public void GivenMinimalDocument_WhenGenerating_ThenOnlyStructurallyRequiredCategoriesAreEmitted()
        {
            string content = environment.GenerateFromDocument(NameListFixtures.MinimalDocument, OutputFileName, DisplayName);

            Assert.That(
                GeneratedNameList.Parse(content).TopLevelBlocks,
                Is.EqualTo(new[] { "ship_names", "fleet_names", "army_names", "planet_names" }));
        }

        [Test]
        public void GivenEmptyDocument_WhenGenerating_ThenGenerationStillProducesAnAssignmentBlock()
        {
            string content = environment.GenerateFromDocument(NameListFixtures.EmptyDocument, OutputFileName, DisplayName);

            Assert.That(GeneratedNameList.Parse(content).IsBalanced, Is.True);
        }

        [Test]
        public void GivenRichDocument_WhenGenerating_ThenEveryLineStaysWithinTheSupportedWidth()
        {
            string content = environment.GenerateRichDocument(OutputFileName);

            int longestLineLength = GeneratedNameList.Parse(content).Lines.Max(line => line.Length);

            Assert.That(longestLineLength, Is.LessThanOrEqualTo(150));
        }

        [Test]
        public void GivenPreExistingOutputFile_WhenGenerating_ThenPreviousContentIsReplaced()
        {
            File.WriteAllText(environment.GetPath(OutputFileName), "STALE CONTENT");

            string content = environment.GenerateFromDocument(NameListFixtures.RichDocument, OutputFileName, DisplayName);

            Assert.That(content, Does.Not.Contain("STALE CONTENT"));
            Assert.That(content, Does.Contain(NameListFixtures.CountryTwo));
        }

        [Test]
        public void GivenNestedOutputDirectory_WhenGenerating_ThenFileIsWrittenThere()
        {
            Directory.CreateDirectory(environment.GetPath("nested"));

            environment.WriteInput(NameListFixtures.RichDocument);
            environment.Generate("nested/nucilandia.txt", NameListFixtures.RichRecordName);

            Assert.That(File.Exists(environment.GetPath("nested/nucilandia.txt")), Is.True);
        }

        [Test]
        public void GivenTwoRecords_WhenGenerating_ThenValuesFromBothRecordsArePresent()
        {
            string content = environment.GenerateFromDocument(NameListFixtures.MergedDocument, OutputFileName, DisplayName);

            Assert.That(content, Does.Contain(NameListFixtures.CountryOne));
            Assert.That(content, Does.Contain(NameListFixtures.CountryTwo));
            Assert.That(content, Does.Contain(NameListFixtures.CountryThree));
        }
    }
}
