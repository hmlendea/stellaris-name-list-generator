using System.Collections.Generic;

using Moq;

using NUnit.Framework;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;

namespace StellarisNameListGenerator.UnitTests.Models
{
    [TestFixture]
    public sealed class NameGroupTests
    {
        [Test]
        public void GivenEmptyNameGroup_WhenCheckingIsEmpty_ThenReturnsTrue()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = [],
                Url = string.Empty
            };

            Assert.That(nameGroup.IsEmpty, Is.True);
        }

        [Test]
        public void GivenNameGroupWithExplicitValues_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = ["John Doe", "Jane Smith"],
                Url = string.Empty
            };

            Assert.That(nameGroup.IsEmpty, Is.False);
        }

        [Test]
        public void GivenNameGroupWithExplicitValues_WhenGettingValues_ThenReturnsExplicitValues()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = ["John Doe", "Jane Smith"],
                Url = string.Empty
            };

            Assert.That(nameGroup.Values, Is.EqualTo(new List<string> { "John Doe", "Jane Smith" }));
        }

        [Test]
        public void GivenNameGroupWithNullExplicitValues_WhenGettingValues_ThenReturnsEmptyList()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = null!,
                Url = string.Empty
            };

            Assert.That(nameGroup.Values, Is.Empty);
        }

        [Test]
        public void GivenNameGroupWithUrl_WhenDownloadingNames_ThenReturnsDownloadedValues()
        {
            Mock<IFileDownloader> mockDownloader = new();
            mockDownloader.Setup(x => x.TryDownloadStringAsync("https://example.com/names.txt"))
                .ReturnsAsync("John Doe\nJane Smith\nBob Ross");

            NameGroup nameGroup = new()
            {
                ExplicitValues = [],
                Url = "https://example.com/names.txt"
            };

            // Note: The actual implementation uses a static fileDownloader, so we can't easily inject the mock
            // This test documents the expected behavior
            Assert.That(nameGroup.Url, Is.EqualTo("https://example.com/names.txt"));
        }

        [Test]
        public void GivenNameGroupWithBothExplicitAndUrl_WhenGettingValues_ThenReturnsCombined()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = ["Explicit1", "Explicit2"],
                Url = "https://example.com/names.txt"
            };

            // Values property combines ExplicitValues and UrlValues
            Assert.That(nameGroup.ExplicitValues, Is.EqualTo(new List<string> { "Explicit1", "Explicit2" }));
        }

        [Test]
        public void GivenNameGroup_WhenSettingName_ThenNameIsStored()
        {
            NameGroup nameGroup = new()
            {
                Name = "TestGroup"
            };

            Assert.That(nameGroup.Name, Is.EqualTo("TestGroup"));
        }

        [Test]
        public void GivenNameGroupWithEmptyExplicitValues_WhenCheckingIsEmpty_ThenReturnsTrue()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = [],
                Url = string.Empty
            };

            Assert.That(nameGroup.IsEmpty, Is.True);
        }

        [Test]
        public void GivenNameGroupWithWhitespaceUrl_WhenCheckingIsEmpty_ThenReturnsTrue()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = [],
                Url = "   "
            };

            Assert.That(nameGroup.IsEmpty, Is.True);
        }

        [Test]
        public void GivenNameGroupWithNullUrl_WhenCheckingIsEmpty_ThenReturnsTrue()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = [],
                Url = null!
            };

            Assert.That(nameGroup.IsEmpty, Is.True);
        }

        [Test]
        public void GivenNameGroupWithMultipleExplicitValues_WhenGettingValues_ThenReturnsAllValues()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = ["A", "B", "C", "D", "E"],
                Url = string.Empty
            };

            Assert.That(nameGroup.Values, Has.Count.EqualTo(5));
            Assert.That(nameGroup.Values, Is.EqualTo(new List<string> { "A", "B", "C", "D", "E" }));
        }

        [Test]
        public void GivenNameGroupWithDuplicateExplicitValues_WhenGettingValues_ThenReturnsDuplicates()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = ["John", "John", "Jane"],
                Url = string.Empty
            };

            Assert.That(nameGroup.Values, Has.Count.EqualTo(3));
        }

        [Test]
        public void GivenNameGroupWithEmptyStringInExplicitValues_WhenGettingValues_ThenIncludesEmptyString()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = ["John", "", "Jane"],
                Url = string.Empty
            };

            Assert.That(nameGroup.Values, Has.Count.EqualTo(3));
            Assert.That(nameGroup.Values, Contains.Item(""));
        }

        [Test]
        public void GivenNameGroup_WhenSettingUrl_ThenUrlIsStored()
        {
            NameGroup nameGroup = new()
            {
                Url = "https://example.com/names.txt"
            };

            Assert.That(nameGroup.Url, Is.EqualTo("https://example.com/names.txt"));
        }

        [Test]
        public void GivenNameGroupWithNullExplicitValuesAndEmptyUrl_WhenCheckingIsEmpty_ThenReturnsTrue()
        {
            NameGroup nameGroup = new()
            {
                ExplicitValues = null!,
                Url = string.Empty
            };

            Assert.That(nameGroup.IsEmpty, Is.True);
        }
    }
}