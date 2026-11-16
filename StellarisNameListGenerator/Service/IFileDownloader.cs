using System.Threading.Tasks;

namespace StellarisNameListGenerator.Service
{
    /// <summary>
    /// Retrieves the content of remote name list sources.
    /// </summary>
    public interface IFileDownloader
    {
        /// <summary>
        /// Attempts to retrieve the content of the specified URL, preferring a valid cached copy.
        /// </summary>
        /// <param name="url">The URL to retrieve the content from.</param>
        /// <returns>The retrieved content.</returns>
        Task<string> TryDownloadStringAsync(string url);
    }
}
