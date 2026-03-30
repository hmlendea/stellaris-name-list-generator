using System;
using System.Collections.Generic;

using NuciCLI.Arguments;
using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;
using StellarisNameListGenerator.Service.NamesBuilders;

namespace StellarisNameListGenerator
{
    public class Program
    {
        private const string InputArgument = "input";
        private const string OutputArgument = "output";
        private const string NameArgument = "name";
        private const string LockedArgument = "locked";

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">CLI arguments</param>
        public static void Main(string[] args)
        {
            string[] normalizedArgs = NormalizeArguments(args);

            ArgumentParser argumentParser = new();
            argumentParser.AddArgument(InputArgument, required: true);
            argumentParser.AddArgument(OutputArgument, required: true);
            argumentParser.AddArgument(NameArgument, required: true);
            argumentParser.AddArgument(LockedArgument, defaultValue: false);

            ArgumentsCollection parsedArgs = argumentParser.ParseArgs(normalizedArgs);

            string inputFilePath = parsedArgs.Get<string>(InputArgument);
            string outputFilePath = parsedArgs.Get<string>(OutputArgument);
            string namelistName = parsedArgs.Get<string>(NameArgument);
            bool isLocked = ReadBool(parsedArgs.Get<object>(LockedArgument));

            IShipNamesBuilder shipNamesBuilder = new ShipNamesBuilder();
            IShipClassNamesBuilder shipClassNamesBuilder = new ShipClassNamesBuilder();
            IFleetNamesBuilder fleetnamesBuilder = new FleetNamesBuilder();
            IArmyNamesBuilder armyNamesBuilder = new ArmyNamesBuilder();
            IPlanetNamesBuilder planetNamesBuilder = new PlanetNamesBuilder();
            ICharacterNamesBuilder characterNamesBuilder = new CharacterNamesBuilder();

            IFileContentBuilder fileContentBuilder = new FileContentBuilder(
                shipNamesBuilder,
                shipClassNamesBuilder,
                fleetnamesBuilder,
                armyNamesBuilder,
                planetNamesBuilder,
                characterNamesBuilder);

            IFileRepository<NameList> nameListRepository = new XmlRepository<NameList>(inputFilePath);
            INameListGenerator nameListGenerator = new NameListGenerator(fileContentBuilder, nameListRepository);

            nameListGenerator.Generate(outputFilePath, namelistName, isLocked);
        }

        private static string[] NormalizeArguments(string[] args)
        {
            List<string> normalized = [];

            for (int i = 0; i < args.Length; i++)
            {
                string current = args[i];

                if (current is "-i" or "--input")
                {
                    normalized.Add($"--{InputArgument}");
                    normalized.Add(ReadNextValue(args, ref i, InputArgument));
                    continue;
                }

                if (current is "-o" or "--output")
                {
                    normalized.Add($"--{OutputArgument}");
                    normalized.Add(ReadNextValue(args, ref i, OutputArgument));
                    continue;
                }

                if (current is "-n" or "--name")
                {
                    normalized.Add($"--{NameArgument}");
                    normalized.Add(ReadNextValue(args, ref i, NameArgument));
                    continue;
                }

                if (current is "-l" or "--locked")
                {
                    normalized.Add($"--{LockedArgument}");

                    if (i + 1 < args.Length && !args[i + 1].StartsWith('-'))
                    {
                        normalized.Add(args[++i]);
                    }
                    else
                    {
                        normalized.Add("true");
                    }

                    continue;
                }

                normalized.Add(current);
            }

            return [.. normalized];
        }

        private static string ReadNextValue(string[] args, ref int index, string argumentName)
        {
            if (index + 1 >= args.Length)
            {
                throw new ArgumentNullException(argumentName);
            }

            return args[++index];
        }

        private static bool ReadBool(object value)
            => value switch
            {
                bool boolValue => boolValue,
                string stringValue when bool.TryParse(stringValue, out bool parsedValue) => parsedValue,
                null => false,
                _ => throw new ArgumentException($"Invalid boolean value: '{value}'")
            };
    }
}
