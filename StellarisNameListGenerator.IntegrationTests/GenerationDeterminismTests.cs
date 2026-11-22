using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers generation determinism: locked generation is byte-identical across repeated runs and across
    /// output identifiers, and locked and unlocked generation differ only by the randomisation flag.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class GenerationDeterminismTests
    {
        private const string OutputFileName = "determinism.txt";
        private const string DisplayName = "Determinism";

        [Test]
        public void GivenALockedInvocation_WhenGeneratingTwice_ThenTheOutputIsByteIdentical()
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationDeterminismTests)}_repeat");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            environment.Generate(OutputFileName, DisplayName, isLocked: true);
            string firstContent = environment.ReadText(OutputFileName);

            environment.Generate(OutputFileName, DisplayName, isLocked: true);

            Assert.That(environment.ReadText(OutputFileName), Is.EqualTo(firstContent));
        }

        [Test]
        public void GivenTwoIdentifiers_WhenGeneratingLocked_ThenBodiesDifferOnlyByTheIdentifier()
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationDeterminismTests)}_identifiers");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            environment.Generate("first.txt", DisplayName, isLocked: true);
            string firstContent = environment.ReadText("first.txt");

            environment.Generate("second.txt", DisplayName, isLocked: true);
            string secondContent = environment.ReadText("second.txt");

            Assert.That(Normalise(firstContent, "first"), Is.EqualTo(Normalise(secondContent, "second")));
        }

        [Test]
        public void GivenLockedAndUnlockedInvocations_WhenGenerating_ThenOnlyTheRandomisationFlagDiffers()
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationDeterminismTests)}_lock_flag");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            environment.Generate(OutputFileName, DisplayName, isLocked: true);
            string lockedContent = environment.ReadText(OutputFileName);

            environment.Generate(OutputFileName, DisplayName);
            string unlockedContent = environment.ReadText(OutputFileName);

            string normalisedLocked = lockedContent.Replace("randomized = no", "randomized = X", System.StringComparison.Ordinal);
            string normalisedUnlocked = unlockedContent.Replace("randomized = yes", "randomized = X", System.StringComparison.Ordinal);

            Assert.That(normalisedLocked, Is.EqualTo(normalisedUnlocked));
        }

        [Test]
        public void GivenALockedInvocation_WhenGenerating_ThenTheRandomisationFlagIsDisabled()
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationDeterminismTests)}_locked_flag_value");
            environment.WriteInput(NameListFixtures.DeterministicHeaderDocument);

            environment.Generate(OutputFileName, DisplayName, isLocked: true);

            Assert.That(environment.ReadText(OutputFileName), Does.Contain("randomized = no"));
        }

        private static string Normalise(string content, string identifier)
            => content
                .Replace($"### {identifier}", "### IDENTIFIER", System.StringComparison.Ordinal)
                .Replace($"{identifier} = {{", "IDENTIFIER = {", System.StringComparison.Ordinal);
    }
}
