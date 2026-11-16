using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using StellarisNameListGenerator.Service;

namespace StellarisNameListGenerator.Models
{
    /// <summary>
    /// Represents a named group of values supplied either inline or by a remote URL.
    /// </summary>
    public sealed class NameGroup
    {
        private static readonly IFileDownloader FileDownloader = new FileDownloader(new CacheManager());

        private List<string> remoteValues;
        private string downloadedUrl;

        public string Name { get; set; }

        public string Url { get; set; }

        [XmlIgnore]
        public List<string> Values
        {
            get
            {
                List<string> values = [];

                if (ExplicitValues is not null)
                {
                    values.AddRange(ExplicitValues);
                }

                values.AddRange(GetRemoteValues());

                return values;
            }
        }

        [XmlArray("Values")]
        public List<string> ExplicitValues { get; set; }

        [XmlIgnore]
        public List<string> UrlValues
        {
            get
            {
                if (!string.Equals(Url, downloadedUrl) || remoteValues is null)
                {
                    remoteValues = DownloadNames(Url);
                    downloadedUrl = Url;
                }

                return remoteValues;
            }
        }

        [XmlIgnore]
        public bool IsEmpty => Values.Count == 0;

        private List<string> GetRemoteValues()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                return [];
            }

            return UrlValues;
        }

        private static List<string> DownloadNames(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return [];
            }

            try
            {
                string namesString = FileDownloader.TryDownloadStringAsync(url).Result;

                return [.. namesString.Replace("\r", string.Empty).Split('\n')];
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to retrieve the name list from {url}: {exception.Message}");
            }

            return [];
        }
    }
}
