using System.Collections.Generic;
using System.IO;

using Moq;

using NUnit.Framework;

using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;

namespace StellarisNameListGenerator.UnitTests.Service
{
    /// <summary>
    /// Exercises the generator service beyond the happy path: identity and lock propagation, merged-list
    /// selection, repeated generation, and output-encoding behaviour.
    /// </summary>
    [TestFixture]
    public sealed class NameListGeneratorServiceTests
    {
        private Mock<IFileContentBuilder> contentBuilder = null!;
        private Mock<IFileRepository<NameList>> repository = null!;
        private NameListGenerator generator = null!;
        private string outputPath = null!;

        [SetUp]
        public void SetUp()
        {
            contentBuilder = new Mock<IFileContentBuilder>();
            repository = new Mock<IFileRepository<NameList>>();
            generator = new NameListGenerator(contentBuilder.Object, repository.Object);
            outputPath = Path.Combine(Path.GetTempPath(), $"nlg-{System.Guid.NewGuid():N}.txt");
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        [Test]
        public void GivenASingleNameList_WhenGettingTheMergedList_ThenTheSameInstanceIsReturned()
        {
            NameList single = new();
            repository.Setup(x => x.GetAll()).Returns([single]);

            Assert.That(generator.GetMergedNameList(), Is.SameAs(single));
        }

        [Test]
        public void GivenNoNameLists_WhenGettingTheMergedList_ThenANewEmptyListIsReturned()
        {
            repository.Setup(x => x.GetAll()).Returns([]);

            NameList merged = generator.GetMergedNameList();

            Assert.That(merged, Is.Not.Null);
            Assert.That(merged.Denonyms, Is.Empty);
        }

        [Test]
        public void GivenMultipleNameLists_WhenGettingTheMergedList_ThenEveryDenonymIsConcatenated()
        {
            repository.Setup(x => x.GetAll()).Returns(
            [
                new NameList { Denonyms = [Group("Alpha")] },
                new NameList { Denonyms = [Group("Beta")] },
                new NameList { Denonyms = [Group("Gamma")] },
            ]);

            NameList merged = generator.GetMergedNameList();

            Assert.That(merged.Denonyms, Has.Count.EqualTo(3));
        }

        [Test]
        public void GivenMultipleNameLists_WhenGettingTheMergedList_ThenAQuadraticCollectionIsConcatenated()
        {
            repository.Setup(x => x.GetAll()).Returns(
            [
                new NameList { GreatPeople = new GreatPeople { DeathDeities = [Group("Mors")] } },
                new NameList { GreatPeople = new GreatPeople { DeathDeities = [Group("Thanatos")] } },
            ]);

            NameList merged = generator.GetMergedNameList();

            Assert.That(merged.GreatPeople.DeathDeities, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenMultipleNameLists_WhenGettingTheMergedList_ThenCharactersAreConcatenated()
        {
            repository.Setup(x => x.GetAll()).Returns(
            [
                new NameList { Characters = [new CharacterNames { Id = "alpha", Weight = 1 }] },
                new NameList { Characters = [new CharacterNames { Id = "beta", Weight = 2 }] },
            ]);

            NameList merged = generator.GetMergedNameList();

            Assert.That(merged.Characters, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenAFilePath_WhenGenerating_ThenTheIdentifierIsTheFileNameStem()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia", false);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => n.Id == Path.GetFileNameWithoutExtension(outputPath))), Times.Once);
        }

        [Test]
        public void GivenADisplayName_WhenGenerating_ThenTheDisplayNameIsApplied()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia Integration", false);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => n.Name == "Nucilandia Integration")), Times.Once);
        }

        [Test]
        public void GivenALockedGeneration_WhenGenerating_ThenTheLockedFlagIsApplied()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia", true);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => n.IsLocked)), Times.Once);
        }

        [Test]
        public void GivenAnUnlockedGeneration_WhenGenerating_ThenTheLockedFlagIsNotApplied()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia", false);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => !n.IsLocked)), Times.Once);
        }

        [Test]
        public void GivenContentFromTheBuilder_WhenGenerating_ThenTheContentIsWrittenVerbatim()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("Nucilandia = {\n}");

            generator.Generate(outputPath, "Nucilandia", false);

            Assert.That(File.ReadAllText(outputPath), Is.EqualTo("Nucilandia = {\n}"));
        }

        [Test]
        public void GivenAGeneration_WhenInspectingTheOutput_ThenItStartsWithAByteOrderMark()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia", false);

            byte[] bytes = File.ReadAllBytes(outputPath);

            Assert.That(bytes[0], Is.EqualTo(0xEF));
            Assert.That(bytes[1], Is.EqualTo(0xBB));
            Assert.That(bytes[2], Is.EqualTo(0xBF));
        }

        [Test]
        public void GivenAPreExistingFile_WhenGenerating_ThenThePreviousContentIsReplaced()
        {
            File.WriteAllText(outputPath, "STALE");
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("FRESH");

            generator.Generate(outputPath, "Nucilandia", false);

            Assert.That(File.ReadAllText(outputPath), Does.Contain("FRESH"));
            Assert.That(File.ReadAllText(outputPath), Does.Not.Contain("STALE"));
        }

        [Test]
        public void GivenRepeatedInvocations_WhenGenerating_ThenTheContentBuilderIsInvokedEachTime()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia", false);
            generator.Generate(outputPath, "Nucilandia", false);
            generator.Generate(outputPath, "Nucilandia", false);

            contentBuilder.Verify(x => x.BuildContent(It.IsAny<NameList>()), Times.Exactly(3));
        }

        [Test]
        public void GivenAnEmptyDisplayName_WhenGenerating_ThenItIsAppliedUnchanged()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, string.Empty, false);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => n.Name == string.Empty)), Times.Once);
        }

        [Test]
        public void GivenAUnicodeDisplayName_WhenGenerating_ThenItIsAppliedUnchanged()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            generator.Generate(outputPath, "Nucilandia 🌍 中文", false);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => n.Name == "Nucilandia 🌍 中文")), Times.Once);
        }

        [Test]
        public void GivenADottedOutputPath_WhenGenerating_ThenOnlyTheLastExtensionIsStripped()
        {
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>())).Returns("content");

            string dottedPath = Path.Combine(Path.GetTempPath(), $"nlg-{System.Guid.NewGuid():N}.names.txt");
            generator.Generate(dottedPath, "Nucilandia", false);

            contentBuilder.Verify(x => x.BuildContent(It.Is<NameList>(n => n.Id == Path.GetFileNameWithoutExtension(dottedPath))), Times.Once);
        }

        [Test]
        public void GivenGenerationsToTwoPaths_WhenGenerating_ThenEachPathSuppliesItsOwnIdentifier()
        {
            List<string> identifiers = [];
            repository.Setup(x => x.GetAll()).Returns([new NameList()]);
            contentBuilder.Setup(x => x.BuildContent(It.IsAny<NameList>()))
                .Callback<NameList>(nameList => identifiers.Add(nameList.Id))
                .Returns("content");

            string secondPath = Path.Combine(Path.GetTempPath(), $"nlg-{System.Guid.NewGuid():N}.txt");

            try
            {
                generator.Generate(outputPath, "Nucilandia", false);
                generator.Generate(secondPath, "Nucilandia", false);

                Assert.That(
                    identifiers,
                    Is.EqualTo(new[]
                    {
                        Path.GetFileNameWithoutExtension(outputPath),
                        Path.GetFileNameWithoutExtension(secondPath),
                    }));
            }
            finally
            {
                if (File.Exists(secondPath))
                {
                    File.Delete(secondPath);
                }
            }
        }

        private static NameGroup Group(string value)
            => new() { Name = value, ExplicitValues = [value] };
    }
}
