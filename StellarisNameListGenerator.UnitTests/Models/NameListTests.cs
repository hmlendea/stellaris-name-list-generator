using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.UnitTests.Models
{
    [TestFixture]
    public sealed class NameListTests
    {
        [Test]
        public void GivenNewNameList_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            NameList nameList = new();

            Assert.That(nameList.Name, Is.EqualTo(string.Empty));
            Assert.That(nameList.IsLocked, Is.True);
            Assert.That(nameList.Denonyms, Is.Not.Null.And.Empty);
            Assert.That(nameList.Places, Is.Not.Null);
            Assert.That(nameList.GreatPeople, Is.Not.Null);
            Assert.That(nameList.Companies, Is.Not.Null);
            Assert.That(nameList.Warfare, Is.Not.Null);
            Assert.That(nameList.BiosphereNames, Is.Not.Null);
            Assert.That(nameList.Ships, Is.Not.Null);
            Assert.That(nameList.ShipClasses, Is.Not.Null);
            Assert.That(nameList.Stations, Is.Not.Null);
            Assert.That(nameList.StationClasses, Is.Not.Null);
            Assert.That(nameList.Armies, Is.Not.Null);
            Assert.That(nameList.Planets, Is.Not.Null);
            Assert.That(nameList.Characters, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenNameListWithValues_WhenAddingAnotherNameList_ThenValuesAreCombined()
        {
            NameList nameList1 = new()
            {
                Name = "List1",
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym1"] }],
                Places = new PlaceNames
                {
                    Countries = [new NameGroup { ExplicitValues = ["Country1"] }],
                    Cities = [new NameGroup { ExplicitValues = ["City1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Name = "List2",
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym2"] }],
                Places = new PlaceNames
                {
                    Countries = [new NameGroup { ExplicitValues = ["Country2"] }],
                    Regions = [new NameGroup { ExplicitValues = ["Region1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Denonyms, Has.Count.EqualTo(2));
            Assert.That(nameList1.Places.Countries, Has.Count.EqualTo(2));
            Assert.That(nameList1.Places.Cities, Has.Count.EqualTo(1));
            Assert.That(nameList1.Places.Regions, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameList_WhenAddingNullNameList_ThenThrowsNullReferenceException()
        {
            NameList nameList = new();

            Assert.Throws<System.NullReferenceException>(() => nameList.AddRange(null!));
        }

        [Test]
        public void GivenNameList_WhenSettingProperties_ThenPropertiesAreStored()
        {
            NameList nameList = new()
            {
                Name = "Test Name List",
                IsLocked = false
            };

            Assert.That(nameList.Name, Is.EqualTo("Test Name List"));
            Assert.That(nameList.IsLocked, Is.False);
        }

        [Test]
        public void GivenNameListWithCharacters_WhenAddingAnotherWithCharacters_ThenCharactersAreCombined()
        {
            NameList nameList1 = new()
            {
                Characters = [new CharacterNames { Id = "char1", Weight = 10 }]
            };

            NameList nameList2 = new()
            {
                Characters = [new CharacterNames { Id = "char2", Weight = 20 }]
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Characters, Has.Count.EqualTo(2));
        }
    }
}