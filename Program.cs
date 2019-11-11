using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;

using NuciCLI;
using NuciDAL.Repositories;

namespace StellarisNameListGenerator
{
    public class Program
    {
        static readonly string[] InputFileOptions = new string[] { "-i", "--input" };
        static readonly string[] OutputFileOptions = new string[] { "-o", "--output" };
        static readonly string[] NameOptions = new string[] { "-n", "--name" };
        static readonly string[] IsLockedOptions = new string[] { "-l", "--locked" };

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">CLI arguments</param>
        public static void Main(string[] args)
        {
            string inputFilePath = CliArgumentsReader.GetOptionValue(args, InputFileOptions);
            string outputFilePath = CliArgumentsReader.GetOptionValue(args, OutputFileOptions);
            string namelistName = CliArgumentsReader.GetOptionValue(args, NameOptions);
            bool isLocked = CliArgumentsReader.HasOption(IsLockedOptions);

            IFileContentBuilder fileContentBuilder = new FileContentBuilder();
            IRepository<NameList> nameListRepository = new XmlRepository<NameList>(inputFilePath);
            INameListGenerator nameListGenerator = new NameListGenerator(fileContentBuilder, nameListRepository);

            nameListGenerator.Generate(outputFilePath, namelistName, isLocked);
        }
    }
}
