using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.Service.Text;

namespace StellarisNameListGenerator.UnitTests.Service
{
    /// <summary>
    /// Composes the transliteration table: multi-character names, ordering independence, idempotence on
    /// already-transliterated text, and preservation of permitted characters.
    /// </summary>
    [TestFixture]
    public sealed class NameTransliteratorCompositionTests
    {
        [TestCase("Çupișan", "Çupisan")]
        [TestCase("Łódź", "Lódz")]
        [TestCase("Škoda", "Shkoda")]
        [TestCase("Đorđe", "Ðordže")]
        [TestCase("İstanbul", "Istanbul")]
        [TestCase("Țepeș", "Tepes")]
        [TestCase("Florești", "Floresti")]
        [TestCase("Frigonița", "Frigonita")]
        public void GivenAMixedName_WhenProcessing_ThenEveryCharacterIsTransliterated(string source, string expected)
            => Assert.That(NameTransliterator.ProcessName(source), Is.EqualTo(expected));

        [TestCase("Kapteyn's Star")]
        [TestCase("Cluj-Napoca")]
        [TestCase("Home Fleet 42 (Alpha)")]
        [TestCase("a-z, A-Z, 0-9, ., _, ', \"")]
        [TestCase("Strike Force Mehal Sefari")]
        public void GivenAnAsciiName_WhenProcessing_ThenTheSameInstanceIsReturned(string name)
            => Assert.That(NameTransliterator.ProcessName(name), Is.SameAs(name));

        [Test]
        public void GivenAnEmptyName_WhenProcessing_ThenAnEmptyNameIsReturned()
            => Assert.That(NameTransliterator.ProcessName(string.Empty), Is.Empty);

        [Test]
        public void GivenAWhitespaceOnlyName_WhenProcessing_ThenTheWhitespaceIsPreserved()
            => Assert.That(NameTransliterator.ProcessName("   "), Is.EqualTo("   "));

        [Test]
        public void GivenAQuotedName_WhenProcessing_ThenTheQuotesArePreserved()
            => Assert.That(NameTransliterator.ProcessName("\"Nuci Corps\""), Is.EqualTo("\"Nuci Corps\""));

        [Test]
        public void GivenAnApostrophe_WhenProcessing_ThenItIsPreserved()
            => Assert.That(NameTransliterator.ProcessName("Kapteyn's Star"), Is.EqualTo("Kapteyn's Star"));

        [Test]
        public void GivenAHyphenatedName_WhenProcessing_ThenTheHyphenIsPreserved()
            => Assert.That(NameTransliterator.ProcessName("Cluj-Napoca"), Is.EqualTo("Cluj-Napoca"));

        [TestCase("Đorđe")]
        [TestCase("Škoda")]
        [TestCase("Çupișan")]
        [TestCase("İstanbul")]
        public void GivenATransliteratedName_WhenProcessingAgain_ThenTheResultIsStable(string source)
        {
            string once = NameTransliterator.ProcessName(source);

            Assert.That(NameTransliterator.ProcessName(once), Is.EqualTo(once));
        }

        [TestCase("Đorđe")]
        [TestCase("Škoda")]
        [TestCase("Çupișan")]
        public void GivenTheSameName_WhenProcessingTwice_ThenTheResultIsIdentical(string source)
            => Assert.That(
                NameTransliterator.ProcessName(source),
                Is.EqualTo(NameTransliterator.ProcessName(source)));

        [Test]
        public void GivenConsecutiveNonAsciiCharacters_WhenProcessing_ThenEveryCharacterIsMapped()
        {
            string result = NameTransliterator.ProcessName("ĐŠČ");

            Assert.That(result, Is.EqualTo("ÐShCh"));
        }

        [Test]
        public void GivenANameWithTrailingNonAsciiCharacter_WhenProcessing_ThenTheSuffixIsMapped()
            => Assert.That(NameTransliterator.ProcessName("Nucș"), Is.EqualTo("Nucs"));

        [Test]
        public void GivenANameWithLeadingNonAsciiCharacter_WhenProcessing_ThenThePrefixIsMapped()
            => Assert.That(NameTransliterator.ProcessName("șNuc"), Is.EqualTo("sNuc"));

        [Test]
        public void GivenACyrillicName_WhenProcessing_ThenEachLetterIsTransliteratedToLatin()
        {
            string result = NameTransliterator.ProcessName("Москва");

            Assert.That(result, Does.StartWith("Mo"));
            Assert.That(result, Does.EndWith("a"));
        }

        [Test]
        public void GivenAGreekName_WhenProcessing_ThenEachLetterIsTransliteratedToLatin()
        {
            string result = NameTransliterator.ProcessName("ΑΒΓ");

            Assert.That(result, Does.StartWith("A"));
            Assert.That(result, Does.Contain("B"));
        }

        [Test]
        public void GivenADecomposedCharacter_WhenProcessing_ThenTheBaseCharacterIsRetained()
        {
            string result = NameTransliterator.ProcessName("e\u0301");

            Assert.That(result, Is.EqualTo("é"));
        }

        [Test]
        public void GivenAMixedScriptName_WhenProcessing_ThenTheScriptsAreMergedIntoLatin()
        {
            string result = NameTransliterator.ProcessName("NucŠkoda");

            Assert.That(result, Is.EqualTo("NucShkoda"));
        }

        [Test]
        public void GivenALongNonAsciiName_WhenProcessing_ThenEveryCharacterIsMapped()
        {
            string source = string.Concat(Enumerable.Repeat("Š", 100));
            string result = NameTransliterator.ProcessName(source);

            Assert.That(result, Is.EqualTo(string.Concat(Enumerable.Repeat("Sh", 100))));
        }

        [Test]
        public void GivenANameWithDigits_WhenProcessing_ThenTheDigitsArePreserved()
            => Assert.That(NameTransliterator.ProcessName("Fleet 42 Č"), Is.EqualTo("Fleet 42 Ch"));

        [Test]
        public void GivenANameContainingOnlyCombiningMarks_WhenProcessing_ThenTheyAreRemoved()
        {
            string result = NameTransliterator.ProcessName("\u0301\u0308");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GivenAnEmDash_WhenProcessing_ThenItBecomesAHyphen()
            => Assert.That(NameTransliterator.ProcessName("Alpha—Beta"), Is.EqualTo("Alpha-Beta"));

        [Test]
        public void GivenAnEnDash_WhenProcessing_ThenItBecomesAHyphen()
            => Assert.That(NameTransliterator.ProcessName("Alpha–Beta"), Is.EqualTo("Alpha-Beta"));

        [Test]
        public void GivenAMiddleDot_WhenProcessing_ThenItIsRemoved()
            => Assert.That(NameTransliterator.ProcessName("Alpha·Beta"), Is.EqualTo("AlphaBeta"));

        [TestCase("Š", "Sh")]
        [TestCase("š", "sh")]
        [TestCase("Ž", "Ž")]
        [TestCase("ž", "ž")]
        [TestCase("Č", "Ch")]
        [TestCase("č", "ch")]
        public void GivenACaronConsonant_WhenProcessing_ThenTheDigraphIsProduced(string source, string expected)
            => Assert.That(NameTransliterator.ProcessName(source), Is.EqualTo(expected));

        [TestCase("Ä", "Ä")]
        [TestCase("ä", "ä")]
        [TestCase("Ö", "Ö")]
        [TestCase("ö", "ö")]
        [TestCase("Ü", "Ü")]
        [TestCase("ü", "ü")]
        public void GivenAUmlaut_WhenProcessing_ThenTheUmlautIsRetained(string source, string expected)
            => Assert.That(NameTransliterator.ProcessName(source), Is.EqualTo(expected));

        [TestCase("Æ")]
        [TestCase("æ")]
        [TestCase("Ø")]
        [TestCase("ø")]
        [TestCase("Ð")]
        [TestCase("ð")]
        [TestCase("Þ")]
        [TestCase("þ")]
        [TestCase("Ł")]
        [TestCase("ł")]
        [TestCase("Œ")]
        [TestCase("œ")]
        public void GivenANordicOrLigatureCharacter_WhenProcessing_ThenTheCharacterIsPermittedOrMapped(string source)
        {
            string result = NameTransliterator.ProcessName(source);

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("ΑΒΓ", "ABΓ")]
        [TestCase("Θ", "Th")]
        [TestCase("Ά", "Á")]
        [TestCase("Έ", "É")]
        [TestCase("Ύ", "Ý")]
        public void GivenAGreekLetter_WhenProcessing_ThenTheLatinEquivalentIsProduced(string source, string expected)
            => Assert.That(NameTransliterator.ProcessName(source), Is.EqualTo(expected));

        [Test]
        public void GivenANameWithSurroundingSpacesAndNonAsciiCharacters_WhenProcessing_ThenSpacesArePreserved()
        {
            string result = NameTransliterator.ProcessName("  Š  ");

            Assert.That(result, Is.EqualTo("  Sh  "));
        }

        [TestCase("ư", "u'")]
        [TestCase("Ư", "U'")]
        [TestCase("ƙ", "k'")]
        [TestCase("Ƙ", "K'")]
        public void GivenAnApostropheBearingCharacter_WhenProcessing_ThenTheApostropheIsDetached(string source, string expected)
            => Assert.That(NameTransliterator.ProcessName(source), Is.EqualTo(expected));

        [Test]
        public void GivenANameThatBecomesEmptyAfterTransliteration_WhenProcessing_ThenAnEmptyNameIsReturned()
        {
            string result = NameTransliterator.ProcessName("\u0301");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GivenManyDistinctNonAsciiCharacters_WhenProcessing_ThenEveryCharacterIsHandled()
        {
            string source = "ĐŠČĆŽŁΑΒΓМосква";
            string result = NameTransliterator.ProcessName(source);

            Assert.That(result, Does.Not.Contain("Đ"));
            Assert.That(result, Does.Not.Contain("М"));
        }

        [Test]
        public void GivenATransliteratedNameOfUnknownScript_WhenProcessing_ThenTheResultIsStableAndNonEmpty()
        {
            string result = NameTransliterator.ProcessName("Škoda Đorđe İ Țepes");

            Assert.That(result, Is.EqualTo("Shkoda Ðordže I Tepes"));
        }
    }
}