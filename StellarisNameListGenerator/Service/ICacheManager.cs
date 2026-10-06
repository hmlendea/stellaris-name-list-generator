namespace StellarisNameListGenerator.Service
{
    public interface ICacheManager
    {
        void StoreNameList(string url, string content);

        string GetNameList(string url);
    }
}
