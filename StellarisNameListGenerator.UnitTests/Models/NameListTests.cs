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

        [Test]
        public void GivenNameListWithPlaces_WhenAddingAnotherWithPlaces_ThenAllPlaceTypesAreCombined()
        {
            NameList nameList1 = new()
            {
                Places = new PlaceNames
                {
                    Countries = [new NameGroup { ExplicitValues = ["Country1"] }],
                    Cities = [new NameGroup { ExplicitValues = ["City1"] }],
                    Regions = [new NameGroup { ExplicitValues = ["Region1"] }],
                    Mountains = [new NameGroup { ExplicitValues = ["Mountain1"] }],
                    Rivers = [new NameGroup { ExplicitValues = ["River1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Places = new PlaceNames
                {
                    Countries = [new NameGroup { ExplicitValues = ["Country2"] }],
                    Cities = [new NameGroup { ExplicitValues = ["City2"] }],
                    Forests = [new NameGroup { ExplicitValues = ["Forest1"] }],
                    Lakes = [new NameGroup { ExplicitValues = ["Lake1"] }],
                    Airports = [new NameGroup { ExplicitValues = ["Airport1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Places.Countries, Has.Count.EqualTo(2));
            Assert.That(nameList1.Places.Cities, Has.Count.EqualTo(2));
            Assert.That(nameList1.Places.Regions, Has.Count.EqualTo(1));
            Assert.That(nameList1.Places.Mountains, Has.Count.EqualTo(1));
            Assert.That(nameList1.Places.Forests, Has.Count.EqualTo(1));
            Assert.That(nameList1.Places.Rivers, Has.Count.EqualTo(1));
            Assert.That(nameList1.Places.Lakes, Has.Count.EqualTo(1));
            Assert.That(nameList1.Places.Airports, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithGreatPeople_WhenAddingAnotherWithGreatPeople_ThenAllCategoriesAreCombined()
        {
            NameList nameList1 = new()
            {
                GreatPeople = new GreatPeople
                {
                    Explorers = [new NameGroup { ExplicitValues = ["Explorer1"] }],
                    Scientists = [new NameGroup { ExplicitValues = ["Scientist1"] }],
                    LeadersTier1 = [new NameGroup { ExplicitValues = ["Leader1"] }]
                }
            };

            NameList nameList2 = new()
            {
                GreatPeople = new GreatPeople
                {
                    Explorers = [new NameGroup { ExplicitValues = ["Explorer2"] }],
                    Pioneers = [new NameGroup { ExplicitValues = ["Pioneer1"] }],
                    LeadersTier2 = [new NameGroup { ExplicitValues = ["Leader2"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.GreatPeople.Explorers, Has.Count.EqualTo(2));
            Assert.That(nameList1.GreatPeople.Scientists, Has.Count.EqualTo(1));
            Assert.That(nameList1.GreatPeople.Pioneers, Has.Count.EqualTo(1));
            Assert.That(nameList1.GreatPeople.LeadersTier1, Has.Count.EqualTo(1));
            Assert.That(nameList1.GreatPeople.LeadersTier2, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithCompanies_WhenAddingAnotherWithCompanies_ThenAllCategoriesAreCombined()
        {
            NameList nameList1 = new()
            {
                Companies = new CompanyNames
                {
                    RobotManufacturers = [new NameGroup { ExplicitValues = ["Robot1"] }],
                    SpacecraftManufacturers = [new NameGroup { ExplicitValues = ["Space1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Companies = new CompanyNames
                {
                    RobotManufacturers = [new NameGroup { ExplicitValues = ["Robot2"] }],
                    WeaponManufacturers = [new NameGroup { ExplicitValues = ["Weapon1"] }],
                    ResearchCompanies = [new NameGroup { ExplicitValues = ["Research1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Companies.RobotManufacturers, Has.Count.EqualTo(2));
            Assert.That(nameList1.Companies.SpacecraftManufacturers, Has.Count.EqualTo(1));
            Assert.That(nameList1.Companies.WeaponManufacturers, Has.Count.EqualTo(1));
            Assert.That(nameList1.Companies.ResearchCompanies, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithWarfare_WhenAddingAnotherWithWarfare_ThenAllCategoriesAreCombined()
        {
            NameList nameList1 = new()
            {
                Warfare = new WarfareNames
                {
                    MilitaryUnitTypes = [new NameGroup { ExplicitValues = ["Unit1"] }],
                    ShipTypes = [new NameGroup { ExplicitValues = ["Ship1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Warfare = new WarfareNames
                {
                    MilitaryUnitTypes = [new NameGroup { ExplicitValues = ["Unit2"] }],
                    Forts = [new NameGroup { ExplicitValues = ["Fort1"] }],
                    BattleLocations = [new NameGroup { ExplicitValues = ["Battle1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Warfare.MilitaryUnitTypes, Has.Count.EqualTo(2));
            Assert.That(nameList1.Warfare.ShipTypes, Has.Count.EqualTo(1));
            Assert.That(nameList1.Warfare.Forts, Has.Count.EqualTo(1));
            Assert.That(nameList1.Warfare.BattleLocations, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithBiosphereNames_WhenAddingAnotherWithBiosphereNames_ThenAllCategoriesAreCombined()
        {
            NameList nameList1 = new()
            {
                BiosphereNames = new BiosphereNames
                {
                    Animals = [new NameGroup { ExplicitValues = ["Animal1"] }]
                }
            };

            NameList nameList2 = new()
            {
                BiosphereNames = new BiosphereNames
                {
                    Animals = [new NameGroup { ExplicitValues = ["Animal2"] }],
                    MythologicalCreatures = [new NameGroup { ExplicitValues = ["Creature1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.BiosphereNames.Animals, Has.Count.EqualTo(2));
            Assert.That(nameList1.BiosphereNames.MythologicalCreatures, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithShips_WhenAddingAnotherWithShips_ThenAllShipTypesAreCombined()
        {
            NameList nameList1 = new()
            {
                Ships = new ShipNames
                {
                    Corvette = [new NameGroup { ExplicitValues = ["Corvette1"] }],
                    Destroyer = [new NameGroup { ExplicitValues = ["Destroyer1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Ships = new ShipNames
                {
                    Corvette = [new NameGroup { ExplicitValues = ["Corvette2"] }],
                    Cruiser = [new NameGroup { ExplicitValues = ["Cruiser1"] }],
                    Battleship = [new NameGroup { ExplicitValues = ["Battleship1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Ships.Corvette, Has.Count.EqualTo(2));
            Assert.That(nameList1.Ships.Destroyer, Has.Count.EqualTo(1));
            Assert.That(nameList1.Ships.Cruiser, Has.Count.EqualTo(1));
            Assert.That(nameList1.Ships.Battleship, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithStations_WhenAddingAnotherWithStations_ThenAllStationTypesAreCombined()
        {
            NameList nameList1 = new()
            {
                Stations = new StationNames
                {
                    MiningStations = [new NameGroup { ExplicitValues = ["Mining1"] }],
                    ResearchStations = [new NameGroup { ExplicitValues = ["Research1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Stations = new StationNames
                {
                    MiningStations = [new NameGroup { ExplicitValues = ["Mining2"] }],
                    ObservationStations = [new NameGroup { ExplicitValues = ["Observation1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Stations.MiningStations, Has.Count.EqualTo(2));
            Assert.That(nameList1.Stations.ResearchStations, Has.Count.EqualTo(1));
            Assert.That(nameList1.Stations.ObservationStations, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithArmies_WhenAddingAnotherWithArmies_ThenAllArmyTypesAreCombined()
        {
            NameList nameList1 = new()
            {
                Armies = new ArmyNames
                {
                    Fleet = [new NameGroup { ExplicitValues = ["Fleet1"] }],
                    DefenceArmy = [new NameGroup { ExplicitValues = ["Defence1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Armies = new ArmyNames
                {
                    Fleet = [new NameGroup { ExplicitValues = ["Fleet2"] }],
                    AssaultArmy = [new NameGroup { ExplicitValues = ["Assault1"] }],
                    OccupationArmy = [new NameGroup { ExplicitValues = ["Occupation1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Armies.Fleet, Has.Count.EqualTo(2));
            Assert.That(nameList1.Armies.DefenceArmy, Has.Count.EqualTo(1));
            Assert.That(nameList1.Armies.AssaultArmy, Has.Count.EqualTo(1));
            Assert.That(nameList1.Armies.OccupationArmy, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameListWithPlanets_WhenAddingAnotherWithPlanets_ThenAllPlanetTypesAreCombined()
        {
            NameList nameList1 = new()
            {
                Planets = new PlanetNames
                {
                    Generic = [new NameGroup { ExplicitValues = ["Generic1"] }],
                    Desert = [new NameGroup { ExplicitValues = ["Desert1"] }]
                }
            };

            NameList nameList2 = new()
            {
                Planets = new PlanetNames
                {
                    Generic = [new NameGroup { ExplicitValues = ["Generic2"] }],
                    Tropical = [new NameGroup { ExplicitValues = ["Tropical1"] }],
                    Ocean = [new NameGroup { ExplicitValues = ["Ocean1"] }]
                }
            };

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Planets.Generic, Has.Count.EqualTo(2));
            Assert.That(nameList1.Planets.Desert, Has.Count.EqualTo(1));
            Assert.That(nameList1.Planets.Tropical, Has.Count.EqualTo(1));
            Assert.That(nameList1.Planets.Ocean, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameList_WhenAddingEmptyNameList_ThenOriginalValuesUnchanged()
        {
            NameList nameList1 = new()
            {
                Name = "Original",
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym1"] }]
            };

            NameList nameList2 = new();

            nameList1.AddRange(nameList2);

            Assert.That(nameList1.Name, Is.EqualTo("Original"));
            Assert.That(nameList1.Denonyms, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNameList_WhenAddingToSelf_ThenValuesAreDuplicated()
        {
            NameList nameList = new()
            {
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym1"] }]
            };

            nameList.AddRange(nameList);

            Assert.That(nameList.Denonyms, Has.Count.EqualTo(2));
        }
    }
}