using System.IO;
using excel.generator;
using excel.parser;
using excel.util;
using NPOI.SS.UserModel;

namespace excel.Generator
{
    public class TableGenerator
    {
        private const string kMarkIgnore = "//";

        private string m_ClassName;
        private string m_FileName;
        private int m_BinaryNum;
        private StructGenerator m_StructGenerator;

        public TableGenerator(string className, string fileName, ISheet sheet)
        {
            m_ClassName = className;
            m_FileName = fileName;

            m_StructGenerator = new StructGenerator("Data", sheet.GetRow(sheet.FirstRowNum), sheet.GetRow(sheet.FirstRowNum + 1), sheet.GetRow(sheet.FirstRowNum + 2));
        }

        public string GenerateScript(string ns, ISheet sheet = null)
        {
            if (1 == m_StructGenerator.GetParserCount())
            {
                return GenerateListScript(ns, sheet);
            }
            else
            {
                return GenerateDictionaryScript(ns, sheet);
            }
        }

        private string GenerateDictionaryScript(string ns, ISheet sheet = null)
        {
            ScriptBuilder builder = new ScriptBuilder();

            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.IO;");
            builder.AppendLine("using System.Text;");

            builder.AppendLine(string.Empty);
            builder.AppendLine(string.Empty);

            builder.BeginNamespace(ns);

            builder.BeginClass("sealed", m_ClassName);

            m_StructGenerator.DeclarationScript(builder);

            var firstParser = m_StructGenerator.FirstParser();
            var secondParser = m_StructGenerator.SecondParser();
            builder.AppendLine($"private Dictionary<{firstParser.TypeName}, {m_StructGenerator.Name}> m_Dict = new Dictionary<{firstParser.TypeName}, {m_StructGenerator.Name}>();");

            if (null != sheet)
            {
                builder.BeginMethod($"private {m_ClassName}()");
            }
            else
            {
                builder.BeginMethod($"public {m_ClassName}()");
            }

            builder.AppendLine("Read(br);");
            builder.AppendLine("br.Close();");
            builder.EndMethod();

            //GetEnumerator
            builder.BeginMethod($"public Dictionary<{firstParser.TypeName}, {m_StructGenerator.Name}>.Enumerator GetEnumerator()");
            builder.AppendLine("return m_Dict.GetEnumerator();");
            builder.EndMethod();

            //Find
            if (null == sheet)
            {
                builder.BeginMethod($"public {m_StructGenerator.Name} Find({firstParser.TypeName} key)");
                builder.AppendLine($"{m_StructGenerator.Name} result;");
                builder.BeginBrace("if (m_Dict.TryGetValue(key, out result))");
                builder.AppendLine("return result;");
                builder.EndBrace();
                builder.AppendLine("throw new KeyNotFoundException(string.Format(\"{0} : {1}\", GetType(), key));");
                builder.EndMethod();
            }

            //Read
            var readerName = "reader";
            var sizeName = "num";
            var targetName = "target";

            builder.BeginMethod($"public void Read(BinaryReader {readerName})");
            builder.AppendLine($"int {sizeName} = {readerName}.ReadInt32();");
            builder.BeginBrace($"for (int i = 0; i < {sizeName}; i++)");
            builder.AppendLine($"var {targetName} = new {m_StructGenerator.Name}();");
            builder.AppendLine($"{targetName}.Read({readerName});");
            builder.AppendLine($"m_Dict.Add({targetName}.{firstParser.FiledName}, {targetName});");
            builder.EndBrace();
            builder.EndMethod();

            // GetCount
            builder.BeginMethod("public int GetCount()");
            builder.AppendLine("return m_Dict.Count;");
            builder.EndMethod();

            //ToString
            builder.BeginMethod("public override string ToString()");
            builder.AppendLine("var sb = new StringBuilder();");
            builder.AppendLine("var rator = m_Dict.GetEnumerator();");
            builder.BeginBrace("while (rator.MoveNext())");
            builder.AppendLine("var target = rator.Current.Value;");
            var rator = m_StructGenerator.GetEnumerator();
            bool first = true;
            while (rator.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.AppendLine("sb.Append('\\t');");
                }
                var current = rator.Current;

                if (current == null) continue;

                if (current.TypeName.Contains("[]"))
                {
                    var boolName = $"first{current.FiledName}";
                    builder.AppendLine($"bool {boolName} = true;");
                    builder.BeginBrace($"foreach (var item in target.{current.FiledName})");
                    builder.BeginBrace($"if ({boolName})");
                    builder.AppendLine($"{boolName} = false;");
                    builder.EndBrace();
                    builder.BeginBrace("else");
                    builder.AppendLine("sb.Append('\\t');");
                    builder.EndBrace();
                    builder.AppendLine("sb.Append(item);");
                    builder.EndBrace();
                }
                else
                {
                    builder.AppendLine($"sb.Append(target.{current.FiledName});");
                }
            }
            rator.Dispose();
            builder.AppendLine("sb.AppendLine();");
            builder.EndBrace();
            builder.AppendLine("rator.Dispose();");
            builder.AppendLine("return sb.ToString();");
            builder.EndMethod();

            if (null != sheet)
            {
                // static init 
                var instanceName = "s_Instance";
                builder.AppendLine($"private static {m_ClassName} {instanceName};");
                builder.BeginMethod("public static void Init()");
                builder.AppendLine($"{instanceName} = new {m_ClassName}();");
                builder.EndMethod();
                // static unint 
                builder.BeginMethod("public static void Uninit()");
                builder.AppendLine($"{instanceName} = null;");
                builder.EndMethod();
                // static find
                builder.BeginMethod($"public static {secondParser.TypeName} Find({firstParser.TypeName} key)");
                builder.AppendLine($"{m_StructGenerator.Name} result;");
                builder.BeginBrace($"if ({instanceName}.m_Dict.TryGetValue(key, out result))");
                builder.AppendLine($"return result.{secondParser.FiledName};");
                builder.EndBrace();
                builder.AppendLine($"throw new KeyNotFoundException(string.Format(\"{{0}} : {{1}}\", {instanceName}.GetType(), key));");
                builder.EndMethod();
                //
                for (int i = sheet.FirstRowNum + ConfigParser.kHeadNum; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (IsIgnore(row)) continue;
                    var col = row.GetCell(firstParser.FirstCell);
                    builder.BeginProperty($"public static {secondParser.TypeName} {col.GetString()}");
                    builder.AppendLine($"get {{ return {instanceName}.m_Dict[\"{col.GetString()}\"].{secondParser.FiledName}; }}");
                    builder.EndProperty();
                }
            }
            //
            builder.EndClass();
            //
            builder.EndNamespace();

            return builder.ToString();
        }

        private string GenerateListScript(string ns, ISheet sheet = null)
        {
            ScriptBuilder builder = new ScriptBuilder();

            // ref
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.IO;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine("using Zh.Engine.BinaryData;");

            builder.AppendLine(string.Empty);
            builder.AppendLine(string.Empty);

            builder.BeginNamespace(ns);

            //class
            builder.BeginClass("sealed", m_ClassName);

            //Data
            m_StructGenerator.DeclarationScript(builder);

            //Dictionary
            var firstParser = m_StructGenerator.FirstParser();
            builder.AppendLine($"private List<{firstParser.TypeName}> m_List = new List<{firstParser.TypeName}>();");

            //constructor
            builder.BeginMethod($"public {m_ClassName}()");

            builder.AppendLine("Read(br);");
            builder.AppendLine("br.Close();");
            builder.EndMethod();

            //Read
            var readerName = "reader";
            var sizeName = "num";
            var targetName = "target";

            builder.BeginMethod($"public void Read(BinaryReader {readerName})");
            builder.AppendLine($"var {targetName} = new {m_StructGenerator.Name}();");
            builder.AppendLine($"int {sizeName} = {readerName}.ReadInt32();");
            builder.BeginBrace($"for (int i = 0; i < {sizeName}; i++)");
            builder.AppendLine($"{targetName}.Read({readerName});");
            builder.AppendLine($"m_List.Add({targetName}.{firstParser.FiledName});");
            builder.EndBrace();
            builder.EndMethod();

            // GetCount
            builder.BeginMethod("public int GetCount()");
            builder.AppendLine("return m_List.Count;");
            builder.EndMethod();
            
            //this[int index]
            builder.BeginMethod($"public {firstParser.TypeName} this[int index]");
            builder.BeginBrace("get");
            builder.AppendLine("return m_List[index];");
            builder.EndBrace();
            builder.EndMethod();
            
            // GetData
            builder.BeginMethod($"public List<{firstParser.TypeName}> GetData()");
            builder.AppendLine("return m_List;");
            builder.EndMethod();
            
            //ToString
            builder.BeginMethod("public override string ToString()");
            builder.AppendLine("var sb = new StringBuilder();");
            builder.BeginBrace("foreach(var rows in m_List )");
            if (m_StructGenerator.FirstParser().TypeName.Contains("[]"))
            {
                builder.AppendLine("var first = true;");
                builder.BeginBrace("foreach (var cell in rows)");
                builder.BeginBrace("if(first)");
                builder.AppendLine("first = false;");
                builder.EndBrace();
                builder.BeginBrace("else");
                builder.AppendLine("sb.Append('\\t');");
                builder.EndBrace();
                builder.AppendLine("sb.Append(cell);");
                builder.EndBrace();
            }
            else
            {
                builder.AppendLine("sb.Append(rows);");
            }

            builder.AppendLine("sb.AppendLine();");
            builder.EndBrace();
            builder.AppendLine("return sb.ToString();");
            builder.EndMethod();

            //
            builder.EndClass();
            //
            builder.EndNamespace();
            return builder.ToString();
        }

        public void StartBinary(BinaryWriter writer)
        {
            m_BinaryNum = 0;
            RefreshHead(writer);
        }

        private void RefreshHead(BinaryWriter writer)
        {
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(m_BinaryNum);
            writer.Seek(0, SeekOrigin.End);
        }

        public void AppendBinary(BinaryWriter writer, ISheet sheet)
        {
            for (var i = sheet.FirstRowNum + ConfigParser.kHeadNum; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if(IsIgnore(row)) continue;
                m_BinaryNum++;
                m_StructGenerator.Write(writer, row);
            }
        }

        public void EndBinary(BinaryWriter writer)
        {
            RefreshHead(writer);
        }

        private bool IsIgnore(IRow row)
        {
            var cell = row.GetCell(row.FirstCellNum);
            if (cell.CellType == CellType.String)
            {
                var strVal = cell.StringCellValue;
                return strVal.StartsWith(kMarkIgnore);
            }

            return cell.CellType == CellType.Blank;
        }
    }
}
