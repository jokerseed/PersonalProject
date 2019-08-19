using excel.parser;
using excel.util;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;

namespace excel.generator
{
    class StructGenerator
    {
        private List<FieldParser> mParsers = new List<FieldParser>();
        public string Name { get; }
        public StructGenerator(string name, IRow nameRow, IRow typeRow, IRow descRow)
        {
            Name = name;
            for (int i = nameRow.FirstCellNum; i < nameRow.LastCellNum;)
            {
                var typeCell = typeRow.GetCell(i);
                var nameCell = nameRow.GetCell(i);
                var descCell = descRow.GetCell(i);

                var parser = new FieldParser(typeCell.StringCellValue, nameCell.StringCellValue,
                    descCell.StringCellValue, i, GetMergedColumnSize(typeCell));

                mParsers.Add(parser);
                i += parser.Size;
            }
        }

        private int GetMergedColumnSize(ICell cell)
        {
            if (cell.IsMergedCell)//是否合并单元格
            {
                for (int i = 0; i < cell.Sheet.NumMergedRegions; i++)
                {
                    var region = cell.Sheet.GetMergedRegion(i);
                    if (region.FirstRow <= cell.RowIndex && region.LastRow >= cell.RowIndex &&
                        region.FirstColumn <= cell.ColumnIndex && region.LastColumn >= cell.ColumnIndex)
                    {
                        return region.LastColumn - region.FirstColumn + 1;
                    }
                }
            }

            return 1;
        }

        public void DeclarationScript(ScriptBuilder builder)
        {
            builder.BeginClass(Name);

            for (int i = 0; i < mParsers.Count; i++)
            {
                var parser = mParsers[i];
                builder.AppendLine($"{parser.DeclarationScript()}");
            }

            var readerName = "reader";

            builder.BeginMethod($"public void Read(BinaryReader {readerName})");

            for (int i = 0; i < mParsers.Count; i++)
            {
                var parser = mParsers[i];
                parser.ReadScript(builder, "reader");
            }

            builder.EndMethod();
            builder.EndClass();
        }

        public void Write(BinaryWriter writer, IRow row)
        {
            for (int i = 0; i < mParsers.Count; i++)
            {
                var parser = mParsers[i];
                parser.Write(writer, row);
            }
        }

        public FieldParser FindParser(string filedName)
        {
            for (int i = 0; i < mParsers.Count; i++)
            {
                var parser = mParsers[i];
                if (0 == string.CompareOrdinal(parser.ToTitleCase(filedName), parser.FiledName))
                {
                    return parser;
                }
            }

            return null;
        }

        public FieldParser FirstParser()
        {
            return mParsers[0];
        }

        public FieldParser SecondParser()
        {
            return mParsers[1];
        }

        public List<FieldParser>.Enumerator GetEnumerator()
        {
            return mParsers.GetEnumerator();
        }

        public int GetParserCount()
        {
            return mParsers.Count;
        }
    }
}
