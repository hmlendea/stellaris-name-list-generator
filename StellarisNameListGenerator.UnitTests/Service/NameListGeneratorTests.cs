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
    }
}