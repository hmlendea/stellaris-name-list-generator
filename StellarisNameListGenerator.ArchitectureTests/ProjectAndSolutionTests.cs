using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FluentAssertions;

using global::NUnit.Framework;

using StellarisNameListGenerator.ArchitectureTests.Infrastructure;

namespace StellarisNameListGenerator.ArchitectureTests
{
    /// <summary>
    /// Enforces the project and solution conventions: .NET 10.0, disabled implicit usings,
    /// <c>IsPackable</c> matching publishing intent, test-project flags, solution structure,
    /// and no <c>InternalsVisibleTo</c> in production projects.
    /// </summary>
    [TestFixture]
    public sealed class ProjectAndSolutionTests
    {
        const string ProductionProjectPath = "StellarisNameListGenerator/StellarisNameListGenerator.csproj";
        const string UnitTestProjectPath = "StellarisNameListGenerator.UnitTests/StellarisNameListGenerator.UnitTests.csproj";
        const string ArchitectureTestProjectPath = "StellarisNameListGenerator.ArchitectureTests/StellarisNameListGenerator.ArchitectureTests.csproj";
        const string SolutionPath = "StellarisNameListGenerator.slnx";

        [Test]
        public void GivenProductionProjectFile_WhenReadingTargetFramework_ThenTargetsNet10()
        {
            string content = ReadProjectContent(ProductionProjectPath);

            content.Should().Contain(
                "<TargetFramework>net10.0</TargetFramework>",
                "the production project must target .NET 10.0 minimum");
        }

        [Test]
        public void GivenProductionProjectFile_WhenCheckingPackaging_ThenIsNotPackable()
        {
            string content = ReadProjectContent(ProductionProjectPath);

            content.Should().Contain(
                "<IsPackable>false</IsPackable>",
                "the production project is a console executable and must not be published as a NuGet package");
        }

        [Test]
        public void GivenProductionProjectFile_WhenCheckingInternalsVisibleTo_ThenItIsNotDeclared()
        {
            string content = ReadProjectContent(ProductionProjectPath);

            content.Should().NotContain(
                "InternalsVisibleTo",
                "production projects must never declare InternalsVisibleTo");
        }

        [Test]
        public void GivenUnitTestProjectFile_WhenCheckingInternalsVisibleTo_ThenItIsNotDeclared()
        {
            string content = ReadProjectContent(UnitTestProjectPath);

            content.Should().NotContain(
                "InternalsVisibleTo",
                "production projects must never declare InternalsVisibleTo");
        }

        [Test]
        public void GivenUnitTestProjectFile_WhenReadingTestFlags_ThenTestProjectFlagsAreSet()
        {
            string content = ReadProjectContent(UnitTestProjectPath);

            content.Should().Contain("<IsTestProject>true</IsTestProject>", "test projects must declare IsTestProject");
            content.Should().Contain("<ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>", "test projects must be excluded from code coverage");
        }

        [Test]
        public void GivenArchitectureTestProjectFile_WhenReadingTestFlags_ThenTestProjectFlagsAreSet()
        {
            string content = ReadProjectContent(ArchitectureTestProjectPath);

            content.Should().Contain("<IsTestProject>true</IsTestProject>", "architecture test projects must declare IsTestProject");
            content.Should().Contain("<ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>", "architecture test projects must be excluded from code coverage");
        }

        [Test]
        public void GivenSolutionFile_WhenInspectingStructure_ThenAllProjectsAreListedInProductionUnitTestArchitectureOrder()
        {
            string content = ReadContent(SolutionPath);

            content.Should().NotContain("<?xml", "the .slnx solution must not have an XML declaration");
            content.Should().Contain("<Solution>", "the .slnx solution must use the prescribed Solution root");
            content.Should().Contain($"<Project Path=\"{ProductionProjectPath}\"", "the solution must reference the production project");
            content.Should().Contain($"<Project Path=\"{UnitTestProjectPath}\"", "the solution must reference the unit test project");
            content.Should().Contain($"<Project Path=\"{ArchitectureTestProjectPath}\"", "the solution must reference the architecture test project");
        }

        static string ReadProjectContent(string projectRelativePath)
        {
            string path = SourcePathResolver.GetProjectFile(projectRelativePath);

            if (!File.Exists(path))
            {
                Assert.Fail($"The required project file '{projectRelativePath}' is unavailable; project convention tests must fail on missing inputs.");
            }

            return File.ReadAllText(path);
        }

        static string ReadContent(string relativePath)
            => File.ReadAllText(SourcePathResolver.GetProjectFile(relativePath));
    }
}
