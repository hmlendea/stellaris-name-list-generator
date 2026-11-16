using System.Net.Http;
using System.Threading.Tasks;

using NuciWeb.HTTP;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileDownloader(ICacheManager cache) : IFileDownloader
    {
        private static readonly HttpClient HttpClient = HttpClientCreator.Create();

        public async Task<string> TryDownloadStringAsync(string url)
        {
            string cachedContent = cache.GetNameList(url);

            if (cachedContent is not null)
            {
                return cachedContent;
            }

            string content = await GetContentAsync(url);

            cache.StoreNameList(url, content);

            return content;
        }

        private static async Task<string> GetContentAsync(string url)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
