using System;

using NuciExtensions;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service.NamesBuilders;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileContentBuilder : IFileContentBuilder
    {
        const int IndentationSize = 4;
        const int MaximumLineLength = 150;

        readonly Random random = new Random(873);

        readonly IShipNamesBuilder shipNamesBuilder;
        readonly IShipClassNamesBuilder shipClassNamesBuilder;
        readonly IFleetNamesBuilder fleetNamesBuilder;
        readonly IArmyNamesBuilder armyNamesBuilder;
        readonly IPlanetNamesBuilder planetNamesBuilder;
        readonly ICharacterNamesBuilder characterNamesBuilder;

        public FileContentBuilder(
            IShipNamesBuilder shipNamesBuilder,
            IShipClassNamesBuilder shipClassNamesBuilder,
            IFleetNamesBuilder fleetNamesBuilder,
            IArmyNamesBuilder armyNamesBuilder,
            IPlanetNamesBuilder planetNamesBuilder,
            ICharacterNamesBuilder characterNamesBuilder)
        {
            this.shipNamesBuilder = shipNamesBuilder;
            this.shipClassNamesBuilder = shipClassNamesBuilder;
            this.fleetNamesBuilder = fleetNamesBuilder;
            this.armyNamesBuilder = armyNamesBuilder;
            this.planetNamesBuilder = planetNamesBuilder;
            this.characterNamesBuilder = characterNamesBuilder;
        }

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

        string BuildRandomisableOption(bool isLocked)
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

        string GetIndentation(int levels)
        {
            return string.Empty.PadRight(levels * IndentationSize, ' ');
        }

        bool DoNamesMatch(string name1, string name2)
        {
            return name1.RemoveDiacritics() == name2.RemoveDiacritics();
        }
    }
}
