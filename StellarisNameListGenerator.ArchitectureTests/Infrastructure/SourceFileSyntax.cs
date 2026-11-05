using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using StellarisNameListGenerator.ArchitectureTests.Infrastructure;

namespace StellarisNameListGenerator.ArchitectureTests.Infrastructure
{
    /// <summary>
    /// Provides Roslyn syntax analysis over the handwritten source files,
    /// including exact, case-sensitive type-declaration extraction.
    /// </summary>
    public static class SourceFileSyntax
    {
        public static List<string> GetTopLevelTypeIdentifiers(SourceFile file)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(file.Content, path: file.FilePath);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            List<string> identifiers = [];

            foreach (NamespaceDeclarationSyntax namespaceDeclaration in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>())
            {
                CollectTypeDeclarations(namespaceDeclaration.Members, identifiers);
            }

            foreach (FileScopedNamespaceDeclarationSyntax fileScopedNamespace in root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>())
            {
                CollectTypeDeclarations(fileScopedNamespace.Members, identifiers);
            }

            if (!root.DescendantNodes().Any(n => n is NamespaceDeclarationSyntax || n is FileScopedNamespaceDeclarationSyntax))
            {
                CollectTypeDeclarations(root.Members, identifiers);
            }

            return identifiers;
        }

        public static bool HasTopLevelStatements(SourceFile file)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(file.Content, path: file.FilePath);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            return root.Members.Any(m => m is GlobalStatementSyntax);
        }

        public static string GetNamespace(SourceFile file)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(file.Content, path: file.FilePath);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            NamespaceDeclarationSyntax blockNamespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            if (blockNamespace is not null)
            {
                return blockNamespace.Name.ToString();
            }

            FileScopedNamespaceDeclarationSyntax fileNamespace = root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            if (fileNamespace is not null)
            {
                return fileNamespace.Name.ToString();
            }

            return string.Empty;
        }

        public static List<TypeDeclarationSyntax> GetTopLevelTypeDeclarations(SourceFile file)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(file.Content, path: file.FilePath);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            List<TypeDeclarationSyntax> declarations = [];

            foreach (NamespaceDeclarationSyntax namespaceDeclaration in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>())
            {
                declarations.AddRange(namespaceDeclaration.Members.OfType<TypeDeclarationSyntax>());
            }

            foreach (FileScopedNamespaceDeclarationSyntax fileScopedNamespace in root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>())
            {
                declarations.AddRange(fileScopedNamespace.Members.OfType<TypeDeclarationSyntax>());
            }

            if (!root.DescendantNodes().Any(n => n is NamespaceDeclarationSyntax || n is FileScopedNamespaceDeclarationSyntax))
            {
                declarations.AddRange(root.Members.OfType<TypeDeclarationSyntax>());
            }

            return declarations;
        }

        static void CollectTypeDeclarations(SyntaxList<MemberDeclarationSyntax> members, List<string> identifiers)
        {
            foreach (MemberDeclarationSyntax member in members)
            {
                if (member is TypeDeclarationSyntax typeDeclaration)
                {
                    identifiers.Add(typeDeclaration.Identifier.ValueText);
                }
                else if (member is DelegateDeclarationSyntax delegateDeclaration)
                {
                    identifiers.Add(delegateDeclaration.Identifier.ValueText);
                }
            }
        }
    }
}
