using excel.util;
using NPOI.SS.UserModel;
using System;
using System.IO;

namespace excel.parser
{
    public enum EFieldType
    {
        Int,
        IntArray,

        Uint,
        UintArray,

        Float,
        FloatArray,

        Double,
        DoubleArray,

        String,
        StringArray,

        Bool,
        BoolArray,

        Enum,

        Text,
    }

    class FieldParser
    {
        //单元格子中元素类型
        public EFieldType FieldType { get; private set; }
        public int FirstCell { get; }
        public int Size { get; }
        public string FiledName { get; }
        public string Desc { get; }
        public string TypeName { get; private set; }

        private const char kSeparator = '|';

        public FieldParser(string text, string filedName, string desc, int firstCell, int size)
        {
            FiledName = filedName;
            Desc = desc;
            FirstCell = firstCell;
            Size = size;
            Parse(text);
        }

        //使类，命名空间名等名称首字母大写
        public string ToTitleCase(string text)
        {
            var chars = text.Trim().ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new String(chars);
        }

        private void Parse(string text)
        {
            var arg = text.Split(':');
            string fieldType;
            if (arg.Length > 1)
            {
                fieldType = arg[0].ToLower();
                TypeName = arg[1];
            }
            else
            {
                fieldType = arg[0].ToLower();
                TypeName = fieldType;
            }

            if (0 == string.CompareOrdinal(fieldType, "int"))
            {
                FieldType = EFieldType.Int;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "int[]"))
            {
                FieldType = EFieldType.IntArray;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "uint"))
            {
                FieldType = EFieldType.Uint;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "uint[]"))
            {
                FieldType = EFieldType.UintArray;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "float"))
            {
                FieldType = EFieldType.Float;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "float[]"))
            {
                FieldType = EFieldType.FloatArray;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "double"))
            {
                FieldType = EFieldType.Double;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "double[]"))
            {
                FieldType = EFieldType.DoubleArray;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "string"))
            {
                FieldType = EFieldType.String;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "string[]"))
            {
                FieldType = EFieldType.StringArray;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "bool"))
            {
                FieldType = EFieldType.Bool;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "bool[]"))
            {
                FieldType = EFieldType.BoolArray;
                return;
            }
            if (0 == string.CompareOrdinal(fieldType, "enum"))
            {
                FieldType = EFieldType.Enum;
                return;
            }

            if (0 == string.CompareOrdinal(fieldType, "text"))
            {
                FieldType = EFieldType.Text;
                return;
            }

            throw new Exception($"not find type {text}");
        }

        public void Write(BinaryWriter writer, IRow row)
        {
            switch (FieldType)
            {
                case EFieldType.Int:
                    WriteInt(writer, row);
                    break;
                case EFieldType.IntArray:
                    WriteInt(writer, row);
                    break;
                case EFieldType.Uint:
                    WriteUint(writer, row);
                    break;
                case EFieldType.UintArray:
                    WriteUintArray(writer, row);
                    break;
                case EFieldType.Double:
                    WriteDouble(writer, row);
                    break;
                case EFieldType.DoubleArray:
                    WriteDoubleArray(writer, row);
                    break;
                case EFieldType.Float:
                    WriteFloat(writer, row);
                    break;
                case EFieldType.FloatArray:
                    WriteFloatArray(writer, row);
                    break;
                case EFieldType.Bool:
                    WriteBool(writer, row);
                    break;
                case EFieldType.BoolArray:
                    WriteBoolArray(writer, row);
                    break;
                case EFieldType.String:
                    WriteString(writer, row);
                    break;
                case EFieldType.StringArray:
                    WriteStringArray(writer, row);
                    break;
                case EFieldType.Enum:
                    WriteEnum(writer, row);
                    break;
            }
        }

        public void ReadScript(ScriptBuilder builder, string readerName)
        {
            switch (FieldType)
            {
                case EFieldType.Int:
                    ReadScript(builder, readerName, "ReadInt32");
                    break;
                case EFieldType.IntArray:
                    ReadArrayScript(builder, readerName, "ReadInt32");
                    break;
                case EFieldType.Uint:
                    ReadScript(builder, readerName, "ReadUInt32");
                    break;
                case EFieldType.UintArray:
                    ReadArrayScript(builder, readerName, "ReadUInt32");
                    break;
                case EFieldType.Float:
                    ReadScript(builder, readerName, "ReadSingle");
                    break;
                case EFieldType.FloatArray:
                    ReadArrayScript(builder, readerName, "ReadSingle");
                    break;
                case EFieldType.Double:
                    ReadScript(builder, readerName, "ReadDouble");
                    break;
                case EFieldType.DoubleArray:
                    ReadArrayScript(builder, readerName, "ReadDouble");
                    break;
                case EFieldType.String:
                    ReadScript(builder, readerName, "ReadString");
                    break;
                case EFieldType.StringArray:
                    ReadArrayScript(builder, readerName, "ReadString");
                    break;
                case EFieldType.Bool:
                    ReadScript(builder, readerName, "ReadBoolean");
                    break;
                case EFieldType.BoolArray:
                    ReadArrayScript(builder, readerName, "ReadBoolean");
                    break;
            }
        }

        private void ReadScript(ScriptBuilder builder, string readerName, string funcName)
        {
            builder.AppendLine($"{FiledName}={readerName}.{funcName}();");
        }

        private void ReadArrayScript(ScriptBuilder builder, string readerName, string funcName)
        {
            var sizeName = $"{FiledName}Size";
            var type = TypeName.TrimEnd('[', ']');
            builder.AppendLine($"int {sizeName} = {readerName}.ReadInt32();");
            builder.AppendLine($"{FiledName} = new {type}[{sizeName}];");
            builder.BeginBrace($"for (var i = 0; i < {sizeName}; i++)");
            builder.AppendLine($"{FiledName}[i] = {readerName}.{funcName}();");

            builder.EndBrace();
        }

        private void ReadEnumScript(ScriptBuilder builder, string readerName, string funcName)
        {
            builder.AppendLine($"var {FiledName}Enums = reader.{funcName}().Split('|');");
            builder.AppendLine($"for (var i = 0; i < {FiledName}Enums.Length; ++i){{");
            builder.AppendLine($"{FiledName} |= ({TypeName})System.Enum.Parse(typeof({TypeName}), {FiledName}Enums[i]);}}");
        }

        private void WriteInt(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetInt());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void WriteIntArray(BinaryWriter writer, IRow row)
        {
            if (1 == Size)
            {
                var cell = row.GetCell(FirstCell);
                var value = cell.GetString();
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    writer.Write((int)0);
                }
                else
                {
                    var array = value.Split(kSeparator);
                    writer.Write((int)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        try
                        {
                            writer.Write(int.TryParse(array[i], out var val) ? val : 0);
                        }
                        catch (Exception ex)
                        {
                            ThrowException(cell, ex);
                        }
                    }
                }
            }
            else
            {
                writer.Write((int)Size);
                for (int i = 0; i < Size; i++)
                {
                    int index = FirstCell + i;
                    var cell = row.GetCell(index);
                    try
                    {
                        writer.Write(cell.GetInt());
                    }
                    catch (Exception ex)
                    {
                        ThrowException(cell, ex);
                        throw;
                    }
                }
            }
        }

        private void WriteUint(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetUint());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void WriteUintArray(BinaryWriter writer, IRow row)
        {
            if (1 == Size)
            {
                var cell = row.GetCell(FirstCell);
                var value = cell.GetString();
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    writer.Write((int)0);
                }
                else
                {
                    var array = value.Split(kSeparator);
                    writer.Write((int)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        try
                        {
                            writer.Write(uint.TryParse(array[i], out var val) ? val : 0);
                        }
                        catch (Exception ex)
                        {
                            ThrowException(cell, ex);
                        }
                    }
                }
            }
            else
            {
                writer.Write(Size);
                for (int i = 0; i < Size; i++)
                {
                    int index = FirstCell + i;
                    var cell = row.GetCell(index);
                    try
                    {
                        writer.Write(cell.GetUint());
                    }
                    catch (Exception ex)
                    {
                        ThrowException(cell, ex);
                    }
                }
            }
        }

        private void WriteDouble(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetDouble());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void WriteDoubleArray(BinaryWriter writer, IRow row)
        {
            if (1 == Size)
            {
                var cell = row.GetCell(FirstCell);
                var value = cell.GetString();
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    writer.Write((int)0);
                }
                else
                {
                    var array = value.Split(kSeparator);
                    writer.Write((int)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        try
                        {
                            writer.Write(double.TryParse(array[i], out var val) ? val : 0);
                        }
                        catch (Exception ex)
                        {
                            ThrowException(cell, ex);
                        }
                    }
                }
            }
            else
            {
                writer.Write((int)Size);
                for (int i = 0; i < Size; i++)
                {
                    int index = FirstCell + i;
                    var cell = row.GetCell(index);
                    try
                    {
                        writer.Write(cell.GetDouble());
                    }
                    catch (Exception ex)
                    {
                        ThrowException(cell, ex);
                    }
                }
            }

        }

        private void WriteFloat(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetFloat());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void WriteFloatArray(BinaryWriter writer, IRow row)
        {
            if (1 == Size)
            {
                var cell = row.GetCell(FirstCell);
                var value = cell.GetString();
                var array = value.Split(kSeparator);
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    writer.Write((int)0);
                }
                else
                {
                    writer.Write((int)array.Length);

                    for (int i = 0; i < array.Length; i++)
                    {
                        try
                        {
                            writer.Write(float.TryParse(array[i], out var val) ? val : 0);
                        }
                        catch (Exception ex)
                        {
                            ThrowException(cell, ex);
                        }
                    }
                }
            }
            else
            {
                writer.Write((int)Size);
                for (int i = 0; i < Size; i++)
                {
                    int index = FirstCell + i;
                    var cell = row.GetCell(index);
                    try
                    {
                        writer.Write(cell.GetFloat());
                    }
                    catch (Exception ex)
                    {
                        ThrowException(cell, ex);
                    }
                }
            }
        }

        private void WriteBool(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetBool());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void WriteBoolArray(BinaryWriter writer, IRow row)
        {
            if (1 == Size)
            {
                var cell = row.GetCell(FirstCell);
                var value = cell.GetString();
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    writer.Write((int)0);
                }
                else
                {
                    var array = value.Split(kSeparator);
                    writer.Write((int)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        try
                        {
                            writer.Write(bool.TryParse(array[i], out var val) && val);
                        }
                        catch (Exception ex)
                        {
                            ThrowException(cell, ex);
                        }
                    }
                }
            }
            else
            {
                writer.Write((int)Size);
                for (int i = 0; i < Size; i++)
                {
                    int index = FirstCell + i;
                    var cell = row.GetCell(index);
                    try
                    {
                        writer.Write(cell.GetBool());
                    }
                    catch (Exception ex)
                    {
                        ThrowException(cell, ex);
                    }
                }
            }
        }

        private void WriteString(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetString());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void WriteStringArray(BinaryWriter writer, IRow row)
        {
            if (1 == Size)
            {
                var cell = row.GetCell(FirstCell);
                var value = cell.GetString();
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    writer.Write((int)0);
                }
                else
                {
                    var array = value.Split(kSeparator);
                    writer.Write((int)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        try
                        {
                            writer.Write(array[i]);
                        }
                        catch (Exception ex)
                        {
                            ThrowException(cell, ex);
                        }
                    }
                }
            }
            else
            {
                writer.Write((int)Size);
                for (int i = 0; i < Size; i++)
                {
                    var cell = row.GetCell(FirstCell + i);
                    try
                    {
                        writer.Write(cell.GetString());
                    }
                    catch (Exception ex)
                    {
                        ThrowException(cell, ex);
                    }
                }
            }
        }

        private void WriteEnum(BinaryWriter writer, IRow row)
        {
            var cell = row.GetCell(FirstCell);
            try
            {
                writer.Write(cell.GetString());
            }
            catch (Exception ex)
            {
                ThrowException(cell, ex);
            }
        }

        private void ThrowException(ICell cell, Exception ex)
        {
            MyDebug.LogError($"sheet: {cell.Sheet.SheetName} rol: {cell.RowIndex} col: {cell.ColumnIndex}");
            MyDebug.LogError(ex.ToString());
        }

        //属性
        public string DeclarationScript()
        {
            return $"public {TypeName} {FiledName} {{get;private set;}} //{Desc}";
        }
    }
}
