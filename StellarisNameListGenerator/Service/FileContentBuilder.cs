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
        private const int IndentationSize = 4;

        public string BuildContent(NameList nameList)
        {
            StringBuilder sb = new();

            sb.Append($"### {nameList.Id}{Environment.NewLine}");
            sb.Append($"### {nameList.Name}{Environment.NewLine}");
            sb.Append($"### Leaders: {characterNamesBuilder.GetRandomName(nameList)}, {characterNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}");
            sb.Append($"### Ships: {shipNamesBuilder.GetRandomName(nameList)}, {shipNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}");
            sb.Append($"### Fleets: {fleetNamesBuilder.GetRandomName(nameList)}, {fleetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}");
            sb.Append($"### Colonies: {planetNamesBuilder.GetRandomName(nameList)}, {planetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}");
            sb.Append(Environment.NewLine);

            sb.Append($"{nameList.Id} = {{{Environment.NewLine}");
            sb.Append(BuildRandomisableOption(nameList.IsLocked));

            sb.Append(shipNamesBuilder.Build(nameList));
            sb.Append(shipClassNamesBuilder.Build(nameList));
            sb.Append(fleetNamesBuilder.Build(nameList));
            sb.Append(armyNamesBuilder.Build(nameList));
            sb.Append(planetNamesBuilder.Build(nameList));
            sb.Append(characterNamesBuilder.Build(nameList));

            sb.Append($"}}{Environment.NewLine}");

            return sb.ToString();
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
