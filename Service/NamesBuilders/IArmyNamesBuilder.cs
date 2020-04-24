using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface IArmyNamesBuilder
    {
        string Build(NameList nameList);
    }
}
