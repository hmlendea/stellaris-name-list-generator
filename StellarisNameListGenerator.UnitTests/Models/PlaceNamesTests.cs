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

        [Test]
        public void GivenPlaceNamesWithOnlyRivers_WhenAccessingWaterBodies_ThenReturnsOnlyRivers()
        {
            PlaceNames placeNames = new()
            {
                Rivers = [new NameGroup { ExplicitValues = ["River1", "River2"] }]
            };

            IEnumerable<NameGroup> waterBodies = placeNames.WaterBodies;

            Assert.That(waterBodies.Count(), Is.EqualTo(1));
            Assert.That(waterBodies.First().ExplicitValues, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenPlaceNamesWithOnlyLakes_WhenAccessingWaterBodies_ThenReturnsOnlyLakes()
        {
            PlaceNames placeNames = new()
            {
                Lakes = [new NameGroup { ExplicitValues = ["Lake1"] }]
            };

            IEnumerable<NameGroup> waterBodies = placeNames.WaterBodies;

            Assert.That(waterBodies.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenPlaceNamesWithOnlySeas_WhenAccessingWaterBodies_ThenReturnsOnlySeas()
        {
            PlaceNames placeNames = new()
            {
                Seas = [new NameGroup { ExplicitValues = ["Sea1", "Sea2", "Sea3"] }]
            };

            IEnumerable<NameGroup> waterBodies = placeNames.WaterBodies;

            Assert.That(waterBodies.Count(), Is.EqualTo(1));
            Assert.That(waterBodies.First().ExplicitValues, Has.Count.EqualTo(3));
        }

        [Test]
        public void GivenPlaceNamesWithMountainsAndForests_WhenAccessingGeographicalPlaces_ThenReturnsMountainsAndForests()
        {
            PlaceNames placeNames = new()
            {
                Mountains = [new NameGroup { ExplicitValues = ["Mountain1"] }],
                Forests = [new NameGroup { ExplicitValues = ["Forest1"] }]
            };

            IEnumerable<NameGroup> geographicalPlaces = placeNames.GeographicalPlaces;

            Assert.That(geographicalPlaces.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenPlaceNamesWithDeserts_WhenAccessingGeographicalPlaces_ThenIncludesDeserts()
        {
            PlaceNames placeNames = new()
            {
                Deserts = [new NameGroup { ExplicitValues = ["Desert1"] }]
            };

            IEnumerable<NameGroup> geographicalPlaces = placeNames.GeographicalPlaces;

            Assert.That(geographicalPlaces.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenPlaceNamesWithAllWaterBodies_WhenAccessingGeographicalPlaces_ThenIncludesWaterBodies()
        {
            PlaceNames placeNames = new()
            {
                Mountains = [new NameGroup { ExplicitValues = ["Mountain1"] }],
                Rivers = [new NameGroup { ExplicitValues = ["River1"] }],
                Lakes = [new NameGroup { ExplicitValues = ["Lake1"] }],
                Seas = [new NameGroup { ExplicitValues = ["Sea1"] }]
            };

            IEnumerable<NameGroup> geographicalPlaces = placeNames.GeographicalPlaces;

            Assert.That(geographicalPlaces.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GivenPlaceNamesWithAirports_WhenAccessingAirports_ThenReturnsAirports()
        {
            PlaceNames placeNames = new()
            {
                Airports = [new NameGroup { ExplicitValues = ["Airport1", "Airport2"] }]
            };

            Assert.That(placeNames.Airports, Has.Count.EqualTo(1));
            Assert.That(placeNames.Airports[0].ExplicitValues, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenPlaceNamesWithMultipleNameGroupsPerCategory_WhenAccessingCollections_ThenAllGroupsArePresent()
        {
            PlaceNames placeNames = new()
            {
                Countries = [
                    new NameGroup { Name = "Group1", ExplicitValues = ["A", "B"] },
                    new NameGroup { Name = "Group2", ExplicitValues = ["C", "D"] }
                ]
            };

            Assert.That(placeNames.Countries, Has.Count.EqualTo(2));
            Assert.That(placeNames.Countries[0].Name, Is.EqualTo("Group1"));
            Assert.That(placeNames.Countries[1].Name, Is.EqualTo("Group2"));
        }

        [Test]
        public void GivenPlaceNames_WhenCheckingAllCollectionsInitialized_ThenNoneAreNull()
        {
            PlaceNames placeNames = new();

            Assert.That(placeNames.Countries, Is.Not.Null);
            Assert.That(placeNames.Regions, Is.Not.Null);
            Assert.That(placeNames.Cities, Is.Not.Null);
            Assert.That(placeNames.Mountains, Is.Not.Null);
            Assert.That(placeNames.Forests, Is.Not.Null);
            Assert.That(placeNames.Deserts, Is.Not.Null);
            Assert.That(placeNames.Rivers, Is.Not.Null);
            Assert.That(placeNames.Lakes, Is.Not.Null);
            Assert.That(placeNames.Seas, Is.Not.Null);
            Assert.That(placeNames.Airports, Is.Not.Null);
            Assert.That(placeNames.WaterBodies, Is.Not.Null);
            Assert.That(placeNames.GeographicalPlaces, Is.Not.Null);
        }
    }
}