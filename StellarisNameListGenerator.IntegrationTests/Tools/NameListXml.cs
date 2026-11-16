using System.Collections.Generic;
using System.Text;

namespace StellarisNameListGenerator.IntegrationTests.Tools
{
    /// <summary>
    /// Builds <c>ArrayOfNameList</c> documents in the exact shape consumed by the generator.
    /// </summary>
    internal static class NameListXml
    {
        internal static string Document(params string[] records)
        {
            StringBuilder document = new();
            document.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            document.AppendLine("<ArrayOfNameList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            foreach (string record in records)
            {
                document.AppendLine(record);
            }

            document.AppendLine("</ArrayOfNameList>");

            return document.ToString();
        }

        internal static string Record(params string[] sections)
        {
            StringBuilder record = new();
            record.AppendLine("<NameList>");

            foreach (string section in sections)
            {
                record.AppendLine(section);
            }

            record.AppendLine("</NameList>");

            return record.ToString();
        }

        internal static string Section(string name, string inner)
            => $"<{name}>{inner}</{name}>";

        internal static string Group(string name, params string[] values)
            => $"<NameGroup><Name>{Escape(name)}</Name>{BuildValues(values)}</NameGroup>";

        internal static string GroupWithUrl(string name, string url, params string[] values)
            => $"<NameGroup><Name>{Escape(name)}</Name><Url>{Escape(url)}</Url>{BuildValues(values)}</NameGroup>";

        internal static string Character(string id, int weight, params string[] groups)
            => BuildCharacter($"<Id>{Escape(id)}</Id>", weight, groups);

        internal static string CharacterWithoutId(int weight, params string[] groups)
            => BuildCharacter(string.Empty, weight, groups);

        private static string BuildValues(IEnumerable<string> values)
        {
            StringBuilder content = new();
            content.Append("<Values>");

            foreach (string value in values)
            {
                content.Append($"<string>{Escape(value)}</string>");
            }

            content.Append("</Values>");

            return content.ToString();
        }

        private static string BuildCharacter(string idElement, int weight, IEnumerable<string> groups)
        {
            StringBuilder character = new();
            character.Append($"<CharacterNames>{idElement}<Weight>{weight}</Weight>");

            foreach (string group in groups)
            {
                character.Append(group);
            }

            character.Append("</CharacterNames>");

            return character.ToString();
        }

        private static string Escape(string value)
            => value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
    }
}
