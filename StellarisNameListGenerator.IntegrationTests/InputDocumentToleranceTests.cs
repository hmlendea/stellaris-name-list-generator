using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers tolerance of the input XML document: a missing declaration, a byte-order mark, leading
    /// whitespace, and an unrecognised root element.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class InputDocumentToleranceTests
    {
        private const string OutputFileName = "tolerance.txt";
        private const string DisplayName = "Tolerance List";

        private const string Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        private const string RootOpen = "<ArrayOfNameList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";

        private const string RootClose = "</ArrayOfNameList>";

        private const string Record = "<NameList><Id>x</Id><Denonyms><NameGroup><Name>Denonyms</Name><Values><string>Nucilandia</string></Values></NameGroup></Denonyms></NameList>";

        private GeneratorEnvironment environment = null!;

        [SetUp]
        public void SetUp()
            => environment = new GeneratorEnvironment(nameof(InputDocumentToleranceTests));

        [TearDown]
        public void TearDown()
            => environment.Dispose();

        [Test]
        public void GivenADocumentWithoutADeclaration_WhenGenerating_ThenItIsParsedSuccessfully()
        {
            environment.WriteInput($"{RootOpen}{Record}{RootClose}");

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenADocumentPrefixedWithAByteOrderMark_WhenGenerating_ThenItIsParsedSuccessfully()
        {
            environment.WriteInput($"\uFEFF{Declaration}{RootOpen}{Record}{RootClose}");

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenADocumentWithLeadingWhitespace_WhenGenerating_ThenItIsParsedSuccessfully()
        {
            environment.WriteInput($"\n\n   {RootOpen}{Record}{RootClose}");

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenAnUnrecognisedRootElement_WhenGenerating_ThenInvalidOperationExceptionIsThrown()
        {
            environment.WriteInput($"{Declaration}<Root>{Record}</Root>");

            Assert.That(
                () => environment.Generate(OutputFileName, DisplayName),
                Throws.TypeOf<System.InvalidOperationException>());
        }

        [Test]
        public void GivenNeighbouringWhitespaceNames_WhenGenerating_ThenTheyAreTrimmedBeforeComparison()
        {
            environment.WriteInput(
                $"{RootOpen}<NameList><Id>trim</Id><Denonyms><NameGroup><Name>Denonyms</Name><Values><string>  {NameListFixtures.CountryTwo}  </string></Values></NameGroup></Denonyms></NameList>{RootClose}");

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenADocumentWithoutADeclaration_WhenGenerating_ThenTheOutputStillCarriesItsByteOrderMark()
        {
            environment.WriteInput($"{RootOpen}{Record}{RootClose}");

            environment.Generate(OutputFileName, DisplayName);

            byte[] bytes = environment.ReadBytes(OutputFileName);

            Assert.That(bytes, Has.Length.GreaterThan(3));
            Assert.That(bytes[0], Is.EqualTo(0xEF));
            Assert.That(bytes[1], Is.EqualTo(0xBB));
            Assert.That(bytes[2], Is.EqualTo(0xBF));
        }

        [Test]
        public void GivenANameWithDiacritics_WhenGenerating_ThenTheOutputIsTransliterated()
        {
            environment.WriteInput($"{RootOpen}<NameList><Id>u</Id><Denonyms><NameGroup><Name>Denonyms</Name><Values><string>Țepeș</string></Values></NameGroup></Denonyms></NameList>{RootClose}");

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", "Tepes"), Is.True);
        }
    }
}
