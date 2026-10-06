using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface IPlanetNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
