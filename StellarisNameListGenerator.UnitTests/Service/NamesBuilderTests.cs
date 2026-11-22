using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service.NamesBuilders;

namespace StellarisNameListGenerator.UnitTests.Service
{
    /// <summary>
    /// Exercises each category builder directly, asserting the emitted section envelope, the merging of
    /// explicit values, the exclusion rules, and the derived-name expansion.
    /// </summary>
    [TestFixture]
    public sealed class NamesBuilderTests
    {
        [Test]
        public void GivenAShipNamesBuilder_WhenBuilding_ThenTheSectionEnvelopeIsEmitted()
        {
            string content = new ShipNamesBuilder().Build(EmptyNameList());

            Assert.That(content, Does.StartWith("    ship_names = {"));
            Assert.That(content.TrimEnd(), Does.EndWith("}"));
        }

        [Test]
        public void GivenAShipNamesBuilderWithNoValues_WhenBuilding_ThenNoSubBlocksAreEmitted()
        {
            string content = new ShipNamesBuilder().Build(EmptyNameList());

            Assert.That(content, Does.Not.Contain("generic = {"));
            Assert.That(content, Does.Not.Contain("corvette = {"));
        }

        [Test]
        public void GivenAGenericShipValue_WhenBuilding_ThenTheValueIsEmittedUnderTheGenericCategory()
        {
            NameList nameList = EmptyNameList();
            nameList.Ships.Generic.Add(Group("Generic"));

            string content = new ShipNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("generic = {"));
            Assert.That(content, Does.Contain("Generic"));
        }

        [Test]
        public void GivenADenonymValue_WhenBuilding_ThenTheGenericCategoryExcludesItFromSpecialisedCategories()
        {
            NameList nameList = EmptyNameList();
            nameList.Denonyms.Add(Group("Denonym"));

            string content = new ShipNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("Denonym"));
            Assert.That(content, Does.Not.Contain("corvette = {"));
        }

        [Test]
        public void GivenAVesselNameWithSpaces_WhenBuilding_ThenTheValueIsQuoted()
        {
            NameList nameList = EmptyNameList();
            nameList.Ships.Generic.Add(Group("Nuci Corps"));

            string content = new ShipNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("\"Nuci Corps\""));
        }

        [Test]
        public void GivenADiacriticName_WhenBuilding_ThenTheValueIsTransliterated()
        {
            NameList nameList = EmptyNameList();
            nameList.Ships.Generic.Add(Group("Çupișan"));

            string content = new ShipNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("Çupisan"));
            Assert.That(content, Does.Contain("# >>> Çupișan"));
        }

        [Test]
        public void GivenAShipNamesBuilder_WhenRequestingARandomName_ThenAnExistingValueIsReturned()
        {
            NameList nameList = EmptyNameList();
            nameList.Ships.Generic.Add(Group("Zephyr"));

            string randomName = new ShipNamesBuilder().GetRandomName(nameList);

            Assert.That(randomName, Is.EqualTo("Zephyr"));
        }

        [Test]
        public void GivenAShipNamesBuilderWithNoValues_WhenRequestingARandomName_ThenAnEmptyStringIsReturned()
            => Assert.That(new ShipNamesBuilder().GetRandomName(EmptyNameList()), Is.Empty);

        [Test]
        public void GivenAShipClassNamesBuilderWithNoValues_WhenBuilding_ThenAnEmptyStringIsReturned()
            => Assert.That(new ShipClassNamesBuilder().Build(EmptyNameList()), Is.Empty);

        [Test]
        public void GivenAShipClassValue_WhenBuilding_ThenTheClassSectionEnvelopeIsEmitted()
        {
            NameList nameList = EmptyNameList();
            nameList.ShipClasses.Generic.Add(Group("Reliant"));

            string content = new ShipClassNamesBuilder().Build(nameList);

            Assert.That(content, Does.StartWith("    ship_class_names = {"));
            Assert.That(content, Does.Contain("Reliant"));
        }

        [Test]
        public void GivenARangedWeapon_WhenBuildingShipClasses_ThenItIsExcludedFromTheGenericClassCategory()
        {
            NameList nameList = EmptyNameList();
            nameList.Warfare.Weapons.Artillery.Add(Group("Ballista"));

            string content = new ShipClassNamesBuilder().Build(nameList);

            Assert.That(content, Does.Not.Contain("generic = {"));
        }

        [Test]
        public void GivenAFleetNamesBuilderWithNoValues_WhenBuilding_ThenTheSequentialFallbackIsEmitted()
        {
            string content = new FleetNamesBuilder().Build(EmptyNameList());

            Assert.That(content, Does.Contain("fleet_names = {"));
            Assert.That(content, Does.Contain("sequential_name = \"%O% Fleet\""));
        }

        [Test]
        public void GivenAnExplicitFleetName_WhenBuilding_ThenItAppearsUnderItsOwnCategory()
        {
            NameList nameList = EmptyNameList();
            nameList.Armies.Fleet.Add(Group("Home Fleet"));

            string content = new FleetNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("\"Home Fleet\""));
            Assert.That(content, Does.Contain("# >>> Home Fleet"));
        }

        [Test]
        public void GivenAWeaponValue_WhenBuildingFleets_ThenEveryTemplateUsesIt()
        {
            NameList nameList = EmptyNameList();
            nameList.Warfare.Weapons.Swords.Add(Group("Shotel"));

            string content = new FleetNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("The Shotel Armada"));
            Assert.That(content, Does.Contain("Strike Force Shotel"));
        }

        [Test]
        public void GivenAFleetNamesBuilder_WhenRequestingARandomName_ThenAValueFromThePoolIsReturned()
        {
            NameList nameList = EmptyNameList();
            nameList.Warfare.Weapons.Swords.Add(Group("Shotel"));

            string randomName = new FleetNamesBuilder().GetRandomName(nameList);

            Assert.That(randomName, Does.Contain("Shotel"));
        }

        [Test]
        public void GivenAnArmyNamesBuilderWithNoValues_WhenBuilding_ThenAllArmyBlocksFoldToSequentialTemplates()
        {
            string content = new ArmyNamesBuilder().Build(EmptyNameList());

            Assert.That(content, Does.Contain("army_names = {"));
            Assert.That(content, Does.Contain("sequential_name = \"%O% Planetary Guard\""));
            Assert.That(content, Does.Contain("sequential_name = \"%O% Expeditionary Force\""));
            Assert.That(content, Does.Contain("sequential_name = \"Primitive Army %C%\""));
        }

        [Test]
        public void GivenAMythologicalCreature_WhenBuildingArmies_ThenEveryPsionicVariantIsGenerated()
        {
            NameList nameList = EmptyNameList();
            nameList.BiosphereNames.MythologicalCreatures.Add(Group("Griffin"));

            string content = new ArmyNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("\"Griffin Commando\""));
            Assert.That(content, Does.Contain("\"Griffin Legion\""));
        }

        [Test]
        public void GivenADeathDeity_WhenBuildingArmies_ThenEveryXenomorphCombinationIsGenerated()
        {
            NameList nameList = EmptyNameList();
            nameList.GreatPeople.DeathDeities.Add(Group("Mors"));

            string content = new ArmyNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("\"Mors's Abomination Brood\""));
            Assert.That(content, Does.Contain("\"Mors's Xenomorph Warband\""));
        }

        [Test]
        public void GivenAPlanetNamesBuilderWithNoValues_WhenBuilding_ThenTheEmptySectionEnvelopeIsEmitted()
        {
            string content = new PlanetNamesBuilder().Build(EmptyNameList());

            Assert.That(content.TrimEnd(), Does.EndWith("}"));
            Assert.That(content, Does.Contain("planet_names = {"));
        }

        [Test]
        public void GivenADesertPlanetValue_WhenBuilding_ThenItIsMappedToTheDesertClass()
        {
            NameList nameList = EmptyNameList();
            nameList.Planets.Desert.Add(Group("Dune"));

            string content = new PlanetNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("pc_desert = {"));
            Assert.That(content, Does.Contain("Dune"));
        }

        [Test]
        public void GivenATombPlanetValue_WhenBuilding_ThenItIsMappedToTheNukedClass()
        {
            NameList nameList = EmptyNameList();
            nameList.Planets.Tomb.Add(Group("Tombia"));

            string content = new PlanetNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("pc_nuked = {"));
        }

        [Test]
        public void GivenAClassSpecificPlanetValue_WhenBuilding_ThenTheGenericClassExcludesIt()
        {
            NameList nameList = EmptyNameList();
            nameList.Planets.Generic.Add(Group("Dune"));
            nameList.Planets.Desert.Add(Group("Dune"));

            string content = new PlanetNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("pc_desert = {"));
            Assert.That(content, Does.Not.Contain("generic = {"));
        }

        [Test]
        public void GivenAPlanetNamesBuilder_WhenRequestingARandomName_ThenAnExistingValueIsReturned()
        {
            NameList nameList = EmptyNameList();
            nameList.Planets.Generic.Add(Group("Solara"));

            Assert.That(new PlanetNamesBuilder().GetRandomName(nameList), Is.EqualTo("Solara"));
        }

        [Test]
        public void GivenACharacterNamesBuilderWithNoCharacters_WhenBuilding_ThenAnEmptyStringIsReturned()
            => Assert.That(new CharacterNamesBuilder().Build(EmptyNameList()), Is.Empty);

        [Test]
        public void GivenAWeightedCharacter_WhenBuilding_ThenTheWeightAndCategoriesAreEmitted()
        {
            NameList nameList = EmptyNameList();
            nameList.Characters.Add(new CharacterNames
            {
                Id = "alpha",
                Weight = 42,
                FemaleFirstNames = [Group("Female first names")],
                SecondNames = [Group("Second names")],
            });

            string content = new CharacterNamesBuilder().Build(nameList);

            Assert.That(content, Does.Contain("alpha = {"));
            Assert.That(content, Does.Contain("weight = 42"));
            Assert.That(content, Does.Contain("first_names_female = {"));
            Assert.That(content, Does.Contain("second_names = {"));
        }

        [Test]
        public void GivenMultipleCharacters_WhenBuilding_ThenTheyAreOrderedByDescendingWeight()
        {
            NameList nameList = EmptyNameList();
            nameList.Characters.Add(new CharacterNames { Id = "light", Weight = 1, SecondNames = [Group("Second names")] });
            nameList.Characters.Add(new CharacterNames { Id = "heavy", Weight = 873, SecondNames = [Group("Second names")] });

            string content = new CharacterNamesBuilder().Build(nameList);

            Assert.That(content.IndexOf("heavy = {", System.StringComparison.Ordinal), Is.LessThan(content.IndexOf("light = {", System.StringComparison.Ordinal)));
        }

        [Test]
        public void GivenACharacterNamesBuilder_WhenRequestingARandomName_ThenAConfiguredNameIsReturned()
        {
            NameList nameList = EmptyNameList();
            nameList.Characters.Add(new CharacterNames
            {
                Id = "alpha",
                Weight = 1,
                FullNames = [Group("John Doe")],
            });

            Assert.That(new CharacterNamesBuilder().GetRandomName(nameList), Is.EqualTo("John Doe"));
        }

        [Test]
        public void GivenACharacterNamesBuilderWithNoCharacters_WhenRequestingARandomName_ThenAnEmptyStringIsReturned()
            => Assert.That(new CharacterNamesBuilder().GetRandomName(EmptyNameList()), Is.Empty);

        private static NameList EmptyNameList()
            => new();

        private static NameGroup Group(string value)
            => new() { Name = value, ExplicitValues = [value] };
    }
}
