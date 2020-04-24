using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
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

            if (!string.IsNullOrWhiteSpace(sequentialName))
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

            processedName = Regex.Replace(processedName, "[ĂĀ]", "Ã");
            processedName = Regex.Replace(processedName, "[ČĆ]", "C");
            processedName = Regex.Replace(processedName, "[ƊḌ]", "D");
            processedName = Regex.Replace(processedName, "[Ē]", "Ë");
            processedName = Regex.Replace(processedName, "[Ę]", "E");
            processedName = Regex.Replace(processedName, "[Ğ]", "G");
            processedName = Regex.Replace(processedName, "[İĪ]", "I");
            processedName = Regex.Replace(processedName, "[Ƙ]", "K");
            processedName = Regex.Replace(processedName, "[Ł]", "L");
            processedName = Regex.Replace(processedName, "[Ń]", "N");
            processedName = Regex.Replace(processedName, "[Ō]", "Ö");
            processedName = Regex.Replace(processedName, "[ȘŞṢŚ]", "S");
            processedName = Regex.Replace(processedName, "[Ț]", "T");
            processedName = Regex.Replace(processedName, "[Ū]", "Ü");
            processedName = Regex.Replace(processedName, "[Ư]", "U'");
            processedName = Regex.Replace(processedName, "[ŹŻ]", "Z");
            processedName = Regex.Replace(processedName, "[ăā]", "ã");
            processedName = Regex.Replace(processedName, "[ćč]", "c");
            processedName = Regex.Replace(processedName, "[đɗḍ]", "d");
            processedName = Regex.Replace(processedName, "[ē]", "ë");
            processedName = Regex.Replace(processedName, "[ę]", "e");
            processedName = Regex.Replace(processedName, "[ğ]", "g");
            processedName = Regex.Replace(processedName, "[īı]", "i");
            processedName = Regex.Replace(processedName, "[ƙ]", "k");
            processedName = Regex.Replace(processedName, "[ł]", "l");
            processedName = Regex.Replace(processedName, "[ń]", "n");
            processedName = Regex.Replace(processedName, "[ō]", "ö");
            processedName = Regex.Replace(processedName, "[șşṣś]", "s");
            processedName = Regex.Replace(processedName, "[ț]", "t");
            processedName = Regex.Replace(processedName, "[ū]", "ü");
            processedName = Regex.Replace(processedName, "[źż]", "z");

            processedName = Regex.Replace(processedName, "[ʻ]", "'");

            return processedName;
        }
        
        protected bool DoNamesMatch(string name1, string name2)
        {
            return name1.RemoveDiacritics() == name2.RemoveDiacritics();
        }
    }
}
