using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    /// <summary>
    /// Builds the Stellaris army name section.
    /// </summary>
    public interface IArmyNamesBuilder
    {
        /// <summary>
        /// Builds the army name section for the specified name list.
        /// </summary>
        /// <param name="nameList">The name list to build the section from.</param>
        /// <returns>The generated section content.</returns>
        string Build(NameList nameList);
    }
}
