using System;
using System.IO;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Extends the command line contract coverage: option aliases that clash with the parser, values that
    /// resemble options, repeated options, and case-insensitive boolean parsing.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class CommandLineScenarioTests
    {
        private const string OutputFileName = "scenarios.txt";
        private const string DisplayName = "Scenario List";

        private GeneratorEnvironment environment = null!;

        [SetUp]
        public void SetUp()
        {
            environment = new GeneratorEnvironment(nameof(CommandLineScenarioTests));
            environment.WriteInput(NameListFixtures.MinimalDocument);
        }

        [TearDown]
        public void TearDown()
            => environment.Dispose();

        [Test]
        public void GivenAnUnrecognisedShortOption_WhenGenerating_ThenArgumentExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName,
                    "-l",
                    "-x"),
                Throws.TypeOf<ArgumentException>());
            Assert.That(File.Exists(environment.GetPath(OutputFileName)), Is.False);
        }

        [Test]
        public void GivenLockedFlagWithMixedCaseValue_WhenGenerating_ThenTheValueIsParsedCaseInsensitively()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath(OutputFileName),
                "-n", DisplayName,
                "-l", "TRUE");

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("randomized = no"));
        }

        [Test]
        public void GivenLockedFlagWithNumericValue_WhenGenerating_ThenArgumentExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName,
                    "-l", "1"),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GivenAnOptionValueContainingEqualsSign_WhenGenerating_ThenArgumentExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run(
                    "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                    "-o", environment.GetPath(OutputFileName),
                    "-n", DisplayName,
                    "--locked=yes"),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GivenRepeatedNameOption_WhenGenerating_ThenTheLastValueIsUsed()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath(OutputFileName),
                "-n", "First",
                "-n", "Second");

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("### Second"));
            Assert.That(environment.ReadText(OutputFileName), Does.Not.Contain("### First"));
        }

        [Test]
        public void GivenInputOptionWithoutValue_WhenGenerating_ThenArgumentNullExceptionIsThrown()
        {
            Assert.That(
                () => environment.Run("-i"),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenOutputFileNameWithoutExtension_WhenGenerating_ThenTheStemBecomesTheIdentifier()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath("no_extension"),
                "-n", DisplayName);

            Assert.That(environment.ReadText("no_extension"), Does.Contain("### no_extension"));
        }

        [Test]
        public void GivenUppercasedOutputFileName_WhenGenerating_ThenTheIdentifierPreservesTheCase()
        {
            environment.Run(
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath("MiXeD.txt"),
                "-n", DisplayName);

            Assert.That(environment.ReadText("MiXeD.txt"), Does.Contain("MiXeD = {"));
        }
    }
}
