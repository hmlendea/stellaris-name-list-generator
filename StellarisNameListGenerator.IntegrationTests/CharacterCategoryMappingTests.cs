using NUnit.Framework;

using StellarisNameListGenerator.IntegrationTests.Fixtures;
using StellarisNameListGenerator.IntegrationTests.Tools;

namespace StellarisNameListGenerator.IntegrationTests
{
    /// <summary>
    /// Covers every character name category: the mapping of each input list to its Paradox script key,
    /// the ordering of the keys within a character block, and the weight attribute.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class CharacterCategoryMappingTests
    {
        private const string OutputFileName = "character_categories.txt";
        private const string DisplayName = "Character Categories";

        private GeneratedNameList output = null!;

        [SetUp]
        public void SetUp()
        {
            using GeneratorEnvironment environment = new(nameof(CharacterCategoryMappingTests));
            environment.WriteInput(NameListFixtures.AllCharacterCategoriesDocument);

            output = environment.Generate(OutputFileName, DisplayName);
        }

        [Test]
        public void GivenEveryCharacterCategory_WhenGenerating_ThenEachListIsEmitted()
        {
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.MaleFullName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.FirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.RegnalFirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.MaleFullEnglishName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.MaleFirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.MaleRegnalFirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.FemaleFullName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.FemaleFirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.FemaleRegnalFirstName), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.SecondSurname), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, NameListFixtures.RoyalSecondName), Is.True);
        }

        [Test]
        public void GivenEveryCharacterCategory_WhenGenerating_ThenEachListUsesItsDedicatedScriptKey()
        {
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "full_names = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "first_names = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "regnal_first_names = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "full_names_male = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "first_names_male = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "regnal_first_names_male = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "full_names_female = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "first_names_female = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "regnal_first_names_female = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "second_names = {"), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, "regnal_second_names = {"), Is.True);
        }

        [Test]
        public void GivenAWeightedCharacter_WhenGenerating_ThenTheWeightIsWrittenIntoTheBlock()
        {
            Assert.That(
                output.SubBlockContains(
                    "character_names",
                    NameListFixtures.LightCharacterId,
                    $"weight = {NameListFixtures.LightCharacterWeight}"),
                Is.True);
        }

        [Test]
        public void GivenSeveralCharacterCategories_WhenGenerating_ThenTheyAppearInTheFixedOrder()
        {
            int fullNamesIndex = IndexInCharacterBlock("full_names = {");
            int firstNamesIndex = IndexInCharacterBlock("first_names = {");
            int maleFirstNamesIndex = IndexInCharacterBlock("first_names_male = {");
            int femaleFirstNamesIndex = IndexInCharacterBlock("first_names_female = {");
            int secondNamesIndex = IndexInCharacterBlock("second_names = {");

            Assert.That(fullNamesIndex, Is.LessThan(firstNamesIndex));
            Assert.That(firstNamesIndex, Is.LessThan(maleFirstNamesIndex));
            Assert.That(maleFirstNamesIndex, Is.LessThan(femaleFirstNamesIndex));
            Assert.That(femaleFirstNamesIndex, Is.LessThan(secondNamesIndex));
        }

        [Test]
        public void GivenSpacedFullNames_WhenGenerating_ThenTheyAreQuoted()
        {
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, $"\"{NameListFixtures.MaleFullName}\""), Is.True);
            Assert.That(output.SubBlockContains("character_names", NameListFixtures.LightCharacterId, $"\"{NameListFixtures.FemaleFullName}\""), Is.True);
        }

        private int IndexInCharacterBlock(string value)
        {
            int index = 0;

            foreach (string line in output.GetBlockLines("character_names", NameListFixtures.LightCharacterId))
            {
                if (line.Contains(value, System.StringComparison.Ordinal))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}