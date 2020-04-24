using System;
using System.Collections.Generic;
using System.Linq;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class CharacterNamesBuilder : NamesBuilder, ICharacterNamesBuilder
    {
        public string Build(NameList nameList)
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

        public string GetRandomName(NameList nameList)
        {
            if (nameList.Characters is null ||
                nameList.Characters.Count == 0 ||
                nameList.Characters.All(x => x.IsEmpty))
            {
                return string.Empty;
            }

            CharacterNames characterNames = nameList.Characters.Where(x => !x.IsEmpty).GetRandomElement();

            bool takeMale = new List<bool> { true, false }.GetRandomElement();
            
            if (characterNames.FullNames.Any(x => !x.IsEmpty))
            {
                return characterNames.FullNames.SelectMany(x => x.Values).GetRandomElement();
            }

            if (takeMale && characterNames.MaleFullNames.Any(x => !x.IsEmpty))
            {
                return characterNames.MaleFullNames.SelectMany(x => x.Values).GetRandomElement();
            }

            if (!takeMale && characterNames.FemaleFullNames.Any(x => !x.IsEmpty))
            {
                return characterNames.FemaleFullNames.SelectMany(x => x.Values).GetRandomElement();
            }

            if (characterNames.SecondNames.Any(x => !x.IsEmpty))
            {
                if (characterNames.FirstNames.Any(x => !x.IsEmpty))
                {
                    return
                        characterNames.FirstNames.SelectMany(x => x.Values).GetRandomElement() + " " +
                        characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement();
                }

                if (takeMale && characterNames.MaleFirstNames.Any(x => !x.IsEmpty))
                {
                    return
                        characterNames.MaleFirstNames.SelectMany(x => x.Values).GetRandomElement() + " " +
                        characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement();
                }

                if (!takeMale && characterNames.FemaleFirstNames.Any(x => !x.IsEmpty))
                {
                    return
                        characterNames.FemaleFirstNames.SelectMany(x => x.Values).GetRandomElement() + " " +
                        characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement();
                }
            }

            return string.Empty;
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
    }
}
