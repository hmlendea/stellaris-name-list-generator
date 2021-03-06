using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public void Generate(string filePath, string name, bool isLocked)
        {
            NameList nameList = GetMergedNameList();
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            nameList.Id = fileName;
            nameList.Name = name;
            nameList.IsLocked = isLocked;

            string fileContent = fileContentBuilder.BuildContent(nameList);

            File.WriteAllText(filePath, fileContent, new UTF8Encoding(true));
        }

        public NameList GetMergedNameList()
        {
            List<NameList> nameLists = nameListRepository.GetAll().ToList();

            if (nameLists.Count == 1)
            {
                return nameLists[0];
            }

            NameList mergedNameList = new NameList();
            nameLists.ForEach(mergedNameList.AddRange);

            foreach (NameList nameList in nameLists)
            {
                mergedNameList.AddRange(nameList);
            }

            return mergedNameList;
        }
    }
}
