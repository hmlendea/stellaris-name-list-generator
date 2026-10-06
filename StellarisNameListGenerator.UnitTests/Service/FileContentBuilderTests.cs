using System.Collections.Generic;

using Moq;

using NUnit.Framework;

using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;

namespace StellarisNameListGenerator.UnitTests.Service
{
    [TestFixture]
    public sealed class FileContentBuilderTests
    {
        private Mock<IFileContentBuilder> mockFileContentBuilder;
        private Mock<IFileRepository<NameList>> mockNameListRepository;
        private NameListGenerator generator;

        [SetUp]
        public void SetUp()
        {
            mockFileContentBuilder = new Mock<IFileContentBuilder>();
            mockNameListRepository = new Mock<IFileRepository<NameList>>();
            generator = new NameListGenerator(mockFileContentBuilder.Object, mockNameListRepository.Object);
        }

        [Test]
        public void GivenNameList_WhenGenerating_ThenCallsBuildContent()
        {
            // Arrange
            NameList nameList = new()
            {
                Name = "Test Name",
                IsLocked = false
            };

            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(nameList)).Returns("<xml/>");

            // Act
            generator.Generate("/tmp/test.xml", "Test Name", false);

            // Assert
            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "Test Name" && nl.IsLocked == false)), Times.Once);
        }

        [Test]
        public void GivenMultipleNameLists_WhenGenerating_ThenMergesLists()
        {
            // Arrange
            NameList nameList1 = new() { Name = "List1" };
            NameList nameList2 = new() { Name = "List2" };

            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList1, nameList2]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            // Act
            generator.Generate("/tmp/merged.xml", "Merged Name", true);

            // Assert
            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "Merged Name" && nl.IsLocked == true)), Times.Once);
        }

        [Test]
        public void GivenFilePath_WhenGenerating_ThenUsesFileNameAsId()
        {
            // Arrange
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            // Act
            generator.Generate("/tmp/my_name.xml", "Test", false);

            // Assert
            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Id == "my_name")), Times.Once);
        }

        [Test]
        public void GivenNameListWithEmptyName_WhenGenerating_ThenBuildContentCalledWithEmptyName()
        {
            NameList nameList = new() { Name = "" };
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", "", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "")), Times.Once);
        }

        [Test]
        public void GivenNameListWithSpecialCharactersInName_WhenGenerating_ThenBuildContentCalledWithSpecialCharacters()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", "Name with spaces & symbols!", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "Name with spaces & symbols!")), Times.Once);
        }

        [Test]
        public void GivenNameListWithUnicodeCharactersInName_WhenGenerating_ThenBuildContentCalledWithUnicode()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", "Näme 中文 🚀", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "Näme 中文 🚀")), Times.Once);
        }

        [Test]
        public void GivenNameListWithVeryLongName_WhenGenerating_ThenBuildContentCalledWithLongName()
        {
            string longName = new('A', 1000);
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", longName, false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == longName)), Times.Once);
        }

        [Test]
        public void GivenNameListWithAllPropertiesPopulated_WhenGenerating_ThenBuildContentCalledWithFullNameList()
        {
            NameList nameList = new()
            {
                Name = "Full List",
                IsLocked = true,
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym1"] }],
                Places = new PlaceNames { Countries = [new NameGroup { ExplicitValues = ["Country1"] }] },
                GreatPeople = new GreatPeople { Explorers = [new NameGroup { ExplicitValues = ["Explorer1"] }] },
                Companies = new CompanyNames { RobotManufacturers = [new NameGroup { ExplicitValues = ["Robot1"] }] },
                Warfare = new WarfareNames { MilitaryUnitTypes = [new NameGroup { ExplicitValues = ["Unit1"] }] },
                BiosphereNames = new BiosphereNames { Animals = [new NameGroup { ExplicitValues = ["Animal1"] }] },
                Ships = new ShipNames { Corvette = [new NameGroup { ExplicitValues = ["Corvette1"] }] },
                ShipClasses = new ShipNames { Destroyer = [new NameGroup { ExplicitValues = ["Destroyer1"] }] },
                Stations = new StationNames { MiningStations = [new NameGroup { ExplicitValues = ["Mining1"] }] },
                StationClasses = new StationNames { ResearchStations = [new NameGroup { ExplicitValues = ["Research1"] }] },
                Armies = new ArmyNames { Fleet = [new NameGroup { ExplicitValues = ["Fleet1"] }] },
                Planets = new PlanetNames { Generic = [new NameGroup { ExplicitValues = ["Planet1"] }] },
                Characters = [new CharacterNames { Id = "char1", Weight = 10 }]
            };

            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/full.xml", "Full List", true);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl =>
                nl.Name == "Full List" &&
                nl.IsLocked == true &&
                nl.Denonyms.Count == 1 &&
                nl.Places.Countries.Count == 1 &&
                nl.GreatPeople.Explorers.Count == 1 &&
                nl.Companies.RobotManufacturers.Count == 1 &&
                nl.Warfare.MilitaryUnitTypes.Count == 1 &&
                nl.BiosphereNames.Animals.Count == 1 &&
                nl.Ships.Corvette.Count == 1 &&
                nl.ShipClasses.Destroyer.Count == 1 &&
                nl.Stations.MiningStations.Count == 1 &&
                nl.StationClasses.ResearchStations.Count == 1 &&
                nl.Armies.Fleet.Count == 1 &&
                nl.Planets.Generic.Count == 1 &&
                nl.Characters.Count == 1)), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGenerateCalledMultipleTimes_ThenBuildContentCalledEachTime()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test1.xml", "Test1", false);
            generator.Generate("/tmp/test2.xml", "Test2", false);
            generator.Generate("/tmp/test3.xml", "Test3", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.IsAny<NameList>()), Times.Exactly(3));
        }

        [Test]
        public void GivenNameListGenerator_WhenGenerateCalledWithDifferentIsLockedValues_ThenBuildContentCalledWithCorrectValues()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            var capturedNameLists = new List<NameList>();
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>()))
                .Returns("<xml/>")
                .Callback<NameList>(nl => capturedNameLists.Add(nl));

            generator.Generate("/tmp/test1.xml", "Test1", true);
            generator.Generate("/tmp/test2.xml", "Test2", false);

            Assert.That(capturedNameLists, Has.Count.EqualTo(2));
            // Same object is reused, so second call overwrites the first
            Assert.That(capturedNameLists[1].Id, Is.EqualTo("test2"));
            Assert.That(capturedNameLists[1].IsLocked, Is.False);
            Assert.That(capturedNameLists[1].Name, Is.EqualTo("Test2"));
        }
    }
}