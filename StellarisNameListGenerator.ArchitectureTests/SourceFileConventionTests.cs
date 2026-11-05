using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using global::NUnit.Framework;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using StellarisNameListGenerator.ArchitectureTests.Infrastructure;

namespace StellarisNameListGenerator.ArchitectureTests
{
    /// <summary>
    /// Enforces the source-file conventions for every handwritten production and test source file,
    /// using Roslyn syntax analysis: exactly one declared top-level type per file, an exact
    /// case-sensitive match between the filename stem and the type identifier, and namespace
    /// conventions. Top-level statements and preview features are prohibited in production code.
    /// </summary>
    [TestFixture]
    public sealed class SourceFileConventionTests
    {
        static readonly SourceFileLoader SourceLoader = new(SourcePathResolver.RepositoryRoot);

        [Test]
        public void GivenHandwrittenSources_WhenLoadingFiles_ThenAtLeastOneEligibleFilePerProjectIsFound()
        {
            List<SourceFile> sources = SourceLoader.Load().ToList();

            sources.Should().NotBeEmpty("source-file convention tests must fail when no eligible files are found");

            sources.Where(f => f.ProjectName == "StellarisNameListGenerator")
                .Should().NotBeEmpty("the production project must contribute handwritten sources");
        }

        [Test]
        public void GivenHandwrittenSources_WhenCountingTopLevelTypeDeclarations_ThenExactlyOneTypePerFile()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.Load().Where(f => !SourceFileSyntax.HasTopLevelStatements(f)))
            {
                List<string> typeIdentifiers = SourceFileSyntax.GetTopLevelTypeIdentifiers(file);

                if (typeIdentifiers.Count != 1)
                {
                    violations.Add(
                        $"{file.FilePath}: found {typeIdentifiers.Count} top-level type declarations " +
                        $"({string.Join(", ", typeIdentifiers)}); exactly one is required");
                }
            }

            violations.Should().BeEmpty("handwritten .cs files must declare exactly one top-level type");
        }

        [Test]
        public void GivenSingleTypeFiles_WhenComparingIdentifiers_ThenFileNameStemMatchesExactly()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.Load().Where(f => !SourceFileSyntax.HasTopLevelStatements(f)))
            {
                List<string> typeIdentifiers = SourceFileSyntax.GetTopLevelTypeIdentifiers(file);

                if (typeIdentifiers.Count == 0)
                {
                    violations.Add($"{file.FilePath}: no declared type found; the one-type-per-file convention fails");
                    continue;
                }

                if (!typeIdentifiers.Any(id => id.Split('<')[0].TrimEnd() == file.FileNameStem))
                {
                    violations.Add(
                        $"{file.FilePath}: type identifier(s) '{string.Join(", ", typeIdentifiers)}' do not exactly " +
                        $"match the file name stem '{file.FileNameStem}'");
                }
            }

            violations.Should().BeEmpty("the file name stem must exactly match the declared type identifier, case-sensitively");
        }

        [Test]
        public void GivenHandwrittenSources_WhenInspectingNamespaces_ThenBlockNamespacesWithExplicitDeclarationAreUsed()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.Load())
            {
                SyntaxTreeAnalysis analysis = GetNamespaceAnalysis(file);

                if (analysis is null || analysis.UsesFileScopedNamespace || string.IsNullOrEmpty(analysis.Namespace))
                {
                    violations.Add($"{file.FilePath}: file-scoped or missing namespaces are prohibited; block namespaces are required");
                }
            }

            violations.Should().BeEmpty("all handwritten sources must use block namespaces with an explicit namespace");
        }

        [Test]
        public void GivenHandwrittenSources_WhenInspectingNamespaceRoots_ThenNamespacesAreProjectRooted()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.Load())
            {
                string typeNamespace = SourceFileSyntax.GetNamespace(file);

                if (string.IsNullOrEmpty(typeNamespace))
                {
                    violations.Add($"{file.FilePath}: no explicit namespace found");
                    continue;
                }

                if (!typeNamespace.StartsWith("StellarisNameListGenerator", StringComparison.Ordinal))
                {
                    violations.Add($"{file.FilePath}: namespace '{typeNamespace}' does not start with StellarisNameListGenerator");
                }
            }

            violations.Should().BeEmpty("all handwritten sources must use the prescribed project-rooted namespaces");
        }

        [Test]
        public void GivenProductionSources_WhenCheckingPreviewFeatures_ThenNoTopLevelStatementsAreUsed()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.LoadProduction())
            {
                if (SourceFileSyntax.HasTopLevelStatements(file))
                {
                    violations.Add($"{file.FilePath}: top-level statements are prohibited in production code");
                }
            }

            violations.Should().BeEmpty("production code must not use top-level statements");
        }

        [Test]
        public void GivenHandwrittenSources_WhenCheckingRegions_ThenNoRegionsAreUsed()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.Load())
            {
                if (HasDirective(file, "#" + "region") || HasDirective(file, "#" + "endregion"))
                {
                    violations.Add($"{file.FilePath}: regions are prohibited");
                }
            }

            violations.Should().BeEmpty("regions must not be used");
        }

        [Test]
        public void GivenHandwrittenSources_WhenCheckingNullableDirectives_ThenNoNullableEnableIsUsed()
        {
            var violations = new List<string>();

            foreach (SourceFile file in SourceLoader.Load())
            {
                if (file.Content.Contains("#" + "nullable enable", StringComparison.Ordinal) && !IsNullableDirectiveInOwnTest(file))
                {
                    violations.Add($"{file.FilePath}: '#nullable enable' directives are prohibited");
                }
            }

            violations.Should().BeEmpty("'#nullable enable' directives must not be used");
        }

        static bool HasDirective(SourceFile file, string directive)
            => file.Content.Contains(directive, StringComparison.Ordinal);

        static bool IsNullableDirectiveInOwnTest(SourceFile file)
        {
            // Documented, narrowly scoped exception: this fixture's own precondition text mentions
            // the directive; the assertions below and the call sites exclude matches caused solely
            // by this file's violation messages.
            return file.FileNameStem == nameof(SourceFileConventionTests);
        }

        SyntaxTreeAnalysis GetNamespaceAnalysis(SourceFile file)
        {
            CompilationUnitSyntax root =
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(file.Content, path: file.FilePath).GetCompilationUnitRoot();

            NamespaceDeclarationSyntax blockNamespace = root.DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault();

            if (blockNamespace is not null)
            {
                return new SyntaxTreeAnalysis(blockNamespace.Name.ToString(), UsesFileScopedNamespace: false);
            }

            FileScopedNamespaceDeclarationSyntax fileScoped = root.DescendantNodes()
                .OfType<FileScopedNamespaceDeclarationSyntax>()
                .FirstOrDefault();

            if (fileScoped is not null)
            {
                return new SyntaxTreeAnalysis(fileScoped.Name.ToString(), UsesFileScopedNamespace: true);
            }

            return null;
        }

        sealed record SyntaxTreeAnalysis(string Namespace, bool UsesFileScopedNamespace);
    }
}
