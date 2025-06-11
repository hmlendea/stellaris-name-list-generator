using System;

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
        private const int IndentationSize = 4;

        public string BuildContent(NameList nameList)
        {
            string content = $"### {nameList.Id}{Environment.NewLine}";
            content += $"### {nameList.Name}{Environment.NewLine}";
            content += $"### Leaders: {characterNamesBuilder.GetRandomName(nameList)}, {characterNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Ships: {shipNamesBuilder.GetRandomName(nameList)}, {shipNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Fleets: {fleetNamesBuilder.GetRandomName(nameList)}, {fleetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Colonies: {planetNamesBuilder.GetRandomName(nameList)}, {planetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += Environment.NewLine;

            content += $"{nameList.Id} = {{{Environment.NewLine}";
            content += BuildRandomisableOption(nameList.IsLocked);

            content += shipNamesBuilder.Build(nameList);
            content += shipClassNamesBuilder.Build(nameList);
            content += fleetNamesBuilder.Build(nameList);
            content += armyNamesBuilder.Build(nameList);
            content += planetNamesBuilder.Build(nameList);
            content += characterNamesBuilder.Build(nameList);

            content += $"}}{Environment.NewLine}";

            return content;
        }

        private static string BuildRandomisableOption(bool isLocked)
        {
            string randomisableLine;

            if (isLocked)
            {
                randomisableLine = $"{GetIndentation(1)}randomized = no";
            }
            else
            {
                randomisableLine = $"{GetIndentation(1)}randomized = yes";
            }

            return $"{randomisableLine}{Environment.NewLine}";
        }

        private static string GetIndentation(int levels)
            => string.Empty.PadRight(levels * IndentationSize, ' ');
    }
}
