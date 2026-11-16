using System;
using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers end-to-end diacritic handling: the transliteration applied to generated values, the
    /// de-duplication that compares transliterated forms, and the quoting applied to spaced names.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class NameTransliterationTests
    {
        private const string OutputFileName = "diacritics.txt";
        private const string DisplayName = "Diacritic List";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(NameTransliterationTests));
            environment.WriteInput(NameListFixtures.DiacriticDocument);
            environment.Generate(OutputFileName, DisplayName);

            output = GeneratedNameList.ReadFromFile(environment.GetPath(OutputFileName));
        }

        [Test]
        public void GivenNamesWithDiacritics_WhenGenerating_ThenTheDiacriticsAreRemoved()
        {
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.TransliteratedDiacriticName), Is.True);
        }

        [Test]
        public void GivenDuplicateFormsDifferingOnlyByDiacritics_WhenGenerating_ThenOnlyOneFormIsKept()
        {
            IReadOnlyList<string> genericLines = output.GetBlockLines("ship_names", "generic");
            int occurrences = 0;

            foreach (string line in genericLines)
            {
                if (line.Contains(NameListFixtures.DuplicateDiacriticName, StringComparison.Ordinal))
                {
                    occurrences += line.Split(NameListFixtures.DuplicateDiacriticName).Length - 1;
                }
            }

            Assert.That(occurrences, Is.EqualTo(1));
        }

        [Test]
        public void GivenNamesContainingSpaces_WhenGenerating_ThenTheWholeNameIsQuoted()
        {
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.TransliteratedSpacedName), Is.True);
        }

        [Test]
        public void GivenValuesWithSurroundingWhitespace_WhenGenerating_ThenTheyAreDiscarded()
        {
            using GeneratorEnvironment environment = new("whitespace");
            environment.GenerateFromDocument(
                NameListXml.Document(
                    NameListXml.Record(
                        "<Id>whitespace</Id>",
                        NameListXml.Section("Denonyms", NameListXml.Group("Denonyms", "   ", NameListFixtures.CountryTwo, string.Empty)))),
                OutputFileName,
                DisplayName);

            GeneratedNameList whitespaceOutput = GeneratedNameList.Parse(environment.ReadText(OutputFileName));
            string genericBlock = string.Join('\n', whitespaceOutput.GetBlockLines("ship_names", "generic"));

            Assert.That(genericBlock, Does.Contain(NameListFixtures.CountryTwo));
            Assert.That(genericBlock.Split('\n').Length, Is.LessThan(6));
        }
    }
}
