using System;
using System.Collections.Generic;
using System.Linq;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ShipClassNamesBuilder : NamesBuilder, IShipClassNamesBuilder
    {
        public string Build(NameList nameList)
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

            IEnumerable<NameGroup> outpostClasses = nameList.StationClasses.Outposts;
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
    }
}
