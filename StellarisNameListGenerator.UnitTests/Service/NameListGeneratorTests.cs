using System.Collections.Generic;
using System.IO;

using Moq;

using NUnit.Framework;

using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;

namespace StellarisNameListGenerator.UnitTests.Service
{
    [TestFixture]
    public sealed class NameListGeneratorTests
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
        public void GivenSingleNameListInRepository_WhenGenerating_ThenReturnsThatNameList()
        {
            NameList expectedNameList = new()
            {
                Name = "Test List",
                Denonyms = [new NameGroup { ExplicitValues = ["Test"] }]
            };

            mockNameListRepository.Setup(x => x.GetAll()).Returns([expectedNameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/output.xml", "Generated Name", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "Generated Name" && nl.IsLocked == false)), Times.Once);
        }

        [Test]
        public void GivenMultipleNameListsInRepository_WhenGenerating_ThenMergesAllNameLists()
        {
            NameList nameList1 = new()
            {
                Name = "List1",
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym1"] }],
                Places = new PlaceNames { Countries = [new NameGroup { ExplicitValues = ["Country1"] }] }
            };

            NameList nameList2 = new()
            {
                Name = "List2",
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym2"] }],
                Places = new PlaceNames { Regions = [new NameGroup { ExplicitValues = ["Region1"] }] }
            };

            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList1, nameList2]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/output.xml", "Merged Name", true);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl =>
                nl.Name == "Merged Name" &&
                nl.IsLocked == true &&
                nl.Denonyms.Count == 2 &&
                nl.Places.Countries.Count == 1 &&
                nl.Places.Regions.Count == 1)), Times.Once);
        }

        [Test]
        public void GivenEmptyRepository_WhenGenerating_ThenCreatesEmptyNameList()
        {
            mockNameListRepository.Setup(x => x.GetAll()).Returns([]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/output.xml", "Empty Name", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl =>
                nl.Name == "Empty Name" &&
                nl.IsLocked == false &&
                nl.Denonyms.Count == 0)), Times.Once);
        }

        [Test]
        public void GivenFilePath_WhenGenerating_ThenUsesFileNameAsId()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/my_name_list.xml", "Test", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Id == "my_name_list")), Times.Once);
        }

        [Test]
        public void GivenFilePathWithDirectory_WhenGenerating_ThenExtractsFileNameCorrectly()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            string tempPath = Path.Combine(Path.GetTempPath(), "custom_names.xml");
            generator.Generate(tempPath, "Test", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Id == "custom_names")), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGetMergedNameListCalledWithSingleList_ThenReturnsThatList()
        {
            NameList expectedNameList = new() { Name = "Single" };
            mockNameListRepository.Setup(x => x.GetAll()).Returns([expectedNameList]);

            NameList result = generator.GetMergedNameList();

            Assert.That(result, Is.SameAs(expectedNameList));
        }

        [Test]
        public void GivenNameListGenerator_WhenGetMergedNameListCalledWithMultipleLists_ThenReturnsMergedList()
        {
            NameList nameList1 = new() { Denonyms = [new NameGroup { ExplicitValues = ["A"] }] };
            NameList nameList2 = new() { Denonyms = [new NameGroup { ExplicitValues = ["B"] }] };
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList1, nameList2]);

            NameList result = generator.GetMergedNameList();

            Assert.That(result.Denonyms, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenNameListGenerator_WhenGetMergedNameListCalledWithEmptyRepository_ThenReturnsNewEmptyNameList()
        {
            mockNameListRepository.Setup(x => x.GetAll()).Returns([]);

            NameList result = generator.GetMergedNameList();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Denonyms, Is.Empty);
            Assert.That(result.Name, Is.EqualTo(string.Empty));
            Assert.That(result.IsLocked, Is.True);
        }

        [Test]
        public void GivenNameListGenerator_WhenGetMergedNameListCalledWithThreeLists_ThenMergesAllThree()
        {
            NameList nameList1 = new() { Denonyms = [new NameGroup { ExplicitValues = ["A"] }] };
            NameList nameList2 = new() { Denonyms = [new NameGroup { ExplicitValues = ["B"] }] };
            NameList nameList3 = new() { Denonyms = [new NameGroup { ExplicitValues = ["C"] }] };
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList1, nameList2, nameList3]);

            NameList result = generator.GetMergedNameList();

            Assert.That(result.Denonyms, Has.Count.EqualTo(3));
        }

        [Test]
        public void GivenNameListGenerator_WhenGeneratingWithIsLockedTrue_ThenSetsIsLockedTrue()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", "Test", true);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.IsLocked == true)), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGeneratingWithIsLockedFalse_ThenSetsIsLockedFalse()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", "Test", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.IsLocked == false)), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGenerating_ThenSetsNameFromParameter()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/test.xml", "Custom Name", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Name == "Custom Name")), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGenerating_ThenCallsFileWriteAllText()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("test content");

            generator.Generate("/tmp/test_output.xml", "Test", false);

            // Verify file was written (we can't easily mock File.WriteAllText, but we can verify the builder was called)
            mockFileContentBuilder.Verify(x => x.BuildContent(It.IsAny<NameList>()), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGeneratingWithFilePathWithoutExtension_ThenUsesFullFileNameAsId()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/name_without_extension", "Test", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Id == "name_without_extension")), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGeneratingWithFilePathWithMultipleDots_ThenUsesNameWithoutLastExtension()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            generator.Generate("/tmp/my.name.list.xml", "Test", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Id == "my.name.list")), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGeneratingWithRelativePath_ThenExtractsFileNameCorrectly()
        {
            NameList nameList = new();
            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            string tempPath = Path.Combine(Path.GetTempPath(), "relative_path_test.xml");
            generator.Generate(tempPath, "Test", false);

            mockFileContentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(nl => nl.Id == "relative_path_test")), Times.Once);
        }

        [Test]
        public void GivenNameListGenerator_WhenGeneratingWithComplexNameList_ThenMergesAllProperties()
        {
            NameList nameList1 = new()
            {
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym1"] }],
                Places = new PlaceNames { Countries = [new NameGroup { ExplicitValues = ["Country1"] }] },
                GreatPeople = new GreatPeople { Explorers = [new NameGroup { ExplicitValues = ["Explorer1"] }] },
                Companies = new CompanyNames { RobotManufacturers = [new NameGroup { ExplicitValues = ["Robot1"] }] },
                Warfare = new WarfareNames { MilitaryUnitTypes = [new NameGroup { ExplicitValues = ["Unit1"] }] },
                BiosphereNames = new BiosphereNames { Animals = [new NameGroup { ExplicitValues = ["Animal1"] }] },
                Ships = new ShipNames { Corvette = [new NameGroup { ExplicitValues = ["Corvette1"] }] },
                Stations = new StationNames { MiningStations = [new NameGroup { ExplicitValues = ["Mining1"] }] },
                Armies = new ArmyNames { Fleet = [new NameGroup { ExplicitValues = ["Fleet1"] }] },
                Planets = new PlanetNames { Generic = [new NameGroup { ExplicitValues = ["Planet1"] }] },
                Characters = [new CharacterNames { Id = "char1", Weight = 10 }]
            };

            NameList nameList2 = new()
            {
                Denonyms = [new NameGroup { ExplicitValues = ["Denonym2"] }],
                Places = new PlaceNames { Regions = [new NameGroup { ExplicitValues = ["Region1"] }] },
                GreatPeople = new GreatPeople { Scientists = [new NameGroup { ExplicitValues = ["Scientist1"] }] },
                Companies = new CompanyNames { SpacecraftManufacturers = [new NameGroup { ExplicitValues = ["Space1"] }] },
                Warfare = new WarfareNames { ShipTypes = [new NameGroup { ExplicitValues = ["Ship1"] }] },
                BiosphereNames = new BiosphereNames { MythologicalCreatures = [new NameGroup { ExplicitValues = ["Creature1"] }] },
                Ships = new ShipNames { Destroyer = [new NameGroup { ExplicitValues = ["Destroyer1"] }] },
                Stations = new StationNames { ResearchStations = [new NameGroup { ExplicitValues = ["Research1"] }] },
                Armies = new ArmyNames { DefenceArmy = [new NameGroup { ExplicitValues = ["Defence1"] }] },
                Planets = new PlanetNames { Desert = [new NameGroup { ExplicitValues = ["Desert1"] }] },
                Characters = [new CharacterNames { Id = "char2", Weight = 20 }]
            };

            mockNameListRepository.Setup(x => x.GetAll()).Returns([nameList1, nameList2]);
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("<xml/>");

            var capturedNameList = (NameList)null;
            mockFileContentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>()))
                .Returns("<xml/>")
                .Callback<NameList>(nl => capturedNameList = nl);

            generator.Generate("/tmp/complex.xml", "Complex", true);

            Assert.That(capturedNameList, Is.Not.Null);
            Assert.That(capturedNameList.Id, Is.EqualTo("complex"));
            Assert.That(capturedNameList.Name, Is.EqualTo("Complex"));
            Assert.That(capturedNameList.IsLocked, Is.True);
            Assert.That(capturedNameList.Denonyms.Count, Is.EqualTo(2));
            Assert.That(capturedNameList.Places.Countries.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Places.Regions.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.GreatPeople.Explorers.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.GreatPeople.Scientists.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Companies.RobotManufacturers.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Companies.SpacecraftManufacturers.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Warfare.MilitaryUnitTypes.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Warfare.ShipTypes.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.BiosphereNames.Animals.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.BiosphereNames.MythologicalCreatures.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Ships.Corvette.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Ships.Destroyer.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Stations.MiningStations.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Stations.ResearchStations.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Armies.Fleet.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Armies.DefenceArmy.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Planets.Generic.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Planets.Desert.Count, Is.EqualTo(1));
            Assert.That(capturedNameList.Characters.Count, Is.EqualTo(2));
        }
    }
}