using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface ICharacterNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
