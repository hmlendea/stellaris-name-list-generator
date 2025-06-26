using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StellarisNameListGenerator.Service
{
    public sealed class CacheManager : ICacheManager
    {
        const string CacheDirectoryPath = ".cache";

        public CacheManager()
            => PrepareFilesystem();

        public void StoreNameList(string url, string content)
        {
            string fileName = GetFileNameFromUrl(url);
            string filePath = Path.Combine(CacheDirectoryPath, fileName);

            File.WriteAllText(filePath, content);
        }

        public string GetNameList(string url)
        {
            string fileName = GetFileNameFromUrl(url);
            string filePath = Path.Combine(CacheDirectoryPath, fileName);

            if (File.Exists(filePath))
            {
                if (DateTime.Now.Subtract(File.GetCreationTime(filePath)).TotalHours >= 8)
                {
                    return null;
                }

                return File.ReadAllText(filePath);
            }

            return null;
        }

        static void PrepareFilesystem()
        {
            if (!Directory.Exists(CacheDirectoryPath))
            {
                Directory.CreateDirectory(CacheDirectoryPath);
            }
        }

        static readonly HashSet<char> AllowedFileNameChars = [.. "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.()".ToCharArray()];

        static string GetFileNameFromUrl(string url)
        {
            StringBuilder sb = new();

            foreach (char c in url)
            {
                if (AllowedFileNameChars.Contains(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
