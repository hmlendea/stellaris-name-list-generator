using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    /// <summary>
    /// Provides a random name sample drawn from the categories supported by a names builder.
    /// </summary>
    public interface INamesBuilder
    {
        /// <summary>
        /// Retrieves a random name from the categories supported by this builder.
        /// </summary>
        /// <param name="nameList">The name list to draw the name from.</param>
        /// <returns>A random name, or an empty string when no name is available.</returns>
        string GetRandomName(NameList nameList);
    }
}
