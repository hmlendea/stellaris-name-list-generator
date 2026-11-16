using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    /// <summary>
    /// Builds the complete Stellaris name list file content for a name list.
    /// </summary>
    public interface IFileContentBuilder
    {
        /// <summary>
        /// Builds the file content for the specified name list.
        /// </summary>
        /// <param name="nameList">The name list to format.</param>
        /// <returns>The generated file content.</returns>
        string BuildContent(NameList nameList);
    }
}
