using System;
using System.IO;

using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class NameListGenerator : INameListGenerator
    {
        readonly IFileContentBuilder fileContentBuilder;
        readonly IRepository<NameList> nameListRepository;

        public NameListGenerator(
            IFileContentBuilder fileContentBuilder,
            IRepository<NameList> nameListRepository)
        {
            this.fileContentBuilder = fileContentBuilder;
            this.nameListRepository = nameListRepository;
        }

        public void Generate(string id)
        {
            NameList nameList = nameListRepository.Get(id);
            string fileContent = fileContentBuilder.BuildContent(nameList);

            File.WriteAllText("test.txt", fileContent);
        }
    }
}
