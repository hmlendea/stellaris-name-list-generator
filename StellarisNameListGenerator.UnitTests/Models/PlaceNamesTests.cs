using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.UnitTests.Models
{
    [TestFixture]
    public sealed class PlaceNamesTests
    {
        [Test]
        public void GivenNewPlaceNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            PlaceNames placeNames = new();

            Assert.That(placeNames.Countries, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Regions, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Cities, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Mountains, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Forests, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Deserts, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Rivers, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Lakes, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Seas, Is.Not.Null.And.Empty);
            Assert.That(placeNames.Airports, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenPlaceNamesWithValues_WhenAccessingWaterBodies_ThenReturnsCombinedWaterCollections()
        {
            PlaceNames placeNames = new()
            {
                Rivers = [new NameGroup { ExplicitValues = ["River1"] }],
                Lakes = [new NameGroup { ExplicitValues = ["Lake1"] }],
                Seas = [new NameGroup { ExplicitValues = ["Sea1"] }]
            };

            IEnumerable<NameGroup> waterBodies = placeNames.WaterBodies;

            Assert.That(waterBodies.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenPlaceNamesWithValues_WhenAccessingGeographicalPlaces_ThenReturnsCombinedGeographicalCollections()
        {
            PlaceNames placeNames = new()
            {
                Mountains = [new NameGroup { ExplicitValues = ["Mountain1"] }],
                Forests = [new NameGroup { ExplicitValues = ["Forest1"] }],
                Deserts = [new NameGroup { ExplicitValues = ["Desert1"] }],
                Rivers = [new NameGroup { ExplicitValues = ["River1"] }],
                Lakes = [new NameGroup { ExplicitValues = ["Lake1"] }],
                Seas = [new NameGroup { ExplicitValues = ["Sea1"] }]
            };

            IEnumerable<NameGroup> geographicalPlaces = placeNames.GeographicalPlaces;

            Assert.That(geographicalPlaces.Count(), Is.EqualTo(6));
        }

        [Test]
        public void GivenPlaceNames_WhenAddingValues_ThenValuesAreStored()
        {
            PlaceNames placeNames = new()
            {
                Countries = [new NameGroup { Name = "Countries", ExplicitValues = ["USA", "Canada"] }],
                Cities = [new NameGroup { Name = "Cities", ExplicitValues = ["New York", "Toronto"] }]
            };

            Assert.That(placeNames.Countries, Has.Count.EqualTo(1));
            Assert.That(placeNames.Countries[0].ExplicitValues, Has.Count.EqualTo(2));
            Assert.That(placeNames.Cities[0].ExplicitValues, Contains.Item("New York"));
        }
    }
}