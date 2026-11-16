using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the command line contract of the generator: argument aliases, the lock flag,
    /// argument validation, and the failure modes of unreadable or malformed input.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class CommandLineTests
    {
        private const string OutputFileName = "generated.txt";
        private const string DisplayName = "Nucilandia";

        private GeneratorEnvironment environment = null!;

        [SetUp]
        public void SetUp()
        {
            environment = new GeneratorEnvironment(nameof(CommandLineTests));
            environment.WriteInput(NameListFixtures.MinimalDocument);
        }

        [TearDown]
        public void TearDown()
            => environment.Dispose();

        [Test]
        public void GivenShortFormArguments_WhenGenerating_ThenOutputIsProduced()
        {
            environment.Run("-i", environment.GetPath(GeneratorEnvironment.InputFileName), "-o", environment.GetPath(OutputFileName), "-n", DisplayName);

            Assert.That(File.Exists(environment.GetPath(OutputFileName)), Is.True);
        }

        [Test]
        public void GivenLongFormArguments_WhenGenerating_ThenOutputIsProduced()
        {
            environment.Run("--input", environment.GetPath(GeneratorEnvironment.InputFileName), "--output", environment.GetPath(OutputFileName), "--name", DisplayName);

            Assert.That(File.Exists(environment.GetPath(OutputFileName)), Is.True);
        }

        [Test]
        public void GivenShortAndLongFormArguments_WhenGenerating_ThenIdenticalContentIsProduced()
        {
            Directory.CreateDirectory(environment.GetPath("short"));
            Directory.CreateDirectory(environment.GetPath("long"));

            environment.Run("-i", environment.GetPath(GeneratorEnvironment.InputFileName), "-o", environment.GetPath("short/generated.txt"), "-n", DisplayName);
            environment.Run("--input", environment.GetPath(GeneratorEnvironment.InputFileName), "--output", environment.GetPath("long/generated.txt"), "--name", DisplayName);

            Assert.That(
                ReadBody(environment.ReadText("long/generated.txt")),
                Is.EqualTo(ReadBody(environment.ReadText("short/generated.txt"))));
        }

        [Test]
        public void GivenExplicitLockedFlag_WhenGenerating_ThenRandomisationIsDisabled()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath(OutputFileName),
                "-n", DisplayName,
                "--locked", "true");

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("randomized = no"));
        }

        [Test]
        public void GivenBareLockedFlag_WhenGenerating_ThenRandomisationIsDisabled()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath(OutputFileName),
                "-n", DisplayName,
                "--locked");

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("randomized = no"));
        }

        [Test]
        public void GivenLockedFlagWithFalseValue_WhenGenerating_ThenRandomisationIsEnabled()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath(OutputFileName),
                "-n", DisplayName,
                "--locked", "false");

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("randomized = yes"));
        }

        [Test]
        public void GivenNoLockedFlag_WhenGenerating_ThenRandomisationIsEnabled()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath(OutputFileName),
                "-n", DisplayName);

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("randomized = yes"));
        }

        [Test]
        public void GivenMissingInputArgument_WhenGenerating_ThenArgumentNullExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run("-o", environment.GetPath(OutputFileName), "-n", DisplayName),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenMissingOutputArgument_WhenGenerating_ThenArgumentNullExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run("-i", environment.GetPath(GeneratorEnvironment.InputFileName), "-n", DisplayName),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenMissingNameArgument_WhenGenerating_ThenArgumentNullExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run("-i", environment.GetPath(GeneratorEnvironment.InputFileName), "-o", environment.GetPath(OutputFileName)),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenNameArgumentWithoutValue_WhenGenerating_ThenArgumentNullExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run("-i", environment.GetPath(GeneratorEnvironment.InputFileName), "-o", environment.GetPath(OutputFileName), "-n"),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenUnknownArgument_WhenGenerating_ThenArgumentExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName,
                    "--unsupported", "value"),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GivenUnparseableLockedValue_WhenGenerating_ThenArgumentExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName,
                    "--locked", "banana"),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GivenNonExistentInputFile_WhenGenerating_ThenFileNotFoundExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath("absent.xml"),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName),
                Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public void GivenMalformedDocument_WhenGenerating_ThenInvalidOperationExceptionIsThrown()
        {
            environment.WriteInput(NameListFixtures.MalformedDocument);

            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GivenNonExistentOutputDirectory_WhenGenerating_ThenDirectoryNotFoundExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(Path.Combine("absent", OutputFileName)),
                    "-n", DisplayName),
                Throws.TypeOf<DirectoryNotFoundException>());
        }

        [Test]
        public void GivenUnmappedElements_WhenGenerating_ThenGenerationSucceeds()
        {
            environment.WriteInput(NameListFixtures.UnknownElementDocument);

            environment.Generate(OutputFileName, DisplayName);

            Assert.That(environment.ReadText(OutputFileName), Does.Contain(NameListFixtures.CountryTwo));
        }

        private static string ReadBody(string content)
            => string.Join(
                "\n",
                GeneratedNameList
                    .Parse(content)
                    .Lines
                    .Where(line => !line.StartsWith(GeneratedNameList.HeaderPrefix, StringComparison.Ordinal)));
    }
}
