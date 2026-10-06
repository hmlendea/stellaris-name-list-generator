using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ShipClassNamesBuilder : NamesBuilder, IShipClassNamesBuilder
    {
        public string Build(NameList nameList)
        {
            StringBuilder content = new();
            content.Append($"{GetIndentation(1)}ship_class_names = {{{Environment.NewLine}");

            IEnumerable<NameGroup> genericShipClasses = nameList.ShipClasses.Generic
                .Concat(nameList.Denonyms)
                .Concat(nameList.Companies.AircraftManufacturers)
                .Concat(nameList.Companies.SpacecraftManufacturers)
                .Concat(nameList.Companies.RocketDesigners)
                .Concat(nameList.Warfare.ShipTypes);

            IEnumerable<NameGroup> corvetteClasses = nameList.ShipClasses.Corvette
                .Concat(nameList.Companies.WeaponManufacturers);
            IEnumerable<NameGroup> destroyerClasses = nameList.ShipClasses.Destroyer
                .Concat(nameList.Companies.WeaponManufacturers);
            IEnumerable<NameGroup> cruisedClasses = nameList.ShipClasses.Cruiser
                .Concat(nameList.Companies.WeaponManufacturers);
            IEnumerable<NameGroup> battleshipClasses = nameList.ShipClasses.Battleship
                .Concat(nameList.Companies.WeaponManufacturers);
            IEnumerable<NameGroup> titanClasses = nameList.ShipClasses.Titan
                .Concat(nameList.Companies.WeaponManufacturers);
            IEnumerable<NameGroup> colossusClasses = nameList.ShipClasses.Colossus
                .Concat(nameList.Companies.WeaponManufacturers);
            IEnumerable<NameGroup> juggernautClasses = nameList.ShipClasses.Juggernaut
                .Concat(nameList.Companies.WeaponManufacturers);

            IEnumerable<NameGroup> constructorClasses = nameList.ShipClasses.Constructor
                .Concat(nameList.Companies.AircraftManufacturers)
                .Concat(nameList.Companies.AutomotiveManufacturers);
            IEnumerable<NameGroup> scienceClasses = nameList.ShipClasses.Science
                .Concat(nameList.Companies.ResearchCompanies);
            IEnumerable<NameGroup> coloniserClasses = nameList.ShipClasses.Coloniser
                .Concat(nameList.Companies.AircraftManufacturers)
                .Concat(nameList.Companies.AutomotiveManufacturers);
            IEnumerable<NameGroup> sponsoredColoniserClasses = coloniserClasses
                .Concat(nameList.Companies.InvestmentCompanies);
            IEnumerable<NameGroup> transportClasses = coloniserClasses
                .Concat(nameList.Companies.AircraftManufacturers)
                .Concat(nameList.Companies.AutomotiveManufacturers);

            IEnumerable<NameGroup> smallMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Small);
            IEnumerable<NameGroup> mediumMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Medium);
            IEnumerable<NameGroup> largeMilitaryStationClasses = nameList.StationClasses.MilitaryStations.Generic.Concat(nameList.StationClasses.MilitaryStations.Large);
            IEnumerable<NameGroup> ionCannonClasses = nameList.ShipClasses.IonCannon
                .Concat(nameList.Warfare.Weapons.Artillery);

            StringBuilder innerContent = new();
            innerContent.Append(BuildNameArray(genericShipClasses, "generic", 2));
            innerContent.Append(BuildNameArray(corvetteClasses, "corvette", 2));
            innerContent.Append(BuildNameArray(destroyerClasses, "destroyer", 2));
            innerContent.Append(BuildNameArray(cruisedClasses, "cruiser", 2));
            innerContent.Append(BuildNameArray(battleshipClasses, "battleship", 2));
            innerContent.Append(BuildNameArray(titanClasses, "titan", 2));
            innerContent.Append(BuildNameArray(colossusClasses, "colossus", 2));
            innerContent.Append(BuildNameArray(juggernautClasses, "juggernaut", 2));
            innerContent.Append(BuildNameArray(constructorClasses, "constructor", 2));
            innerContent.Append(BuildNameArray(scienceClasses, "science", 2));
            innerContent.Append(BuildNameArray(coloniserClasses, "coloniser", 2));
            innerContent.Append(BuildNameArray(sponsoredColoniserClasses, "sponsored_coloniser", 2));
            innerContent.Append(BuildNameArray(transportClasses, "transport", 2));
            innerContent.Append(BuildNameArray(nameList.StationClasses.MiningStations, "mining_station", 2));
            innerContent.Append(BuildNameArray(nameList.StationClasses.ResearchStations, "research_station", 2));
            innerContent.Append(BuildNameArray(nameList.StationClasses.ObservationStations, "observation_station", 2));
            innerContent.Append(BuildNameArray(smallMilitaryStationClasses, "military_station_small", 2));
            innerContent.Append(BuildNameArray(mediumMilitaryStationClasses, "military_station_medium", 2));
            innerContent.Append(BuildNameArray(largeMilitaryStationClasses, "military_station_large", 2));
            innerContent.Append(BuildNameArray(ionCannonClasses, "ion_cannon", 2));

            if (innerContent.Length == 0)
            {
                return string.Empty;
            }

            content.Append(innerContent);
            content.Append($"{GetIndentation(1)}}}{Environment.NewLine}");

            return content.ToString();
        }
    }
}
