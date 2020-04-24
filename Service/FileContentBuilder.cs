using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileContentBuilder : IFileContentBuilder
    {
        const int IndentationSize = 4;
        const int MaximumLineLength = 150;

        readonly Random random = new Random(873);

        readonly IShipNamesBuilder shipNamesBuilder;
        readonly INamesBuilder shipClassNamesBuilder;
        readonly IFleetNamesBuilder fleetNamesBuilder;
        readonly INamesBuilder armyNamesBuilder;
        readonly IPlanetNamesBuilder planetNamesBuilder;
        readonly ICharacterNamesBuilder characterNamesBuilder;

        public FileContentBuilder(
            IShipNamesBuilder shipNamesBuilder,
            INamesBuilder shipClassNamesBuilder,
            IFleetNamesBuilder fleetNamesBuilder,
            INamesBuilder armyNamesBuilder,
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
            Dictionary<string, string> sections = GenerateNameListSections(nameList);

            string content = $"### {nameList.Id}{Environment.NewLine}";
            content += $"### {nameList.Name}{Environment.NewLine}";
            content += $"### Leaders: {characterNamesBuilder.GetRandomName(nameList)}, {characterNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Ships: {shipNamesBuilder.GetRandomName(nameList)}, {shipNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Fleets: {fleetNamesBuilder.GetRandomName(nameList)}, {fleetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Colonies: {planetNamesBuilder.GetRandomName(nameList)}, {planetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += Environment.NewLine;

            content += $"{nameList.Id} = {{{Environment.NewLine}";
            content += BuildRandomisableOption(nameList.IsLocked);

            content += sections["ShipNames"];
            content += sections["ShipClassNames"];
            content += sections["FleetNames"];
            content += sections["ArmyNames"];
            content += sections["PlanetNames"];
            content += sections["CharacterNames"];

            content += $"}}{Environment.NewLine}";

            return content;
        }

        Dictionary<string, string> GenerateNameListSections(NameList nameList)
        {
            Dictionary<string, string> sections = new Dictionary<string, string>()
            {
                { "ShipNames", string.Empty },
                { "ShipClassNames", string.Empty },
                { "FleetNames", string.Empty },
                { "ArmyNames", string.Empty },
                { "PlanetNames", string.Empty },
                { "CharacterNames", string.Empty }
            };

            Parallel.ForEach(sections, section =>
            {
                switch (section.Key)
                {
                    case "ShipNames":
                        sections[section.Key] = shipNamesBuilder.Build(nameList);
                        break;

                    case "ShipClassNames":
                        sections[section.Key] = shipClassNamesBuilder.Build(nameList);
                        break;

                    case "FleetNames":
                        sections[section.Key] = fleetNamesBuilder.Build(nameList);
                        break;

                    case "ArmyNames":
                        sections[section.Key] = armyNamesBuilder.Build(nameList);
                        break;

                    case "PlanetNames":
                        sections[section.Key] = planetNamesBuilder.Build(nameList);
                        break;

                    case "CharacterNames":
                        sections[section.Key] = characterNamesBuilder.Build(nameList);
                        break;
                }
            });

            return sections;
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
