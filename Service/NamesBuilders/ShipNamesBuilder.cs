using System;
using System.Collections.Generic;
using System.Linq;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ShipNamesBuilder : NamesBuilder, IShipNamesBuilder
    {
        public string Build(NameList nameList)
        {
            string content = string.Empty;
            
            content += $"{GetIndentation(1)}ship_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> genericNames = nameList.Ships.Generic.Concat(nameList.Denonyms);
            IEnumerable<NameGroup> commonFighterNames = nameList.Warfare.MilitaryUnitTypes
                .Concat(nameList.Warfare.MilitaryUnitTypes)
                .Concat(nameList.BiosphereNames.MythologicalCreatures)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.TimeDeities)
                .Concat(nameList.GreatPeople.ProphecyDeities)
                .Concat(nameList.GreatPeople.JusticeDeities)
                .Concat(nameList.GreatPeople.ProtectionDeities)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.LoyaltyDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities)
                .Concat(nameList.GreatPeople.SleepDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .Concat(nameList.GreatPeople.LightDeities)
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
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(commonFighterNames);
            IEnumerable<NameGroup> titanNames = nameList.Ships.Titan
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.CreationDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.VictoryDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .Concat(nameList.GreatPeople.OtherDeities);
            IEnumerable<NameGroup> colossusNames = nameList.Ships.Colossus
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.CreationDeities)
                .Concat(nameList.GreatPeople.DestructionDeities)
                .Concat(nameList.GreatPeople.VictoryDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.SunDeities);
            IEnumerable<NameGroup> juggernautNames = nameList.Ships.Juggernaut;
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
                .Concat(nameList.GreatPeople.PeaceDeities)
                .Concat(nameList.GreatPeople.HealthDeities)
                .Concat(nameList.GreatPeople.LoveDeities)
                .Concat(nameList.GreatPeople.FeastDeities)
                .Concat(nameList.GreatPeople.FortuneDeities)
                .Concat(nameList.GreatPeople.SleepDeities);
            IEnumerable<NameGroup> transportNames = nameList.Ships.Transport
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.Warfare.MilitaryUnitTypes)
                .Concat(nameList.Warfare.Forts)
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.Heroes)
                .Concat(nameList.GreatPeople.TimeDeities)
                .Concat(nameList.GreatPeople.ProphecyDeities)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.CreationDeities)
                .Concat(nameList.GreatPeople.DestructionDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.VictoryDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            IEnumerable<NameGroup> miningStations = nameList.Stations.MiningStations
                .Concat(nameList.GreatPeople.LabourDeities);
            IEnumerable<NameGroup> researchStations = nameList.Stations.ResearchStations
                .Concat(nameList.Companies.ResearchCompanies)
                .Concat(scienceNames);
            IEnumerable<NameGroup> observationStations = nameList.Stations.ObservationStations
                .Concat(nameList.GreatPeople.KnowledgeDeities);
            IEnumerable<NameGroup> genericStarbases = nameList.Stations.Starbases.Generic
                .Concat(nameList.GreatPeople.Explorers)
                .Concat(nameList.GreatPeople.AllDeities);
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
                .Concat(nameList.GreatPeople.AllLeaders)
                .Concat(nameList.GreatPeople.AllGenerals)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.HatredDeities);
            IEnumerable<NameGroup> mediumMilitaryStations = nameList.Stations.MilitaryStations.Medium
                .Concat(genericMilitaryStations)
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.GreatPeople.LeadersTier2)
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier2)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.PeaceDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.HatredDeities);
            IEnumerable<NameGroup> largeMilitaryStations = nameList.Stations.MilitaryStations.Large
                .Concat(genericMilitaryStations)
                .Concat(nameList.Warfare.Weapons.All)
                .Concat(nameList.GreatPeople.LeadersTier3)
                .Concat(nameList.GreatPeople.GeneralsTier3)
                .Concat(nameList.GreatPeople.PowerDeities)
                .Concat(nameList.GreatPeople.CreationDeities)
                .Concat(nameList.GreatPeople.DestructionDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.VictoryDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.HatredDeities);
            IEnumerable<NameGroup> ionCannonNames = nameList.Ships.IonCannon
                .Concat(nameList.Warfare.Weapons.Ranged);

            genericNames = CleanGenericNames(
                genericNames,
                corvetteNames,
                destroyerNames,
                cruiserNames,
                battleshipNames,
                titanNames,
                colossusNames,
                juggernautNames,
                constructorNames,
                scienceNames,
                coloniserNames,
                transportNames);
            
            content += BuildNameArray(genericNames, "generic", 2);
            content += BuildNameArray(corvetteNames, "corvette", 2);
            content += BuildNameArray(destroyerNames, "destroyer", 2);
            content += BuildNameArray(cruiserNames, "cruiser", 2);
            content += BuildNameArray(battleshipNames, "battleship", 2);
            content += BuildNameArray(titanNames, "titan", 2);
            content += BuildNameArray(colossusNames, "colossus", 2);
            content += BuildNameArray(juggernautNames, "juggernaut", 2);
            content += BuildNameArray(constructorNames, "constructor", 2);
            content += BuildNameArray(scienceNames, "science", 2);
            content += BuildNameArray(coloniserNames, "colonizer", 2);
            content += BuildNameArray(coloniserNames, "sponsored_colonizer", 2);
            content += BuildNameArray(transportNames, "transport", 2);
            content += BuildNameArray(miningStations, "mining_station", 2);
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

        public string GetRandomName(NameList nameList)
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

            return shipNames.SelectMany(x => x.Values).GetRandomElement();
        }
    }
}
