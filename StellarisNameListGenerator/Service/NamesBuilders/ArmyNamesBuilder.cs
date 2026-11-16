using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ArmyNamesBuilder : NamesBuilder, IArmyNamesBuilder
    {
        private static readonly string[] XenomorphArmyFirstWords =
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

        private static readonly string[] XenomorphArmySecondWords =
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
            StringBuilder builder = new();
            builder.Append($"{GetIndentation(1)}army_names = {{{Environment.NewLine}");

            IEnumerable<NameGroup> psionicArmies = GeneratePsionicArmyNames(nameList);
            List<NameGroup> xenomorphArmies = GenerateXenomorphArmyNames(nameList);

            StringBuilder innerContent = new();

            innerContent.Append(BuildNameArray([], "machine_defense", 2, nameList.Armies.DefenceArmySequentialName));
            innerContent.Append(BuildNameArray([], "machine_assault_1", 2, nameList.Armies.AssaultArmySequentialName));
            innerContent.Append(BuildNameArray([], "machine_assault_2", 2, nameList.Armies.AssaultArmySequentialName));
            innerContent.Append(BuildNameArray([], "machine_assault_3", 2, nameList.Armies.AssaultArmySequentialName));

            innerContent.Append(BuildNameArray(nameList.Armies.DefenceArmy, "defense_army", 2, nameList.Armies.DefenceArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.AssaultArmy, "assault_army", 2, nameList.Armies.AssaultArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.OccupationArmy, "occupation_army", 2, nameList.Armies.OccupationArmySequentialName));

            innerContent.Append(BuildNameArray(nameList.Armies.SlaveArmy, "slave_army", 2, nameList.Armies.SlaveArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.CloneArmy, "clone_army", 2, nameList.Armies.CloneArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.PerfectedCloneArmy, "perfected_clone_army", 2, nameList.Armies.PerfectedCloneArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.UndeadArmy, "undead_army", 2, nameList.Armies.UndeadArmySequentialName));

            innerContent.Append(BuildNameArray(nameList.Armies.RoboticDefenceArmy, "robotic_defense_army", 2, nameList.Armies.RoboticDefenceArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.RoboticAssaultArmy, "robotic_army", 2, nameList.Armies.RoboticAssaultArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.RoboticOccupationArmy, "robotic_occupation_army", 2, nameList.Armies.RoboticOccupationArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.AndroidDefenceArmy, "android_defense_army", 2, nameList.Armies.AndroidDefenceArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.AndroidAssaultArmy, "android_army", 2, nameList.Armies.AndroidAssaultArmySequentialName));

            innerContent.Append(BuildNameArray(psionicArmies, "psionic_army", 2, nameList.Armies.PsionicArmySequentialName));
            innerContent.Append(BuildNameArray(xenomorphArmies, "xenomorph_army", 2, nameList.Armies.XenomorphArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.SuperSoldierArmy, "gene_warrior_army", 2, nameList.Armies.SuperSoldierArmySequentialName));

            innerContent.Append(BuildNameArray(nameList.Armies.PrimitiveArmy, "primitive_army", 2, nameList.Armies.PrimitiveArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.IndustrialArmy, "industrial_army", 2, nameList.Armies.IndustrialArmySequentialName));
            innerContent.Append(BuildNameArray(nameList.Armies.PostAtomicArmy, "postatomic_army", 2, nameList.Armies.PostAtomicArmySequentialName));

            if (innerContent.Length == 0)
            {
                return string.Empty;
            }

            builder.Append(innerContent);
            builder.Append($"{GetIndentation(1)}}}{Environment.NewLine}");

            return builder.ToString();
        }

        private static IEnumerable<NameGroup> GeneratePsionicArmyNames(NameList nameList)
        {
            IEnumerable<NameGroup> psionicArmyNames = nameList.Armies.PsionicArmy
                .Concat(nameList.BiosphereNames.MythologicalCreatures
                    .SelectMany(x => new List<NameGroup>
                    {
                        new() { Name = "Commandos - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Commando")] },
                        new() { Name = "Covert Ops - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Covert Ops")] },
                        new() { Name = "Divisions - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Division")] },
                        new() { Name = "Elite Corps - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Elite Corps")] },
                        new() { Name = "Legions - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Legion")] },
                        new() { Name = "Platoons - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Platoon")] },
                        new() { Name = "Squadrons - Mythological creatures", ExplicitValues = [.. x.Values.Select(y => $"{y} Squadron")] },
                    }));

            return psionicArmyNames;
        }

        private static List<NameGroup> GenerateXenomorphArmyNames(NameList nameList)
        {
            List<NameGroup> xenomorphArmies = [.. nameList.Armies.XenomorphArmy];
            List<NameGroup> deitiesForXenomorph = nameList.GreatPeople.DeathDeities
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities)
                .ToList();

            foreach (string firstWord in XenomorphArmyFirstWords)
            {
                foreach (string secondWord in XenomorphArmySecondWords)
                {
                    xenomorphArmies.Add(
                        BuildUnifiedNameGroup(deitiesForXenomorph,
                        $"{firstWord} {Pluralise(secondWord)}",
                        "Deities",
                        $"{{0}}'s {firstWord} {secondWord}")
                    );
                }
            }

            return xenomorphArmies;
        }

        private static string Pluralise(string word) => $"{word}s".Replace("ss", "s");
    }
}
