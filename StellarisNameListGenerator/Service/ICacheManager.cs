namespace StellarisNameListGenerator.Service
{
    /// <summary>
    /// Caches remotely retrieved name list content on the local filesystem.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Stores the specified content in the cache entry for the given URL.
        /// </summary>
        /// <param name="url">The URL the content was retrieved from.</param>
        /// <param name="content">The content to cache.</param>
        void StoreNameList(string url, string content);

        /// <summary>
        /// Retrieves the cached content for the given URL.
        /// </summary>
        /// <param name="url">The URL to retrieve the cached content for.</param>
        /// <returns>The cached content, or null when no valid entry exists.</returns>
        string GetNameList(string url);
    }
}
