using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        readonly INamesBuilder shipNamesBuilder;
        readonly INamesBuilder shipClassNamesBuilder;
        readonly IFleetNamesBuilder fleetNamesBuilder;
        readonly INamesBuilder planetNamesBuilder;

        public FileContentBuilder(
            INamesBuilder shipNamesBuilder,
            INamesBuilder shipClassNamesBuilder,
            IFleetNamesBuilder fleetNamesBuilder,
            INamesBuilder planetNamesBuilder)
        {
            this.shipNamesBuilder = shipNamesBuilder;
            this.shipClassNamesBuilder = shipClassNamesBuilder;
            this.fleetNamesBuilder = fleetNamesBuilder;
            this.planetNamesBuilder = planetNamesBuilder;
        }

        public string BuildContent(NameList nameList)
        {
            Dictionary<string, string> sections = GenerateNameListSections(nameList);

            string content = $"### {nameList.Id}{Environment.NewLine}";
            content += $"### {nameList.Name}{Environment.NewLine}";
            content += $"### Leaders: {GetRandomLeaderName(nameList)}, {GetRandomLeaderName(nameList)}{Environment.NewLine}";
            content += $"### Ships: {GetRandomShipName(nameList)}, {GetRandomShipName(nameList)}{Environment.NewLine}";
            content += $"### Fleets: {fleetNamesBuilder.GetRandomName(nameList)}, {fleetNamesBuilder.GetRandomName(nameList)}{Environment.NewLine}";
            content += $"### Colonies: {GetRandomPlanetName(nameList)}, {GetRandomPlanetName(nameList)}{Environment.NewLine}";
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
                        sections[section.Key] = BuildArmyNames(nameList);
                        break;

                    case "PlanetNames":
                        sections[section.Key] = planetNamesBuilder.Build(nameList);
                        break;

                    case "CharacterNames":
                        sections[section.Key] = BuildCharacterNames(nameList);
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
        string BuildArmyNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}army_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> psionicArmyNames = nameList.Armies.PsionicArmy
                .Concat(nameList.BiosphereNames.MythologicalCreatures
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Covert Ops - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Covert Ops").ToList() },
                        new NameGroup { Name = $"Divisions - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Divison").ToList() },
                        new NameGroup { Name = $"Legions - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Legion").ToList() },
                        new NameGroup { Name = $"Squadrons - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Squadron").ToList() },
                    }));

            IList<NameGroup> xenomorphArmies = nameList.Armies.XenomorphArmy;
            IEnumerable<NameGroup> deitiesForXenomorph = nameList.GreatPeople.DeathDeities
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);

            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Abomination Flocks", "Deities", "{0}'s Abomination Flock"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Abomination Packs", "Deities", "{0}'s Abomination Pack"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Beast Legions", "Deities", "{0}'s Beast Legion"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Beast Warbands", "Deities", "{0}'s Beast Warband"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Death Flocks", "Deities", "{0}'s Death Flock"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Hybrid Packs", "Deities", "{0}'s Hybrid Pack"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Morphling Marauders", "Deities", "{0}'s Morphling Marauders"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Mutant Flocks", "Deities", "{0}'s Mutant Flock"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Mutant Lurkers", "Deities", "{0}'s Mutant Lurkers"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Mutant Swarms", "Deities", "{0}'s Mutant Swarm"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Broods", "Deities", "{0}'s Xenomorph Brood"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Hordes", "Deities", "{0}'s Xenomorph Horde"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Packs", "Deities", "{0}'s Xenomorph Pack"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Swarms", "Deities", "{0}'s Xenomorph Swarm"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Troopers", "Deities", "{0}'s Xenomorph Troopers"));

            string innerContent = string.Empty;
            innerContent += BuildNameArray(nameList.Armies.DefenceArmy, "defense_army", 2, nameList.Armies.DefenceArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.AssaultArmy, "assault_army", 2, nameList.Armies.AssaultArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.OccupationArmy, "occupation_army", 2, nameList.Armies.OccupationArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.SlaveArmy, "slave_army", 2, nameList.Armies.SlaveArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.CloneArmy, "clone_army", 2, nameList.Armies.CloneArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.RoboticDefenceArmy, "robotic_defense_army", 2, nameList.Armies.RoboticDefenceArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.RoboticAssaultArmy, "robotic_army", 2, nameList.Armies.RoboticAssaultArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.RoboticOccupationArmy, "robotic_occupation_army", 2, nameList.Armies.RoboticOccupationArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.AndroidDefenceArmy, "android_defense_army", 2, nameList.Armies.AndroidDefenceArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.AndroidAssaultArmy, "android_army", 2, nameList.Armies.AndroidAssaultArmySequentialName);
            innerContent += BuildNameArray(psionicArmyNames, "psionic_army", 2, nameList.Armies.PsionicArmySequentialName);
            innerContent += BuildNameArray(xenomorphArmies, "xenomorph_army", 2, nameList.Armies.XenomorphArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.SuperSoldierArmy, "gene_warrior_army", 2, nameList.Armies.SuperSoldierArmySequentialName);

            if (string.IsNullOrWhiteSpace(innerContent))
            {
                return string.Empty;
            }

            content += innerContent;
            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildCharacterNames(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<CharacterNames> characterNameLists = nameList.Characters
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .OrderByDescending(x => x.Weight);
            
            if (characterNameLists.All(x => x.IsEmpty))
            {
                return content;
            }

            content += $"{GetIndentation(1)}character_names = {{{Environment.NewLine}";

            foreach (CharacterNames characterNames in characterNameLists)
            {
                content += BuildCharacterNamesArray(characterNames);
            }

            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildCharacterNamesArray(CharacterNames characterNames)
        {
            string content = string.Empty;

            string characterNamesId = characterNames.Id;

            if (string.IsNullOrWhiteSpace(characterNamesId))
            {
                characterNamesId = $"names{DateTime.Now.Ticks}";
            }
            
            content += $"{GetIndentation(2)}{characterNamesId} = {{{Environment.NewLine}";
            content += $"{GetIndentation(3)}weight = {characterNames.Weight}{Environment.NewLine}";

            string innerContent = string.Empty;
            innerContent += BuildNameArray(characterNames.FullNames, "full_names", 3);
            innerContent += BuildNameArray(characterNames.FirstNames, "first_names", 3);
            innerContent += BuildNameArray(characterNames.RoyalFirstNames, "regnal_first_names", 3);
            innerContent += BuildNameArray(characterNames.MaleFullNames, "full_names_male", 3);
            innerContent += BuildNameArray(characterNames.MaleFirstNames, "first_names_male", 3);
            innerContent += BuildNameArray(characterNames.MaleRoyalFirstNames, "regnal_first_names_male", 3);
            innerContent += BuildNameArray(characterNames.FemaleFullNames, "full_names_female", 3);
            innerContent += BuildNameArray(characterNames.FemaleFirstNames, "first_names_female", 3);
            innerContent += BuildNameArray(characterNames.FemaleRoyalFirstNames, "regnal_first_names_female", 3);
            innerContent += BuildNameArray(characterNames.SecondNames, "second_names", 3);
            innerContent += BuildNameArray(characterNames.RoyalSecondNames, "regnal_second_names", 3);

            if (string.IsNullOrWhiteSpace(innerContent))
            {
                return string.Empty;
            }

            content += innerContent;
            content += $"{GetIndentation(2)}}}{Environment.NewLine}";

            return content;
        }

        string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels)
            => BuildNameArray(nameGroups, arrayName, indentationLevels, string.Empty);

        string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels, string sequentialName)
        {
            string content = string.Empty;

            if (string.IsNullOrWhiteSpace(sequentialName) && nameGroups.All(x => x.Values.Count == 0))
            {
                return content;
            }

            content += $"{GetIndentation(indentationLevels)}{arrayName} = {{{Environment.NewLine}";


            if (nameGroups?.Sum(x => x.Values.Count) > 0)
            {
                if (!string.IsNullOrWhiteSpace(sequentialName))
                {
                    content += $"{GetIndentation(indentationLevels + 1)}random_names = {{{Environment.NewLine}";
                    indentationLevels += 1;
                }

                content += GetFormattedNameCollection(nameGroups, indentationLevels + 1);
                
                if (!string.IsNullOrWhiteSpace(sequentialName))
                {
                    content += $"{GetIndentation(indentationLevels)}}}{Environment.NewLine}";
                    indentationLevels -= 1;
                }
            }

            if (!string.IsNullOrWhiteSpace(sequentialName))
            {
                content += $"{GetIndentation(indentationLevels + 1)}sequential_name = \"{sequentialName}\"{Environment.NewLine}";
            }

            content += $"{GetIndentation(indentationLevels)}}}{Environment.NewLine}";

            return content;
        }

        string GetFormattedNameCollection(IEnumerable<NameGroup> groups, int indentationLevels)
        {
            string indentation = GetIndentation(indentationLevels);
            IList<string> usedNames = new List<string>();
            IList<string> values = new List<string>();

            groups = groups
                .GroupBy(x => x.Name)
                .Select(g => new NameGroup
                {
                    Name = g.First().Name,
                    ExplicitValues = g.SelectMany(x => x.Values).ToList()
                });

            foreach (NameGroup group in groups.OrderBy(x => x.Name))
            {
                IList<string> lines = new List<string> { indentation };
                bool hasNames = false;

                IEnumerable<string> validNames = group.Values
                    .Where(x => !usedNames.Any(y => DoNamesMatch(x, y)) && !string.IsNullOrWhiteSpace(x))
                    .OrderBy(x => x)
                    .Distinct();

                foreach (string name in validNames)
                {
                    hasNames = true;
                    usedNames.Add(name);

                    string formattedName = ProcessName(name);

                    if (formattedName.Contains(" "))
                    {
                        formattedName = $"\"{formattedName}\"";
                    }

                    if (lines.Last().Length + 1 + formattedName.Length > MaximumLineLength)
                    {
                        lines.Add(indentation);
                    }

                    lines[lines.Count() - 1] += $"{formattedName} ";
                }

                if (!hasNames)
                {
                    continue;
                }

                string value = string.Empty;

                if (!string.IsNullOrWhiteSpace(group.Name))
                {
                    value += $"{indentation}# >>> {group.Name}{Environment.NewLine}";
                }

                foreach (string line in lines)
                {
                    value += $"{line.Substring(0, line.Length - 1)}{Environment.NewLine}";
                }
                
                values.Add(value);
            }

            return string.Join('\n', values);
        }

        string ProcessName(string name)
        {
            string processedName = name;

            processedName = Regex.Replace(processedName, "[ĂĀ]", "Ã");
            processedName = Regex.Replace(processedName, "[Đ]", "D");
            processedName = Regex.Replace(processedName, "[ĒĘ]", "E");
            processedName = Regex.Replace(processedName, "[Ğ]", "G");
            processedName = Regex.Replace(processedName, "[İĪ]", "I");
            processedName = Regex.Replace(processedName, "[Ł]", "L");
            processedName = Regex.Replace(processedName, "[Ń]", "N");
            processedName = Regex.Replace(processedName, "[Ó]", "O'");
            processedName = Regex.Replace(processedName, "[Ō]", "Ö");
            processedName = Regex.Replace(processedName, "[ȘŞṢŚ]", "S");
            processedName = Regex.Replace(processedName, "[Ț]", "T");
            processedName = Regex.Replace(processedName, "[Ū]", "Ü");
            processedName = Regex.Replace(processedName, "[Ư]", "U");
            processedName = Regex.Replace(processedName, "[ŹŻ]", "Z");
            processedName = Regex.Replace(processedName, "[ăā]", "ã");
            processedName = Regex.Replace(processedName, "[đ]", "d");
            processedName = Regex.Replace(processedName, "[ēę]", "e");
            processedName = Regex.Replace(processedName, "[ğ]", "g");
            processedName = Regex.Replace(processedName, "[īı]", "i");
            processedName = Regex.Replace(processedName, "[ł]", "l");
            processedName = Regex.Replace(processedName, "[ń]", "n");
            processedName = Regex.Replace(processedName, "[ō]", "ö");
            processedName = Regex.Replace(processedName, "[șşṣś]", "s");
            processedName = Regex.Replace(processedName, "[ț]", "t");
            processedName = Regex.Replace(processedName, "[ū]", "ü");
            processedName = Regex.Replace(processedName, "[źż]", "z");

            processedName = Regex.Replace(processedName, "[ʻ]", "'");

            return processedName;
        }

        string GetIndentation(int levels)
        {
            return string.Empty.PadRight(levels * IndentationSize, ' ');
        }

        NameGroup GenerateUnifiedNameGroup(IEnumerable<NameGroup> nameGroups, string category, string groupName, string nameFormat)
        {
            NameGroup group = new NameGroup();
            group.Name = $"{category} - {groupName}";
            group.ExplicitValues = nameGroups
                .SelectMany(x => x.Values)
                .Distinct()
                .Select(y => string.Format(nameFormat, y))
                .ToList();
            
            return group;
        }

        string GetRandomLeaderName(NameList nameList)
        {
            if (nameList.Characters is null ||
                nameList.Characters.Count == 0 ||
                nameList.Characters.All(x => x.IsEmpty))
            {
                return string.Empty;
            }

            CharacterNames characterNames = nameList.Characters.Where(x => !x.IsEmpty).GetRandomElement(random);

            bool takeMale = new List<bool> { true, false }.GetRandomElement(random);
            
            if (characterNames.FullNames.Any(x => !x.IsEmpty))
            {
                return characterNames.FullNames.SelectMany(x => x.Values).GetRandomElement(random);
            }

            if (takeMale && characterNames.MaleFullNames.Any(x => !x.IsEmpty))
            {
                return characterNames.MaleFullNames.SelectMany(x => x.Values).GetRandomElement(random);
            }

            if (!takeMale && characterNames.FemaleFullNames.Any(x => !x.IsEmpty))
            {
                return characterNames.FemaleFullNames.SelectMany(x => x.Values).GetRandomElement(random);
            }

            if (characterNames.SecondNames.Any(x => !x.IsEmpty))
            {
                if (characterNames.FirstNames.Any(x => !x.IsEmpty))
                {
                    return
                        characterNames.FirstNames.SelectMany(x => x.Values).GetRandomElement(random) + " " +
                        characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement(random);
                }

                if (takeMale && characterNames.MaleFirstNames.Any(x => !x.IsEmpty))
                {
                    return
                        characterNames.MaleFirstNames.SelectMany(x => x.Values).GetRandomElement(random) + " " +
                        characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement(random);
                }

                if (!takeMale && characterNames.FemaleFirstNames.Any(x => !x.IsEmpty))
                {
                    return
                        characterNames.FemaleFirstNames.SelectMany(x => x.Values).GetRandomElement(random) + " " +
                        characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement(random);
                }
            }

            return string.Empty;
        }

        string GetRandomShipName(NameList nameList)
        {
            IEnumerable<NameGroup> shipNames = nameList.Ships.Generic
                .Concat(nameList.Denonyms)
                .Concat(nameList.GreatPeople.Explorers)
                .Concat(nameList.GreatPeople.Pioneers)
                .Concat(nameList.GreatPeople.Scientists)
                .Concat(nameList.GreatPeople.FlyingAces)
                .Concat(nameList.GreatPeople.Heroes)
                .Concat(nameList.GreatPeople.Admirals)
                .Concat(nameList.GreatPeople.AllLeaders)
                .Concat(nameList.GreatPeople.AllGenerals)
                .Concat(nameList.BiosphereNames.MythologicalCreatures)
                .Concat(nameList.Places.Cities)
                .Concat(nameList.Places.Countries)
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.Places.Forests)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Mountains)
                .Concat(nameList.Places.Regions)
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Seas)
                .Concat(nameList.Ships.Battleship)
                .Concat(nameList.Ships.Coloniser)
                .Concat(nameList.Ships.Constructor)
                .Concat(nameList.Ships.Corvette)
                .Concat(nameList.Ships.Cruiser)
                .Concat(nameList.Ships.Destroyer)
                .Concat(nameList.Ships.Science)
                .Concat(nameList.Ships.Titan)
                .Concat(nameList.Ships.Transport)
                .Concat(nameList.Stations.MilitaryStations.Generic)
                .Concat(nameList.Stations.MilitaryStations.Large)
                .Concat(nameList.Stations.MilitaryStations.Medium)
                .Concat(nameList.Stations.MilitaryStations.Small)
                .Concat(nameList.Stations.Starbases.Citadels)
                .Concat(nameList.Stations.Starbases.Generic)
                .Concat(nameList.Stations.Starbases.Outposts)
                .Concat(nameList.Stations.Starbases.Starfortresses)
                .Concat(nameList.Stations.Starbases.Starholds)
                .Concat(nameList.Stations.Starbases.Starports)
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.Warfare.Forts)
                .Concat(nameList.Warfare.MilitaryUnitTypes)
                .Concat(nameList.Warfare.ShipTypes)
                .Concat(nameList.Warfare.Weapons.All);

            return shipNames.SelectMany(x => x.Values).GetRandomElement(random);
        }

        string GetRandomPlanetName(NameList nameList)
        {
            IEnumerable<NameGroup> planetNames = nameList.Planets.Generic
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.Places.Forests)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Mountains)
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Seas)
                .Concat(nameList.Planets.Alpine)
                .Concat(nameList.Planets.Arctic)
                .Concat(nameList.Planets.Arid)
                .Concat(nameList.Planets.Asteroid)
                .Concat(nameList.Planets.Barren)
                .Concat(nameList.Planets.Continental)
                .Concat(nameList.Planets.Desert)
                .Concat(nameList.Planets.Gaia)
                .Concat(nameList.Planets.Molten)
                .Concat(nameList.Planets.Ocean)
                .Concat(nameList.Planets.Savannah)
                .Concat(nameList.Planets.Tomb)
                .Concat(nameList.Planets.Tropical)
                .Concat(nameList.Planets.Tundra);

            return planetNames.SelectMany(x => x.Values).GetRandomElement(random);
        }

        bool DoNamesMatch(string name1, string name2)
        {
            return name1.RemoveDiacritics() == name2.RemoveDiacritics();
        }
    }
}
