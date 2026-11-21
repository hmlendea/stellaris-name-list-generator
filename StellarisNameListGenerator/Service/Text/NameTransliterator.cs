using System.Text.RegularExpressions;

namespace StellarisNameListGenerator.Service.Text
{
    /// <summary>
    /// Converts arbitrary Unicode name text into the ASCII-compatible character forms accepted by Stellaris.
    /// </summary>
    public static class NameTransliterator
    {
        /// <summary>
        /// Converts the specified name into its Stellaris-compatible representation.
        /// </summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>The converted name.</returns>
        public static string ProcessName(string name)
        {
            if (IsAscii(name))
            {
                return name;
            }

            name = ReplaceUppercaseCharactersFromAToH(name);
            name = ReplaceUppercaseCharactersFromIToR(name);
            name = ReplaceUppercaseCharactersFromSToZ(name);
            name = ReplaceLowercaseCharactersFromAToF(name);
            name = ReplaceLowercaseCharactersFromGToR(name);
            name = ReplaceLowercaseCharactersFromSToZ(name);
            name = ReplaceApostropheCharacters(name);
            name = ReplaceSecondaryAccentDiacritics(name);
            name = ReplaceSecondaryGraveDiacritics(name);
            name = ReplaceSecondaryHookDiacritics(name);
            name = ReplaceFloatingVerticalLines(name);
            name = ReplaceFloatingAccents(name);
            name = ReplaceFloatingGraveAccents(name);
            name = ReplaceFloatingUmlauts(name);
            name = ReplaceFloatingTildas(name);
            name = ReplaceFloatingCarets(name);
            name = ReplaceFloatingCommas(name);
            name = ReplaceOtherFloatingDiacritics(name);
            name = ReplaceCharsetCharacters(name);

            return name;
        }

        static bool IsAscii(string name)
        {
            foreach (char character in name)
            {
                if (character > '\u007F')
                {
                    return false;
                }
            }

            return true;
        }

        static string ReplaceUppercaseCharactersFromAToH(string name)
        {

            name = Regex.Replace(name, "([АΑᎪꓮ𝖠]|A‍)", "A");
            name = Regex.Replace(name, "([Ά])", "Á");
            name = Regex.Replace(name, "([ᾺȀ])", "À");
            name = Regex.Replace(name, "([ẮẶ])", "Ă");
            name = Regex.Replace(name, "([Ẩ])", "Â");
            name = Regex.Replace(name, "( ᐋ)", " Â");
            name = Regex.Replace(name, "([ΒᏴꓐḆ]|B‍|B‌|پ)", "B");
            name = Regex.Replace(name, "([Χ])", "Ch");
            name = Regex.Replace(name, "([СϹᏟꓚ])", "C");
            name = Regex.Replace(name, "([ĈЦ])", "C");
            name = Regex.Replace(name, "([Ꭰꓓ]|D‍)", "D");
            name = Regex.Replace(name, "([Џ])", "Dž");
            name = Regex.Replace(name, "([Ɖ])", "Đ");
            name = Regex.Replace(name, "([ЕΕᎬꓰƐЭ])", "E");
            name = Regex.Replace(name, "([Ё])", "Ë");
            name = Regex.Replace(name, "([Έ])", "É");
            name = Regex.Replace(name, "([∃])", "Ǝ");
            name = Regex.Replace(name, "([ꓝḞ])", "F");
            name = Regex.Replace(name, "([Ꮐꓖ]|G‍)", "G");
            name = Regex.Replace(name, "([Ƣ])", "Ğ"); // Untested in the games
            name = Regex.Replace(name, "([Ȝ])", "Gh"); // Or G
            name = Regex.Replace(name, "([Ɣ])", "Gh");
            name = Regex.Replace(name, "([Ю])", "Iu");
            name = Regex.Replace(name, "([ΗᎻꓧḤ])", "H");

            return name;
        }

        static string ReplaceUppercaseCharactersFromIToR(string name)
        {
            name = Regex.Replace(name, "([ІΙӀӏΊƗ])", "I");
            name = Regex.Replace(name, "([Ỉ])", "Ì");
            name = Regex.Replace(name, "([ЇΪḮ]|Ϊ́)", "Ï");
            name = Regex.Replace(name, "([Ǐ])", "Ĭ");
            name = Regex.Replace(name, "([ЈᎫꓙ])", "J");
            name = Regex.Replace(name, "([КΚᏦꓗ]|K‍)", "K");
            name = Regex.Replace(name, "([Ќ])", "Ḱ");
            name = Regex.Replace(name, "H̱", "Kh");
            name = Regex.Replace(name, "([ᏞꓡԼ]|L‌)", "L");
            name = Regex.Replace(name, "([МΜᎷꓟṀ]|M̄|M̐)", "M");
            name = Regex.Replace(name, "(Ǌ)", "NJ");
            name = Regex.Replace(name, "([НΝꓠṈ]|N‌)", "N");
            name = Regex.Replace(name, "(Ṉ)", "Ņ");
            name = Regex.Replace(name, "[Ƞ]", "Ŋ");
            name = Regex.Replace(name, "([ОΟꓳՕƆỢ])", "O");
            name = Regex.Replace(name, "([Ӧ])", "Ö");
            name = Regex.Replace(name, "([ỚΌ])", "Ó");
            name = Regex.Replace(name, "([Ỏ])", "Ò");
            name = Regex.Replace(name, "([Ỗ])", "Ô");
            name = Regex.Replace(name, "([Ǒ])", "Ŏ");
            name = Regex.Replace(name, "([Ǭ])", "Ǫ");
            name = Regex.Replace(name, "([РΡᏢꓑ]|P‍|П)", "P");
            name = Regex.Replace(name, "([ǷỼ])", "Uu"); // Or W
            name = Regex.Replace(name, "^ɸ", "P");
            name = Regex.Replace(name, " ɸ", " P");
            name = Regex.Replace(name, "([Ԛ])", "Q");
            name = Regex.Replace(name, "([ᏒꓣṞ]|R‍|R‌)", "R");
            name = Regex.Replace(name, "(R̥̄)", "Ŕu");
            name = Regex.Replace(name, "(R̥)", "Ru");

            return name;
        }

        static string ReplaceUppercaseCharactersFromSToZ(string name)
        {
            name = Regex.Replace(name, "([ЅᏚꓢՏ]|S‍|S‌)", "S");
            name = Regex.Replace(name, "([ṮΘ])", "Th");
            name = Regex.Replace(name, "([ТΤᎢꓔ])", "T");
            name = Regex.Replace(name, "Ṭ‍", "Ṭ");
            name = Regex.Replace(name, "([ՍꓴƱ])", "U");
            name = Regex.Replace(name, "([Ǔ])", "Ŭ");
            name = Regex.Replace(name, "([Ǚ])", "Ŭ"); // Or Ü
            name = Regex.Replace(name, "([Ǜ])", "Ü");
            name = Regex.Replace(name, "([ВᏙꓦ])", "V");
            name = Regex.Replace(name, "([ᎳꓪԜ])", "W");
            name = Regex.Replace(name, "([Ẇ])", "Ẃ");
            name = Regex.Replace(name, "([ХΧꓫ])", "X");
            name = Regex.Replace(name, "([ҮΥꓬ])", "Y");
            name = Regex.Replace(name, "([Ύ])", "Ý");
            name = Regex.Replace(name, "([ΖᏃꓜƵ])", "Z");
            name = Regex.Replace(name, "[Ǯ]", "Ž");

            return name;
        }

        static string ReplaceLowercaseCharactersFromAToF(string name)
        {
            name = Regex.Replace(name, "([ә])", "æ");
            name = Regex.Replace(name, "([αа𝖺]|a‍)", "a");
            name = Regex.Replace(name, "([ὰȁ])", "à");
            name = Regex.Replace(name, "([άȧ])", "á");
            name = Regex.Replace(name, "([ӑắǎẵặ])", "ă");
            name = Regex.Replace(name, "([ẩ])", "â");
            name = Regex.Replace(name, "([^ ])ᐋ", "$1â");
            name = Regex.Replace(name, "([ᏼḇ]|b‍|b‌)", "b");
            name = Regex.Replace(name, "([χ])", "ch");
            name = Regex.Replace(name, "([ĉц])", "c");
            name = Regex.Replace(name, "([ⅾ𝖽]|d‍)", "d");
            name = Regex.Replace(name, "([џ])", "dž");
            name = Regex.Replace(name, "([еεɛэ])", "e");
            name = Regex.Replace(name, "([ĕ])", "ě");
            name = Regex.Replace(name, "([ǝ])", "ə");
            name = Regex.Replace(name, "([ё])", "ë");
            name = Regex.Replace(name, "([έ])", "é");
            name = Regex.Replace(name, "([ḟ])", "f");
            name = Regex.Replace(name, "([г]|g‍|g‌)", "g");
            name = Regex.Replace(name, "([ƣ]|ḡ)", "ğ"); // Untested in the games
            name = Regex.Replace(name, "(ڭ)", "ġ");
            name = Regex.Replace(name, "([ȝ])", "gh"); // Or g
            name = Regex.Replace(name, "([ɣ])", "gh");

            return name;
        }

        static string ReplaceLowercaseCharactersFromGToR(string name)
        {
            name = Regex.Replace(name, "([ḥ])", "h");
            name = Regex.Replace(name, "([ю])", "iu");
            name = Regex.Replace(name, "([я])", "ia");
            name = Regex.Replace(name, "([іιɨ])", "i");
            name = Regex.Replace(name, "([ỉ])", "ì");
            name = Regex.Replace(name, "([ɩ])", "ı");
            name = Regex.Replace(name, "([ǐ])", "ĭ");
            name = Regex.Replace(name, "([їϊΐḯ])", "ï");
            name = Regex.Replace(name, "([ј]|j‌)", "j");
            name = Regex.Replace(name, "([кκ]|k‍)", "k");
            name = Regex.Replace(name, "([ќ])", "ḱ");
            name = Regex.Replace(name, "([ẖ])", "kh");
            name = Regex.Replace(name, "([л]|l‌)", "l");
            name = Regex.Replace(name, "([ɬƚ])", "ł");
            name = Regex.Replace(name, "([ṁ]|m̄|m̐|m̃)", "m");
            name = Regex.Replace(name, "(ǌ)", "nj");
            name = Regex.Replace(name, "([нṉ]|n‌)", "n");
            name = Regex.Replace(name, "(ṉ)", "ņ");
            name = Regex.Replace(name, "[ƞ]", "ŋ");
            name = Regex.Replace(name, "([оοօɔợ])", "o");
            name = Regex.Replace(name, "([ӧ])", "ö");
            name = Regex.Replace(name, "([όớ])", "ó");
            name = Regex.Replace(name, "([ỏ])", "ò");
            name = Regex.Replace(name, "([ỗ])", "ô");
            name = Regex.Replace(name, "([ǒ])", "ŏ");
            name = Regex.Replace(name, "([ǭ])", "ǫ");
            name = Regex.Replace(name, "([рṗɸ]|p‍|п)", "p");
            name = Regex.Replace(name, "([ԥ])", "p"); // It's actually ṗ but that doesn't work either
            name = Regex.Replace(name, "([ꮢṟ]|r‍|r‌)", "r");
            name = Regex.Replace(name, "(r̥̄)", "ŕu");
            name = Regex.Replace(name, "(r̥)", "ru");

            return name;
        }

        static string ReplaceLowercaseCharactersFromSToZ(string name)
        {
            name = Regex.Replace(name, "(s‍|s‌)", "s");
            name = Regex.Replace(name, "([ṯθ])", "th");
            name = Regex.Replace(name, "([т])", "t");
            name = Regex.Replace(name, "([‡])", "t"); // Guessed
            name = Regex.Replace(name, "ṭ‍", "ṭ");
            name = Regex.Replace(name, "([ƿỽ])", "uu"); // Or w
            name = Regex.Replace(name, "([уʊ])", "u");
            name = Regex.Replace(name, "([ǔ])", "ŭ");
            name = Regex.Replace(name, "([ǚ])", "ŭ"); // Or ü
            name = Regex.Replace(name, "([ύ])", "ú");
            name = Regex.Replace(name, "([ǜ])", "ü");
            name = Regex.Replace(name, "([ẇ])", "ẃ");
            name = Regex.Replace(name, "([γ])", "y");
            name = Regex.Replace(name, "([ƶᶻ])", "z");
            name = Regex.Replace(name, "[ǯ]", "ž");

            return name;
        }

        static string ReplaceApostropheCharacters(string name)
        {
            // Characters with apostrophe that needs to be detached
            name = name.Replace("ƙ", "k'");
            name = name.Replace("Ƙ", "K'");
            name = name.Replace("ư", "u'");
            name = name.Replace("Ư", "U'");
            name = name.Replace("ứ", "ú'");
            name = name.Replace("Ứ", "Ú'");
            name = name.Replace("ừ", "ù'");
            name = name.Replace("Ừ", "Ù'");
            name = name.Replace("ử", "ủ'");
            name = name.Replace("Ử", "Ủ'");

            return name;
        }

        static string ReplaceSecondaryAccentDiacritics(string name)
        {
            // Secondary accent diacritic
            name = name.Replace('Ấ', 'Â');
            name = name.Replace('Ḗ', 'Ē');
            name = name.Replace('Ế', 'Ê');
            name = name.Replace('Ṓ', 'Ō');
            name = name.Replace('Ố', 'Ô');
            name = name.Replace('ấ', 'â');
            name = name.Replace('ḗ', 'ē');
            name = name.Replace('ế', 'ê');
            name = name.Replace('ṓ', 'ō');
            name = name.Replace('ố', 'ô');

            return name;
        }

        static string ReplaceSecondaryGraveDiacritics(string name)
        {
            // Secondary grave accent diacritic
            name = name.Replace('Ầ', 'Â');
            name = name.Replace('Ề', 'Ê');
            name = name.Replace('Ồ', 'Ô');
            name = name.Replace('ầ', 'â');
            name = name.Replace('ề', 'ê');
            name = name.Replace('ồ', 'ô');

            return name;
        }

        static string ReplaceSecondaryHookDiacritics(string name)
        {
            // Secondary hook diacritic
            name = name.Replace('Ể', 'Ê');
            name = name.Replace('Ổ', 'Ô');
            name = name.Replace('ể', 'ê');
            name = name.Replace('ổ', 'ô');

            return name;
        }

        static string ReplaceFloatingVerticalLines(string name)
        {
            // Floating vertical lines
            name = name.Replace("a̍", "ȧ");
            name = name.Replace("e̍", "ė");
            name = name.Replace("i̍", "i");
            name = name.Replace("o̍", "ȯ");
            name = name.Replace("u̍", "ú");

            return name;
        }

        static string ReplaceFloatingAccents(string name)
        {
            // Floating accents
            name = name.Replace("á", "á");
            name = name.Replace("ć", "ć");
            name = name.Replace("é", "é");
            name = name.Replace("ǵ", "ǵ");
            name = name.Replace("í", "í");
            name = name.Replace("ḿ", "ḿ");
            name = name.Replace("ń", "ń");
            name = name.Replace("ṕ", "ṕ");
            name = name.Replace("ŕ", "ŕ");
            name = name.Replace("ś", "ś");
            name = name.Replace("ú", "ú");
            name = name.Replace("ý", "ý");
            name = name.Replace("ź", "ź");

            return name;
        }

        static string ReplaceFloatingGraveAccents(string name)
        {
            // Floating grave accents
            name = name.Replace("ì", "ì");
            name = name.Replace("ǹ", "ǹ");
            name = name.Replace("ò", "ò");
            name = name.Replace("ù", "ù");
            name = name.Replace("ỳ", "ỳ");

            return name;
        }

        static string ReplaceFloatingUmlauts(string name)
        {
            // Floating umlauts
            name = name.Replace("T̈", "T̈");
            name = name.Replace("ä", "ä");
            name = name.Replace("ā̈", "ǟ");
            name = name.Replace("ą̈", "ą̈");
            name = name.Replace("b̈", "b̈");
            name = name.Replace("c̈", "c̈");
            name = name.Replace("ë", "ë");
            name = name.Replace("ɛ̈̈", "ë");
            name = name.Replace("ḧ", "ḧ");
            name = name.Replace("ï", "ï");
            name = name.Replace("j̈", "j̈");
            name = name.Replace("k̈", "k̈");
            name = name.Replace("l̈", "l̈");
            name = name.Replace("m̈", "m̈");
            name = name.Replace("n̈", "n̈");
            name = name.Replace("ö", "ö");
            name = name.Replace("ō̈", "ȫ");
            name = name.Replace("ǫ̈", "ǫ̈");
            name = name.Replace("ɔ̈̈", "ö");
            name = name.Replace("p̈", "p̈");
            name = name.Replace("q̈", "q̈");
            name = name.Replace("q̣̈", "q̣̈");
            name = name.Replace("r̈", "r̈");
            name = name.Replace("s̈", "s̈");
            name = name.Replace("ẗ", "t"); // Because ẗ is a
            name = name.Replace("ü", "ü");
            name = name.Replace("v̈", "v̈");
            name = name.Replace("ẅ", "ẅ");
            name = name.Replace("ẍ", "ẍ");
            name = name.Replace("ÿ", "ÿ");
            name = name.Replace("z̈", "z̈");

            return name;
        }

        static string ReplaceFloatingTildas(string name)
        {
            // Floating tildas
            name = name.Replace("ã", "ã");
            name = name.Replace("ẽ", "ẽ");
            name = name.Replace("ĩ", "ĩ");
            name = name.Replace("ñ", "ñ");
            name = name.Replace("õ", "õ");
            name = name.Replace("ũ", "ũ");
            name = name.Replace("ṽ", "ṽ");
            name = name.Replace("ỹ", "ỹ");

            return name;
        }

        static string ReplaceFloatingCarets(string name)
        {
            // Floating carets
            name = name.Replace("ṳ̂", "û");

            return name;
        }

        static string ReplaceFloatingCommas(string name)
        {
            // Floating commas
            name = name.Replace("A̓", "Á"); // Or Á?

            return name;
        }

        static string ReplaceOtherFloatingDiacritics(string name)
        {
            // Other floating diacritics
            name = Regex.Replace(name, "[̧̣̤̦̓́̀̆̂̌̈̋̄̍̃͘᠌̬]", "");
            name = Regex.Replace(name, "(ॎ|઼|‌ॎ)", ""); // ???
            name = Regex.Replace(name, "[・̲̥̮̱̇̐͡]", ""); // Diacritics that attach to characters... I guess

            name = Regex.Replace(name, "[ʔ]", "ʾ");
            name = Regex.Replace(name, "[ʾʻʼʽʹ′]", "´");
            name = Regex.Replace(name, "[ʿ]", "`");
            name = Regex.Replace(name, "[ꞌʿʲь]", "'");
            name = Regex.Replace(name, "[ʺ″ⁿ]", "\"");
            name = Regex.Replace(name, "[‌‍]", "");
            name = Regex.Replace(name, "[–—]", "-");
            name = Regex.Replace(name, "[‎·]", "");
            name = Regex.Replace(name, "([‎‎])", ""); // Invisible characters

            return name;
        }

        static string ReplaceCharsetCharacters(string name)
        {
            // Charset replacements
            name = Regex.Replace(name, "[Ǣ]", "Æ");
            name = Regex.Replace(name, "[ẠƏ]", "A");
            name = Regex.Replace(name, "[Ả]", "À");
            name = Regex.Replace(name, "[Ậ]", "Â");
            name = Regex.Replace(name, "[ĂĀ]", "Ã");
            name = Regex.Replace(name, "[Ǟ]", "Ä");
            name = Regex.Replace(name, "[ḂḄ]", "B");
            name = Regex.Replace(name, "[ĆĊ]", "C");
            name = Regex.Replace(name, "[Č]", "Ch");
            name = Regex.Replace(name, "[ḎƊḐĎḌ]", "D");
            name = Regex.Replace(name, "[ĐƉ]", "Ð");
            name = Regex.Replace(name, "[ĒẸẼ]", "Ë");
            name = Regex.Replace(name, "[Ė]", "É");
            name = Regex.Replace(name, "[Ẻ]", "È");
            name = Regex.Replace(name, "[ỆĚ]", "Ê");
            name = Regex.Replace(name, "[ĘƎ]", "E");
            name = Regex.Replace(name, "([Ĕ])", "Ê");
            name = Regex.Replace(name, "[ĞĜĢǴ]", "G");
            name = Regex.Replace(name, "[Ġ]([^h])", "Gh$1");
            name = Regex.Replace(name, "[Ġ](h)", "Gh");
            name = Regex.Replace(name, "[ĤȞḦḨĦ]", "H");
            name = Regex.Replace(name, "[İĮỊ]", "I");
            name = Regex.Replace(name, "[ĬĪĨ]", "Ï");
            name = Regex.Replace(name, "[ĴǦ]", "J");
            name = Regex.Replace(name, "J̌", "J");
            name = Regex.Replace(name, "[Ḫ]", "Kh");
            name = Regex.Replace(name, "[ḰḲĶḴǨ]", "K");
            name = Regex.Replace(name, "[ĹŁĽḶĻ]", "L");
            name = Regex.Replace(name, "[ṂḾ]", "M");
            name = Regex.Replace(name, "[Ň]", "Ñ");
            name = Regex.Replace(name, "[Ǹ]", "En");
            name = Regex.Replace(name, "[ŃŅṄṆŊƝ]", "N");
            name = Regex.Replace(name, "[ƠỌ]", "O");
            name = Regex.Replace(name, "[Ȯ]", "Ó");
            name = Regex.Replace(name, "[Ờ]", "Ò");
            name = Regex.Replace(name, "[ỠŌ]", "Õ");
            name = Regex.Replace(name, "[Ȫ]", "Õ");
            name = Regex.Replace(name, "[Ŏ̤Ŏ]", "Õ"); // Maybe replace with "Eo"
            name = Regex.Replace(name, "[ŐǪ]", "Ö");
            name = Regex.Replace(name, "[Ǿ]", "Ø");
            name = Regex.Replace(name, "[Ộ]", "Ô");
            name = Regex.Replace(name, "[Ṕ]", "P");
            name = Regex.Replace(name, "[Ř]", "Rz");
            name = Regex.Replace(name, "[ŔṘṚŖ]", "R");
            name = Regex.Replace(name, "[ŚŜŞȘṢṠ]", "S");
            name = Regex.Replace(name, "[Š]", "Sh");
            name = Regex.Replace(name, "[Ť]", "Ty");
            name = Regex.Replace(name, "[ȚŢṬT̈Ŧ]", "T");
            name = Regex.Replace(name, "[ŮŲỤ]", "U");
            name = Regex.Replace(name, "[ŨŪŬŰṲ]", "Ü");
            name = Regex.Replace(name, "[Ủ]", "Ù");
            name = Regex.Replace(name, "[Ṿ]", "V");
            name = Regex.Replace(name, "[ẂẄŴ]", "W");
            name = Regex.Replace(name, "[Ẍ]", "X");
            name = Regex.Replace(name, "[Ŷ]", "Y");
            name = Regex.Replace(name, "[Ȳ]", "Ÿ");
            name = Regex.Replace(name, "[ỲẎ]", "Ý");
            name = Regex.Replace(name, "[ŹẒ]", "Z");
            name = Regex.Replace(name, "[Ż]", "Ž");
            name = Regex.Replace(name, "[ǣ]", "æ");
            name = Regex.Replace(name, "[ạəą]", "a");
            name = Regex.Replace(name, "[ẗ]", "ah");
            name = Regex.Replace(name, "[ả]", "à");
            name = Regex.Replace(name, "[ậ]", "â");
            name = Regex.Replace(name, "[ăā]", "ã");
            name = Regex.Replace(name, "[ǟ]", "ä");
            name = Regex.Replace(name, "[ḃḅ]", "b");
            name = Regex.Replace(name, "[ćċ]", "c");
            name = Regex.Replace(name, "[č]", "ch");
            name = Regex.Replace(name, "[đ]", "dž");
            name = Regex.Replace(name, "[ḏɗɖḑďḍ]", "d");
            name = Regex.Replace(name, "[ēẽ]", "ë");
            name = Regex.Replace(name, "[ė]", "é");
            name = Regex.Replace(name, "[ẻ]", "è");
            name = Regex.Replace(name, "[ệě]", "ê");
            name = Regex.Replace(name, "[ęẹ]", "e");
            name = Regex.Replace(name, "[ğĝģǵ]", "g");
            name = Regex.Replace(name, "[ġ]([^h])", "gh$1");
            name = Regex.Replace(name, "[ġ](h)", "gh");
            name = Regex.Replace(name, "[ĥȟḧḩħ]", "h");
            name = Regex.Replace(name, "[ıįị]", "i");
            name = Regex.Replace(name, "[ĭīĩ]", "ï");
            name = Regex.Replace(name, "[ĵǰǧ]", "j");
            name = Regex.Replace(name, "[ḫ]", "kh");
            name = Regex.Replace(name, "[ḱḳķḵǩ]", "k");
            name = Regex.Replace(name, "[ĺłľḷļ]", "l");
            name = Regex.Replace(name, "[ṃḿ]", "m");
            name = Regex.Replace(name, "[ň]", "ñ");
            name = Regex.Replace(name, "[ǹ]", "en");
            name = Regex.Replace(name, "[ńņṅṇŋɲ]", "n");
            name = Regex.Replace(name, "[ơọ]", "o");
            name = Regex.Replace(name, "[ȯ]", "ó");
            name = Regex.Replace(name, "[ờ]", "ò");
            name = Regex.Replace(name, "[ỡō]", "õ");
            name = Regex.Replace(name, "[ȫ]", "õ");
            name = Regex.Replace(name, "[ŏ̤ŏ]", "õ"); // Maybe replace with "eo"
            name = Regex.Replace(name, "[őǫ]", "ö");
            name = Regex.Replace(name, "[ǿ]", "ø");
            name = Regex.Replace(name, "[ộ]", "ô");
            name = Regex.Replace(name, "[ṕ]", "p");
            name = Regex.Replace(name, "[ř]", "rz");
            name = Regex.Replace(name, "[ŕṙṛŗ]", "r");
            name = Regex.Replace(name, "[śŝşșṣṡ]", "s");
            name = Regex.Replace(name, "[š]", "sh");
            name = Regex.Replace(name, "[ť]", "ty");
            name = Regex.Replace(name, "[țţṭŧ]", "t");
            name = Regex.Replace(name, "[ůųụ]", "u");
            name = Regex.Replace(name, "[ũūŭűṳ]", "ü");
            name = Regex.Replace(name, "[ủ]", "ù");
            name = Regex.Replace(name, "[ṿ]", "v");
            name = Regex.Replace(name, "[ẅŵ]", "w");
            name = Regex.Replace(name, "[ẍ]", "x");
            name = Regex.Replace(name, "[ŷ]", "y");
            name = Regex.Replace(name, "[ȳ]", "ÿ");
            name = Regex.Replace(name, "[ỳẏ]", "ý");
            name = Regex.Replace(name, "[źẓʐ]", "z");
            name = Regex.Replace(name, "[ż]", "ž");

            name = Regex.Replace(name, "[ʻ]", "'");

            return name;
        }
    }
}
