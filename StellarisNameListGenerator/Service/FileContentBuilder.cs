using System;
using System.Text;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service.NamesBuilders;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileContentBuilder(
        IShipNamesBuilder shipNamesBuilder,
        IShipClassNamesBuilder shipClassNamesBuilder,
        IFleetNamesBuilder fleetNamesBuilder,
        IArmyNamesBuilder armyNamesBuilder,
        IPlanetNamesBuilder planetNamesBuilder,
        ICharacterNamesBuilder characterNamesBuilder) : IFileContentBuilder
    {
        private static int IndentationSize => 4;

        public string BuildContent(NameList nameList)
        {
            StringBuilder content = new();

            content.Append(BuildHeaders(nameList));
            content.Append($"{nameList.Id} = {{{Environment.NewLine}");
            content.Append(BuildRandomisedOption(nameList.IsLocked));

            content.Append(shipNamesBuilder.Build(nameList));
            content.Append(shipClassNamesBuilder.Build(nameList));
            content.Append(fleetNamesBuilder.Build(nameList));
            content.Append(armyNamesBuilder.Build(nameList));
            content.Append(planetNamesBuilder.Build(nameList));
            content.Append(characterNamesBuilder.Build(nameList));

            content.Append($"}}{Environment.NewLine}");

            return content.ToString();
        }

        private string BuildHeaders(NameList nameList)
        {
            StringBuilder headers = new();

            headers.Append($"### {nameList.Id}{Environment.NewLine}");
            headers.Append($"### {nameList.Name}{Environment.NewLine}");
            headers.Append($"### Leaders: {NameSampleBuilder.Build(characterNamesBuilder, nameList)}{Environment.NewLine}");
            headers.Append($"### Ships: {NameSampleBuilder.Build(shipNamesBuilder, nameList)}{Environment.NewLine}");
            headers.Append($"### Fleets: {NameSampleBuilder.Build(fleetNamesBuilder, nameList)}{Environment.NewLine}");
            headers.Append($"### Colonies: {NameSampleBuilder.Build(planetNamesBuilder, nameList)}{Environment.NewLine}");
            headers.Append(Environment.NewLine);

            return headers.ToString();
        }

        private static string BuildRandomisedOption(bool isLocked)
        {
            string randomisedValue;

            if (isLocked)
            {
                randomisedValue = "no";
            }
            else
            {
                randomisedValue = "yes";
            }

            return $"{GetIndentation(1)}randomized = {randomisedValue}{Environment.NewLine}";
        }

        private static string GetIndentation(int levels)
            => string.Empty.PadRight(levels * IndentationSize, ' ');
    }
}
