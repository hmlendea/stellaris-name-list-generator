using System.Collections.Generic;

using NUnit.Framework;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.UnitTests.Models
{
    [TestFixture]
    public sealed class CharacterNamesTests
    {
        [Test]
        public void GivenNewCharacterNames_WhenCheckingDefaults_ThenAllCollectionsAreInitialized()
        {
            CharacterNames characterNames = new();

            Assert.That(characterNames.FullNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.FirstNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.RoyalFirstNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.MaleFullNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.MaleFirstNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.MaleRoyalFirstNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.FemaleFullNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.FemaleFirstNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.FemaleRoyalFirstNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.SecondNames, Is.Not.Null.And.Empty);
            Assert.That(characterNames.RoyalSecondNames, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GivenCharacterNamesWithAllEmptyGroups_WhenCheckingIsEmpty_ThenReturnsTrue()
        {
            CharacterNames characterNames = new();

            Assert.That(characterNames.IsEmpty, Is.True);
        }

        [Test]
        public void GivenCharacterNamesWithOneNonEmptyGroup_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            CharacterNames characterNames = new()
            {
                FullNames = [new NameGroup { ExplicitValues = ["John Doe"] }]
            };

            Assert.That(characterNames.IsEmpty, Is.False);
        }

        [Test]
        public void GivenCharacterNamesWithMaleNames_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            CharacterNames characterNames = new()
            {
                MaleFirstNames = [new NameGroup { ExplicitValues = ["John"] }]
            };

            Assert.That(characterNames.IsEmpty, Is.False);
        }

        [Test]
        public void GivenCharacterNamesWithFemaleNames_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            CharacterNames characterNames = new()
            {
                FemaleFirstNames = [new NameGroup { ExplicitValues = ["Jane"] }]
            };

            Assert.That(characterNames.IsEmpty, Is.False);
        }

        [Test]
        public void GivenCharacterNamesWithRoyalNames_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            CharacterNames characterNames = new()
            {
                RoyalFirstNames = [new NameGroup { ExplicitValues = ["King"] }]
            };

            Assert.That(characterNames.IsEmpty, Is.False);
        }

        [Test]
        public void GivenCharacterNamesWithSecondNames_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            CharacterNames characterNames = new()
            {
                SecondNames = [new NameGroup { ExplicitValues = ["Smith"] }]
            };

            Assert.That(characterNames.IsEmpty, Is.False);
        }

        [Test]
        public void GivenCharacterNames_WhenSettingIdAndWeight_ThenPropertiesAreStored()
        {
            CharacterNames characterNames = new()
            {
                Id = "test_character",
                Weight = 100
            };

            Assert.That(characterNames.Id, Is.EqualTo("test_character"));
            Assert.That(characterNames.Weight, Is.EqualTo(100));
        }

        [Test]
        public void GivenCharacterNamesWithMultipleNameGroups_WhenCheckingIsEmpty_ThenReturnsFalse()
        {
            CharacterNames characterNames = new()
            {
                FullNames = [new NameGroup { ExplicitValues = ["John Doe"] }],
                FirstNames = [new NameGroup { ExplicitValues = ["John"] }],
                MaleFirstNames = [new NameGroup { ExplicitValues = ["Robert"] }],
                FemaleFirstNames = [new NameGroup { ExplicitValues = ["Mary"] }],
                SecondNames = [new NameGroup { ExplicitValues = ["Smith"] }]
            };

            Assert.That(characterNames.IsEmpty, Is.False);
        }
    }
}