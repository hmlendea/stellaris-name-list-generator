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
    }
}