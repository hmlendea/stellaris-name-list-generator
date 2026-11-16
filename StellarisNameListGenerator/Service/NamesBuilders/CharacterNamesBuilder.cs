using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuciExtensions;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public sealed class CharacterNamesBuilder : NamesBuilder, ICharacterNamesBuilder
    {
        private static bool[] Genders => [true, false];

        public string Build(NameList nameList)
        {
            List<CharacterNames> characterNameLists = nameList.Characters
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .OrderByDescending(x => x.Weight)
                .ToList();

            if (characterNameLists.All(x => x.IsEmpty))
            {
                return string.Empty;
            }

            StringBuilder content = new();
            content.Append($"{GetIndentation(1)}character_names = {{{Environment.NewLine}");

            foreach (CharacterNames characterNames in characterNameLists)
            {
                content.Append(BuildCharacterNamesArray(characterNames));
            }

            content.Append($"{GetIndentation(1)}}}{Environment.NewLine}");

            return content.ToString();
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

            bool takeMale = Genders.GetRandomElement();

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

            if (!characterNames.SecondNames.Any(x => !x.IsEmpty))
            {
                return string.Empty;
            }

            string secondName = characterNames.SecondNames.SelectMany(x => x.Values).GetRandomElement();

            if (characterNames.FirstNames.Any(x => !x.IsEmpty))
            {
                return $"{characterNames.FirstNames.SelectMany(x => x.Values).GetRandomElement()} {secondName}";
            }

            if (takeMale && characterNames.MaleFirstNames.Any(x => !x.IsEmpty))
            {
                return $"{characterNames.MaleFirstNames.SelectMany(x => x.Values).GetRandomElement()} {secondName}";
            }

            if (!takeMale && characterNames.FemaleFirstNames.Any(x => !x.IsEmpty))
            {
                return $"{characterNames.FemaleFirstNames.SelectMany(x => x.Values).GetRandomElement()} {secondName}";
            }

            return string.Empty;
        }

        private string BuildCharacterNamesArray(CharacterNames characterNames)
        {
            string characterNamesId = characterNames.Id;

            if (string.IsNullOrWhiteSpace(characterNamesId))
            {
                characterNamesId = $"names{DateTime.Now.Ticks}";
            }

            StringBuilder innerContent = new();
            innerContent.Append(BuildNameArray(characterNames.FullNames, "full_names", 3));
            innerContent.Append(BuildNameArray(characterNames.FirstNames, "first_names", 3));
            innerContent.Append(BuildNameArray(characterNames.RoyalFirstNames, "regnal_first_names", 3));
            innerContent.Append(BuildNameArray(characterNames.MaleFullNames, "full_names_male", 3));
            innerContent.Append(BuildNameArray(characterNames.MaleFirstNames, "first_names_male", 3));
            innerContent.Append(BuildNameArray(characterNames.MaleRoyalFirstNames, "regnal_first_names_male", 3));
            innerContent.Append(BuildNameArray(characterNames.FemaleFullNames, "full_names_female", 3));
            innerContent.Append(BuildNameArray(characterNames.FemaleFirstNames, "first_names_female", 3));
            innerContent.Append(BuildNameArray(characterNames.FemaleRoyalFirstNames, "regnal_first_names_female", 3));
            innerContent.Append(BuildNameArray(characterNames.SecondNames, "second_names", 3));
            innerContent.Append(BuildNameArray(characterNames.RoyalSecondNames, "regnal_second_names", 3));

            if (innerContent.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder content = new();
            content.Append($"{GetIndentation(2)}{characterNamesId} = {{{Environment.NewLine}");
            content.Append($"{GetIndentation(3)}weight = {characterNames.Weight}{Environment.NewLine}");
            content.Append(innerContent);
            content.Append($"{GetIndentation(2)}}}{Environment.NewLine}");

            return content.ToString();
        }
    }
}
