using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public interface IFleetNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
