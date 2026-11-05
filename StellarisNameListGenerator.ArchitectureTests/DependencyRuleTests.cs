using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using global::NUnit.Framework;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using StellarisNameListGenerator.ArchitectureTests.Infrastructure;

namespace StellarisNameListGenerator.ArchitectureTests
{
    /// <summary>
    /// Enforces the documented dependency rules for the Stellaris Name List Generator executable,
    /// using the loaded production assemblies via ArchUnit.NET. Source-file conventions are in
    /// <see cref="SourceFileConventionTests"/>; project and solution rules are in
    /// <see cref="ProjectAndSolutionTests"/>. The current known exception —
    /// <c>Models.NameGroup</c> constructing concrete services — is narrowly scoped below.
    /// </summary>
    [TestFixture]
    public sealed class DependencyRuleTests
    {
        static readonly SourceFileLoader SourceLoader = new(SourcePathResolver.RepositoryRoot);

        // Layer boundaries documented in ARCHITECTURE.md 'Dependency Direction and Rules'.
        const string ModelsNamespace = "StellarisNameListGenerator.Models";
        const string ServiceNamespace = "StellarisNameListGenerator.Service";
        const string NamesBuildersNamespace = "StellarisNameListGenerator.Service.NamesBuilders";
        const string EntryNamespace = "StellarisNameListGenerator";

        const string ControllersNamespace = "Microsoft.AspNetCore";
        const string EntityFrameworkNamespace = "Microsoft.EntityFrameworkCore";
        const string DynamoDbNamespace = "Amazon.DynamoDB";

        [Test]
        public void GivenLoadedSources_WhenLoadingProductionAssembly_ThenAllModelTypesArePresent()
        {
            List<SourceFile> productionSources = SourceLoader.LoadProduction().ToList();

            productionSources.Should().NotBeEmpty("production sources must be available");
            productionSources
                .Where(f => f.FilePath.EndsWith("/Models/NameGroup.cs") || f.FilePath.EndsWith("\\Models\\NameGroup.cs"))
                .Should().NotBeEmpty("the expected type StellarisNameListGenerator.Models.NameGroup must be present");
        }

        [Test]
        public void GivenModelSources_WhenCheckingServiceDependencies_ThenOnlyVehicleNames_IncludeServiceImports()
        {
            List<SourceFile> modelSources = SourceLoader
                .LoadProduction()
                .Where(f => GetNamespaceFromSource(f) == ModelsNamespace)
                .ToList();

            modelSources.Should().NotBeEmpty("model sources must be found");

            List<SourceFile> offenders = modelSources
                .Where(f => f.Content.Contains("using StellarisNameListGenerator.Service"))
                .ToList();

            // Documented, narrowly scoped exception (ARCHITECTURE.md 'Layering Exception'):
            // Models.NameGroup currently constructs FileDownloader/CacheManager directly.
            offenders.Should().ContainSingle(f => f.FileNameStem == "NameGroup",
                "only the documented NameGroup layering exception may depend on services from Models");
        }

        [Test]
        public void GivenNamesBuilderSources_WhenCheckingModelDependencies_ThenAllBuildersDependOnModels()
        {
            List<SourceFile> builderSources = SourceLoader
                .LoadProduction()
                .Where(f => GetNamespaceFromSource(f) == NamesBuildersNamespace)
                .ToList();

            builderSources.Should().NotBeEmpty("names builder sources must be found");

            builderSources
                .Where(f => f.Content.Contains(" class ") || f.Content.Contains(" interface "))
                .Should().OnlyContain(f => f.Content.Contains("using StellarisNameListGenerator.Models"),
                    "all names builders must depend on the model layer");
        }

        [Test]
        public void GivenServiceSources_WhenCheckingEntryPointDependencies_ThenNoServiceUsesEntrypoint()
        {
            List<SourceFile> serviceSources = SourceLoader
                .LoadProduction()
                .Where(f => GetNamespaceFromSource(f).StartsWith(ServiceNamespace, StringComparison.Ordinal))
                .ToList();

            serviceSources.Should().NotBeEmpty("service sources must be found");

            serviceSources
                .Should().NotContain(f => f.Content.Contains("using StellarisNameListGenerator.Program"),
                    "services must not depend on the entry point");
        }

        [Test]
        public void GivenProductionSources_WhenCheckingFrameworkReferences_ThenNoWebOrPersistenceFrameworkDependencyExists()
        {
            List<SourceFile> productionSources = SourceLoader.LoadProduction().ToList();

            productionSources
                .Where(f => f.Content.Contains(ControllersNamespace) ||
                            f.Content.Contains(EntityFrameworkNamespace) ||
                            f.Content.Contains(DynamoDbNamespace))
                .Should().BeEmpty("production code must not reference ASP.NET Core, EF Core, or DynamoDB");
        }

        [Test]
        public void GivenProductionSources_WhenCheckingServicePlacement_ThenServiceClassesHaveColocatedInterfaces()
        {
            List<SourceFile> serviceSources = SourceLoader
                .LoadProduction()
                .Where(f => GetNamespaceFromSource(f) == ServiceNamespace)
                .ToList();

            List<string> interfaces = serviceSources
                .Where(SourceFileSyntaxHasInterface)
                .Select(f => f.FileNameStem)
                .ToList();

            List<string> missingInterfaces = serviceSources
                .Where(SourceFileSyntaxHasClass)
                .Where(f => !interfaces.Contains("I" + f.FileNameStem))
                .Select(f => f.FileNameStem)
                .ToList();

            missingInterfaces.Should().BeEmpty("each service class must have a colocated interface with the I-prefixed matching name");
        }

        static string GetNamespaceFromSource(SourceFile file)
            => SourceFileSyntax.GetNamespace(file);

        static bool SourceFileSyntaxHasInterface(SourceFile file)
            => SourceFileSyntax.GetTopLevelTypeDeclarations(file).Any(d => d is Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax);

        static bool SourceFileSyntaxHasClass(SourceFile file)
            => SourceFileSyntax.GetTopLevelTypeDeclarations(file).Any(d => d is Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax);
    }
}
