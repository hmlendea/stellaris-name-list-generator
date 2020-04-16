using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public interface IPlanetNamesBuilder
    {
        string Build(NameList nameList);
    }
}
