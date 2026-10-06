using System.Threading.Tasks;

namespace StellarisNameListGenerator.Service
{
    public interface IFileDownloader
    {
        Task<string> TryDownloadStringAsync(string url);
    }
}
