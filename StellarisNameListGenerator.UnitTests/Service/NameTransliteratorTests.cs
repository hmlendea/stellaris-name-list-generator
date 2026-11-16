using NUnit.Framework;

using StellarisNameListGenerator.Service.Text;

namespace StellarisNameListGenerator.UnitTests.Service
{
    [TestFixture]
    public sealed class NameTransliteratorTests
    {
        [TestCase("Café", "Café")]
        [TestCase("Łódź", "Lódz")]
        [TestCase("Škoda", "Shkoda")]
        [TestCase("Đorđe", "Ðordže")]
        [TestCase("İstanbul", "Istanbul")]
        public void GivenASingleCharacterSubstitution_WhenProcessingName_ThenReturnsExpectedName(
            string name,
            string expectedName)
            => Assert.That(
                NameTransliterator.ProcessName(name),
                Is.EqualTo(expectedName));

        [Test]
        public void GivenAnUnmappedLatinName_WhenProcessingName_ThenReturnsTheNameUnchanged()
            => Assert.That(
                NameTransliterator.ProcessName("Kapteyn's Star"),
                Is.EqualTo("Kapteyn's Star"));

        [Test]
        public void GivenAnApostropheCharacter_WhenProcessingName_ThenDetachesTheApostrophe()
            => Assert.That(
                NameTransliterator.ProcessName("ƙ"),
                Is.EqualTo("k'"));

        [Test]
        public void GivenALigatureCharacter_WhenProcessingName_ThenReplacesItWithTheCharsetEquivalent()
            => Assert.That(
                NameTransliterator.ProcessName("Ǣ"),
                Is.EqualTo("Æ"));

        [Test]
        public void GivenAnEmptyName_WhenProcessingName_ThenReturnsAnEmptyName()
            => Assert.That(
                NameTransliterator.ProcessName(string.Empty),
                Is.Empty);

        [Test]
        public void GivenTheSameName_WhenProcessingTwice_ThenReturnsTheSameResult()
        {
            string firstResult = NameTransliterator.ProcessName("Đorđe");
            string secondResult = NameTransliterator.ProcessName("Đorđe");

            Assert.That(secondResult, Is.EqualTo(firstResult));
        }
    }
}
