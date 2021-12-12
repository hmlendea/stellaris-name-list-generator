using System;
using System.Collections.Generic;
using System.Linq;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class ArmyNamesBuilder : NamesBuilder, IArmyNamesBuilder
    {
        static readonly List<NameGroup> EmptyNameList = new List<NameGroup>(); // TODO: Temporary solution. Remove this

        static readonly IList<string> xenomorphArmyFirstWords = new List<string>
        {
            "Abomination",
            "Beast",
            "Death",
            "Hybrid",
            "Morphling",
            "Mutant",
            "Xenomorph"
        };
        
        static readonly IList<string> xenomorphArmySecondWords = new List<string>
        {
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
        };

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

        IEnumerable<NameGroup> GeneratePsionicArmyNames(NameList nameList)
        {
            IEnumerable<NameGroup> psionicArmyNames = nameList.Armies.PsionicArmy
                .Concat(nameList.BiosphereNames.MythologicalCreatures
                    .SelectMany(x => new List<NameGroup>
                    {
                        new NameGroup { Name = $"Commandos - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Commando").ToList() },
                        new NameGroup { Name = $"Covert Ops - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Covert Ops").ToList() },
                        new NameGroup { Name = $"Divisions - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Divison").ToList() },
                        new NameGroup { Name = $"Elite Corps - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Elite Corps").ToList() },
                        new NameGroup { Name = $"Legions - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Legion").ToList() },
                        new NameGroup { Name = $"Squadrons - Mythological creatures", ExplicitValues = x.Values.Select(y => $"{y} Squadron").ToList() },
                    }));

            return psionicArmyNames;
        }

        IEnumerable<NameGroup> GenerateXenomorphArmyNames(NameList nameList)
        {
            IList<NameGroup> xenomorphArmies = nameList.Armies.XenomorphArmy;
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
