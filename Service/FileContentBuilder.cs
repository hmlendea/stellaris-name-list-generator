using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using StellarisNameListGenerator.Models;

using NuciExtensions;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileContentBuilder : IFileContentBuilder
    {
        const int IndentationSize = 4;
        const int MaximumLineLength = 150;

        readonly Random random = new Random(873);

        public string BuildContent(NameList nameList)
        {
            string content = $"### {nameList.Id}{Environment.NewLine}";
            content += $"### {nameList.Name}{Environment.NewLine}";
            content += $"### Leaders: {GetRandomLeaderName(nameList)}, {GetRandomLeaderName(nameList)}{Environment.NewLine}";
            content += $"### Ships: {GetRandomShipName(nameList)}, {GetRandomShipName(nameList)}{Environment.NewLine}";
            content += $"### Fleets: {GetRandomFleetName(nameList)}, {GetRandomFleetName(nameList)}{Environment.NewLine}";
            content += $"### Colonies: {GetRandomPlanetName(nameList)}, {GetRandomPlanetName(nameList)}{Environment.NewLine}";
            content += Environment.NewLine;

            content += $"{nameList.Id} = {{{Environment.NewLine}";
            content += BuildRandomisableOption(nameList.IsLocked);
            content += BuildShipNames(nameList);
            content += BuildShipClassNames(nameList);
            content += BuildFleetNames(nameList);
            content += BuildArmyNames(nameList);
            content += BuildPlanetNames(nameList);
            content += BuildCharacterNames(nameList);
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

        string BuildShipNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}ship_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> genericNames = nameList.Ships.Generic.Concat(nameList.Denonyms);
            IEnumerable<NameGroup> commonFighterNames = nameList.Warfare.MilitaryUnitTypes
                .Concat(nameList.Warfare.MilitaryUnitTypes)
                .Concat(nameList.BiosphereNames.MythologicalCreatures)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.TimeDeities)
                .Concat(nameList.GreatPeople.JusticeDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.SkyDeities)
                .Concat(nameList.GreatPeople.AirDeities)
                .Concat(nameList.GreatPeople.ColdDeities)
                .Concat(nameList.GreatPeople.WarmthDeities)
                .Concat(nameList.GreatPeople.WaterDeities)
                .Concat(nameList.GreatPeople.OtherDeities);

            IEnumerable<NameGroup> corvetteNames = nameList.Ships.Corvette
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.GreatPeople.LeadersTier1)
                .Concat(nameList.GreatPeople.FlyingAces)
                .Concat(nameList.GreatPeople.Heroes)
                .Concat(nameList.GreatPeople.GeneralsTier1)
                .Concat(nameList.BiosphereNames.Animals)
                .Concat(commonFighterNames);
            IEnumerable<NameGroup> destroyerNames = nameList.Ships.Destroyer
                .Concat(nameList.Places.Cities)
                .Concat(nameList.Warfare.Forts)
                .Concat(commonFighterNames);
            IEnumerable<NameGroup> cruiserNames = nameList.Ships.Cruiser
                .Concat(nameList.Places.Regions)
                .Concat(nameList.Warfare.Forts)
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.LeadersTier2)
                .Concat(nameList.GreatPeople.Admirals)
                .Concat(nameList.GreatPeople.GeneralsTier2)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(commonFighterNames);
            IEnumerable<NameGroup> battleshipNames = nameList.Ships.Battleship
                .Concat(nameList.Places.Countries)
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.Admirals)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(commonFighterNames);
            IEnumerable<NameGroup> titanNames = nameList.Ships.Titan
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .Concat(nameList.GreatPeople.OtherDeities);
            IEnumerable<NameGroup> constructorNames = nameList.Ships.Constructor
                .Concat(nameList.Places.Countries)
                .Concat(nameList.Places.Regions)
                .Concat(nameList.Places.Cities)
                .Concat(nameList.Places.Mountains)
                .Concat(nameList.Places.Forests)
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Seas)
                .Concat(nameList.GreatPeople.LabourDeities);
            IEnumerable<NameGroup> scienceNames = nameList.Ships.Science
                .Concat(nameList.GreatPeople.Explorers)
                .Concat(nameList.GreatPeople.Pioneers)
                .Concat(nameList.GreatPeople.Scientists)
                .Concat(nameList.GreatPeople.NatureDeities)
                .Concat(nameList.GreatPeople.HealthDeities)
                .Concat(nameList.GreatPeople.KnowledgeDeities);
            IEnumerable<NameGroup> coloniserNames = nameList.Ships.Coloniser
                .Concat(nameList.Places.Countries)
                .Concat(nameList.Places.Regions)
                .Concat(nameList.Places.Cities)
                .Concat(nameList.Places.Mountains)
                .Concat(nameList.Places.Forests)
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Seas)
                .Concat(nameList.GreatPeople.HealthDeities)
                .Concat(nameList.GreatPeople.LoveDeities)
                .Concat(nameList.GreatPeople.FeastDeities)
                .Concat(nameList.GreatPeople.FortuneDeities);
            IEnumerable<NameGroup> transportNames = nameList.Ships.Transport
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.Warfare.MilitaryUnitTypes)
                .Concat(nameList.Warfare.Forts)
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.Heroes)
                .Concat(nameList.GreatPeople.TimeDeities)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            IEnumerable<NameGroup> researchStations = nameList.Stations.ResearchStations
                .Concat(nameList.Companies.ResearchCompanies)
                .Concat(scienceNames);
            IEnumerable<NameGroup> observationStations = nameList.Stations.ObservationStations
                .Concat(nameList.GreatPeople.KnowledgeDeities);
            IEnumerable<NameGroup> genericStarbases = nameList.Stations.Starbases.Generic
                .Concat(nameList.GreatPeople.Explorers)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.TimeDeities)
                .Concat(nameList.GreatPeople.JusticeDeities)
                .Concat(nameList.GreatPeople.LabourDeities)
                .Concat(nameList.GreatPeople.NatureDeities)
                .Concat(nameList.GreatPeople.HealthDeities)
                .Concat(nameList.GreatPeople.LoveDeities)
                .Concat(nameList.GreatPeople.KnowledgeDeities)
                .Concat(nameList.GreatPeople.ArtDeities)
                .Concat(nameList.GreatPeople.FeastDeities)
                .Concat(nameList.GreatPeople.FortuneDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.SkyDeities)
                .Concat(nameList.GreatPeople.AirDeities)
                .Concat(nameList.GreatPeople.ColdDeities)
                .Concat(nameList.GreatPeople.WarmthDeities)
                .Concat(nameList.GreatPeople.WaterDeities)
                .Concat(nameList.GreatPeople.OtherDeities);
            IEnumerable<NameGroup> outpostNames = nameList.Stations.Starbases.Outposts.Concat(genericStarbases);
            IEnumerable<NameGroup> starportNames = nameList.Stations.Starbases.Starports.Concat(genericStarbases);
            IEnumerable<NameGroup> starholdNames = nameList.Stations.Starbases.Starholds.Concat(genericStarbases);
            IEnumerable<NameGroup> starfortressNames = nameList.Stations.Starbases.Starfortresses.Concat(genericStarbases);
            IEnumerable<NameGroup> citadelNames = nameList.Stations.Starbases.Citadels.Concat(genericStarbases);
            IEnumerable<NameGroup> genericMilitaryStations = nameList.Stations.MilitaryStations.Generic
                .Concat(nameList.Warfare.Forts)
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.Admirals);
            IEnumerable<NameGroup> smallMilitaryStations = nameList.Stations.MilitaryStations.Small
                .Concat(genericMilitaryStations)
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.GreatPeople.LeadersTier1)
                .Concat(nameList.GreatPeople.LeadersTier2)
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier1)
                .Concat(nameList.GreatPeople.GeneralsTier2)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.HatredDeities);
            IEnumerable<NameGroup> mediumMilitaryStations = nameList.Stations.MilitaryStations.Medium
                .Concat(genericMilitaryStations)
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.GreatPeople.LeadersTier2)
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier2)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.HatredDeities);
            IEnumerable<NameGroup> largeMilitaryStations = nameList.Stations.MilitaryStations.Large
                .Concat(genericMilitaryStations)
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.HatredDeities);
            IEnumerable<NameGroup> ionCannonNames = nameList.Ships.IonCannon
                .Concat(nameList.Warfare.Weapons.Ranged);
            
            content += BuildNameArray(genericNames, "generic", 2);
            content += BuildNameArray(corvetteNames, "corvette", 2);
            content += BuildNameArray(destroyerNames, "destroyer", 2);
            content += BuildNameArray(cruiserNames, "cruiser", 2);
            content += BuildNameArray(battleshipNames, "battleship", 2);
            content += BuildNameArray(titanNames, "titan", 2);
            content += BuildNameArray(nameList.Ships.Colossus, "colossus", 2);
            content += BuildNameArray(nameList.Ships.Juggernaut, "juggernaut", 2);
            content += BuildNameArray(constructorNames, "constructor", 2);
            content += BuildNameArray(scienceNames, "science", 2);
            content += BuildNameArray(coloniserNames, "colonizer", 2);
            content += BuildNameArray(coloniserNames, "sponsored_colonizer", 2);
            content += BuildNameArray(transportNames, "transport", 2);
            content += BuildNameArray(nameList.Stations.MiningStations, "mining_station", 2);
            content += BuildNameArray(researchStations, "research_station", 2);
            content += BuildNameArray(observationStations, "observation_station", 2);
            content += BuildNameArray(outpostNames, "starbase_outpost", 2, "%O% Starbase");
            content += BuildNameArray(starportNames, "starbase_starport", 2, "%O% Starbase");
            content += BuildNameArray(starholdNames, "starbase_starhold", 2, "%O% Starbase");
            content += BuildNameArray(starfortressNames, "starbase_starfortress", 2, "%O% Starbase");
            content += BuildNameArray(citadelNames, "starbase_citadel", 2, "%O% Starbase");
            content += BuildNameArray(smallMilitaryStations, "military_station_small", 2);
            content += BuildNameArray(mediumMilitaryStations, "military_station_medium", 2);
            content += BuildNameArray(largeMilitaryStations, "military_station_large", 2);
            content += BuildNameArray(ionCannonNames, "ion_cannon", 2);

            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildShipClassNames(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}ship_class_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> genericShipClasses = nameList.ShipClasses.Generic
                .Concat(nameList.Denonyms)
                .Concat(nameList.Companies.AircraftManufacturers)
                .Concat(nameList.Companies.SpacecraftManufacturers)
                .Concat(nameList.Companies.RocketDesigners)
                .Concat(nameList.Warfare.ShipTypes);

            IEnumerable<NameGroup> constructorClasses = nameList.ShipClasses.Constructor.Concat(nameList.Companies.AutomotiveManufacturers);
            IEnumerable<NameGroup> scienceClasses = nameList.ShipClasses.Science.Concat(nameList.Companies.ResearchCompanies);
            IEnumerable<NameGroup> coloniserClasses = nameList.ShipClasses.Coloniser.Concat(nameList.Companies.AutomotiveManufacturers);
            IEnumerable<NameGroup> sponsoredColoniserClasses = coloniserClasses.Concat(nameList.Companies.InvestmentCompanies);

            IEnumerable<NameGroup> outpostClasses = nameList.StationClasses.Starbases.Generic.Concat(nameList.StationClasses.Starbases.Outposts);
            IEnumerable<NameGroup> starportClasses = nameList.StationClasses.Starbases.Generic.Concat(nameList.StationClasses.Starbases.Starports);
            IEnumerable<NameGroup> starholdClasses = nameList.StationClasses.Starbases.Generic.Concat(nameList.StationClasses.Starbases.Starholds);
            IEnumerable<NameGroup> starfortressClasses = nameList.StationClasses.Starbases.Generic.Concat(nameList.StationClasses.Starbases.Starfortresses);
            IEnumerable<NameGroup> citadelClasses = nameList.StationClasses.Starbases.Generic.Concat(nameList.StationClasses.Starbases.Citadels);
            IEnumerable<NameGroup> smallMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Small);
            IEnumerable<NameGroup> mediumMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Medium);
            IEnumerable<NameGroup> largeMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Large);
            IEnumerable<NameGroup> ionCannonClasses = nameList.ShipClasses.IonCannon
                .Concat(nameList.Warfare.Weapons.Artillery);

            string innerContent = string.Empty;
            innerContent += BuildNameArray(genericShipClasses, "generic", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Corvette, "corvette", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Destroyer, "destroyer", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Cruiser, "cruiser", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Battleship, "battleship", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Titan, "titan", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Colossus, "colossus", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Juggernaut, "juggernaut", 2);
            innerContent += BuildNameArray(constructorClasses, "constructor", 2);
            innerContent += BuildNameArray(scienceClasses, "science", 2);
            innerContent += BuildNameArray(coloniserClasses, "coloniser", 2);
            innerContent += BuildNameArray(sponsoredColoniserClasses, "sponsored_coloniser", 2);
            innerContent += BuildNameArray(nameList.ShipClasses.Transport, "transport", 2);
            innerContent += BuildNameArray(nameList.StationClasses.MiningStations, "mining_station", 2);
            innerContent += BuildNameArray(nameList.StationClasses.ResearchStations, "research_station", 2);
            innerContent += BuildNameArray(nameList.StationClasses.ObservationStations, "observation_station", 2);
            innerContent += BuildNameArray(outpostClasses, "starbase_outpost", 2);
            innerContent += BuildNameArray(starportClasses, "starbase_starport", 2);
            innerContent += BuildNameArray(starholdClasses, "starbase_starhold", 2);
            innerContent += BuildNameArray(starfortressClasses, "starbase_starfortress", 2);
            innerContent += BuildNameArray(citadelClasses, "starbase_citadel", 2);
            innerContent += BuildNameArray(smallMilitaryStationClasses, "military_station_small", 2);
            innerContent += BuildNameArray(mediumMilitaryStationClasses, "military_station_medium", 2);
            innerContent += BuildNameArray(largeMilitaryStationClasses, "military_station_large", 2);
            innerContent += BuildNameArray(ionCannonClasses, "ion_cannon", 2);

            if (string.IsNullOrWhiteSpace(innerContent))
            {
                return string.Empty;
            }

            content += innerContent;
            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
        }

        string BuildFleetNames(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<NameGroup> fleetNames = GenerateFleetNames(nameList);
            content += BuildNameArray(fleetNames, "fleet_names", 1, nameList.Armies.FleetSequentialName);

            return content;
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
            IEnumerable<NameGroup> xenomorphArmies = nameList.Armies.XenomorphArmy
                .Concat(nameList.GreatPeople.DeathDeities
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Abomination Flocks - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Abomination Flock").ToList() },
                        new NameGroup { Name = $"Abomination Packs - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Abomination Pack").ToList() },
                        new NameGroup { Name = $"Beast Legions - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Beast Legion").ToList() },
                        new NameGroup { Name = $"Beast Warbands - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Beast Warband").ToList() },
                        new NameGroup { Name = $"Death Flocks - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Death Flock").ToList() },
                        new NameGroup { Name = $"Hybrid Packs - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Hybrid Pack").ToList() },
                        new NameGroup { Name = $"Morphling Marauderss - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Morphling Marauders").ToList() },
                        new NameGroup { Name = $"Mutant Flocks - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Mutant Flock").ToList() },
                        new NameGroup { Name = $"Mutant Lurkers - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Mutant Lurkers").ToList() },
                        new NameGroup { Name = $"Mutant Swarms - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Mutant Swarm").ToList() },
                        new NameGroup { Name = $"Xenomorph Broods - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Xenomorph Brood").ToList() },
                        new NameGroup { Name = $"Xenomorph Hordes - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Xenomorph Horde").ToList() },
                        new NameGroup { Name = $"Xenomorph Packs - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Xenomorph Pack").ToList() },
                        new NameGroup { Name = $"Xenomorph Swarms - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Xenomorph Swarm").ToList() },
                        new NameGroup { Name = $"Xenomorph Troopers - Deities", ExplicitValues = x.Values.Select(y => $"{y}'s Xenomorph Troopers").ToList() },
                    }));

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

        string BuildPlanetNames(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<NameGroup> genericNames = nameList.Planets.Generic
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.TimeDeities)
                .Concat(nameList.GreatPeople.JusticeDeities)
                .Concat(nameList.GreatPeople.KnowledgeDeities)
                .Concat(nameList.GreatPeople.SkyDeities)
                .Concat(nameList.GreatPeople.AirDeities
                .Concat(nameList.GreatPeople.OtherDeities));
            IEnumerable<NameGroup> desertNames = nameList.Planets.Desert
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> aridNames = nameList.Planets.Arid
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> tropicalNames = nameList.Planets.Tropical
                .Concat(nameList.Places.Forests)
                .Concat(nameList.GreatPeople.NatureDeities);
            IEnumerable<NameGroup> continentalNames = nameList.Planets.Continental;
            IEnumerable<NameGroup> gaiaNames = nameList.Planets.Gaia
                .Concat(nameList.GreatPeople.NatureDeities)
                .Concat(nameList.GreatPeople.HealthDeities)
                .Concat(nameList.GreatPeople.LoveDeities)
                .Concat(nameList.GreatPeople.ArtDeities)
                .Concat(nameList.GreatPeople.FeastDeities)
                .Concat(nameList.GreatPeople.FortuneDeities);
            IEnumerable<NameGroup> oceanNames = nameList.Planets.Ocean
                .Concat(nameList.Places.Rivers)
                .Concat(nameList.Places.Lakes)
                .Concat(nameList.Places.Seas)
                .Concat(nameList.GreatPeople.WaterDeities);
            IEnumerable<NameGroup> tundraNames = nameList.Planets.Tundra
                .Concat(nameList.GreatPeople.ColdDeities);
            IEnumerable<NameGroup> arcticNames = nameList.Planets.Arctic
                .Concat(nameList.GreatPeople.ColdDeities);
            IEnumerable<NameGroup> tombNames = nameList.Planets.Tomb
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            IEnumerable<NameGroup> savannahNames = nameList.Planets.Savannah
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> alpineNames = nameList.Planets.Alpine
                .Concat(nameList.Places.Mountains)
                .Concat(nameList.GreatPeople.ColdDeities);
            IEnumerable<NameGroup> moltenNames = nameList.Planets.Molten
                .Concat(nameList.GreatPeople.LabourDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> barrenNames = nameList.Planets.Barren
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            IEnumerable<NameGroup> asteroidNames = nameList.Planets.Asteroid
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            
            content += $"{GetIndentation(1)}planet_names = {{{Environment.NewLine}";
            content += BuildPlanetNameArray(genericNames, "generic");
            content += BuildPlanetNameArray(desertNames, "pc_desert");
            content += BuildPlanetNameArray(aridNames, "pc_arid");
            content += BuildPlanetNameArray(tropicalNames, "pc_tropical");
            content += BuildPlanetNameArray(continentalNames, "pc_continental");
            content += BuildPlanetNameArray(gaiaNames, "pc_gaia");
            content += BuildPlanetNameArray(oceanNames, "pc_ocean");
            content += BuildPlanetNameArray(tundraNames, "pc_tundra");
            content += BuildPlanetNameArray(arcticNames, "pc_arctic");
            content += BuildPlanetNameArray(tombNames, "pc_nuked");
            content += BuildPlanetNameArray(savannahNames, "pc_savannah");
            content += BuildPlanetNameArray(alpineNames, "pc_alpine");
            content += BuildPlanetNameArray(moltenNames, "pc_molten");
            content += BuildPlanetNameArray(barrenNames, "pc_barren");
            content += BuildPlanetNameArray(asteroidNames, "pc_asteroid");
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

        string BuildPlanetNameArray(IEnumerable<NameGroup> nameGroups, string planetClass)
        {
            string content = string.Empty;

            if (nameGroups.All(x => x.Values.Count == 0))
            {
                return content;
            }

            content += $"{GetIndentation(2)}{planetClass} = {{{Environment.NewLine}";
            content += BuildNameArray(nameGroups, "names", 3);
            content += $"{GetIndentation(2)}}}{Environment.NewLine}";

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
            processedName = Regex.Replace(processedName, "[Ō]", "O");
            processedName = Regex.Replace(processedName, "[ȘŞṢŚ]", "S");
            processedName = Regex.Replace(processedName, "[Ț]", "T");
            processedName = Regex.Replace(processedName, "[Ū]", "U");
            processedName = Regex.Replace(processedName, "[Ư]", "Ü");
            processedName = Regex.Replace(processedName, "[ŹŻ]", "Z");
            processedName = Regex.Replace(processedName, "[ăā]", "ã");
            processedName = Regex.Replace(processedName, "[đ]", "d");
            processedName = Regex.Replace(processedName, "[ēę]", "e");
            processedName = Regex.Replace(processedName, "[ğ]", "g");
            processedName = Regex.Replace(processedName, "[īı]", "i");
            processedName = Regex.Replace(processedName, "[ł]", "l");
            processedName = Regex.Replace(processedName, "[ń]", "n");
            processedName = Regex.Replace(processedName, "[ō]", "o");
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

        IEnumerable<NameGroup> GenerateFleetNames(NameList nameList)
        {
            return nameList.Armies.Fleet
                .Concat(nameList.Warfare.Weapons.All
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Armadas - Weapons", ExplicitValues = x.Values.Select(y => $"The {y} Armada").ToList() },
                        new NameGroup { Name = $"Battle Groups - Weapons", ExplicitValues = x.Values.Select(y => $"The {y} Battle Group").ToList() },
                        new NameGroup { Name = $"Corps - Weapons", ExplicitValues = x.Values.Select(y => $"The {y} Corps").ToList() },
                        new NameGroup { Name = $"Expeditionary Fleets - Weapons", ExplicitValues = x.Values.Select(y => $"{y} Expeditionary Fleet").ToList() },
                        new NameGroup { Name = $"Fleets - Weapons", ExplicitValues = x.Values.Select(y => $"The {y} Fleet").ToList() },
                        new NameGroup { Name = $"Flotillas - Weapons", ExplicitValues = x.Values.Select(y => $"The {y} Flotilla").ToList() },
                        new NameGroup { Name = $"Squadrons - Weapons", ExplicitValues = x.Values.Select(y => $"{y} Squadron").ToList() },
                        new NameGroup { Name = $"Starfleets - Weapons", ExplicitValues = x.Values.Select(y => $"{y} Starfleet").ToList() },
                        new NameGroup { Name = $"Strike Forces - Weapons", ExplicitValues = x.Values.Select(y => $"Strike Force {y}").ToList() },
                        new NameGroup { Name = $"Strike Teams - Weapons", ExplicitValues = x.Values.Select(y => $"Strike Team {y}").ToList() },
                        new NameGroup { Name = $"Task Forces - Weapons", ExplicitValues = x.Values.Select(y => $"Task Force {y}").ToList() }
                    }))
                .Concat(nameList.Warfare.MilitaryUnitTypes
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Armadas - Military Unit Types", ExplicitValues = x.Values.Select(y => $"The {y} Armada").ToList() },
                        new NameGroup { Name = $"Battle Groups - Military Unit Types", ExplicitValues = x.Values.Select(y => $"The {y} Battle Group").ToList() },
                        new NameGroup { Name = $"Corps - Military Unit Types", ExplicitValues = x.Values.Select(y => $"The {y} Corps").ToList() },
                        new NameGroup { Name = $"Expeditionary Fleets - Military Unit Types", ExplicitValues = x.Values.Select(y => $"{y} Expeditionary Fleet").ToList() },
                        new NameGroup { Name = $"Fleets - Military Unit Types", ExplicitValues = x.Values.Select(y => $"The {y} Fleet").ToList() },
                        new NameGroup { Name = $"Flotillas - Military Unit Types", ExplicitValues = x.Values.Select(y => $"The {y} Flotilla").ToList() },
                        new NameGroup { Name = $"Squadrons - Military Unit Types", ExplicitValues = x.Values.Select(y => $"{y} Squadron").ToList() },
                        new NameGroup { Name = $"Starfleets - Military Unit Types", ExplicitValues = x.Values.Select(y => $"{y} Starfleet").ToList() },
                        new NameGroup { Name = $"Strike Forces - Military Unit Types", ExplicitValues = x.Values.Select(y => $"Strike Force {y}").ToList() },
                        new NameGroup { Name = $"Strike Teams - Military Unit Types", ExplicitValues = x.Values.Select(y => $"Strike Team {y}").ToList() },
                        new NameGroup { Name = $"Task Forces - Military Unit Types", ExplicitValues = x.Values.Select(y => $"Task Force {y}").ToList() }
                    }))
                .Concat(nameList.Warfare.ShipTypes
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Armadas - Ship Types", ExplicitValues = x.Values.Select(y => $"The {y} Armada").ToList() },
                        new NameGroup { Name = $"Battle Groups - Ship Types", ExplicitValues = x.Values.Select(y => $"The {y} Battle Group").ToList() },
                        new NameGroup { Name = $"Corps - Ship Types", ExplicitValues = x.Values.Select(y => $"The {y} Corps").ToList() },
                        new NameGroup { Name = $"Expeditionary Fleets - Ship Types", ExplicitValues = x.Values.Select(y => $"{y} Expeditionary Fleet").ToList() },
                        new NameGroup { Name = $"Fleets - Ship Types", ExplicitValues = x.Values.Select(y => $"The {y} Fleet").ToList() },
                        new NameGroup { Name = $"Flotillas - Ship Types", ExplicitValues = x.Values.Select(y => $"The {y} Flotilla").ToList() },
                        new NameGroup { Name = $"Squadrons - Ship Types", ExplicitValues = x.Values.Select(y => $"{y} Squadron").ToList() },
                        new NameGroup { Name = $"Starfleets - Ship Types", ExplicitValues = x.Values.Select(y => $"{y} Starfleet").ToList() },
                        new NameGroup { Name = $"Strike Forces - Ship Types", ExplicitValues = x.Values.Select(y => $"Strike Force {y}").ToList() },
                        new NameGroup { Name = $"Strike Teams - Ship Types", ExplicitValues = x.Values.Select(y => $"Strike Team {y}").ToList() },
                        new NameGroup { Name = $"Task Forces - Ship Types", ExplicitValues = x.Values.Select(y => $"Task Force {y}").ToList() }
                    }))
                .Concat(nameList.BiosphereNames.MythologicalCreatures
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Armadas - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"The {y} Armada").ToList() },
                        new NameGroup { Name = $"Battle Groups - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"The {y} Battle Group").ToList() },
                        new NameGroup { Name = $"Corps - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"The {y} Corps").ToList() },
                        new NameGroup { Name = $"Expeditionary Fleets - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"{y} Expeditionary Fleet").ToList() },
                        new NameGroup { Name = $"Fleets - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"The {y} Fleet").ToList() },
                        new NameGroup { Name = $"Flotillas - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"The {y} Flotilla").ToList() },
                        new NameGroup { Name = $"Squadrons - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"{y} Squadron").ToList() },
                        new NameGroup { Name = $"Starfleets - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"{y} Starfleet").ToList() },
                        new NameGroup { Name = $"Strike Forces - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"Strike Force {y}").ToList() },
                        new NameGroup { Name = $"Strike Teams - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"Strike Team {y}").ToList() },
                        new NameGroup { Name = $"Task Forces - Mythological Creatures", ExplicitValues = x.Values.Select(y => $"Task Force {y}").ToList() }
                    }));
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
                .Concat(nameList.GreatPeople.GeneralsTier1)
                .Concat(nameList.GreatPeople.GeneralsTier2)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.LeadersTier1)
                .Concat(nameList.GreatPeople.LeadersTier2)
                .Concat(nameList.GreatPeople.LeadersTier3)
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

        string GetRandomFleetName(NameList nameList)
        {
            IEnumerable<NameGroup> fleetNames = GenerateFleetNames(nameList);

            if (fleetNames.Any(x => !x.IsEmpty))
            {
                return fleetNames
                    .SelectMany(x => x.Values)
                    .GetRandomElement(random);
            }

            List<string> cardinalNumbers = new List<string> { "1", "10", "33", "42", "56", "86", "101", "303", "500", "613", "743", "873" };
            List<string> ordinalNumbers = new List<string> { "1st", "21st", "101st", "42nd", "62nd", "72nd", "53rd", "83rd", "123rd", "103rd", "4th", "12th", "14th", "404th" };
            List<string> romanNumbers = new List<string> { "I", "II", "IV", "XI", "XXXII", "CXXXII", "CDII", "DLXII" };

            return nameList.Armies.FleetSequentialName
                .Replace("%O%", ordinalNumbers.GetRandomElement())
                .Replace("%C%", cardinalNumbers.GetRandomElement())
                .Replace("%R%", romanNumbers.GetRandomElement());
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
