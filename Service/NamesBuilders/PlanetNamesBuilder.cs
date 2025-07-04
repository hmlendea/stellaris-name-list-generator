using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class PlanetNamesBuilder : NamesBuilder, IPlanetNamesBuilder
    {
        public string Build(NameList nameList)
        {
            string content = string.Empty;

            IEnumerable<NameGroup> genericNames = nameList.Planets.Generic
                .Concat(nameList.GreatPeople.AllDeities);
            IEnumerable<NameGroup> desertNames = nameList.Planets.Desert
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities)
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> aridNames = nameList.Planets.Arid
                .Concat(nameList.Places.Deserts)
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> tropicalNames = nameList.Planets.Tropical
                .Concat(nameList.Places.Forests)
                .Concat(nameList.GreatPeople.NatureDeities);
            IEnumerable<NameGroup> continentalNames = nameList.Planets.Continental
                .Concat(nameList.GreatPeople.PeaceDeities)
                .Concat(nameList.GreatPeople.VictoryDeities)
                .Concat(nameList.GreatPeople.ProtectionDeities);
            IEnumerable<NameGroup> gaiaNames = nameList.Planets.Gaia
                .Concat(nameList.GreatPeople.CreationDeities)
                .Concat(nameList.GreatPeople.PeaceDeities)
                .Concat(nameList.GreatPeople.VictoryDeities)
                .Concat(nameList.GreatPeople.ProtectionDeities)
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
                .Concat(nameList.GreatPeople.ColdDeities)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities);
            IEnumerable<NameGroup> tombNames = nameList.Planets.Tomb
                .Concat(nameList.Warfare.BattleLocations)
                .Concat(nameList.GreatPeople.DestructionDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            IEnumerable<NameGroup> savannahNames = nameList.Planets.Savannah
                .Concat(nameList.GreatPeople.SunDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> alpineNames = nameList.Planets.Alpine
                .Concat(nameList.Places.Mountains)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.ColdDeities);
            IEnumerable<NameGroup> moltenNames = nameList.Planets.Molten
                .Concat(nameList.GreatPeople.DestructionDeities)
                .Concat(nameList.GreatPeople.WarDeities)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities)
                .Concat(nameList.GreatPeople.LabourDeities)
                .Concat(nameList.GreatPeople.WarmthDeities);
            IEnumerable<NameGroup> barrenNames = nameList.Planets.Barren
                .Concat(nameList.GreatPeople.DeathDeities)
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);
            IEnumerable<NameGroup> asteroidNames = nameList.Planets.Asteroid
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.PunishmentDeities)
                .Concat(nameList.GreatPeople.DisloyaltyDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);

            genericNames = CleanGenericNames(
                genericNames,
                desertNames,
                aridNames,
                tropicalNames,
                continentalNames,
                gaiaNames,
                oceanNames,
                tundraNames,
                arcticNames,
                tombNames,
                savannahNames,
                alpineNames,
                moltenNames,
                barrenNames,
                asteroidNames);

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

        public string GetRandomName(NameList nameList) => nameList.Planets.Generic
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
            .Concat(nameList.Planets.Tundra)
            .SelectMany(x => x.Values)
            .GetRandomElement();

        string BuildPlanetNameArray(IEnumerable<NameGroup> nameGroups, string planetClass)
        {
            StringBuilder sb = new();

            if (nameGroups.All(x => x.Values.Count == 0))
            {
                return string.Empty;
            }

            sb.Append($"{GetIndentation(2)}{planetClass} = {{{Environment.NewLine}");
            sb.Append(BuildNameArray(nameGroups, "names", 3));
            sb.Append($"{GetIndentation(2)}}}{Environment.NewLine}");

            return sb.ToString();
        }
    }
}
