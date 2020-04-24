using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface IShipClassNamesBuilder
    {
        string Build(NameList nameList);
    }
}
