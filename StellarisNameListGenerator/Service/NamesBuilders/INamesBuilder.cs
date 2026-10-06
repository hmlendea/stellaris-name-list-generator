using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    public interface INamesBuilder
    {
        string Build(NameList nameList);
    }
}
