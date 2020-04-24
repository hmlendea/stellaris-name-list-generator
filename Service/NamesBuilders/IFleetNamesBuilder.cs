using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface IFleetNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
