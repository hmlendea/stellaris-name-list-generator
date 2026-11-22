using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;
namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers value handling inside every generated array: sorting, case-sensitive distinctness,
    /// whitespace trimming, XML entity decoding, and the line-wrapping limit.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class NameListValueHandlingTests
    {
        private const string OutputFileName = "values.txt";
        private const string DisplayName = "Value List";

        [Test]
        public void GivenUnsortedValues_WhenGenerating_ThenTheyAreEmittedInOrdinalOrder()
        {
            string genericLine = GetGenericShipLine(NameListFixtures.DuplicateValueDocument);

            Assert.That(genericLine, Does.Contain("Acheron Achilles alpha Beta Zeta"));
        }

        [Test]
        public void GivenADuplicatedValue_WhenGenerating_ThenItAppearsOnlyOnce()
        {
            string genericLine = GetGenericShipLine(NameListFixtures.DuplicateValueDocument);

            Assert.That(genericLine.Split(NameListFixtures.DuplicateName).Length - 1, Is.EqualTo(1));
        }

        [Test]
        public void GivenValuesThatDifferOnlyInCase_WhenGenerating_ThenBothFormsAreRetained()
        {
            string genericLine = GetGenericShipLine(NameListFixtures.DuplicateValueDocument);

            Assert.That(genericLine, Does.Contain("alpha"));
            Assert.That(genericLine, Does.Contain("Beta"));
        }

        [Test]
        public void GivenAnXmlEscapedName_WhenGenerating_ThenTheEntityIsDecodedAndQuoted()
        {
            string genericLine = GetGenericShipLine(NameListFixtures.DuplicateValueDocument);

            Assert.That(genericLine, Does.Contain($"\"{NameListFixtures.AmpersandShipName}\""));
        }

        [Test]
        public void GivenNamesContainingQuotesOrAngleBrackets_WhenGenerating_ThenTheRawCharactersAreEmitted()
        {
            string genericLine = GetGenericShipLine(NameListFixtures.DuplicateValueDocument);

            Assert.That(genericLine, Does.Contain("Ze <Alpha>"));
            Assert.That(genericLine, Does.Not.Contain("&lt;"));
            Assert.That(genericLine, Does.Not.Contain("&quot;"));
        }

        [Test]
        public void GivenManyValues_WhenGenerating_ThenAllOfThemArePresentAcrossWrappedLines()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_wrapping");
            environment.WriteInput(BuildManyValuesDocument());

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            string joined = string.Join('\n', output.GetBlockLines("ship_names", "generic"));

            Assert.That(joined, Does.Contain("Alpha000"));
            Assert.That(joined, Does.Contain("Alpha059"));
        }

        [Test]
        public void GivenManyValues_WhenGenerating_ThenNoLineExceedsTheWrappingLimit()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_line_length");
            environment.WriteInput(BuildManyValuesDocument());

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            foreach (string line in output.Lines)
            {
                Assert.That(line.Length, Is.LessThanOrEqualTo(150));
            }
        }

        [Test]
        public void GivenManyValues_WhenGenerating_ThenTheGenericCategorySpansSeveralLines()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_multi_line");
            environment.WriteInput(BuildManyValuesDocument());

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.GetBlockLines("ship_names", "generic"), Has.Count.GreaterThan(2));
        }

        [Test]
        public void GivenValuesInSeveralNameGroups_WhenGenerating_ThenEachGroupKeepsItsOwnCommentHeader()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_groups");
            environment.WriteInput(NameListFixtures.ShipCompositionDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "corvette", "# >>> Animals"), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "corvette", "# >>> Swords"), Is.True);
        }

        [Test]
        public void GivenTwoGroupsWithTheSameName_WhenGenerating_ThenTheirValuesAreMergedUnderOneCommentHeader()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_same_name_groups");
            environment.WriteInput(
                NameListXml.Document(
                    NameListXml.Record(
                        "<Id>same_name</Id>",
                        NameListXml.Section("Ships", NameListXml.Section("Generic", string.Concat(
                            NameListXml.Group("Generic", "Alpha"),
                            NameListXml.Group("Generic", "Beta")))))));

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            IReadOnlyList<string> lines = output.GetBlockLines("ship_names", "generic");
            int headerOccurrences = 0;

            foreach (string line in lines)
            {
                if (line.Contains("# >>> Generic", System.StringComparison.Ordinal))
                {
                    headerOccurrences++;
                }
            }

            Assert.That(headerOccurrences, Is.EqualTo(1));
            Assert.That(output.SubBlockContains("ship_names", "generic", "Alpha Beta"), Is.True);
        }

        [Test]
        public void GivenAGroupWithoutAName_WhenGenerating_ThenValuesAreEmittedWithoutACommentHeader()
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_unnamed_group");
            environment.WriteInput(
                NameListXml.Document(
                    NameListXml.Record(
                        "<Id>unnamed</Id>",
                        NameListXml.Section("Ships", NameListXml.Section("Generic", NameListXml.Group(string.Empty, "Solitary"))))));

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", "Solitary"), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "generic", "# >>>"), Is.False);
        }

        private static string GetGenericShipLine(string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(NameListValueHandlingTests)}_single");
            environment.WriteInput(document);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            return string.Join(' ', output.GetBlockLines("ship_names", "generic"));
        }

        private static string BuildManyValuesDocument()
        {
            List<string> values = [];

            for (int index = 0; index < 60; index++)
            {
                values.Add($"Alpha{index:D3}");
            }

            return NameListXml.Document(
                NameListXml.Record(
                    "<Id>many_values</Id>",
                    NameListXml.Section("Ships", NameListXml.Section("Generic", NameListXml.Group("Generic", [.. values])))));
        }
    }
}