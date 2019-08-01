using excel.Generator;
using excel.parser;
using excel.util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Text;

namespace excel
{
    class Program
    {
        private static readonly int kExitCode = 0;
        private static readonly int kErrorCode = -1;
        private static readonly string kExtension = ".bytes";

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                MyDebug.LogError("没有参数！");
                Console.ReadLine();
                return kErrorCode;
            }

            string command = args[0];
            switch (command)
            {
                default:
                    Help();
                    break;
                case "-config":
                    ExportExcel(args[1]);
                    break;
            }
            Console.ReadLine();
            return kExitCode;
        }

        private static IWorkbook NewWorkbook(FileStream stream)
        {
            return stream.Name.EndsWith(".xlsx") ? (IWorkbook)new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
        }

        private static void ExportExcel(string file)
        {
            MyDebug.Log($"Config {file}");
            var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var workbook = NewWorkbook(stream);
            try
            {
                var configParser = new ConfigParser();
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);

                    MyDebug.Log($"Parsing {file} : {sheet.SheetName}");
                    if (sheet.LastRowNum < 1) continue;
                    configParser.AddSheet(sheet);
                }

                foreach (var config in configParser.configs)
                {
                    Parsing(config);
                }
            }
            catch (Exception ex)
            {
                MyDebug.LogError(ex.ToString());
            }
            workbook.Close();
            stream.Close();
        }

        private static void Parsing(ConfigParser.Data config)
        {
            MyDebug.Log($"Parsing {config.file}");

            if (string.IsNullOrEmpty(config.sheetName)) return;

            MakeDirectory(config.classOutputFolder);
            MakeDirectory(config.dataOutputFolder);
            var stream = new FileStream(config.file, FileMode.Open, FileAccess.Read);
            var workbook = NewWorkbook(stream);
            try
            {
                var fileName = $"{config.dataName}{kExtension}";
                var path = $"{config.dataOutputFolder}/{fileName}";
                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                var writer = new BinaryWriter(fileStream, Encoding.UTF8);

                var sheet = workbook.GetSheet(config.sheetName);
                if (null == sheet)
                {
                    MyDebug.LogError($"{config.file} not find sheet {config.sheetName}.");
                    return;
                }

                MyDebug.Log($"Parsing {config.file} : {sheet.SheetName} ------> {path}");
                if (sheet.LastRowNum < 2)
                {
                    MyDebug.LogError($"{config.file} sheet {sheet.SheetName} header format error.");
                    return;
                }

                var generator = new TableGenerator(config.className, fileName, sheet);
                File.WriteAllText($"{config.classOutputFolder}/{config.className}.cs",
                    generator.GenerateScript(config.classNamespace, config.staticText ? sheet : null));
                generator.StartBinary(writer);

                generator.AppendBinary(writer, sheet);
                generator.EndBinary(writer);

                writer.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                MyDebug.LogError(ex.ToString());
            }

            workbook.Close();
            stream.Close();
        }

        private static void MakeDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void Help()
        {
            MyDebug.LogWarning("help");
            MyDebug.LogWarning("检查参数！");
        }
    }
}
