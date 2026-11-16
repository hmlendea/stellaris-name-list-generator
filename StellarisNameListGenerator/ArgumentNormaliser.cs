using System;
using System.Collections.Generic;

namespace StellarisNameListGenerator
{
    /// <summary>
    /// Expands the recognised short command line option aliases into their canonical long forms.
    /// </summary>
    public static class ArgumentNormaliser
    {
        private static string InputArgument => "input";
        private static string OutputArgument => "output";
        private static string NameArgument => "name";
        private static string LockedArgument => "locked";
        private static string TrueValue => "true";

        /// <summary>
        /// Normalises the specified command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments to normalise.</param>
        /// <returns>The normalised command line arguments.</returns>
        public static string[] Normalise(string[] args)
        {
            List<string> normalizedArguments = [];

            for (int index = 0; index < args.Length; index++)
            {
                string argument = args[index];

                if (argument is "-i" or "--input")
                {
                    AddValueArgument(normalizedArguments, args, ref index, InputArgument);
                    continue;
                }

                if (argument is "-o" or "--output")
                {
                    AddValueArgument(normalizedArguments, args, ref index, OutputArgument);
                    continue;
                }

                if (argument is "-n" or "--name")
                {
                    AddValueArgument(normalizedArguments, args, ref index, NameArgument);
                    continue;
                }

                if (argument is "-l" or "--locked")
                {
                    AddLockedArgument(normalizedArguments, args, ref index);
                    continue;
                }

                normalizedArguments.Add(argument);
            }

            return [.. normalizedArguments];
        }

        private static void AddValueArgument(List<string> normalizedArguments, string[] args, ref int index, string argumentName)
        {
            normalizedArguments.Add($"--{argumentName}");
            normalizedArguments.Add(ReadNextValue(args, ref index, argumentName));
        }

        private static void AddLockedArgument(List<string> normalizedArguments, string[] args, ref int index)
        {
            normalizedArguments.Add($"--{LockedArgument}");

            if (index + 1 < args.Length && !args[index + 1].StartsWith('-'))
            {
                normalizedArguments.Add(args[++index]);
                return;
            }

            normalizedArguments.Add(TrueValue);
        }

        private static string ReadNextValue(string[] args, ref int index, string argumentName)
        {
            if (index + 1 >= args.Length)
            {
                throw new ArgumentNullException(argumentName, $"The '{argumentName}' argument requires a value.");
            }

            return args[++index];
        }
    }
}
