using System;
using System.IO;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the remote-name retrieval boundary. The generator resolves URLs through a static
    /// downloader, so these tests only assert the observable degradation contract: an unreachable
    /// source must not fail generation or discard the explicit values.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class RemoteNameResolutionTests
    {
        private const string OutputFileName = "remote.txt";
        private const string DisplayName = "Remote List";

        private static string UnresolvableUrl => "http://127.0.0.1:9/names.txt";

        private static string InvalidUrl => "not-a-valid-url";

        [TestCaseSource(nameof(UnusableUrls))]
        public void GivenUnreachableUrl_WhenGenerating_ThenGenerationSucceeds(string url)
        {
            using GeneratorEnvironment environment = new(nameof(RemoteNameResolutionTests));
            environment.WriteInput(NameListFixtures.UrlDocument(url));

            environment.Generate(OutputFileName, DisplayName);

            Assert.That(environment.ReadText(OutputFileName), Does.Contain(NameListFixtures.CountryTwo));
        }

        [TestCaseSource(nameof(UnusableUrls))]
        public void GivenUnreachableUrl_WhenGenerating_ThenExplicitValuesAreRetained(string url)
        {
            using GeneratorEnvironment environment = new(nameof(RemoteNameResolutionTests));
            environment.WriteInput(NameListFixtures.UrlDocument(url));

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenUnreachableUrl_WhenGenerating_ThenNoCacheEntryIsStored()
        {
            using GeneratorEnvironment environment = new(nameof(RemoteNameResolutionTests));
            environment.WriteInput(NameListFixtures.UrlDocument(UnresolvableUrl));

            environment.Generate(OutputFileName, DisplayName);

            string cachePath = environment.GetPath(".cache");

            Assert.That(Directory.Exists(cachePath), Is.False);
        }

        [Test]
        public void GivenDocumentWithoutUrls_WhenGenerating_ThenNoCacheDirectoryIsCreated()
        {
            using GeneratorEnvironment environment = new(nameof(RemoteNameResolutionTests));
            environment.WriteInput(NameListFixtures.MinimalDocument);

            environment.Generate(OutputFileName, DisplayName);

            Assert.That(Directory.Exists(environment.GetPath(".cache")), Is.False);
        }

        [Test]
        public void GivenWhitespaceUrl_WhenGenerating_ThenExplicitValuesAreUsed()
        {
            using GeneratorEnvironment environment = new(nameof(RemoteNameResolutionTests));
            environment.WriteInput(NameListFixtures.UrlDocument("   "));

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
            Assert.That(Directory.Exists(environment.GetPath(".cache")), Is.False);
        }

        [Test]
        public void GivenAPopulatedCacheEntry_WhenGenerating_ThenCachedNamesAreMergedWithoutNetworkAccess()
        {
            using GeneratorEnvironment environment = new($"{nameof(RemoteNameResolutionTests)}_cached");
            environment.WriteInput(NameListFixtures.UrlDocument(CachedUrl));

            Directory.CreateDirectory(environment.GetPath(".cache"));
            File.WriteAllText(environment.GetPath(Path.Combine(".cache", CachedUrl)), $"{CachedNameOne}\n{CachedNameTwo}");

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", CachedNameOne), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "generic", CachedNameTwo), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
        }

        [Test]
        public void GivenNoCacheEntry_WhenGeneratingFromAnUnreachableUrl_ThenOnlyExplicitValuesRemain()
        {
            using GeneratorEnvironment environment = new($"{nameof(RemoteNameResolutionTests)}_uncached");
            environment.WriteInput(NameListFixtures.UrlDocument(UnresolvableUrl));

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "generic", NameListFixtures.CountryTwo), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "generic", CachedNameOne), Is.False);
        }

        private static string CachedUrl => "dummy-url.com";

        private static string CachedNameOne => "Alpha";

        private static string CachedNameTwo => "Zenith";

        private static string[] UnusableUrls => [UnresolvableUrl, InvalidUrl];
    }
}
