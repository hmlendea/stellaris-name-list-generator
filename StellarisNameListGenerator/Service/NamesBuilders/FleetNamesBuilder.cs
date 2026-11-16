using System.Collections.Generic;
using System.Linq;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class FleetNamesBuilder : NamesBuilder, IFleetNamesBuilder
    {
        private static readonly string[] FleetNameFormats =
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

        private static readonly string[] CardinalNumbers =
            ["1", "10", "33", "42", "56", "86", "101", "303", "500", "613", "743", "873"];

        private static readonly string[] OrdinalNumbers =
            ["1st", "21st", "101st", "42nd", "62nd", "72nd", "53rd", "83rd", "123rd", "103rd", "4th", "12th", "14th", "404th"];

        private static readonly string[] RomanNumbers =
            ["I", "II", "IV", "XI", "XXXII", "CXXXII", "CDII", "DLXII"];

        public string Build(NameList nameList)
            => BuildNameArray(GenerateFleetNames(nameList), "fleet_names", 1, nameList.Armies.FleetSequentialName);

        public string GetRandomName(NameList nameList)
        {
            List<NameGroup> fleetNames = GenerateFleetNames(nameList);

            if (fleetNames.Any(x => !x.IsEmpty))
            {
                return fleetNames
                    .SelectMany(x => x.Values)
                    .GetRandomElement();
            }

            return BuildSequentialName(nameList.Armies.FleetSequentialName);
        }

        private static string BuildSequentialName(string nameFormat)
            => nameFormat
                .Replace("%O%", OrdinalNumbers.GetRandomElement())
                .Replace("%C%", CardinalNumbers.GetRandomElement())
                .Replace("%R%", RomanNumbers.GetRandomElement());

        private static List<NameGroup> GenerateFleetNames(NameList nameList)
        {
            List<NameGroup> fleetNames = [.. nameList.Armies.Fleet];

            foreach (string fleetNameFormat in FleetNameFormats)
            {
                fleetNames.AddRange(GenerateFleetNamesCategory(nameList, GetCategory(fleetNameFormat), fleetNameFormat));
            }

            return fleetNames;
        }

        private static string GetCategory(string fleetNameFormat)
        {
            string category = fleetNameFormat
                .Replace("{0}", string.Empty)
                .Replace("The ", string.Empty)
                .Trim();

            return Pluralise(category);
        }

        private static string Pluralise(string word) => $"{word}s".Replace("ss", "s");

        private static IEnumerable<NameGroup> GenerateFleetNamesCategory(NameList nameList, string category, string nameFormat) =>
        [
            BuildUnifiedNameGroup(nameList.Warfare.Weapons.All, category, "Weapons", nameFormat),
            BuildUnifiedNameGroup(nameList.Warfare.MilitaryUnitTypes, category, "Military Unit Types", nameFormat),
            BuildUnifiedNameGroup(nameList.Warfare.ShipTypes, category, "Ship Types", nameFormat),
            BuildUnifiedNameGroup(nameList.BiosphereNames.MythologicalCreatures, category, "Mythological Creatures", nameFormat),
        ];
    }
}
