using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers the station name and station class categories: the accumulation of generic military stations
    /// into each size category, the derived station classes, and the omission of unpopulated categories.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class StationNameGenerationTests
    {
        private const string OutputFileName = "stations.txt";
        private const string DisplayName = "Station List";

        [Test]
        public void GivenStationNames_WhenGenerating_ThenEachStationNameCategoryBecomesItsOwnSubBlock()
        {
            using GeneratorEnvironment environment = new($"{nameof(StationNameGenerationTests)}_names");
            environment.WriteInput(NameListFixtures.StationNameDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "mining_station", NameListFixtures.MiningStationName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "research_station", NameListFixtures.ResearchStationName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "observation_station", NameListFixtures.ObservationStationName), Is.True);
        }

        [Test]
        public void GivenGenericMilitaryStationNames_WhenGenerating_ThenTheyAccumulateIntoEverySizeCategory()
        {
            using GeneratorEnvironment environment = new($"{nameof(StationNameGenerationTests)}_military");
            environment.WriteInput(NameListFixtures.StationNameDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_names", "military_station_small", NameListFixtures.GenericStationName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_small", NameListFixtures.SmallStationName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_medium", NameListFixtures.MediumStationName), Is.True);
            Assert.That(output.SubBlockContains("ship_names", "military_station_large", NameListFixtures.LargeStationName), Is.True);
        }

        [Test]
        public void GivenStationClassNames_WhenGenerating_ThenEachClassCategoryBecomesItsOwnSubBlock()
        {
            using GeneratorEnvironment environment = new($"{nameof(StationNameGenerationTests)}_classes");
            environment.WriteInput(NameListFixtures.StationClassNameDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.SubBlockContains("ship_class_names", "military_station_small", NameListFixtures.SmallStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_medium", NameListFixtures.MediumStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_large", NameListFixtures.LargeStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_large", NameListFixtures.GenericStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "mining_station", NameListFixtures.MiningStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "research_station", NameListFixtures.ResearchStationClassName), Is.True);
            Assert.That(output.SubBlockContains("ship_class_names", "observation_station", NameListFixtures.ObservationStationClassName), Is.True);
        }

        [Test]
        public void GivenStationClassNames_WhenGenerating_ThenStationNameBlocksAreNotEmitted()
        {
            using GeneratorEnvironment environment = new($"{nameof(StationNameGenerationTests)}_classes_only");
            environment.WriteInput(NameListFixtures.StationClassNameDocument);

            GeneratedNameList output = environment.Generate(OutputFileName, DisplayName);

            Assert.That(output.BlockContains("ship_names", "mining_station"), Is.False);
            Assert.That(output.SubBlockContains("ship_class_names", "military_station_small", NameListFixtures.GenericStationClassName), Is.True);
        }

        [Test]
        public void GivenNoStationValues_WhenGenerating_ThenNoStationNameSubBlocksAreEmitted()
        {
            using GeneratorEnvironment environment = new($"{nameof(StationNameGenerationTests)}_empty");

            GeneratedNameList output = GeneratedNameList.Parse(
                environment.GenerateFromDocument(
                    NameListFixtures.MinimalDocument,
                    OutputFileName,
                    DisplayName));

            Assert.That(output.BlockContains("ship_names", "mining_station"), Is.False);
            Assert.That(output.BlockContains("ship_names", "research_station"), Is.False);
            Assert.That(output.BlockContains("ship_class_names", "military_station_small"), Is.False);
        }
    }
}
