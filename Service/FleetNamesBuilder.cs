using System.Collections.Generic;
using System.Linq;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class FleetNamesBuilder : NamesBuilder, IFleetNamesBuilder
    {
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

            List<string> cardinalNumbers = new List<string> { "1", "10", "33", "42", "56", "86", "101", "303", "500", "613", "743", "873" };
            List<string> ordinalNumbers = new List<string> { "1st", "21st", "101st", "42nd", "62nd", "72nd", "53rd", "83rd", "123rd", "103rd", "4th", "12th", "14th", "404th" };
            List<string> romanNumbers = new List<string> { "I", "II", "IV", "XI", "XXXII", "CXXXII", "CDII", "DLXII" };

            return nameList.Armies.FleetSequentialName
                .Replace("%O%", ordinalNumbers.GetRandomElement())
                .Replace("%C%", cardinalNumbers.GetRandomElement())
                .Replace("%R%", romanNumbers.GetRandomElement());
        }

        IEnumerable<NameGroup> GenerateFleetNames(NameList nameList)
        {
            return nameList.Armies.Fleet
                .Concat(GenerateFleetNamesCategory(nameList, "Armadas", "The {0} Armada"))
                .Concat(GenerateFleetNamesCategory(nameList, "Battle Groups", "The {0} Battle Group"))
                .Concat(GenerateFleetNamesCategory(nameList, "Corps", "The {0} Corps"))
                .Concat(GenerateFleetNamesCategory(nameList, "Expeditionary Fleets", "The {0} Expeditionary Fleet"))
                .Concat(GenerateFleetNamesCategory(nameList, "Fleets", "The {0} Fleet"))
                .Concat(GenerateFleetNamesCategory(nameList, "Flotillas", "The {0} Flotilla"))
                .Concat(GenerateFleetNamesCategory(nameList, "Squadrons", "{0} Squadron"))
                .Concat(GenerateFleetNamesCategory(nameList, "Starfleets", "{0} Starfleet"))
                .Concat(GenerateFleetNamesCategory(nameList, "Strike Forces", "Strike Force {0}"))
                .Concat(GenerateFleetNamesCategory(nameList, "Strike Teams", "Strike Teams {0}"))
                .Concat(GenerateFleetNamesCategory(nameList, "Task Forces", "Task Force {0}"))
                .ToList();
        }

        IEnumerable<NameGroup> GenerateFleetNamesCategory(NameList nameList, string category, string nameFormat)
        {
            IList<NameGroup> fleetNames = new List<NameGroup>();
            
            fleetNames.Add(GenerateUnifiedNameGroup(nameList.Warfare.Weapons.All, category, "Weapons", nameFormat));
            fleetNames.Add(GenerateUnifiedNameGroup(nameList.Warfare.MilitaryUnitTypes, category, "Military Unit Types", nameFormat));
            fleetNames.Add(GenerateUnifiedNameGroup(nameList.Warfare.ShipTypes, category, "Ship Types", nameFormat));
            fleetNames.Add(GenerateUnifiedNameGroup(nameList.BiosphereNames.MythologicalCreatures, category, "Mythological Creatures", nameFormat));

            return fleetNames;
        }
    }
}
