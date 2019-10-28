using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public interface IFileContentBuilder
    {
        string BuildContent(NameList nameList);
    }
}
