using System.Net.Http;
using System.Threading.Tasks;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileDownloader : IFileDownloader
    {
        readonly ICacheManager cache;

        readonly HttpClient httpClient;

        public FileDownloader(ICacheManager cache)
        {
            this.cache = cache;

            httpClient = new HttpClient();
        }

        public async Task<string> TryDownloadStringAsync(string url)
        {
            string content = cache.GetNameList(url);

            if (!(content is null))
            {
                return content;
            }

            content = await GetAsync(url);

            cache.StoreNameList(url, content);
            return content;
        }

        async Task<string> GetAsync(string url)
        {
            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                using (HttpContent content = response.Content)
                {
                    return await content.ReadAsStringAsync();
                }
            }
        }
    }
}
