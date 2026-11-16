using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    /// <summary>
    /// Generates a Stellaris name list file from the configured repository of name lists.
    /// </summary>
    public interface INameListGenerator
    {
        /// <summary>
        /// Generates the name list file at the specified path.
        /// </summary>
        /// <param name="filePath">The path of the file to generate.</param>
        /// <param name="name">The display name of the generated name list.</param>
        /// <param name="isLocked">Whether the generated name list is locked for randomisation.</param>
        void Generate(string filePath, string name, bool isLocked);

        /// <summary>
        /// Retrieves the single merged name list from the configured repository.
        /// </summary>
        /// <returns>The merged name list.</returns>
        NameList GetMergedNameList();
    }
}
