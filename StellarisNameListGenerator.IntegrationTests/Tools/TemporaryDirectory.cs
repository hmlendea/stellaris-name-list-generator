using System;
using System.IO;

namespace StellarisNameListGenerator.IntegrationTests.Tools
{
    /// <summary>
    /// Creates and deletes uniquely-named temporary directories for integration tests.
    /// Mirrors the pattern used by NuciDAL integration tests so test output never
    /// leaks into the repository working directory.
    /// </summary>
    internal static class TemporaryDirectory
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), nameof(StellarisNameListGenerator));

        internal static string Create(string ownerName)
        {
            string path = Path.Combine(BasePath, ownerName, Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return path;
        }

        internal static void Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                return;
            }

            Directory.Delete(path, true);
        }
    }
}
