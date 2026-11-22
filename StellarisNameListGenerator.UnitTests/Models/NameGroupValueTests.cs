using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.UnitTests.Models
{
    /// <summary>
    /// Exercises <see cref="NameGroup"/> value resolution, caching, emptiness, and URL handling without
    /// performing network access.
    /// </summary>
    [TestFixture]
    public sealed class NameGroupValueTests
    {
        [Test]
        public void GivenExplicitValues_WhenReadingValues_ThenTheyAreReturnedInOrder()
            => Assert.That(
                new NameGroup { ExplicitValues = ["Alpha", "Beta", "Gamma"] }.Values,
                Is.EqualTo(new List<string> { "Alpha", "Beta", "Gamma" }));

        [Test]
        public void GivenNoExplicitValuesAndNoUrl_WhenReadingValues_ThenAnEmptyListIsReturned()
            => Assert.That(new NameGroup { ExplicitValues = null!, Url = null! }.Values, Is.Empty);

        [Test]
        public void GivenAnEmptyExplicitList_WhenReadingValues_ThenAnEmptyListIsReturned()
            => Assert.That(new NameGroup { ExplicitValues = [], Url = string.Empty }.Values, Is.Empty);

        [Test]
        public void GivenAWhitespaceUrl_WhenReadingValues_ThenOnlyExplicitValuesAreReturned()
            => Assert.That(
                new NameGroup { ExplicitValues = ["Alpha"], Url = "   " }.Values,
                Is.EqualTo(new List<string> { "Alpha" }));

        [Test]
        public void GivenExplicitValues_WhenReadingValuesTwice_ThenTheSameInstanceIsReturned()
        {
            NameGroup nameGroup = new() { ExplicitValues = ["Alpha"] };

            Assert.That(nameGroup.Values, Is.SameAs(nameGroup.Values));
        }

        [Test]
        public void GivenExplicitValues_WhenReplacingTheList_ThenTheCacheIsInvalidated()
        {
            NameGroup nameGroup = new() { ExplicitValues = ["Alpha"] };
            IReadOnlyList<string> first = nameGroup.Values;

            nameGroup.ExplicitValues = ["Beta"];

            Assert.That(nameGroup.Values, Is.EqualTo(new List<string> { "Beta" }));
            Assert.That(nameGroup.Values, Is.Not.SameAs(first));
        }

        [Test]
        public void GivenCachedExplicitValues_WhenMutatingTheSameList_ThenTheCacheIsServedUntilTheReferenceChanges()
        {
            NameGroup nameGroup = new() { ExplicitValues = ["Alpha"] };
            IReadOnlyList<string> first = nameGroup.Values;

            nameGroup.ExplicitValues.Add("Beta");

            Assert.That(nameGroup.Values, Is.SameAs(first), "the cache is keyed on the list reference, not its contents");

            nameGroup.ExplicitValues = ["Alpha", "Beta"];

            Assert.That(nameGroup.Values, Is.EqualTo(new List<string> { "Alpha", "Beta" }));
        }

        [Test]
        public void GivenNoValues_WhenCheckingIsEmpty_ThenTrueIsReturned()
            => Assert.That(new NameGroup { ExplicitValues = [], Url = string.Empty }.IsEmpty, Is.True);

        [Test]
        public void GivenAValue_WhenCheckingIsEmpty_ThenFalseIsReturned()
            => Assert.That(new NameGroup { ExplicitValues = ["Alpha"] }.IsEmpty, Is.False);

        [Test]
        public void GivenAnUnreachableUrl_WhenReadingUrlValues_ThenAnEmptyListIsReturned()
            => Assert.That(new NameGroup { Url = "http://127.0.0.1:9/names.txt" }.UrlValues, Is.Empty);

        [Test]
        public void GivenAnInvalidUrl_WhenReadingUrlValues_ThenAnEmptyListIsReturned()
            => Assert.That(new NameGroup { Url = "not-a-url" }.UrlValues, Is.Empty);

        [Test]
        public void GivenAWhitespaceUrl_WhenReadingUrlValues_ThenAnEmptyListIsReturned()
            => Assert.That(new NameGroup { Url = "  " }.UrlValues, Is.Empty);

        [Test]
        public void GivenAUrlWithExplicitValues_WhenReadingValues_ThenOnlyTheExplicitValuesSurviveAnUnreachableUrl()
            => Assert.That(
                new NameGroup { ExplicitValues = ["Alpha"], Url = "http://127.0.0.1:9/names.txt" }.Values,
                Is.EqualTo(new List<string> { "Alpha" }));

        [TestCase("http://127.0.0.1:9/a.txt")]
        [TestCase("not-a-url")]
        [TestCase("ftp://127.0.0.1/b.txt")]
        public void GivenAnUnreachableUrl_WhenCheckingIsEmpty_ThenExplicitValuesDetermineEmptiness(string url)
            => Assert.That(new NameGroup { ExplicitValues = ["Alpha"], Url = url }.IsEmpty, Is.False);

        [Test]
        public void GivenANameGroupWithAName_WhenReadingTheName_ThenItIsReturnedUnchanged()
        {
            const string name = "Denonyms";
            NameGroup nameGroup = new() { Name = name };

            Assert.That(nameGroup.Name, Is.EqualTo(name));
        }

        [Test]
        public void GivenANameGroupWithAUrl_WhenReadingTheUrl_ThenItIsReturnedUnchanged()
        {
            const string url = "https://example.com/names.txt";
            NameGroup nameGroup = new() { Url = url };

            Assert.That(nameGroup.Url, Is.EqualTo(url));
        }

        [Test]
        public void GivenExplicitValues_WhenReadingValuesManyTimes_ThenTheCountIsStable()
        {
            NameGroup nameGroup = new() { ExplicitValues = ["Alpha", "Beta", "Gamma"] };

            for (int index = 0; index < 10; index++)
            {
                Assert.That(nameGroup.Values, Has.Count.EqualTo(3));
            }
        }

        [Test]
        public void GivenEmptyExplicitValues_WhenReadingValues_ThenAnEmptyListIsReturnedEvenAfterRepeatedReads()
        {
            NameGroup nameGroup = new() { ExplicitValues = [] };

            Assert.That(nameGroup.Values, Is.Empty);
            Assert.That(nameGroup.Values, Is.Empty);
        }

        [Test]
        public void GivenAUrlAndNoExplicitValues_WhenReadingValues_ThenTheResultIsEmptyForAnUnreachableUrl()
        {
            NameGroup nameGroup = new() { Url = "http://127.0.0.1:9/names.txt" };

            Assert.That(nameGroup.Values, Is.Empty);
            Assert.That(nameGroup.IsEmpty, Is.True);
        }
    }
}
