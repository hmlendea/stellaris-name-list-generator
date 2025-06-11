using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using StellarisNameListGenerator.Service;

namespace StellarisNameListGenerator.Models
{
    public sealed class NameGroup
    {
        // TODO: Move this from here
        // TODO: Use dependency injection
        static readonly IFileDownloader fileDownloader = new FileDownloader(new CacheManager());

        public string Name { get; set; }

        public string Url { get; set; }

        [XmlIgnore]
        public List<string> Values
        {
            get
            {
                List<string> values = ExplicitValues.ToList();

                if (UrlValues.Count != 0)
                {
                    values.AddRange(UrlValues);
                }

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
                if (string.IsNullOrWhiteSpace(downloadedUrl) || !Url.Equals(downloadedUrl))
                {
                    urlValues = DownloadNames(Url);
                    downloadedUrl = Url;
                }

                return urlValues;
            }
        }

        [XmlIgnore]
        public bool IsEmpty => Values?.Count == 0;

        List<string> urlValues;
        string downloadedUrl;

        List<string> DownloadNames(string url)
        {
            if (!string.IsNullOrWhiteSpace(Url))
            {
                try
                {
                    // TODO: Use async
                    string namesString = fileDownloader.TryDownloadStringAsync(url).Result;
                    return namesString.Replace("\r" , "").Split('\n').ToList();
                }
                catch
                {
                    Console.WriteLine($"Failed to retrieve the name list from {url}");
                }
            }

            return [];
        }
    }
}
