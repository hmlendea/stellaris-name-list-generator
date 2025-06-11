using System;
using System.Collections.Generic;
using System.Linq;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ArmyNamesBuilder : NamesBuilder, IArmyNamesBuilder
    {
        private static readonly List<NameGroup> EmptyNameList = []; // TODO: Temporary solution. Remove this

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
            string content = $"{GetIndentation(1)}army_names = {{{Environment.NewLine}";

            IEnumerable<NameGroup> psionicArmies = GeneratePsionicArmyNames(nameList);
            IEnumerable<NameGroup> xenomorphArmies = GenerateXenomorphArmyNames(nameList);

            string innerContent = string.Empty;

            innerContent += BuildNameArray(EmptyNameList, "machine_defense", 2, nameList.Armies.DefenceArmySequentialName);
            innerContent += BuildNameArray(EmptyNameList, "machine_assault_1", 2, nameList.Armies.AssaultArmySequentialName);
            innerContent += BuildNameArray(EmptyNameList, "machine_assault_2", 2, nameList.Armies.AssaultArmySequentialName);
            innerContent += BuildNameArray(EmptyNameList, "machine_assault_3", 2, nameList.Armies.AssaultArmySequentialName);

            innerContent += BuildNameArray(nameList.Armies.DefenceArmy, "defense_army", 2, nameList.Armies.DefenceArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.AssaultArmy, "assault_army", 2, nameList.Armies.AssaultArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.OccupationArmy, "occupation_army", 2, nameList.Armies.OccupationArmySequentialName);

            innerContent += BuildNameArray(nameList.Armies.SlaveArmy, "slave_army", 2, nameList.Armies.SlaveArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.CloneArmy, "clone_army", 2, nameList.Armies.CloneArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.PerfectedCloneArmy, "perfected_clone_army", 2, nameList.Armies.PerfectedCloneArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.UndeadArmy, "undead_army", 2, nameList.Armies.UndeadArmySequentialName);

            innerContent += BuildNameArray(nameList.Armies.RoboticDefenceArmy, "robotic_defense_army", 2, nameList.Armies.RoboticDefenceArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.RoboticAssaultArmy, "robotic_army", 2, nameList.Armies.RoboticAssaultArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.RoboticOccupationArmy, "robotic_occupation_army", 2, nameList.Armies.RoboticOccupationArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.AndroidDefenceArmy, "android_defense_army", 2, nameList.Armies.AndroidDefenceArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.AndroidAssaultArmy, "android_army", 2, nameList.Armies.AndroidAssaultArmySequentialName);

            innerContent += BuildNameArray(psionicArmies, "psionic_army", 2, nameList.Armies.PsionicArmySequentialName);
            innerContent += BuildNameArray(xenomorphArmies, "xenomorph_army", 2, nameList.Armies.XenomorphArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.SuperSoldierArmy, "gene_warrior_army", 2, nameList.Armies.SuperSoldierArmySequentialName);

            innerContent += BuildNameArray(nameList.Armies.PrimitiveArmy, "primitive_army", 2, nameList.Armies.PrimitiveArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.IndustrialArmy, "industrial_army", 2, nameList.Armies.IndustrialArmySequentialName);
            innerContent += BuildNameArray(nameList.Armies.PostAtomicArmy, "postatomic_army", 2, nameList.Armies.PostAtomicArmySequentialName);

            if (string.IsNullOrWhiteSpace(innerContent))
            {
                return string.Empty;
            }

            content += innerContent;
            content += $"{GetIndentation(1)}}}{Environment.NewLine}";

            return content;
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
            List<NameGroup> xenomorphArmies = nameList.Armies.XenomorphArmy;
            IEnumerable<NameGroup> deitiesForXenomorph = nameList.GreatPeople.DeathDeities
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);

            foreach (string firstWord in xenomorphArmyFirstWords)
            {
                foreach (string secondWord in xenomorphArmySecondWords)
                {
                    string secondWordPlural = $"{secondWord}s".Replace("ss", "s");

                    xenomorphArmies.Add(
                        GenerateUnifiedNameGroup(deitiesForXenomorph,
                        $"{firstWord} {secondWordPlural}",
                        "Deities",
                        $"{{0}}'s {firstWord} {secondWord}"));
                }
            }

            return xenomorphArmies;
        }
    }
}
