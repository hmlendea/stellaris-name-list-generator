using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public abstract class NamesBuilder
    {
        const int IndentationSize = 4;
        const int MaximumLineLength = 150;

        protected string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels)
            => BuildNameArray(nameGroups, arrayName, indentationLevels, string.Empty);

        protected string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels, string sequentialName)
        {
            string content = string.Empty;

            if (string.IsNullOrWhiteSpace(sequentialName) && nameGroups.All(x => x.Values.Count == 0))
            {
                return content;
            }

            content += $"{GetIndentation(indentationLevels)}{arrayName} = {{{Environment.NewLine}";


            if (nameGroups?.Sum(x => x.Values.Count) > 0)
            {
                if (!string.IsNullOrWhiteSpace(sequentialName))
                {
                    content += $"{GetIndentation(indentationLevels + 1)}random_names = {{{Environment.NewLine}";
                    indentationLevels += 1;
                }

                content += GetFormattedNameCollection(nameGroups, indentationLevels + 1);

                if (!string.IsNullOrWhiteSpace(sequentialName))
                {
                    content += $"{GetIndentation(indentationLevels)}}}{Environment.NewLine}";
                    indentationLevels -= 1;
                }
            }
            else if (!string.IsNullOrWhiteSpace(sequentialName))
            {
                content += $"{GetIndentation(indentationLevels + 1)}sequential_name = \"{sequentialName}\"{Environment.NewLine}";
            }

            content += $"{GetIndentation(indentationLevels)}}}{Environment.NewLine}";

            return content;
        }

        protected string GetFormattedNameCollection(IEnumerable<NameGroup> groups, int indentationLevels)
        {
            string indentation = GetIndentation(indentationLevels);
            IList<string> usedNames = new List<string>();
            IList<string> values = new List<string>();

            groups = groups
                .GroupBy(x => x.Name)
                .Select(g => new NameGroup
                {
                    Name = g.First().Name,
                    ExplicitValues = g.SelectMany(x => x.Values).ToList()
                });

            foreach (NameGroup group in groups.OrderBy(x => x.Name))
            {
                IList<string> lines = new List<string> { indentation };
                bool hasNames = false;

                IEnumerable<string> validNames = group.Values
                    .Where(x => !usedNames.Any(y => DoNamesMatch(x, y)) && !string.IsNullOrWhiteSpace(x))
                    .OrderBy(x => x)
                    .Distinct();

                foreach (string name in validNames)
                {
                    hasNames = true;
                    usedNames.Add(name);

                    string formattedName = ProcessName(name);

                    if (formattedName.Contains(" "))
                    {
                        formattedName = $"\"{formattedName}\"";
                    }

                    if (lines.Last().Length + 1 + formattedName.Length > MaximumLineLength)
                    {
                        lines.Add(indentation);
                    }

                    lines[lines.Count() - 1] += $"{formattedName} ";
                }

                if (!hasNames)
                {
                    continue;
                }

                string value = string.Empty;

                if (!string.IsNullOrWhiteSpace(group.Name))
                {
                    value += $"{indentation}# >>> {group.Name}{Environment.NewLine}";
                }

                foreach (string line in lines)
                {
                    value += $"{line.Substring(0, line.Length - 1)}{Environment.NewLine}";
                }

                values.Add(value);
            }

            return string.Join('\n', values);
        }

        protected List<NameGroup> CleanGenericNames(IEnumerable<NameGroup> genericNameGroups, params IEnumerable<NameGroup>[] specificNameGroupLists)
        {
            ConcurrentBag<NameGroup> cleanGenericNameGroups = new ConcurrentBag<NameGroup>();

            Parallel.ForEach(genericNameGroups, genericNameGroup =>
            {
                NameGroup cleanGenericNameGroup = new NameGroup();
                cleanGenericNameGroup.Name = genericNameGroup.Name;
                cleanGenericNameGroup.ExplicitValues = genericNameGroup.Values.Where(genericName =>
                    specificNameGroupLists.All(specificNameGroups => specificNameGroups.All(specificNameGroup =>
                        specificNameGroup.Values.All(specificName => !DoNamesMatch(specificName, genericName))))).ToList();

                cleanGenericNameGroups.Add(cleanGenericNameGroup);
            });

            return cleanGenericNameGroups.ToList();
        }

        protected NameGroup GenerateUnifiedNameGroup(IEnumerable<NameGroup> nameGroups, string category, string groupName, string nameFormat)
        {
            NameGroup group = new NameGroup();
            group.Name = $"{category} - {groupName}";
            group.ExplicitValues = nameGroups
                .SelectMany(x => x.Values)
                .Distinct()
                .Select(y => string.Format(nameFormat, y))
                .ToList();

            return group;
        }

        protected string GetIndentation(int levels)
        {
            return string.Empty.PadRight(levels * IndentationSize, ' ');
        }

        protected string ProcessName(string name)
        {
            string processedName = name;

            processedName = Regex.Replace(processedName, "([АΑᎪꓮ𝖠]|A‍)", "A");
            processedName = Regex.Replace(processedName, "([Ά])", "Á");
            processedName = Regex.Replace(processedName, "([ᾺȀ])", "À");
            processedName = Regex.Replace(processedName, "([ẮẶ])", "Ă");
            processedName = Regex.Replace(processedName, "([Ẩ])", "Â");
            processedName = Regex.Replace(processedName, "( ᐋ)", " Â");
            processedName = Regex.Replace(processedName, "([ΒᏴꓐḆ]|B‍|B‌|پ)", "B");
            processedName = Regex.Replace(processedName, "([Χ])", "Ch");
            processedName = Regex.Replace(processedName, "([СϹᏟꓚ])", "C");
            processedName = Regex.Replace(processedName, "([ĈЦ])", "C");
            processedName = Regex.Replace(processedName, "([Ꭰꓓ]|D‍)", "D");
            processedName = Regex.Replace(processedName, "([Џ])", "Dž");
            processedName = Regex.Replace(processedName, "([Ɖ])", "Đ");
            processedName = Regex.Replace(processedName, "([ЕΕᎬꓰƐЭ])", "E");
            processedName = Regex.Replace(processedName, "([Ё])", "Ë");
            processedName = Regex.Replace(processedName, "([Έ])", "É");
            processedName = Regex.Replace(processedName, "([∃])", "Ǝ");
            processedName = Regex.Replace(processedName, "([ꓝḞ])", "F");
            processedName = Regex.Replace(processedName, "([Ꮐꓖ]|G‍)", "G");
            processedName = Regex.Replace(processedName, "([Ƣ])", "Ğ"); // Untested in the games
            processedName = Regex.Replace(processedName, "([Ȝ])", "Gh"); // Or G
            processedName = Regex.Replace(processedName, "([Ɣ])", "Gh");
            processedName = Regex.Replace(processedName, "([Ю])", "Iu");
            processedName = Regex.Replace(processedName, "([ΗᎻꓧḤ])", "H");
            processedName = Regex.Replace(processedName, "([ІΙӀӏΊƗ])", "I");
            processedName = Regex.Replace(processedName, "([Ỉ])", "Ì");
            processedName = Regex.Replace(processedName, "([ЇΪḮ]|Ϊ́)", "Ï");
            processedName = Regex.Replace(processedName, "([Ǐ])", "Ĭ");
            processedName = Regex.Replace(processedName, "([ЈᎫꓙ])", "J");
            processedName = Regex.Replace(processedName, "([КΚᏦꓗ]|K‍)", "K");
            processedName = Regex.Replace(processedName, "([Ќ])", "Ḱ");
            processedName = Regex.Replace(processedName, "H̱", "Kh");
            processedName = Regex.Replace(processedName, "([ᏞꓡԼ]|L‌)", "L");
            processedName = Regex.Replace(processedName, "([МΜᎷꓟṀ]|M̄|M̐)", "M");
            processedName = Regex.Replace(processedName, "(Ǌ)", "NJ");
            processedName = Regex.Replace(processedName, "([НΝꓠṈ]|N‌)", "N");
            processedName = Regex.Replace(processedName, "(Ṉ)", "Ņ");
            processedName = Regex.Replace(processedName, "[Ƞ]", "Ŋ");
            processedName = Regex.Replace(processedName, "([ОΟꓳՕƆỢ])", "O");
            processedName = Regex.Replace(processedName, "([Ӧ])", "Ö");
            processedName = Regex.Replace(processedName, "([ỚΌ])", "Ó");
            processedName = Regex.Replace(processedName, "([Ỏ])", "Ò");
            processedName = Regex.Replace(processedName, "([Ỗ])", "Ô");
            processedName = Regex.Replace(processedName, "([Ǒ])", "Ŏ");
            processedName = Regex.Replace(processedName, "([Ǭ])", "Ǫ");
            processedName = Regex.Replace(processedName, "([РΡᏢꓑ]|P‍|П)", "P");
            processedName = Regex.Replace(processedName, "([ǷỼ])", "Uu"); // Or W
            processedName = Regex.Replace(processedName, "^ɸ", "P");
            processedName = Regex.Replace(processedName, " ɸ", " P");
            processedName = Regex.Replace(processedName, "([Ԛ])", "Q");
            processedName = Regex.Replace(processedName, "([ᏒꓣṞ]|R‍|R‌)", "R");
            processedName = Regex.Replace(processedName, "(R̥̄)", "Ŕu");
            processedName = Regex.Replace(processedName, "(R̥)", "Ru");
            processedName = Regex.Replace(processedName, "([ЅᏚꓢՏ]|S‍|S‌)", "S");
            processedName = Regex.Replace(processedName, "([ṮΘ])", "Th");
            processedName = Regex.Replace(processedName, "([ТΤᎢꓔ])", "T");
            processedName = Regex.Replace(processedName, "Ṭ‍", "Ṭ");
            processedName = Regex.Replace(processedName, "([ՍꓴƱ])", "U");
            processedName = Regex.Replace(processedName, "([Ǔ])", "Ŭ");
            processedName = Regex.Replace(processedName, "([Ǚ])", "Ŭ"); // Or Ü
            processedName = Regex.Replace(processedName, "([Ǜ])", "Ü");
            processedName = Regex.Replace(processedName, "([ВᏙꓦ])", "V");
            processedName = Regex.Replace(processedName, "([ᎳꓪԜ])", "W");
            processedName = Regex.Replace(processedName, "([Ẇ])", "Ẃ");
            processedName = Regex.Replace(processedName, "([ХΧꓫ])", "X");
            processedName = Regex.Replace(processedName, "([ҮΥꓬ])", "Y");
            processedName = Regex.Replace(processedName, "([Ύ])", "Ý");
            processedName = Regex.Replace(processedName, "([ΖᏃꓜƵ])", "Z");
            processedName = Regex.Replace(processedName, "[Ǯ]", "Ž");
            processedName = Regex.Replace(processedName, "([ә])", "æ");
            processedName = Regex.Replace(processedName, "([αа𝖺]|a‍)", "a");
            processedName = Regex.Replace(processedName, "([ὰȁ])", "à");
            processedName = Regex.Replace(processedName, "([άȧ])", "á");
            processedName = Regex.Replace(processedName, "([ӑắǎẵặ])", "ă");
            processedName = Regex.Replace(processedName, "([ẩ])", "â");
            processedName = Regex.Replace(processedName, "([^ ])ᐋ", "$1â");
            processedName = Regex.Replace(processedName, "([ᏼḇ]|b‍|b‌)", "b");
            processedName = Regex.Replace(processedName, "([χ])", "ch");
            processedName = Regex.Replace(processedName, "([ĉц])", "c");
            processedName = Regex.Replace(processedName, "([ⅾ𝖽]|d‍)", "d");
            processedName = Regex.Replace(processedName, "([џ])", "dž");
            processedName = Regex.Replace(processedName, "([еεɛэ])", "e");
            processedName = Regex.Replace(processedName, "([ĕ])", "ě");
            processedName = Regex.Replace(processedName, "([ǝ])", "ə");
            processedName = Regex.Replace(processedName, "([ё])", "ë");
            processedName = Regex.Replace(processedName, "([έ])", "é");
            processedName = Regex.Replace(processedName, "([ḟ])", "f");
            processedName = Regex.Replace(processedName, "([г]|g‍|g‌)", "g");
            processedName = Regex.Replace(processedName, "([ƣ]|ḡ)", "ğ"); // Untested in the games
            processedName = Regex.Replace(processedName, "(ڭ)", "ġ");
            processedName = Regex.Replace(processedName, "([ȝ])", "gh"); // Or g
            processedName = Regex.Replace(processedName, "([ɣ])", "gh");
            processedName = Regex.Replace(processedName, "([ḥ])", "h");
            processedName = Regex.Replace(processedName, "([ю])", "iu");
            processedName = Regex.Replace(processedName, "([я])", "ia");
            processedName = Regex.Replace(processedName, "([іιɨ])", "i");
            processedName = Regex.Replace(processedName, "([ỉ])", "ì");
            processedName = Regex.Replace(processedName, "([ɩ])", "ı");
            processedName = Regex.Replace(processedName, "([ǐ])", "ĭ");
            processedName = Regex.Replace(processedName, "([їϊΐḯ])", "ï");
            processedName = Regex.Replace(processedName, "([ј]|j‌)", "j");
            processedName = Regex.Replace(processedName, "([кκ]|k‍)", "k");
            processedName = Regex.Replace(processedName, "([ќ])", "ḱ");
            processedName = Regex.Replace(processedName, "([ẖ])", "kh");
            processedName = Regex.Replace(processedName, "([л]|l‌)", "l");
            processedName = Regex.Replace(processedName, "([ɬƚ])", "ł");
            processedName = Regex.Replace(processedName, "([ṁ]|m̄|m̐|m̃)", "m");
            processedName = Regex.Replace(processedName, "(ǌ)", "nj");
            processedName = Regex.Replace(processedName, "([нṉ]|n‌)", "n");
            processedName = Regex.Replace(processedName, "(ṉ)", "ņ");
            processedName = Regex.Replace(processedName, "[ƞ]", "ŋ");
            processedName = Regex.Replace(processedName, "([оοօɔợ])", "o");
            processedName = Regex.Replace(processedName, "([ӧ])", "ö");
            processedName = Regex.Replace(processedName, "([όớ])", "ó");
            processedName = Regex.Replace(processedName, "([ỏ])", "ò");
            processedName = Regex.Replace(processedName, "([ỗ])", "ô");
            processedName = Regex.Replace(processedName, "([ǒ])", "ŏ");
            processedName = Regex.Replace(processedName, "([ǭ])", "ǫ");
            processedName = Regex.Replace(processedName, "([рṗɸ]|p‍|п)", "p");
            processedName = Regex.Replace(processedName, "([ԥ])", "p"); // It's actually ṗ but that doesn't work either
            processedName = Regex.Replace(processedName, "([ꮢṟ]|r‍|r‌)", "r");
            processedName = Regex.Replace(processedName, "(r̥̄)", "ŕu");
            processedName = Regex.Replace(processedName, "(r̥)", "ru");
            processedName = Regex.Replace(processedName, "(s‍|s‌)", "s");
            processedName = Regex.Replace(processedName, "([ṯθ])", "th");
            processedName = Regex.Replace(processedName, "([т])", "t");
            processedName = Regex.Replace(processedName, "([‡])", "t"); // Guessed
            processedName = Regex.Replace(processedName, "ṭ‍", "ṭ");
            processedName = Regex.Replace(processedName, "([ƿỽ])", "uu"); // Or w
            processedName = Regex.Replace(processedName, "([уʊ])", "u");
            processedName = Regex.Replace(processedName, "([ǔ])", "ŭ");
            processedName = Regex.Replace(processedName, "([ǚ])", "ŭ"); // Or ü
            processedName = Regex.Replace(processedName, "([ύ])", "ú");
            processedName = Regex.Replace(processedName, "([ǜ])", "ü");
            processedName = Regex.Replace(processedName, "([ẇ])", "ẃ");
            processedName = Regex.Replace(processedName, "([γ])", "y");
            processedName = Regex.Replace(processedName, "([ƶᶻ])", "z");
            processedName = Regex.Replace(processedName, "[ǯ]", "ž");

            // Characters with apostrophe that needs to be detached
            processedName = processedName.Replace("ƙ", "k'");
            processedName = processedName.Replace("Ƙ", "K'");
            processedName = processedName.Replace("ư", "u'");
            processedName = processedName.Replace("Ư", "U'");
            processedName = processedName.Replace("ứ", "ú'");
            processedName = processedName.Replace("Ứ", "Ú'");
            processedName = processedName.Replace("ừ", "ù'");
            processedName = processedName.Replace("Ừ", "Ù'");
            processedName = processedName.Replace("ử", "ủ'");
            processedName = processedName.Replace("Ử", "Ủ'");

            // Secondary accent diacritic
            processedName = processedName.Replace('Ấ', 'Â');
            processedName = processedName.Replace('Ḗ', 'Ē');
            processedName = processedName.Replace('Ế', 'Ê');
            processedName = processedName.Replace('Ṓ', 'Ō');
            processedName = processedName.Replace('Ố', 'Ô');
            processedName = processedName.Replace('ấ', 'â');
            processedName = processedName.Replace('ḗ', 'ē');
            processedName = processedName.Replace('ế', 'ê');
            processedName = processedName.Replace('ṓ', 'ō');
            processedName = processedName.Replace('ố', 'ô');

            // Secondary grave accent diacritic
            processedName = processedName.Replace('Ầ', 'Â');
            processedName = processedName.Replace('Ề', 'Ê');
            processedName = processedName.Replace('Ồ', 'Ô');
            processedName = processedName.Replace('ầ', 'â');
            processedName = processedName.Replace('ề', 'ê');
            processedName = processedName.Replace('ồ', 'ô');

            // Secondary hook diacritic
            processedName = processedName.Replace('Ể', 'Ê');
            processedName = processedName.Replace('Ổ', 'Ô');
            processedName = processedName.Replace('ể', 'ê');
            processedName = processedName.Replace('ổ', 'ô');

            // Floating vertical lines
            processedName = processedName.Replace("a̍", "ȧ");
            processedName = processedName.Replace("e̍", "ė");
            processedName = processedName.Replace("i̍", "i");
            processedName = processedName.Replace("o̍", "ȯ");
            processedName = processedName.Replace("u̍", "ú");

            // Floating accents
            processedName = processedName.Replace("á", "á");
            processedName = processedName.Replace("ć", "ć");
            processedName = processedName.Replace("é", "é");
            processedName = processedName.Replace("ǵ", "ǵ");
            processedName = processedName.Replace("í", "í");
            processedName = processedName.Replace("ḿ", "ḿ");
            processedName = processedName.Replace("ń", "ń");
            processedName = processedName.Replace("ṕ", "ṕ");
            processedName = processedName.Replace("ŕ", "ŕ");
            processedName = processedName.Replace("ś", "ś");
            processedName = processedName.Replace("ú", "ú");
            processedName = processedName.Replace("ý", "ý");
            processedName = processedName.Replace("ź", "ź");

            // Floating grave accents
            processedName = processedName.Replace("ì", "ì");
            processedName = processedName.Replace("ǹ", "ǹ");
            processedName = processedName.Replace("ò", "ò");
            processedName = processedName.Replace("ù", "ù");
            processedName = processedName.Replace("ỳ", "ỳ");

            // Floating umlauts
            processedName = processedName.Replace("T̈", "T̈");
            processedName = processedName.Replace("ä", "ä");
            processedName = processedName.Replace("ā̈", "ǟ");
            processedName = processedName.Replace("ą̈", "ą̈");
            processedName = processedName.Replace("b̈", "b̈");
            processedName = processedName.Replace("c̈", "c̈");
            processedName = processedName.Replace("ë", "ë");
            processedName = processedName.Replace("ɛ̈̈", "ë");
            processedName = processedName.Replace("ḧ", "ḧ");
            processedName = processedName.Replace("ï", "ï");
            processedName = processedName.Replace("j̈", "j̈");
            processedName = processedName.Replace("k̈", "k̈");
            processedName = processedName.Replace("l̈", "l̈");
            processedName = processedName.Replace("m̈", "m̈");
            processedName = processedName.Replace("n̈", "n̈");
            processedName = processedName.Replace("ö", "ö");
            processedName = processedName.Replace("ō̈", "ȫ");
            processedName = processedName.Replace("ǫ̈", "ǫ̈");
            processedName = processedName.Replace("ɔ̈̈", "ö");
            processedName = processedName.Replace("p̈", "p̈");
            processedName = processedName.Replace("q̈", "q̈");
            processedName = processedName.Replace("q̣̈", "q̣̈");
            processedName = processedName.Replace("r̈", "r̈");
            processedName = processedName.Replace("s̈", "s̈");
            processedName = processedName.Replace("ẗ", "t"); // Because ẗ is a
            processedName = processedName.Replace("ü", "ü");
            processedName = processedName.Replace("v̈", "v̈");
            processedName = processedName.Replace("ẅ", "ẅ");
            processedName = processedName.Replace("ẍ", "ẍ");
            processedName = processedName.Replace("ÿ", "ÿ");
            processedName = processedName.Replace("z̈", "z̈");

            // Floating tildas
            processedName = processedName.Replace("ã", "ã");
            processedName = processedName.Replace("ẽ", "ẽ");
            processedName = processedName.Replace("ĩ", "ĩ");
            processedName = processedName.Replace("ñ", "ñ");
            processedName = processedName.Replace("õ", "õ");
            processedName = processedName.Replace("ũ", "ũ");
            processedName = processedName.Replace("ṽ", "ṽ");
            processedName = processedName.Replace("ỹ", "ỹ");

            // Floating carets
            processedName = processedName.Replace("ṳ̂", "û");

            // Floating commas
            processedName = processedName.Replace("A̓", "Á"); // Or Á?

            // Other floating diacritics
            processedName = Regex.Replace(processedName, "[̧̣̤̦̓́̀̆̂̌̈̋̄̍̃͘᠌̬]", "");
            processedName = Regex.Replace(processedName, "(ॎ|઼|‌ॎ)", ""); // ???
            processedName = Regex.Replace(processedName, "[・̲̥̮̱̇̐͡]", ""); // Diacritics that attach to characters... I guess

            processedName = Regex.Replace(processedName, "[ʔ]", "ʾ");
            processedName = Regex.Replace(processedName, "[ʾʻʼʽʹ′]", "´");
            processedName = Regex.Replace(processedName, "[ʿ]", "`");
            processedName = Regex.Replace(processedName, "[ꞌʿʲь]", "'");
            processedName = Regex.Replace(processedName, "[ʺ″ⁿ]", "\"");
            processedName = Regex.Replace(processedName, "[‌‍]", "");
            processedName = Regex.Replace(processedName, "[–—]", "-");
            processedName = Regex.Replace(processedName, "[‎·]", "");
            processedName = Regex.Replace(processedName, "([‎‎])", ""); // Invisible characters

            // Charset replacements
            processedName = Regex.Replace(processedName, "[Ǣ]", "Æ");
            processedName = Regex.Replace(processedName, "[ẠƏ]", "A");
            processedName = Regex.Replace(processedName, "[Ả]", "À");
            processedName = Regex.Replace(processedName, "[Ậ]", "Â");
            processedName = Regex.Replace(processedName, "[ĂĀ]", "Ã");
            processedName = Regex.Replace(processedName, "[Ǟ]", "Ä");
            processedName = Regex.Replace(processedName, "[ḂḄ]", "B");
            processedName = Regex.Replace(processedName, "[ĆĊ]", "C");
            processedName = Regex.Replace(processedName, "[Č]", "Ch");
            processedName = Regex.Replace(processedName, "[ḎƊḐĎḌ]", "D");
            processedName = Regex.Replace(processedName, "[ĐƉ]", "Ð");
            processedName = Regex.Replace(processedName, "[ĒẸẼ]", "Ë");
            processedName = Regex.Replace(processedName, "[Ė]", "É");
            processedName = Regex.Replace(processedName, "[Ẻ]", "È");
            processedName = Regex.Replace(processedName, "[ỆĚ]", "Ê");
            processedName = Regex.Replace(processedName, "[ĘƎ]", "E");
            processedName = Regex.Replace(processedName, "([Ĕ])", "Ê");
            processedName = Regex.Replace(processedName, "[ĞĜĢǴ]", "G");
            processedName = Regex.Replace(processedName, "[Ġ]([^h])", "Gh$1");
            processedName = Regex.Replace(processedName, "[Ġ](h)", "Gh");
            processedName = Regex.Replace(processedName, "[ĤȞḦḨĦ]", "H");
            processedName = Regex.Replace(processedName, "[İĮỊ]", "I");
            processedName = Regex.Replace(processedName, "[ĬĪĨ]", "Ï");
            processedName = Regex.Replace(processedName, "[ĴǦ]", "J");
            processedName = Regex.Replace(processedName, "J̌", "J");
            processedName = Regex.Replace(processedName, "[Ḫ]", "Kh");
            processedName = Regex.Replace(processedName, "[ḰḲĶḴǨ]", "K");
            processedName = Regex.Replace(processedName, "[ĹŁĽḶĻ]", "L");
            processedName = Regex.Replace(processedName, "[ṂḾ]", "M");
            processedName = Regex.Replace(processedName, "[Ň]", "Ñ");
            processedName = Regex.Replace(processedName, "[Ǹ]", "En");
            processedName = Regex.Replace(processedName, "[ŃŅṄṆŊƝ]", "N");
            processedName = Regex.Replace(processedName, "[ƠỌ]", "O");
            processedName = Regex.Replace(processedName, "[Ȯ]", "Ó");
            processedName = Regex.Replace(processedName, "[Ờ]", "Ò");
            processedName = Regex.Replace(processedName, "[ỠŌ]", "Õ");
            processedName = Regex.Replace(processedName, "[Ȫ]", "Õ");
            processedName = Regex.Replace(processedName, "[Ŏ̤Ŏ]", "Õ"); // Maybe replace with "Eo"
            processedName = Regex.Replace(processedName, "[ŐǪ]", "Ö");
            processedName = Regex.Replace(processedName, "[Ǿ]", "Ø");
            processedName = Regex.Replace(processedName, "[Ộ]", "Ô");
            processedName = Regex.Replace(processedName, "[Ṕ]", "P");
            processedName = Regex.Replace(processedName, "[Ř]", "Rz");
            processedName = Regex.Replace(processedName, "[ŔṘṚŖ]", "R");
            processedName = Regex.Replace(processedName, "[ŚŜŞȘṢṠ]", "S");
            processedName = Regex.Replace(processedName, "[Ť]", "Ty");
            processedName = Regex.Replace(processedName, "[ȚŢṬT̈Ŧ]", "T");
            processedName = Regex.Replace(processedName, "[ŮŲỤ]", "U");
            processedName = Regex.Replace(processedName, "[ŨŪŬŰṲ]", "Ü");
            processedName = Regex.Replace(processedName, "[Ủ]", "Ù");
            processedName = Regex.Replace(processedName, "[Ṿ]", "V");
            processedName = Regex.Replace(processedName, "[ẂẄŴ]", "W");
            processedName = Regex.Replace(processedName, "[Ẍ]", "X");
            processedName = Regex.Replace(processedName, "[Ŷ]", "Y");
            processedName = Regex.Replace(processedName, "[Ȳ]", "Ÿ");
            processedName = Regex.Replace(processedName, "[ỲẎ]", "Ý");
            processedName = Regex.Replace(processedName, "[ŹẒ]", "Z");
            processedName = Regex.Replace(processedName, "[Ż]", "Ž");
            processedName = Regex.Replace(processedName, "[ǣ]", "æ");
            processedName = Regex.Replace(processedName, "[ạəą]", "a");
            processedName = Regex.Replace(processedName, "[ẗ]", "ah");
            processedName = Regex.Replace(processedName, "[ả]", "à");
            processedName = Regex.Replace(processedName, "[ậ]", "â");
            processedName = Regex.Replace(processedName, "[ăā]", "ã");
            processedName = Regex.Replace(processedName, "[ǟ]", "ä");
            processedName = Regex.Replace(processedName, "[ḃḅ]", "b");
            processedName = Regex.Replace(processedName, "[ćċ]", "c");
            processedName = Regex.Replace(processedName, "[č]", "ch");
            processedName = Regex.Replace(processedName, "[đ]", "dž");
            processedName = Regex.Replace(processedName, "[ḏɗɖḑďḍ]", "d");
            processedName = Regex.Replace(processedName, "[ēẽ]", "ë");
            processedName = Regex.Replace(processedName, "[ė]", "é");
            processedName = Regex.Replace(processedName, "[ẻ]", "è");
            processedName = Regex.Replace(processedName, "[ệě]", "ê");
            processedName = Regex.Replace(processedName, "[ęẹ]", "e");
            processedName = Regex.Replace(processedName, "[ğĝģǵ]", "g");
            processedName = Regex.Replace(processedName, "[ġ]([^h])", "gh$1");
            processedName = Regex.Replace(processedName, "[ġ](h)", "gh");
            processedName = Regex.Replace(processedName, "[ĥȟḧḩħ]", "h");
            processedName = Regex.Replace(processedName, "[ıįị]", "i");
            processedName = Regex.Replace(processedName, "[ĭīĩ]", "ï");
            processedName = Regex.Replace(processedName, "[ĵǰǧ]", "j");
            processedName = Regex.Replace(processedName, "[ḫ]", "kh");
            processedName = Regex.Replace(processedName, "[ḱḳķḵǩ]", "k");
            processedName = Regex.Replace(processedName, "[ĺłľḷļ]", "l");
            processedName = Regex.Replace(processedName, "[ṃḿ]", "m");
            processedName = Regex.Replace(processedName, "[ň]", "ñ");
            processedName = Regex.Replace(processedName, "[ǹ]", "en");
            processedName = Regex.Replace(processedName, "[ńņṅṇŋɲ]", "n");
            processedName = Regex.Replace(processedName, "[ơọ]", "o");
            processedName = Regex.Replace(processedName, "[ȯ]", "ó");
            processedName = Regex.Replace(processedName, "[ờ]", "ò");
            processedName = Regex.Replace(processedName, "[ỡō]", "õ");
            processedName = Regex.Replace(processedName, "[ȫ]", "õ");
            processedName = Regex.Replace(processedName, "[ŏ̤ŏ]", "õ"); // Maybe replace with "eo"
            processedName = Regex.Replace(processedName, "[őǫ]", "ö");
            processedName = Regex.Replace(processedName, "[ǿ]", "ø");
            processedName = Regex.Replace(processedName, "[ộ]", "ô");
            processedName = Regex.Replace(processedName, "[ṕ]", "p");
            processedName = Regex.Replace(processedName, "[ř]", "rz");
            processedName = Regex.Replace(processedName, "[ŕṙṛŗ]", "r");
            processedName = Regex.Replace(processedName, "[śŝşșṣṡ]", "s");
            processedName = Regex.Replace(processedName, "[ť]", "ty");
            processedName = Regex.Replace(processedName, "[țţṭŧ]", "t");
            processedName = Regex.Replace(processedName, "[ůųụ]", "u");
            processedName = Regex.Replace(processedName, "[ũūŭűṳ]", "ü");
            processedName = Regex.Replace(processedName, "[ủ]", "ù");
            processedName = Regex.Replace(processedName, "[ṿ]", "v");
            processedName = Regex.Replace(processedName, "[ẅŵ]", "w");
            processedName = Regex.Replace(processedName, "[ẍ]", "x");
            processedName = Regex.Replace(processedName, "[ŷ]", "y");
            processedName = Regex.Replace(processedName, "[ȳ]", "ÿ");
            processedName = Regex.Replace(processedName, "[ỳẏ]", "ý");
            processedName = Regex.Replace(processedName, "[źẓʐ]", "z");
            processedName = Regex.Replace(processedName, "[ż]", "ž");

            processedName = Regex.Replace(processedName, "[ʻ]", "'");

            return processedName;
        }

        protected bool DoNamesMatch(string name1, string name2)
        {
            return name1.RemoveDiacritics() == name2.RemoveDiacritics();
        }
    }
}
