using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StellarisNameListGenerator.IntegrationTests.Tools
{
    /// <summary>
    /// Provides structured, locale-independent access to a generated Stellaris name-list file.
    /// </summary>
    internal sealed class GeneratedNameList
    {
        private static readonly Regex TopLevelBlockPattern = new(@"^ {4}([a-z_]+) = \{$", RegexOptions.Multiline);
        private static readonly Regex AssignmentPattern = new(@"^(?<indent> *)(?<identifier>\S+) = \{$", RegexOptions.Multiline);

        private readonly IReadOnlyList<string> lines;

        private GeneratedNameList(string content, IReadOnlyList<string> lines)
        {
            Content = content;
            this.lines = lines;
        }

        internal static string HeaderPrefix => "### ";

        internal string Content { get; }

        internal IReadOnlyList<string> Lines => lines;

        internal IReadOnlyList<string> HeaderLines => [.. lines.Where(line => line.StartsWith(HeaderPrefix, StringComparison.Ordinal))];

        internal IReadOnlyList<string> TopLevelBlocks => [.. TopLevelBlockPattern
            .Matches(Content)
            .Cast<Match>()
            .Select(match => match.Groups[1].Value)];

        internal string AssignmentLine => AssignmentPattern.Match(Content).Value;

        internal bool IsBalanced => CountCharacter(Content, '{') == CountCharacter(Content, '}');

        private static int CountCharacter(string content, char character)
            => content.Count(current => current == character);

        internal static GeneratedNameList Parse(string content)
        {
            string normalisedContent = content.Replace("\r\n", "\n").Replace('\r', '\n');

            return new GeneratedNameList(normalisedContent, [.. normalisedContent.Split('\n')]);
        }

        internal static GeneratedNameList ReadFromFile(string filePath)
            => Parse(File.ReadAllText(filePath));

        internal bool Contains(string value)
            => Content.Contains(value, StringComparison.Ordinal);

        internal bool BlockContains(string blockName, string value)
            => GetBlockLines(blockName).Any(line => line.Contains(value, StringComparison.Ordinal));

        internal bool SubBlockContains(string blockName, string subBlockName, string value)
            => GetBlockLines(blockName, subBlockName).Any(line => line.Contains(value, StringComparison.Ordinal));

        internal IReadOnlyList<string> GetBlockLines(string blockName)
            => ExtractBlock(lines, blockName);

        internal IReadOnlyList<string> GetBlockLines(string blockName, string subBlockName)
            => ExtractBlock(ExtractBlock(lines, blockName), subBlockName);

        private static IReadOnlyList<string> ExtractBlock(IReadOnlyList<string> sourceLines, string blockName)
        {
            string openingSuffix = $"{blockName} = {{";
            List<string> blockLines = [];
            string closingLine = string.Empty;
            bool isInsideBlock = false;

            foreach (string line in sourceLines)
            {
                if (!isInsideBlock)
                {
                    if (!line.EndsWith(openingSuffix, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    closingLine = string.Empty.PadRight(line.Length - line.TrimStart().Length, ' ') + "}";
                    isInsideBlock = true;
                    continue;
                }

                if (string.Equals(line, closingLine, StringComparison.Ordinal))
                {
                    break;
                }

                blockLines.Add(line);
            }

            return blockLines;
        }
    }
}
