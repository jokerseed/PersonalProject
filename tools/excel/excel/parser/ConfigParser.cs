using excel.generator;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace excel.parser
{
    class ConfigParser
    {
        public class Data
        {
            public string file;
            public string classOutputFolder;
            public string dataOutputFolder;
            public string className;
            public string classNamespace;
            public string dataName;
            public string sheetName;
            public bool staticText;
        }

        public List<Data> configs => new List<Data>();
        public const int kHeadNum = 3;

        public void AddSheet(ISheet sheet)
        {
            //表前三行做描述性用
            var nameRow = sheet.GetRow(sheet.FirstRowNum);//名字
            var typeRow = sheet.GetRow(sheet.FirstRowNum + 1);//类型
            var descRow = sheet.GetRow(sheet.FirstRowNum + 2);//描述
            var generator = new StructGenerator("Data", nameRow, typeRow, descRow);
            var fileField = generator.FindParser("file");
            var classOutputFolderField = generator.FindParser("classOutputFolder");
            var dataOutputFolderField = generator.FindParser("dataOutputFolder");
            var classNameField = generator.FindParser("className");
            var classNameSpaceFiled = generator.FindParser("classNameSpace");
            var dataNameField = generator.FindParser("dataName");
            var sheetNameField = generator.FindParser("sheetName");
            var staticTextField = generator.FindParser("staticText");

            for (int i = sheet.FirstRowNum + ConfigParser.kHeadNum; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (null == row)
                {
                    continue;
                }

                configs.Add(new Data
                {
                    file = row.GetCell(fileField.FirstCell).GetString(),
                    classOutputFolder = row.GetCell(classOutputFolderField.FirstCell).GetString(),
                    dataOutputFolder = row.GetCell(dataOutputFolderField.FirstCell).GetString(),
                    className = row.GetCell(classNameField.FirstCell).GetString(),
                    classNamespace = row.GetCell(classNameSpaceFiled.FirstCell).GetString(),
                    dataName = row.GetCell(dataNameField.FirstCell).GetString(),
                    sheetName = row.GetCell(sheetNameField.FirstCell).GetString(),
                    staticText = row.GetCell(staticTextField.FirstCell).GetBool(),
                });
            }
        }
    }
}
