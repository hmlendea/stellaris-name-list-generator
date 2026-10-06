using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ArmyNamesBuilder : NamesBuilder, IArmyNamesBuilder
    {
        private static readonly IList<string> xenomorphArmyFirstWords =
        [
            "Abomination",
            "Beast",
            "Death",
            "Horror",
            "Hybrid",
            "Morphling",
            "Mutant",
            "Rabid",
            "Xenomorph"
        ];

        private static readonly IList<string> xenomorphArmySecondWords =
        [
            "Brood",
            "Flock",
            "Horde",
            "Legion",
            "Lurkers",
            "Marauders",
            "Pack",
            "Swarm",
            "Troopers",
            "Warband"
        ];

        public string Build(NameList nameList)
        {
            StringBuilder sb = new();
            sb.Append($"{GetIndentation(1)}army_names = {{{Environment.NewLine}");

            var psionicArmies = GeneratePsionicArmyNames(nameList);
            var xenomorphArmies = GenerateXenomorphArmyNames(nameList);

            StringBuilder innerSb = new();

            innerSb.Append(BuildNameArray([], "machine_defense", 2, nameList.Armies.DefenceArmySequentialName));
            innerSb.Append(BuildNameArray([], "machine_assault_1", 2, nameList.Armies.AssaultArmySequentialName));
            innerSb.Append(BuildNameArray([], "machine_assault_2", 2, nameList.Armies.AssaultArmySequentialName));
            innerSb.Append(BuildNameArray([], "machine_assault_3", 2, nameList.Armies.AssaultArmySequentialName));

            innerSb.Append(BuildNameArray(nameList.Armies.DefenceArmy, "defense_army", 2, nameList.Armies.DefenceArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.AssaultArmy, "assault_army", 2, nameList.Armies.AssaultArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.OccupationArmy, "occupation_army", 2, nameList.Armies.OccupationArmySequentialName));

            innerSb.Append(BuildNameArray(nameList.Armies.SlaveArmy, "slave_army", 2, nameList.Armies.SlaveArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.CloneArmy, "clone_army", 2, nameList.Armies.CloneArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.PerfectedCloneArmy, "perfected_clone_army", 2, nameList.Armies.PerfectedCloneArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.UndeadArmy, "undead_army", 2, nameList.Armies.UndeadArmySequentialName));

            innerSb.Append(BuildNameArray(nameList.Armies.RoboticDefenceArmy, "robotic_defense_army", 2, nameList.Armies.RoboticDefenceArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.RoboticAssaultArmy, "robotic_army", 2, nameList.Armies.RoboticAssaultArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.RoboticOccupationArmy, "robotic_occupation_army", 2, nameList.Armies.RoboticOccupationArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.AndroidDefenceArmy, "android_defense_army", 2, nameList.Armies.AndroidDefenceArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.AndroidAssaultArmy, "android_army", 2, nameList.Armies.AndroidAssaultArmySequentialName));

            innerSb.Append(BuildNameArray(psionicArmies, "psionic_army", 2, nameList.Armies.PsionicArmySequentialName));
            innerSb.Append(BuildNameArray(xenomorphArmies, "xenomorph_army", 2, nameList.Armies.XenomorphArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.SuperSoldierArmy, "gene_warrior_army", 2, nameList.Armies.SuperSoldierArmySequentialName));

            innerSb.Append(BuildNameArray(nameList.Armies.PrimitiveArmy, "primitive_army", 2, nameList.Armies.PrimitiveArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.IndustrialArmy, "industrial_army", 2, nameList.Armies.IndustrialArmySequentialName));
            innerSb.Append(BuildNameArray(nameList.Armies.PostAtomicArmy, "postatomic_army", 2, nameList.Armies.PostAtomicArmySequentialName));

            if (innerSb.Length == 0)
            {
                return string.Empty;
            }

            sb.Append(innerSb);
            sb.Append($"{GetIndentation(1)}}}{Environment.NewLine}");

            return sb.ToString();
        }

        private static IEnumerable<NameGroup> GeneratePsionicArmyNames(NameList nameList)
        {
            IEnumerable<NameGroup> psionicArmyNames = nameList.Armies.PsionicArmy
                .Concat(nameList.BiosphereNames.MythologicalCreatures
                    .SelectMany(x => new List<NameGroup>
                    {
                        new() { Name = $"Commandos - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Commando")] },
                        new() { Name = $"Covert Ops - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Covert Ops")] },
                        new() { Name = $"Divisions - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Division")] },
                        new() { Name = $"Elite Corps - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Elite Corps")] },
                        new() { Name = $"Legions - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Legion")] },
                        new() { Name = $"Platoons - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Platoon")] },
                        new() { Name = $"Squadrons - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Squadron")] },
                    }));

            return psionicArmyNames;
        }

        private static List<NameGroup> GenerateXenomorphArmyNames(NameList nameList)
        {
            List<NameGroup> xenomorphArmies = [.. nameList.Armies.XenomorphArmy];
            var deitiesForXenomorph = nameList.GreatPeople.DeathDeities
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .ToList();

            List<NameGroup> newGroups = [];

            foreach (var firstWord in xenomorphArmyFirstWords)
            {
                foreach (var secondWord in xenomorphArmySecondWords)
                {
                    var secondWordPlural = $"{secondWord}s".Replace("ss", "s");

                    newGroups.Add(
                        GenerateUnifiedNameGroup(deitiesForXenomorph,
                        $"{firstWord} {secondWordPlural}",
                        "Deities",
                        $"{{0}}'s {firstWord} {secondWord}")
                    );
                }
            }
            xenomorphArmies.AddRange(newGroups);

            return xenomorphArmies;
        }
    }
}
