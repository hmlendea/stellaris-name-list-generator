using System.Collections.Generic;
using System.IO;
using System.Text;

using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class NameListGenerator(
        IFileContentBuilder fileContentBuilder,
        IRepository<NameList> nameListRepository) : INameListGenerator
    {
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
            List<NameList> nameLists = [.. nameListRepository.GetAll()];

            if (nameLists.Count == 1)
            {
                return nameLists[0];
            }

            NameList mergedNameList = new();

            foreach (NameList nameList in nameLists)
            {
                mergedNameList.AddRange(nameList);
            }

            return mergedNameList;
        }
    }
}
