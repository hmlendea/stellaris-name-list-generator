using System;
using System.IO;

using StellarisNameListGenerator.IntegrationTests.Fixtures;

namespace StellarisNameListGenerator.IntegrationTests.Tools
{
    /// <summary>
    /// Provides an isolated filesystem and working directory for a single end-to-end generator invocation.
    /// The repository is never used as a working directory, so the URL cache and generated files stay in
    /// a portable temporary location.
    /// </summary>
    internal sealed class GeneratorEnvironment : IDisposable
    {
        private readonly string originalDirectoryPath;

        internal GeneratorEnvironment(string ownerName)
        {
            originalDirectoryPath = System.IO.Directory.GetCurrentDirectory();
            DirectoryPath = TemporaryDirectory.Create(ownerName);
            System.IO.Directory.SetCurrentDirectory(DirectoryPath);
        }

        internal static string InputFileName => "input.xml";

        internal string DirectoryPath { get; }

        internal string WriteInput(string content)
        {
            string filePath = Path.Combine(DirectoryPath, InputFileName);
            File.WriteAllText(filePath, content);

            return filePath;
        }

        internal string GetPath(string relativePath)
            => Path.Combine(DirectoryPath, relativePath);

        internal string ReadText(string relativePath)
            => File.ReadAllText(GetPath(relativePath));

        internal byte[] ReadBytes(string relativePath)
            => File.ReadAllBytes(GetPath(relativePath));

        internal GeneratedNameList Generate(string outputFileName, string displayName)
        {
            Generate(outputFileName, displayName, isLocked: false);

            return GeneratedNameList.ReadFromFile(GetPath(outputFileName));
        }

        internal void Generate(string outputFileName, string displayName, bool isLocked)
        {
            if (isLocked)
            {
                NameListGeneratorCli.Run(
                    "-i", GetPath(InputFileName),
                    "-o", GetPath(outputFileName),
                    "-n", displayName,
                    "-l");

                return;
            }

            NameListGeneratorCli.Run(
                "-i", GetPath(InputFileName),
                "-o", GetPath(outputFileName),
                "-n", displayName);
        }

        internal void Run(params string[] arguments)
            => NameListGeneratorCli.Run(arguments);

        internal string GenerateFromDocument(string document, string outputFileName, string displayName)
        {
            WriteInput(document);

            return Generate(outputFileName, displayName).Content;
        }

        internal string GenerateRichDocument(string outputFileName)
            => GenerateFromDocument(NameListFixtures.RichDocument, outputFileName, NameListFixtures.RichRecordName);

        public void Dispose()
        {
            System.IO.Directory.SetCurrentDirectory(originalDirectoryPath);
            TemporaryDirectory.Delete(DirectoryPath);
        }
    }
}
