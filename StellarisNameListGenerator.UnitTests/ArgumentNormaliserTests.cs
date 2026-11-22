using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace StellarisNameListGenerator.UnitTests
{
    /// <summary>
    /// Exercises the argument normaliser directly: every alias form, the boolean lock emulation, and the
    /// pass-through of unrecognised arguments.
    /// </summary>
    [TestFixture]
    public sealed class ArgumentNormaliserTests
    {
        [Test]
        public void GivenTheShortInputAlias_WhenNormalising_ThenItBecomesTheLongForm()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-i", "values.xml"]),
                Is.EqualTo(new[] { "--input", "values.xml" }));

        [Test]
        public void GivenTheLongInputAlias_WhenNormalising_ThenItIsUnchanged()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--input", "values.xml"]),
                Is.EqualTo(new[] { "--input", "values.xml" }));

        [Test]
        public void GivenTheShortOutputAlias_WhenNormalising_ThenItBecomesTheLongForm()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-o", "names.txt"]),
                Is.EqualTo(new[] { "--output", "names.txt" }));

        [Test]
        public void GivenTheLongOutputAlias_WhenNormalising_ThenItIsUnchanged()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--output", "names.txt"]),
                Is.EqualTo(new[] { "--output", "names.txt" }));

        [Test]
        public void GivenTheShortNameAlias_WhenNormalising_ThenItBecomesTheLongForm()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-n", "Nucilandia"]),
                Is.EqualTo(new[] { "--name", "Nucilandia" }));

        [Test]
        public void GivenTheLongNameAlias_WhenNormalising_ThenItIsUnchanged()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--name", "Nucilandia"]),
                Is.EqualTo(new[] { "--name", "Nucilandia" }));

        [Test]
        public void GivenTheBareShortLockedAlias_WhenNormalising_ThenTrueIsAppended()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-l"]),
                Is.EqualTo(new[] { "--locked", "true" }));

        [Test]
        public void GivenTheBareLongLockedAlias_WhenNormalising_ThenTrueIsAppended()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--locked"]),
                Is.EqualTo(new[] { "--locked", "true" }));

        [Test]
        public void GivenTheShortLockedAliasWithAValue_WhenNormalising_ThenTheValueIsPreserved()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-l", "false"]),
                Is.EqualTo(new[] { "--locked", "false" }));

        [Test]
        public void GivenTheLongLockedAliasWithAValue_WhenNormalising_ThenTheValueIsPreserved()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--locked", "false"]),
                Is.EqualTo(new[] { "--locked", "false" }));

        [Test]
        public void GivenALockedAliasFollowedByAnOption_WhenNormalising_ThenTrueIsInserted()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-l", "-n", "Nucilandia"]),
                Is.EqualTo(new[] { "--locked", "true", "--name", "Nucilandia" }));

        [Test]
        public void GivenAnUnrecognisedArgument_WhenNormalising_ThenItIsPassedThrough()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--extra", "value"]),
                Is.EqualTo(new[] { "--extra", "value" }));

        [Test]
        public void GivenNoArguments_WhenNormalising_ThenAnEmptyArrayIsReturned()
            => Assert.That(ArgumentNormaliser.Normalise([]), Is.Empty);

        [Test]
        public void GivenTheCompleteShortForm_WhenNormalising_ThenEveryArgumentIsExpanded()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-i", "in.xml", "-o", "out.txt", "-n", "Nucilandia", "-l"]),
                Is.EqualTo(new[] { "--input", "in.xml", "--output", "out.txt", "--name", "Nucilandia", "--locked", "true" }));

        [Test]
        public void GivenTheCompleteLongForm_WhenNormalising_ThenEveryArgumentIsUnchanged()
            => Assert.That(
                ArgumentNormaliser.Normalise(["--input", "in.xml", "--output", "out.txt", "--name", "Nucilandia", "--locked"]),
                Is.EqualTo(new[] { "--input", "in.xml", "--output", "out.txt", "--name", "Nucilandia", "--locked", "true" }));

        [Test]
        public void GivenTheShortInputAliasWithoutAValue_WhenNormalising_ThenArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ArgumentNormaliser.Normalise(["-i"]),
                Throws.TypeOf<ArgumentNullException>());

        [Test]
        public void GivenTheShortOutputAliasWithoutAValue_WhenNormalising_ThenArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ArgumentNormaliser.Normalise(["-o"]),
                Throws.TypeOf<ArgumentNullException>());

        [Test]
        public void GivenTheShortNameAliasWithoutAValue_WhenNormalising_ThenArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ArgumentNormaliser.Normalise(["-n"]),
                Throws.TypeOf<ArgumentNullException>());

        [Test]
        public void GivenAValueStartingWithADash_WhenNormalising_ThenItIsTreatedAsTheValue()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-n", "-dashed"]),
                Is.EqualTo(new[] { "--name", "-dashed" }));

        [Test]
        public void GivenRepeatedAliases_WhenNormalising_ThenEveryOccurrenceIsExpanded()
            => Assert.That(
                ArgumentNormaliser.Normalise(["-n", "first", "-n", "second"]),
                Is.EqualTo(new[] { "--name", "first", "--name", "second" }));

        [TestCase("-i", "--input")]
        [TestCase("-o", "--output")]
        [TestCase("-n", "--name")]
        public void GivenAValueAlias_WhenNormalising_ThenTheLongFormIsAlwaysProduced(string alias, string expected)
        {
            IReadOnlyList<string> normalised = ArgumentNormaliser.Normalise([alias, "value"]);

            Assert.That(normalised[0], Is.EqualTo(expected));
            Assert.That(normalised[1], Is.EqualTo("value"));
        }
    }
}
