using System.Collections.Generic;
using System.IO;
using System.Text;

using NuciDAL.Repositories;

using StellarisNameListGenerator.Models;

namespace StellarisNameListGenerator.Service
{
    public sealed class NameListGenerator(
        IFileContentBuilder fileContentBuilder,
        IFileRepository<NameList> nameListRepository) : INameListGenerator
    {
        private static UTF8Encoding OutputEncoding => new(encoderShouldEmitUTF8Identifier: true);

        public void Generate(string filePath, string name, bool isLocked)
        {
            NameList nameList = GetMergedNameList();
            nameList.Id = Path.GetFileNameWithoutExtension(filePath);
            nameList.Name = name;
            nameList.IsLocked = isLocked;

            string fileContent = fileContentBuilder.BuildContent(nameList);

            File.WriteAllText(filePath, fileContent, OutputEncoding);
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
