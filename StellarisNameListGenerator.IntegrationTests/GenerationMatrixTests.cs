using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Drives independent end-to-end invocations across the cross-product of document shapes, lock flag
    /// forms, and output identifiers, asserting the invariants that must hold for every combination.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class GenerationMatrixTests
    {
        private const string DisplayName = "Matrix";

        private static readonly object[][] Documents =
        [
            new object[] { "empty", NameListFixtures.EmptyDocument },
            new object[] { "minimal", NameListFixtures.MinimalDocument },
            new object[] { "rich", NameListFixtures.RichDocument },
            new object[] { "merged", NameListFixtures.MergedDocument },
            new object[] { "ships", NameListFixtures.ShipCompositionDocument },
            new object[] { "classes", NameListFixtures.ShipClassCompositionDocument },
            new object[] { "fleets", NameListFixtures.FleetCompositionDocument },
            new object[] { "armies", NameListFixtures.ArmyCompositionDocument },
            new object[] { "planets", NameListFixtures.PlanetClassDocument },
            new object[] { "characters", NameListFixtures.CharacterDocument },
            new object[] { "diacritics", NameListFixtures.DiacriticDocument },
            new object[] { "unknown", NameListFixtures.UnknownElementDocument },
        ];

        private static readonly object[][] LockForms =
        [
            new object[] { "locked-short", new string[] { "-l" } },
            new object[] { "locked-long", new string[] { "--locked" } },
            new object[] { "locked-true", new string[] { "-l", "true" } },
            new object[] { "unlocked-false", new string[] { "-l", "false" } },
            new object[] { "default", new string[] { } },
        ];

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGeneratingLocked_ThenTheOutputIsBalancedAndAssignsTheIdentifier(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_{label}");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(document, "generated.txt", DisplayName));

            Assert.That(output.IsBalanced, Is.True, label);
            Assert.That(output.AssignmentLine, Is.EqualTo("generated = {"), label);
            Assert.That(output.HeaderLines[0], Is.EqualTo("### generated"), label);
            Assert.That(output.HeaderLines[1], Is.EqualTo($"### {DisplayName}"), label);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGenerating_ThenSixHeaderLinesAreAlwaysEmitted(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_headers_{label}");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(document, "generated.txt", DisplayName));

            Assert.That(output.HeaderLines, Has.Count.EqualTo(6), label);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGenerating_ThenEveryLineStaysWithinTheSupportedWidth(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_width_{label}");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(document, "generated.txt", DisplayName));

            Assert.That(output.Lines.Max(line => line.Length), Is.LessThanOrEqualTo(150), label);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGenerating_ThenTheOutputUsesLineFeedEndingsOnly(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_endings_{label}");

            string content = environment.GenerateFromDocument(document, "generated.txt", DisplayName);

            Assert.That(content, Does.Not.Contain("\r"), label);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGenerating_ThenTheOutputStartsWithAByteOrderMark(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_bom_{label}");
            environment.WriteInput(document);
            environment.Generate("generated.txt", DisplayName);

            byte[] bytes = environment.ReadBytes("generated.txt");

            Assert.That(bytes[..3], Is.EquivalentTo(new byte[] { 0xEF, 0xBB, 0xBF }), label);
        }

        [TestCaseSource(nameof(LockForms))]
        public void GivenAnyLockForm_WhenGenerating_ThenTheRandomisationFlagReflectsTheLockState(string label, string[] lockArguments)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_{label}");
            environment.WriteInput(NameListFixtures.MinimalDocument);

            List<string> arguments =
            [
                "-i", environment.GetPath(GeneratorEnvironment.InputFileName),
                "-o", environment.GetPath("locked.txt"),
                "-n", DisplayName,
            ];
            arguments.AddRange(lockArguments);

            environment.Run([.. arguments]);

            bool expectsLocked = label.Contains("locked", System.StringComparison.Ordinal) && !label.Contains("unlocked", System.StringComparison.Ordinal);
            string expectedFlag = expectsLocked ? "randomized = no" : "randomized = yes";

            Assert.That(environment.ReadText("locked.txt"), Does.Contain(expectedFlag), label);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGeneratingTwiceLocked_ThenTheOutputIsReproducible(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_repeat_{label}");
            environment.WriteInput(document);

            environment.Generate("generated.txt", DisplayName, isLocked: true);
            string first = NormaliseFleetSample(environment.ReadText("generated.txt"));

            environment.Generate("generated.txt", DisplayName, isLocked: true);
            string second = NormaliseFleetSample(environment.ReadText("generated.txt"));

            Assert.That(second, Is.EqualTo(first), label);
        }

        private static string NormaliseFleetSample(string content)
        {
            List<string> lines = [.. content.Split('\n')];
            string[] samplePrefixes = ["### Leaders: ", "### Ships: ", "### Fleets: ", "### Colonies: "];

            for (int index = 0; index < lines.Count; index++)
            {
                foreach (string prefix in samplePrefixes)
                {
                    if (lines[index].StartsWith(prefix, System.StringComparison.Ordinal))
                    {
                        lines[index] = $"{prefix}<sample>";
                    }
                }
            }

            return string.Join('\n', lines);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGeneratingToANestedPath_ThenTheFileIsWrittenThere(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_nested_{label}");
            environment.WriteInput(document);
            Directory.CreateDirectory(environment.GetPath("nested"));

            environment.Generate("nested/generated.txt", DisplayName);

            Assert.That(File.Exists(environment.GetPath("nested/generated.txt")), Is.True, label);
        }

        [TestCaseSource(nameof(Documents))]
        public void GivenADocument_WhenGenerating_ThenTheTopLevelCategoryOrderIsStable(string label, string document)
        {
            using GeneratorEnvironment environment = new($"{nameof(GenerationMatrixTests)}_order_{label}");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(document, "generated.txt", DisplayName));

            List<string> expectedOrder = ["ship_names", "ship_class_names", "fleet_names", "army_names", "planet_names", "character_names"];
            List<string> emitted = [.. output.TopLevelBlocks];

            int previous = -1;

            foreach (string block in emitted)
            {
                int position = expectedOrder.IndexOf(block);

                Assert.That(position, Is.GreaterThan(previous), $"{label}: {block} out of order");
                previous = position;
            }
        }
    }
}
