using System.Collections.Generic;
using System.Linq;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class FleetNamesBuilder : NamesBuilder, IFleetNamesBuilder
    {
        static readonly IList<string> fleetNameFormats =
        [
            "{0} Extraorbital Corps",
            "{0} Squadron",
            "{0} Star Corps",
            "{0} Star Order",
            "{0} Starfleet",
            "Strike Force {0}",
            "Strike Team {0}",
            "Task Force {0}",
            "The {0} Armada",
            "The {0} Battle Group",
            "The {0} Corps",
            "The {0} Expeditionary Fleet",
            "The {0} Fleet",
            "The {0} Flotilla",
            "The {0} Navy",
        ];

        public string Build(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<NameGroup> fleetNames = GenerateFleetNames(nameList);
            content += BuildNameArray(fleetNames, "fleet_names", 1, nameList.Armies.FleetSequentialName);

            return content;
        }

        public string GetRandomName(NameList nameList)
        {
            IEnumerable<NameGroup> fleetNames = GenerateFleetNames(nameList);

            if (fleetNames.Any(x => !x.IsEmpty))
            {
                return fleetNames
                    .SelectMany(x => x.Values)
                    .GetRandomElement();
            }

            List<string> cardinalNumbers = ["1", "10", "33", "42", "56", "86", "101", "303", "500", "613", "743", "873"];
            List<string> ordinalNumbers = ["1st", "21st", "101st", "42nd", "62nd", "72nd", "53rd", "83rd", "123rd", "103rd", "4th", "12th", "14th", "404th"];
            List<string> romanNumbers = ["I", "II", "IV", "XI", "XXXII", "CXXXII", "CDII", "DLXII"];

            return nameList.Armies.FleetSequentialName
                .Replace("%O%", ordinalNumbers.GetRandomElement())
                .Replace("%C%", cardinalNumbers.GetRandomElement())
                .Replace("%R%", romanNumbers.GetRandomElement());
        }

        IEnumerable<NameGroup> GenerateFleetNames(NameList nameList)
        {
            var fleetNames = nameList.Armies.Fleet.ToList();
            List<NameGroup> result = [.. fleetNames];

            foreach (var fleetNameFormat in fleetNameFormats)
            {
                string category = fleetNameFormat
                    .Replace("{0}", "")
                    .Replace("The ", "")
                    .Trim();

                category = $"{category}s".Replace("ss", "s");

                var categoryNames = GenerateFleetNamesCategory(nameList, category, fleetNameFormat);

                result.AddRange(categoryNames);
            }

            return result;
        }

        static IEnumerable<NameGroup> GenerateFleetNamesCategory(NameList nameList, string category, string nameFormat) =>
        [
            GenerateUnifiedNameGroup(nameList.Warfare.Weapons.All, category, "Weapons", nameFormat),
            GenerateUnifiedNameGroup(nameList.Warfare.MilitaryUnitTypes, category, "Military Unit Types", nameFormat),
            GenerateUnifiedNameGroup(nameList.Warfare.ShipTypes, category, "Ship Types", nameFormat),
            GenerateUnifiedNameGroup(nameList.BiosphereNames.MythologicalCreatures, category, "Mythological Creatures", nameFormat),
        ];
    }
}
