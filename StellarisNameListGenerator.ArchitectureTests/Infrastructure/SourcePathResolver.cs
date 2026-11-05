using System;
using System.IO;
using System.Linq;

namespace StellarisNameListGenerator.ArchitectureTests.Infrastructure
{
    /// <summary>
    /// Resolves repository-relative production source paths portably, from explicit project metadata.
    /// </summary>
    public static class SourcePathResolver
    {
        public static string RepositoryRoot { get; } = FindRepositoryRoot();

        public static string GetSourceDirectory(string relativePath)
            => Path.Combine(RepositoryRoot, relativePath);

        public static string GetProjectFile(string projectRelativePath)
            => Path.Combine(RepositoryRoot, projectRelativePath);

        static string FindRepositoryRoot()
        {
            string directory = AppContext.BaseDirectory;

            while (directory != null && !IsRepositoryRoot(directory))
            {
                directory = Path.GetDirectoryName(directory);
            }

            if (directory is null)
            {
                throw new InvalidOperationException(
                    "Could not locate the repository root containing the solution file. " +
                    "The architecture tests require an explicit repository layout.");
            }

            return directory;
        }

        static bool IsRepositoryRoot(string directory)
        {
            return Directory
                .EnumerateFiles(directory, "*.slnx", SearchOption.TopDirectoryOnly)
                .Any();
        }
    }
}
