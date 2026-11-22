using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.Service.Text;

namespace StellarisNameListGenerator.UnitTests.Service
{
    /// <summary>
    /// Exhaustive coverage of every <see cref="NameTransliterator"/> replacement branch against verified
    /// CLI output. Each pair was captured by running the generator over an input containing exactly that
    /// source value and reading the resulting generated entry. Duplicate sources are intentionally omitted.
    /// </summary>
    [TestFixture]
    [TestOf(nameof(NameTransliterator))]
    public sealed class NameTransliteratorTableTests
    {
        [TestCaseSource(nameof(TransliterationCases))]
        public void GivenASourceCharacter_WhenProcessingName_ThenReturnsTheVerifiedCharacter(string source, string expected)
            => Assert.That(NameTransliterator.ProcessName(source), Is.EqualTo(expected));

        private static object[] TransliterationCases
        {
            get
            {
                var cases = new List<(string source, string expected)>
                {
                    ("Ά", "Á"), ("Ὰ", "À"), ("Ȁ", "À"), ("Ắ", "Ã"),
                    ("Ặ", "Ã"), ("Ẩ", "Â"), ("Β", "B"), ("Χ", "Ch"),
                    ("С", "C"), ("Σ", "Σ"), ("Ĉ", "C"), ("Ц", "C"),
                    ("Џ", "Dž"), ("Ɖ", "Ð"), ("Ё", "Ë"), ("Έ", "É"),
                    ("∃", "E"), ("Ḟ", "F"), ("Ƣ", "G"), ("Ȝ", "Gh"),
                    ("Ɣ", "Gh"), ("Ю", "Iu"), ("Ḥ", "H"), ("І", "I"),
                    ("Ỉ", "Ì"), ("Ї", "Ï"), ("Ǐ", "Ï"), ("Ј", "J"),
                    ("Ќ", "K"), ("Լ", "L"), ("Ṁ", "M"), ("Ṉ", "N"),
                    ("Ƞ", "N"), ("Ợ", "O"), ("Ӧ", "Ö"), ("Ớ", "Ó"),
                    ("Ỏ", "Ò"), ("Ỗ", "Ô"), ("Ǒ", "Õ"), ("Ǭ", "Ö"),
                    ("П", "P"), ("Ƿ", "Uu"), ("Ỽ", "Uu"), ("Ԛ", "Q"),
                    ("Ṟ", "R"), ("Ѕ", "S"), ("Տ", "S"), ("Ṯ", "Th"),
                    ("Θ", "Th"), ("Т", "T"), ("Ս", "U"), ("Ʊ", "U"),
                    ("Ǔ", "Ü"), ("Ǚ", "Ü"), ("Ǜ", "Ü"), ("В", "V"),
                    ("Ԝ", "W"), ("Ẇ", "W"), ("Ү", "Y"), ("Ύ", "Ý"),
                    ("Ƶ", "Z"), ("Ǯ", "Ž"), ("ә", "æ"), ("ȁ", "à"),
                    ("ȧ", "á"), ("ӑ", "ã"), ("ẵ", "ã"), ("ẩ", "â"),
                    ("ḇ", "b"), ("χ", "ch"), ("ĉ", "c"), ("џ", "dž"),
                    ("ĕ", "ê"), ("ǝ", "a"), ("ḟ", "f"), ("ƣ", "g"),
                    ("ḡ", "g"), ("ڭ", "ġ"), ("ȝ", "gh"), ("ɣ", "gh"),
                    ("ḥ", "h"), ("ю", "iu"), ("я", "ia"), ("ɨ", "i"),
                    ("ỉ", "ì"), ("ɩ", "i"), ("ǐ", "ï"), ("ḯ", "ï"),
                    ("ќ", "k"), ("ẖ", "kh"), ("ɬ", "l"), ("ƚ", "l"),
                    ("ṁ", "m"), ("ṉ", "n"), ("ƞ", "n"), ("ợ", "o"),
                    ("ӧ", "ö"), ("ớ", "ó"), ("ỏ", "ò"), ("ỗ", "ô"),
                    ("ǒ", "õ"), ("ǭ", "ö"), ("ԥ", "p"), ("ꮢ", "r"),
                    ("ṟ", "r"), ("ṯ", "th"), ("θ", "th"), ("‡", "t"),
                    ("ƿ", "uu"), ("ỽ", "uu"), ("ʊ", "u"), ("ǔ", "ü"),
                    ("ǚ", "ü"), ("ύ", "ú"), ("ǜ", "ü"), ("ẇ", "ẃ"),
                    ("γ", "y"), ("ƶ", "z"), ("ǯ", "ž"), ("ƙ", "k'"),
                    ("Ƙ", "K'"), ("ư", "u'"), ("Ư", "U'"), ("ứ", "ú'"),
                    ("Ứ", "Ú'"), ("ừ", "ù'"), ("Ừ", "Ù'"), ("ử", "ù'"),
                    ("Ử", "Ù'"), ("Ǣ", "Æ"), ("Ạ", "A"), ("Ə", "A"),
                    ("Ả", "À"), ("Ậ", "Â"), ("Ă", "Ã"), ("Ā", "Ã"),
                    ("Ǟ", "Ä"), ("Ḃ", "B"), ("Ḅ", "B"), ("Ć", "C"),
                    ("Ċ", "C"), ("Č", "Ch"), ("Ḏ", "D"), ("Ɗ", "D"),
                    ("Ḑ", "D"), ("Ď", "D"), ("Ḍ", "D"), ("Đ", "Ð"),
                    ("Ǹ", "En"), ("Ň", "Ñ"), ("Ń", "N"), ("Ņ", "N"),
                    ("Ṅ", "N"), ("Ṇ", "N"), ("Ŋ", "N"), ("Ɲ", "N"),
                    ("Ȯ", "Ó"), ("Ờ", "Ò"), ("Ỡ", "Õ"), ("Ō", "Õ"),
                    ("Ȫ", "Õ"), ("Ŏ", "Õ"), ("Ő", "Ö"), ("Ǫ", "Ö"),
                    ("Ǿ", "Ø"), ("Ộ", "Ô"), ("Ṕ", "P"), ("Ř", "Rz"),
                    ("Ŕ", "R"), ("Ṙ", "R"), ("Ṛ", "R"), ("Ŗ", "R"),
                    ("Ś", "S"), ("Ŝ", "S"), ("Ş", "S"), ("Ș", "S"),
                    ("Ṣ", "S"), ("Ṡ", "S"), ("Š", "Sh"), ("Ť", "Ty"),
                    ("Ț", "T"), ("Ţ", "T"), ("Ṭ", "T"), ("Ŧ", "T"),
                    ("Ů", "U"), ("Ų", "U"), ("Ụ", "U"), ("Ũ", "Ü"),
                    ("Ū", "Ü"), ("Ŭ", "Ü"), ("Ű", "Ü"), ("Ṳ", "Ü"),
                    ("Ủ", "Ù"), ("Ṿ", "V"), ("Ẃ", "W"), ("Ẅ", "W"),
                    ("Ŵ", "W"), ("Ẍ", "X"), ("Ŷ", "Y"), ("Ȳ", "Ÿ"),
                    ("Ỳ", "Ý"), ("Ẏ", "Ý"), ("Ź", "Z"), ("Ẓ", "Z"),
                    ("Ż", "Ž"), ("Ĥ", "H"), ("Ȟ", "H"), ("Ḧ", "H"),
                    ("Ḩ", "H"), ("Ħ", "H"), ("Ğ", "G"), ("Ĝ", "G"),
                    ("Ģ", "G"), ("Ǵ", "G"), ("ǣ", "æ"), ("ạ", "a"),
                    ("ə", "a"), ("ą", "a"), ("ẗ", "ah"), ("ả", "à"),
                    ("ậ", "â"), ("ă", "ã"), ("ā", "ã"), ("ǟ", "ä"),
                    ("ḃ", "b"), ("ḅ", "b"), ("ć", "c"), ("ċ", "c"),
                    ("č", "ch"), ("đ", "dž"), ("ḏ", "d"), ("ɗ", "d"),
                    ("ɖ", "d"), ("ḑ", "d"), ("ď", "d"), ("ḍ", "d"),
                    ("ē", "ë"), ("ẽ", "ë"), ("ė", "é"), ("ẻ", "è"),
                    ("ệ", "ê"), ("ě", "ê"), ("ę", "e"), ("ẹ", "e"),
                    ("ğ", "g"), ("ĝ", "g"), ("ģ", "g"), ("ǵ", "g"),
                    ("ĥ", "h"), ("ȟ", "h"), ("ḧ", "h"), ("ḩ", "h"),
                    ("ħ", "h"), ("ı", "i"), ("į", "i"), ("ị", "i"),
                    ("ĭ", "ï"), ("ī", "ï"), ("ĩ", "ï"), ("ĵ", "j"),
                    ("ǰ", "j"), ("ǧ", "j"), ("ḫ", "kh"), ("ḱ", "k"),
                    ("ḳ", "k"), ("ķ", "k"), ("ḵ", "k"), ("ǩ", "k"),
                    ("ĺ", "l"), ("ł", "l"), ("ľ", "l"), ("ḷ", "l"),
                    ("ļ", "l"), ("ṃ", "m"), ("ḿ", "m"), ("ň", "ñ"),
                    ("ǹ", "en"), ("ń", "n"), ("ņ", "n"), ("ṅ", "n"),
                    ("ṇ", "n"), ("ŋ", "n"), ("ɲ", "n"), ("ơ", "o"),
                    ("ọ", "o"), ("ȯ", "ó"), ("ờ", "ò"), ("ỡ", "õ"),
                    ("ō", "õ"), ("ȫ", "õ"), ("ŏ", "õ"), ("ő", "ö"),
                    ("ǫ", "ö"), ("ǿ", "ø"), ("ộ", "ô"), ("ṕ", "p"),
                    ("ř", "rz"), ("ŕ", "r"), ("ṙ", "r"), ("ṛ", "r"),
                    ("ŗ", "r"), ("ś", "s"), ("ŝ", "s"), ("ş", "s"),
                    ("ș", "s"), ("ṣ", "s"), ("ṡ", "s"), ("š", "sh"),
                    ("ť", "ty"), ("ț", "t"), ("ţ", "t"), ("ṭ", "t"),
                    ("ŧ", "t"), ("ů", "u"), ("ų", "u"), ("ụ", "u"),
                    ("ũ", "ü"), ("ū", "ü"), ("ŭ", "ü"), ("ű", "ü"),
                    ("ṳ", "ü"), ("ủ", "ù"), ("ṿ", "v"), ("ẅ", "w"),
                    ("ŵ", "w"), ("ẍ", "x"), ("ŷ", "y"), ("ȳ", "ÿ"),
                    ("ỳ", "ý"), ("ẏ", "ý"), ("ź", "z"), ("ẓ", "z"),
                    ("ʐ", "z"), ("ż", "ž"), ("ʻ", "´"),
                };
                return cases
                    .Select(pair => new object[] { pair.source, pair.expected, })
                    .ToArray();
            }
        }

        [Test]
        public void GivenAPureAsciiName_WhenProcessingName_ThenTheNameIsReturnedUnchanged()
            => Assert.That(NameTransliterator.ProcessName("Kapteyn's Star"), Is.EqualTo("Kapteyn's Star"));

        [Test]
        public void GivenAPureAsciiNameWithSpaces_WhenProcessingName_ThenTheSameInstanceIsReturned()
            => Assert.That(NameTransliterator.ProcessName("Cluj-Napoca"), Is.SameAs(NameTransliterator.ProcessName("Cluj-Napoca")));

        [TestCase("Đorđe")]
        [TestCase("İstanbul")]
        public void GivenANonAsciiName_WhenProcessingTwice_ThenResultsAreIdentical(string source)
        {
            var first = NameTransliterator.ProcessName(source);
            var second = NameTransliterator.ProcessName(source);

            Assert.That(second, Is.EqualTo(first));
        }

        [Test]
        public void GivenAnEmptyName_WhenProcessingName_ThenAnEmptyNameIsReturned()
            => Assert.That(NameTransliterator.ProcessName(string.Empty), Is.Empty);
    }
}
