using System;
using System.Collections.Generic;
using System.Linq;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class ArmyNamesBuilder : NamesBuilder, INamesBuilder
    {
        public string Build(NameList nameList)
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

            IList<NameGroup> xenomorphArmies = nameList.Armies.XenomorphArmy;
            IEnumerable<NameGroup> deitiesForXenomorph = nameList.GreatPeople.DeathDeities
                .Concat(nameList.GreatPeople.HatredDeities)
                .Concat(nameList.GreatPeople.FearDeities)
                .Concat(nameList.GreatPeople.SorrowDeities)
                .Concat(nameList.GreatPeople.BeastsDeities)
                .Concat(nameList.GreatPeople.DarknessDeities);

            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Abomination Flocks", "Deities", "{0}'s Abomination Flock"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Abomination Packs", "Deities", "{0}'s Abomination Pack"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Beast Legions", "Deities", "{0}'s Beast Legion"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Beast Warbands", "Deities", "{0}'s Beast Warband"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Death Flocks", "Deities", "{0}'s Death Flock"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Hybrid Packs", "Deities", "{0}'s Hybrid Pack"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Morphling Marauders", "Deities", "{0}'s Morphling Marauders"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Mutant Flocks", "Deities", "{0}'s Mutant Flock"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Mutant Lurkers", "Deities", "{0}'s Mutant Lurkers"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Mutant Swarms", "Deities", "{0}'s Mutant Swarm"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Broods", "Deities", "{0}'s Xenomorph Brood"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Hordes", "Deities", "{0}'s Xenomorph Horde"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Packs", "Deities", "{0}'s Xenomorph Pack"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Swarms", "Deities", "{0}'s Xenomorph Swarm"));
            xenomorphArmies.Add(GenerateUnifiedNameGroup(deitiesForXenomorph, "Xenomorph Troopers", "Deities", "{0}'s Xenomorph Troopers"));

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
    }
}
