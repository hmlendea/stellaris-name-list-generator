using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    /// <summary>
    /// Builds the Stellaris ship and station name sections.
    /// </summary>
    public interface IShipNamesBuilder : INamesBuilder
    {
        /// <summary>
        /// Builds the ship and station name sections for the specified name list.
        /// </summary>
        /// <param name="nameList">The name list to build the sections from.</param>
        /// <returns>The generated section content.</returns>
        string Build(NameList nameList);
    }
}
