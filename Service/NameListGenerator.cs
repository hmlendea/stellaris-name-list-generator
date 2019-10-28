using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            NameList nameList = GetMergedNameList();
            string fileContent = fileContentBuilder.BuildContent(nameList);

            File.WriteAllText("test.txt", fileContent);
        }

        public NameList GetMergedNameList()
        {
            List<NameList> nameLists = nameListRepository.GetAll().ToList();

            if (nameLists.Count == 1)
            {
                return nameLists[0];
            }

            NameList mergedNameList = new NameList();

            foreach (NameList nameList in nameLists.OrderByDescending(x => x.Name))
            {
                mergedNameList.AddRange(nameList);
            }

            return mergedNameList;
        }
    }
}
