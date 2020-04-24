using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public interface IShipNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
