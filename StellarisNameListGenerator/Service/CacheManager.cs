using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StellarisNameListGenerator.Service
{
    public sealed class CacheManager : ICacheManager
    {
        private static string CacheDirectoryPath => ".cache";
        private static TimeSpan CacheValidityPeriod => TimeSpan.FromHours(8);

        private static readonly HashSet<char> AllowedFileNameChars =
            [.. "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.()".ToCharArray()];

        public CacheManager()
            => PrepareFilesystem();

        public void StoreNameList(string url, string content)
            => File.WriteAllText(GetCacheFilePath(url), content);

        public string GetNameList(string url)
        {
            string filePath = GetCacheFilePath(url);

            if (!File.Exists(filePath))
            {
                return null;
            }

            if (DateTime.Now.Subtract(File.GetCreationTime(filePath)) >= CacheValidityPeriod)
            {
                return null;
            }

            return File.ReadAllText(filePath);
        }

        private static void PrepareFilesystem()
        {
            if (!Directory.Exists(CacheDirectoryPath))
            {
                Directory.CreateDirectory(CacheDirectoryPath);
            }
        }

        private static string GetCacheFilePath(string url)
            => Path.Combine(CacheDirectoryPath, GetFileNameFromUrl(url));

        private static string GetFileNameFromUrl(string url)
        {
            StringBuilder fileName = new();

            foreach (char character in url)
            {
                if (AllowedFileNameChars.Contains(character))
                {
                    fileName.Append(character);
                }
            }

            return fileName.ToString();
        }
    }
}
