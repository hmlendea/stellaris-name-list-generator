using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service.NamesBuilders
{
    /// <summary>
    /// Builds the random name samples placed in the comment headers of a generated name list.
    /// </summary>
    internal static class NameSampleBuilder
    {
        private static string Separator => ", ";

        /// <summary>
        /// Builds a sample composed of two random names drawn from the specified builder.
        /// </summary>
        /// <param name="namesBuilder">The builder used to draw the sample names.</param>
        /// <param name="nameList">The name list to draw the names from.</param>
        /// <returns>The formatted sample.</returns>
        internal static string Build(INamesBuilder namesBuilder, NameList nameList)
        {
            string firstName = namesBuilder.GetRandomName(nameList);
            string secondName = namesBuilder.GetRandomName(nameList);

            return $"{firstName}{Separator}{secondName}";
        }
    }
}
