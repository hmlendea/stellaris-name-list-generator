using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NuciExtensions;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service.Text;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public abstract class NamesBuilder
    {
        private static int IndentationSize => 4;
        private static int MaximumLineLength => 150;

        protected string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels)
            => BuildNameArray(nameGroups, arrayName, indentationLevels, string.Empty);

        protected static string BuildNameArray(
            IEnumerable<NameGroup> nameGroups,
            string arrayName,
            int indentationLevels,
            string sequentialName)
        {
            if (string.IsNullOrWhiteSpace(sequentialName) && nameGroups.All(x => x.Values.Count.Equals(0)))
            {
                return string.Empty;
            }

            string content = $"{GetIndentation(indentationLevels)}{arrayName} = {{{Environment.NewLine}";

            if (nameGroups.Sum(x => x.Values.Count) > 0)
            {
                int contentIndentation = indentationLevels;

                if (!string.IsNullOrWhiteSpace(sequentialName))
                {
                    content += $"{GetIndentation(contentIndentation + 1)}random_names = {{{Environment.NewLine}";
                    contentIndentation += 1;
                }

                content += BuildNameCollection(nameGroups, contentIndentation + 1);

                if (!string.IsNullOrWhiteSpace(sequentialName))
                {
                    content += $"{GetIndentation(contentIndentation)}}}{Environment.NewLine}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(sequentialName))
            {
                content += $"{GetIndentation(indentationLevels + 1)}sequential_name = \"{sequentialName}\"{Environment.NewLine}";
            }

            content += $"{GetIndentation(indentationLevels)}}}{Environment.NewLine}";

            return content;
        }

        protected static string BuildNameCollection(IEnumerable<NameGroup> nameGroups, int indentationLevels)
        {
            string indentation = GetIndentation(indentationLevels);
            HashSet<string> comparableNames = [];
            List<string> values = [];

            IEnumerable<NameGroup> groupedNameGroups = nameGroups
                .GroupBy(x => x.Name)
                .Select(group => new NameGroup
                {
                    Name = group.First().Name,
                    ExplicitValues = [.. group.SelectMany(x => x.Values)]
                });

            foreach (NameGroup nameGroup in groupedNameGroups.OrderBy(x => x.Name))
            {
                List<string> lines = [indentation];
                bool hasNames = false;

                foreach (string formattedName in GetUniqueFormattedNames(nameGroup, comparableNames))
                {
                    hasNames = true;

                    if (lines[^1].Length + 1 + formattedName.Length > MaximumLineLength)
                    {
                        lines.Add(indentation);
                    }

                    lines[^1] += $"{formattedName} ";
                }

                if (!hasNames)
                {
                    continue;
                }

                string value = string.Empty;

                if (!string.IsNullOrWhiteSpace(nameGroup.Name))
                {
                    value += $"{indentation}# >>> {nameGroup.Name}{Environment.NewLine}";
                }

                foreach (string line in lines)
                {
                    value += $"{line[..^1]}{Environment.NewLine}";
                }

                values.Add(value);
            }

            return string.Join('\n', values);
        }

        private static IEnumerable<string> GetUniqueFormattedNames(NameGroup nameGroup, HashSet<string> comparableNames)
        {
            IEnumerable<string> uniqueNames = nameGroup.Values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .Distinct();

            foreach (string uniqueName in uniqueNames)
            {
                if (!comparableNames.Add(GetComparableName(uniqueName)))
                {
                    continue;
                }

                yield return GetFormattedName(uniqueName);
            }
        }

        private static string GetFormattedName(string name)
        {
            string formattedName = NameTransliterator.ProcessName(name);

            if (formattedName.Contains(' '))
            {
                return $"\"{formattedName}\"";
            }

            return formattedName;
        }

        private static string GetComparableName(string name) => name.RemoveDiacritics();

        protected static IEnumerable<NameGroup> ExcludeSpecificNames(
            IEnumerable<NameGroup> genericNameGroups,
            params IEnumerable<NameGroup>[] specificNameGroupLists)
        {
            ConcurrentBag<NameGroup> exclusiveNameGroups = [];

            Parallel.ForEach(genericNameGroups, genericNameGroup =>
            {
                exclusiveNameGroups.Add(new NameGroup
                {
                    Name = genericNameGroup.Name,
                    ExplicitValues = [.. genericNameGroup.Values.Where(genericName => !IsUsedInAny(specificNameGroupLists, genericName))]
                });
            });

            return exclusiveNameGroups;
        }

        private static bool IsUsedInAny(IEnumerable<IEnumerable<NameGroup>> specificNameGroupLists, string name)
            => specificNameGroupLists.Any(specificGroupList => specificGroupList.Any(
                specificGroup => specificGroup.Values.Any(specificName => DoNamesMatch(specificName, name))));

        protected static NameGroup BuildUnifiedNameGroup(
            IEnumerable<NameGroup> nameGroups,
            string category,
            string groupName,
            string nameFormat)
            => new()
            {
                Name = $"{category} - {groupName}",
                ExplicitValues = [.. nameGroups
                    .SelectMany(x => x.Values)
                    .Distinct()
                    .Select(y => string.Format(nameFormat, y))]
            };

        protected static string GetIndentation(int levels)
            => string.Empty.PadRight(levels * IndentationSize, ' ');

        protected static bool DoNamesMatch(string name1, string name2)
            => name1.RemoveDiacritics().Equals(name2.RemoveDiacritics());
    }
}
