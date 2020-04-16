using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public interface INamesBuilder
    {
        string Build(NameList nameList);
    }
}
