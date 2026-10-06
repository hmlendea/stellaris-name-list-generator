using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface IShipNamesBuilder
    {
        string Build(NameList nameList);

        string GetRandomName(NameList nameList);
    }
}
