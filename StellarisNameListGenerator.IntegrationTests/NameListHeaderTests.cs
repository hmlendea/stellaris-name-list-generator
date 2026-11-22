using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the generated comment header block: the identifier and display-name lines, the four
    /// deterministic sample lines, and the blank separator line that precedes the assignment block.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class NameListHeaderTests
    {
        private const string OutputFileName = "headers.txt";
        private const string DisplayName = "Header List";

        [Test]
        public void GivenASingleValuedDocument_WhenGenerating_ThenEachSampleLineRepeatsItsOnlyValue()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_single");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.HeaderLines[2], Is.EqualTo($"### Leaders: {NameListFixtures.MaleFullName}, {NameListFixtures.MaleFullName}"));
            Assert.That(output.HeaderLines[3], Is.EqualTo($"### Ships: {NameListFixtures.CountryTwo}, {NameListFixtures.CountryTwo}"));
            Assert.That(output.HeaderLines[4], Is.EqualTo($"### Fleets: {NameListFixtures.ExplicitFleetName}, {NameListFixtures.ExplicitFleetName}"));
            Assert.That(output.HeaderLines[5], Is.EqualTo($"### Colonies: {NameListFixtures.PlanetName}, {NameListFixtures.PlanetName}"));
        }

        [Test]
        public void GivenAHeaderBlock_WhenGenerating_ThenItIsFollowedByABlankSeparatorLine()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_separator");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.Lines[6], Is.Empty);
            Assert.That(output.AssignmentLine, Is.EqualTo("headers = {"));
        }

        [Test]
        public void GivenAnEmptyDocument_WhenGenerating_ThenTheSampleLinesArePresentButVacant()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_empty");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(
                    NameListFixtures.EmptyDocument,
                    OutputFileName,
                    DisplayName));

            Assert.That(output.HeaderLines[2], Is.EqualTo("### Leaders: , "));
            Assert.That(output.HeaderLines[3], Is.EqualTo("### Ships: , "));
            Assert.That(output.HeaderLines[5], Is.EqualTo("### Colonies: , "));
        }

        [Test]
        public void GivenAFleetlessDocument_WhenGenerating_ThenTheFleetSampleFallsBackToTheSequentialTemplate()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_fleetless");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(
                    NameListFixtures.MinimalDocument,
                    OutputFileName,
                    DisplayName));

            Assert.That(output.HeaderLines[4], Does.StartWith("### Fleets: "));
            Assert.That(output.HeaderLines[4], Does.Contain("Fleet"));
        }

        [Test]
        public void GivenASpacedOutputFileName_WhenGenerating_ThenTheIdentifierKeepsTheSpaces()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_spaces");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            GeneratedNameList output = environment.Generate("My Header List.txt", DisplayName);

            Assert.That(output.HeaderLines[0], Is.EqualTo("### My Header List"));
            Assert.That(output.Content, Does.Contain("My Header List = {"));
        }

        [Test]
        public void GivenAMultiDottedOutputFileName_WhenGenerating_ThenOnlyTheLastExtensionIsStripped()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_dots");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            GeneratedNameList output = environment.Generate("my.header.list.txt", DisplayName);

            Assert.That(output.AssignmentLine, Is.EqualTo("my.header.list = {"));
        }

        [Test]
        public void GivenACharacterWithFullNames_WhenGenerating_ThenTheLeadersSampleUsesTheFullName()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_full_names");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.HeaderLines[2], Is.EqualTo($"### Leaders: {NameListFixtures.MaleFullName}, {NameListFixtures.MaleFullName}"));
        }

        [Test]
        public void GivenACharacterWithOnlySecondNames_WhenGenerating_ThenTheLeadersSampleIsVacant()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListHeaderTests)}_second_names_only");
            environment.WriteInput(
                NameListXml.Document(
                    NameListXml.Record(
                        "<Id>second_only</Id>",
                        NameListXml.Section("Characters", NameListXml.Character(
                            NameListFixtures.LightCharacterId,
                            NameListFixtures.LightCharacterWeight,
                            NameListXml.Section("SecondNames", NameListXml.Group("Second names", NameListFixtures.SecondSurname)))))));

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.HeaderLines[2], Is.EqualTo("### Leaders: , "));
        }
    }
}