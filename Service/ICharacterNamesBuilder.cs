using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public interface ICharacterNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
