using System;
using System.Collections.Generic;
using System.Linq;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileContentBuilder : IFileContentBuilder
    {
        const int IndentationSize = 4;
        const int MaximumLineLength = 140;

        public string BuildContent(NameList nameList)
        {
            string content = $"### {nameList.Name}{Environment.NewLine}{Environment.NewLine}";

            content += $"{nameList.Id} = {{{Environment.NewLine}";
            content += BuildShipNames(nameList);
            content += Environment.NewLine;
            content += BuildShipClassNames(nameList);
            content += Environment.NewLine;
            content += BuildFleetNames(nameList);
            content += Environment.NewLine;
            content += BuildArmyNames(nameList);
            content += Environment.NewLine;
            content += BuildPlanetNames(nameList);
            content += Environment.NewLine;
            content += BuildCharacterNames(nameList);
            content += "}";

            return content;
        }

        string BuildShipNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}ship_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> genericShipNames = nameList.Ships.Generic.Concat(nameList.Nationalities);
            IEnumerable<NameGroup> corvetteNames = nameList.Ships.Corvette
                .Concat(nameList.Weapons)
                .Concat(nameList.MilitaryUnitTypes)
                .Concat(nameList.MythologicalCreatures);
            IEnumerable<NameGroup> destroyerNames = nameList.Ships.Destroyer
                .Concat(nameList.Places.Cities)
                .Concat(nameList.MilitaryUnitTypes)
                .Concat(nameList.MythologicalCreatures);
            IEnumerable<NameGroup> cruiserNames = nameList.Ships.Cruiser
                .Concat(nameList.Places.States)
                .Concat(nameList.MilitaryUnitTypes)
                .Concat(nameList.MythologicalCreatures);
            IEnumerable<NameGroup> battleshipNames = nameList.Ships.Battleship
                .Concat(nameList.Places.Countries)
                .Concat(nameList.MilitaryUnitTypes)
                .Concat(nameList.MythologicalCreatures);
            IEnumerable<NameGroup> constructorNames = nameList.Ships.Constructor
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Seas);
            IEnumerable<NameGroup> coloniserShipNames = nameList.Ships.Coloniser
                .Concat(nameList.Places.Countries)
                .Concat(nameList.Places.States)
                .Concat(nameList.Places.Cities)
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Seas);
            IEnumerable<NameGroup> transportShipNames = nameList.Ships.Transport.Concat(nameList.Weapons);
            IEnumerable<NameGroup> outpostNames = nameList.Stations.StarbaseNames.Generic.Concat(nameList.Stations.StarbaseNames.Outposts);
            IEnumerable<NameGroup> starportNames = nameList.Stations.StarbaseNames.Generic.Concat(nameList.Stations.StarbaseNames.Starports);
            IEnumerable<NameGroup> starholdNames = nameList.Stations.StarbaseNames.Generic.Concat(nameList.Stations.StarbaseNames.Starholds);
            IEnumerable<NameGroup> starfortressNames = nameList.Stations.StarbaseNames.Generic.Concat(nameList.Stations.StarbaseNames.Starfortresses);
            IEnumerable<NameGroup> citadelNames = nameList.Stations.StarbaseNames.Generic.Concat(nameList.Stations.StarbaseNames.Citadels);
            IEnumerable<NameGroup> smallMilitaryStations = nameList.Stations.MilitaryStations.Generic.Concat(nameList.Stations.MilitaryStations.Small);
            IEnumerable<NameGroup> mediumMilitaryStations = nameList.Stations.MilitaryStations.Generic.Concat(nameList.Stations.MilitaryStations.Medium);
            IEnumerable<NameGroup> largeMilitaryStations = nameList.Stations.MilitaryStations.Generic.Concat(nameList.Stations.MilitaryStations.Large);
            
            content += BuildNameArray(genericShipNames, "generic", 2);
            content += Environment.NewLine;
            content += BuildNameArray(corvetteNames, "corvette", 2);
            content += BuildNameArray(destroyerNames, "destroyer", 2);
            content += BuildNameArray(cruiserNames, "cruiser", 2);
            content += BuildNameArray(battleshipNames, "battleship", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Ships.Titan, "titan", 2);
            content += BuildNameArray(nameList.Ships.Colossus, "colossus", 2);
            content += Environment.NewLine;
            content += BuildNameArray(constructorNames, "constructor", 2);
            content += BuildNameArray(nameList.Ships.Science, "science", 2);
            content += BuildNameArray(coloniserShipNames, "colonizer", 2);
            content += BuildNameArray(coloniserShipNames, "sponsored_colonizer", 2);
            content += BuildNameArray(transportShipNames, "transport", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Stations.MiningStations, "mining_station", 2);
            content += BuildNameArray(nameList.Stations.ResearchStations, "research_station", 2);
            content += BuildNameArray(nameList.Stations.ObservationStations, "observation_station", 2);
            content += Environment.NewLine;
            content += BuildNameArray(outpostNames, "starbase_outpost", 2, "%O% Starbase");
            content += BuildNameArray(starportNames, "starbase_starport", 2, "%O% Starbase");
            content += BuildNameArray(starholdNames, "starbase_starhold", 2, "%O% Starbase");
            content += BuildNameArray(starfortressNames, "starbase_starfortress", 2, "%O% Starbase");
            content += BuildNameArray(citadelNames, "starbase_citadel", 2, "%O% Starbase");
            content += Environment.NewLine;
            content += BuildNameArray(smallMilitaryStations, "military_station_small", 2);
            content += BuildNameArray(mediumMilitaryStations, "military_station_medium", 2);
            content += BuildNameArray(largeMilitaryStations, "military_station_large", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Ships.IonCannon, "ion_cannon", 2);

            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildShipClassNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}ship_class_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> genericShipClasses = nameList.ShipClasses.Generic.Concat(nameList.Nationalities);
            IEnumerable<NameGroup> outpostClasses = nameList.StationClasses.StarbaseNames.Generic.Concat(nameList.StationClasses.StarbaseNames.Outposts);
            IEnumerable<NameGroup> starportClasses = nameList.StationClasses.StarbaseNames.Generic.Concat(nameList.StationClasses.StarbaseNames.Starports);
            IEnumerable<NameGroup> starholdClasses = nameList.StationClasses.StarbaseNames.Generic.Concat(nameList.StationClasses.StarbaseNames.Starholds);
            IEnumerable<NameGroup> starfortressClasses = nameList.StationClasses.StarbaseNames.Generic.Concat(nameList.StationClasses.StarbaseNames.Starfortresses);
            IEnumerable<NameGroup> citadelClasses = nameList.StationClasses.StarbaseNames.Generic.Concat(nameList.StationClasses.StarbaseNames.Citadels);
            IEnumerable<NameGroup> smallMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Small);
            IEnumerable<NameGroup> mediumMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Medium);
            IEnumerable<NameGroup> largeMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Large);
            
            content += BuildNameArray(genericShipClasses, "generic", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.ShipClasses.Corvette, "corvette", 2);
            content += BuildNameArray(nameList.ShipClasses.Destroyer, "destroyer", 2);
            content += BuildNameArray(nameList.ShipClasses.Cruiser, "cruiser", 2);
            content += BuildNameArray(nameList.ShipClasses.Battleship, "battleship", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.ShipClasses.Titan, "titan", 2);
            content += BuildNameArray(nameList.ShipClasses.Colossus, "colossus", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.ShipClasses.Constructor, "constructor", 2);
            content += BuildNameArray(nameList.ShipClasses.Science, "science", 2);
            content += BuildNameArray(nameList.ShipClasses.Coloniser, "coloniser", 2);
            content += BuildNameArray(nameList.ShipClasses.Coloniser, "sponsored_coloniser", 2);
            content += BuildNameArray(nameList.ShipClasses.Transport, "transport", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.StationClasses.MiningStations, "mining_station", 2);
            content += BuildNameArray(nameList.StationClasses.ResearchStations, "research_station", 2);
            content += BuildNameArray(nameList.StationClasses.ObservationStations, "observation_station", 2);
            content += Environment.NewLine;
            content += BuildNameArray(outpostClasses, "starbase_outpost", 2);
            content += BuildNameArray(starportClasses, "starbase_starport", 2);
            content += BuildNameArray(starholdClasses, "starbase_starhold", 2);
            content += BuildNameArray(starfortressClasses, "starbase_starfortress", 2);
            content += BuildNameArray(citadelClasses, "starbase_citadel", 2);
            content += Environment.NewLine;
            content += BuildNameArray(smallMilitaryStationClasses, "military_station_small", 2);
            content += BuildNameArray(mediumMilitaryStationClasses, "military_station_medium", 2);
            content += BuildNameArray(largeMilitaryStationClasses, "military_station_large", 2);
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Ships.IonCannon, "ion_cannon", 2);

            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildFleetNames(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<NameGroup> fleetNames = nameList.Armies.Fleet
                .Concat(nameList.Weapons
                            .Concat(nameList.MilitaryUnitTypes)
                            .Concat(nameList.MythologicalCreatures)
                    .Select(x => new NameGroup
                    {
                        Name = $"{x.Name} (Fleets)",
                        Values = x.Values.Select(y => $"The {y} Fleet").ToList()
                    }))
                .Concat(nameList.MythologicalCreatures
                    .Select(x => new NameGroup
                    {
                        Name = $"{x.Name} (Strike Teams)",
                        Values = x.Values.Select(y => $"Strike Team {y}").ToList()
                    }));
            
            content += $"{GetIndentation(1)}fleet_names = {{{Environment.NewLine}";
            content += BuildNameArray(fleetNames, "random_names", 2);
            content += $"{GetIndentation(2)}sequential_name = \"%O% Fleet\"{Environment.NewLine}";
            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildArmyNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}army_names = {{{Environment.NewLine}";

            content += BuildNameArray(nameList.Armies.DefenceArmy, "defense_army", 2, "%O% Planetary Guard");
            content += BuildNameArray(nameList.Armies.AssaultArmy, "assault_army", 2, "%O% Expeditionary Force");
            content += BuildNameArray(nameList.Armies.OccupationArmy, "occupation_army", 2, "%O% Garrison Force");
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Armies.SlaveArmy, "slave_army", 2, "%O% Indentured Rifles");
            content += BuildNameArray(nameList.Armies.CloneArmy, "clone_army", 2, "%O% Clone Army");
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Armies.RoboticDefenceArmy, "robotic_defense_army", 2, "%O% Ground Defence Matrix");
            content += BuildNameArray(nameList.Armies.RoboticAssaultArmy, "robotic_army", 2, "%O% Hunter-Killer Group");
            content += BuildNameArray(nameList.Armies.RoboticOccupationArmy, "robotic_occupation_army", 2, "%O% Mechanised Garrison");
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Armies.AndroidDefenceArmy, "android_defense_army", 2, "%O% Synthetic Sentinels");
            content += BuildNameArray(nameList.Armies.AndroidAssaultArmy, "android_army", 2, "%O% Synthetic Rangers");
            content += Environment.NewLine;
            content += BuildNameArray(nameList.Armies.PsionicArmy, "psionic_army", 2, "%O% Psi Commando");
            content += BuildNameArray(nameList.Armies.XenomorphArmy, "xenomorph_army", 2, "%O% Bio-Warfare Division");
            content += BuildNameArray(nameList.Armies.SuperSoldierArmy, "gene_warrior_army", 2, "%O% Bio-Engineered Squadron");

            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildPlanetNames(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<NameGroup> oceanNames = nameList.Planets.Ocean
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Seas);
            
            content += $"{GetIndentation(1)}planet_names = {{{Environment.NewLine}";
            content += BuildPlanetNameArray(nameList.Planets.Generic, "generic");
            content += BuildPlanetNameArray(nameList.Planets.Desert, "pc_desert");
            content += BuildPlanetNameArray(nameList.Planets.Arid, "pc_arid");
            content += BuildPlanetNameArray(nameList.Planets.Tropical, "pc_tropical");
            content += BuildPlanetNameArray(nameList.Planets.Continental, "pc_continental");
            content += BuildPlanetNameArray(nameList.Planets.Gaia, "pc_gaia");
            content += BuildPlanetNameArray(oceanNames, "pc_ocean");
            content += BuildPlanetNameArray(nameList.Planets.Tundra, "pc_tundra");
            content += BuildPlanetNameArray(nameList.Planets.Arctic, "pc_arctic");
            content += BuildPlanetNameArray(nameList.Planets.Tomb, "pc_nuked");
            content += BuildPlanetNameArray(nameList.Planets.Savannah, "pc_savannah");
            content += BuildPlanetNameArray(nameList.Planets.Alpine, "pc_alpine");
            content += BuildPlanetNameArray(nameList.Planets.Molten, "pc_molten");
            content += BuildPlanetNameArray(nameList.Planets.Barren, "pc_barren");
            content += BuildPlanetNameArray(nameList.Planets.Asteroid, "pc_asteroid");
            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildCharacterNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}character_names = {{{Environment.NewLine}";
            
            foreach (CharacterNames characterNames in nameList.Characters.OrderByDescending(x => x.Weight))
            {
                content += BuildCharacterNamesArray(characterNames);
            }

            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildPlanetNameArray(IEnumerable<NameGroup> nameGroups, string planetClass)
        {
            string content = string.Empty;

            content += $"{GetIndentation(2)}{planetClass} = {{{Environment.NewLine}";
            content += BuildNameArray(nameGroups, "names", 3);
            content += $"{GetIndentation(2)}}}{Environment.NewLine}";

            return content;
        }

        string BuildCharacterNamesArray(CharacterNames characterNames)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(2)}{characterNames.Id} = {{{Environment.NewLine}";
            content += $"{GetIndentation(3)}weight = {characterNames.Weight}{Environment.NewLine}";
            content += BuildNameArray(characterNames.MaleFirstNames, "first_names_male", 3);
            content += BuildNameArray(characterNames.MaleRoyalFirstNames, "regnal_first_names_male", 3);
            content += BuildNameArray(characterNames.FemaleFirstNames, "first_names_female", 3);
            content += BuildNameArray(characterNames.FemaleRoyalFirstNames, "regnal_first_names_female", 3);
            content += BuildNameArray(characterNames.SecondNames, "second_first_names", 3);
            content += BuildNameArray(characterNames.RoyalSecondNames, "regnal_second_names", 3);
            content += $"{GetIndentation(2)}}}{Environment.NewLine}";

            return content;
        }

        string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels)
            => BuildNameArray(nameGroups, arrayName, indentationLevels, string.Empty);

        string BuildNameArray(IEnumerable<NameGroup> nameGroups, string arrayName, int indentationLevels, string sequentialName)
        {
            string content = string.Empty;

            content += $"{GetIndentation(indentationLevels)}{arrayName} = {{{Environment.NewLine}";

            if (!string.IsNullOrWhiteSpace(sequentialName))
            {
                content += $"{GetIndentation(indentationLevels + 1)}sequential_name = \"{sequentialName}\"{Environment.NewLine}";
            }

            if (!(nameGroups is null))
            {
                content += GetFormattedNameCollection(nameGroups, indentationLevels + 1);
            }

            content += $"{GetIndentation(indentationLevels)}}}{Environment.NewLine}";

            return content;
        }

        string GetFormattedNameCollection(IEnumerable<NameGroup> groups, int indentationLevels)
        {
            string indentation = GetIndentation(indentationLevels);
            IList<string> usedNames = new List<string>();
            IList<string> values = new List<string>();

            foreach (NameGroup group in groups.OrderBy(x => x.Name))
            {
                IList<string> lines = new List<string> { indentation };
                bool hasNames = false;

                foreach (string name in group.Values.OrderBy(x => x).Distinct())
                {
                    if (usedNames.Contains(name))
                    {
                        continue;
                    }

                    hasNames = true;
                    usedNames.Add(name);

                    string formattedName = $"\"{name}\"";

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

        string GetIndentation(int levels)
        {
            return string.Empty.PadRight(levels * IndentationSize, ' ');
        }
    }
}
