using NuciCLI;
using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;
using StellarisNameListGenerator.Service.NamesBuilders;

namespace StellarisNameListGenerator
{
    public class Program
    {
        private static readonly string[] InputFileOptions = ["-i", "--input"];
        private static readonly string[] OutputFileOptions = ["-o", "--output"];
        private static readonly string[] NameOptions = ["-n", "--name"];
        private static readonly string[] IsLockedOptions = ["-l", "--locked"];

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">CLI arguments</param>
        public static void Main(string[] args)
        {
            string inputFilePath = CliArgumentsReader.GetOptionValue(args, InputFileOptions);
            string outputFilePath = CliArgumentsReader.GetOptionValue(args, OutputFileOptions);
            string namelistName = CliArgumentsReader.GetOptionValue(args, NameOptions);
            bool isLocked = CliArgumentsReader.HasOption(args, IsLockedOptions);

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

            IRepository<NameList> nameListRepository = new XmlRepository<NameList>(inputFilePath);
            INameListGenerator nameListGenerator = new NameListGenerator(fileContentBuilder, nameListRepository);

            nameListGenerator.Generate(outputFilePath, namelistName, isLocked);
        }
    }
}
