using System.Collections.Generic;

using StellarisNameListGenerator.Models;
using StellarisNameListGenerator.Service;

using NuciDAL.Repositories;

namespace StellarisNameListGenerator
{
    public class Program
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">CLI arguments</param>
        public static void Main(string[] args)
        {
            string fileName = "names";

            IFileContentBuilder fileContentBuilder = new FileContentBuilder();
            IRepository<NameList> nameListRepository = new XmlRepository<NameList>($"{fileName}.xml");
            INameListGenerator nameListGenerator = new NameListGenerator(fileContentBuilder, nameListRepository);

            nameListGenerator.Generate(fileName);
        }
    }
}
