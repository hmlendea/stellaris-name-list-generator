using System;

using NuciCLI.Arguments;
using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;
using StellarisNameListGenerator.Service.NamesBuilders;

namespace StellarisNameListGenerator
{
    public sealed class Program
    {
        private static string InputArgument => "input";
        private static string OutputArgument => "output";
        private static string NameArgument => "name";
        private static string LockedArgument => "locked";

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            ArgumentParser argumentParser = new();
            argumentParser.AddArgument(InputArgument, required: true);
            argumentParser.AddArgument(OutputArgument, required: true);
            argumentParser.AddArgument(NameArgument, required: true);
            argumentParser.AddArgument(LockedArgument, defaultValue: false);

            ArgumentsCollection parsedArguments = argumentParser.ParseArgs(ArgumentNormaliser.Normalise(args));

            GenerateNameList(parsedArguments);
        }

        private static void GenerateNameList(ArgumentsCollection parsedArguments)
        {
            string inputFilePath = parsedArguments.Get<string>(InputArgument);
            string outputFilePath = parsedArguments.Get<string>(OutputArgument);
            string nameListName = parsedArguments.Get<string>(NameArgument);
            bool isLocked = ReadBool(parsedArguments.Get<object>(LockedArgument));

            INameListGenerator nameListGenerator = BuildNameListGenerator(inputFilePath);

            nameListGenerator.Generate(outputFilePath, nameListName, isLocked);
        }

        private static INameListGenerator BuildNameListGenerator(string inputFilePath)
        {
            IFileContentBuilder fileContentBuilder = new FileContentBuilder(
                new ShipNamesBuilder(),
                new ShipClassNamesBuilder(),
                new FleetNamesBuilder(),
                new ArmyNamesBuilder(),
                new PlanetNamesBuilder(),
                new CharacterNamesBuilder());

            IFileRepository<NameList> nameListRepository = new XmlRepository<NameList>(inputFilePath);

            return new NameListGenerator(fileContentBuilder, nameListRepository);
        }

        private static bool ReadBool(object value)
        {
            if (value is null)
            {
                return false;
            }

            if (value is bool boolValue)
            {
                return boolValue;
            }

            if (value is string stringValue)
            {
                if (bool.TryParse(stringValue, out bool parsedValue))
                {
                    return parsedValue;
                }
            }

            throw new ArgumentException($"Invalid boolean value: '{value}'", nameof(value));
        }
    }
}
