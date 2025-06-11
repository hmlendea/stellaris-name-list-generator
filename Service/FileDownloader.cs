using System.Net.Http;
using System.Threading.Tasks;

namespace StellarisNameListGenerator.Service
{
    public sealed class FileDownloader(ICacheManager cache) : IFileDownloader
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<string> TryDownloadStringAsync(string url)
        {
            string content = cache.GetNameList(url);

            if (content is not null)
            {
                return content;
            }

            content = await GetAsync(url);

            cache.StoreNameList(url, content);
            return content;
        }

        private async Task<string> GetAsync(string url)
            => await (await httpClient.GetAsync(url)).Content.ReadAsStringAsync();
    }
}
