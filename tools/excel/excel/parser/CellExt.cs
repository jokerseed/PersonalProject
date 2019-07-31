using NPOI.SS.UserModel;
using System;

namespace excel.parser
{
    //internal只能在同一程序集中访问
    internal static class CellExt
    {
        public static int GetInt(this ICell cell)
        {
            if (null == cell) return 0;
            int result = 0;
            if (CellType.Numeric == cell.CellType || CellType.Formula == cell.CellType)
            {
                return (int)cell.NumericCellValue;
            }
            else
            {
                var text = cell.GetString();
                if (!int.TryParse(text, out result))//不是数字字符串
                {
                    throw new InvalidOperationException(text);
                }
            }
            return result;
        }

        public static uint GetUint(this ICell cell)
        {
            if (null == cell) return 0;
            uint result = 0;
            if (CellType.Numeric == cell.CellType || CellType.Formula == cell.CellType)
            {
                result = (uint)cell.NumericCellValue;
            }
            else
            {
                var text = cell.GetString();
                if (!uint.TryParse(text, out result))
                {
                    throw new InvalidOperationException(text);
                }
            }

            return result;
        }

        public static float GetFloat(this ICell cell)
        {
            if (null == cell) return 0;
            float result = 0;
            if (CellType.Numeric == cell.CellType || CellType.Formula == cell.CellType)
            {
                result = (float)cell.NumericCellValue;
            }
            else
            {
                var text = cell.GetString();
                if (!float.TryParse(text, out result))
                {
                    throw new InvalidOperationException(text);
                }
            }

            return result;
        }

        public static double GetDouble(this ICell cell)
        {
            if (null == cell) return 0;
            double result = 0;
            if (CellType.Numeric == cell.CellType || CellType.Formula == cell.CellType)
            {
                result = cell.NumericCellValue;
            }
            else
            {
                var text = cell.GetString();
                if (!double.TryParse(text, out result))
                {
                    throw new InvalidOperationException();
                }
            }
            return result;
        }

        public static bool GetBool(this ICell cell)
        {
            if (null == cell) return false;
            bool result = false;
            if (CellType.Boolean == cell.CellType)
            {
                result = cell.BooleanCellValue;
            }
            else
            {
                var text = cell.GetString();
                if (!bool.TryParse(text, out result))
                {
                    throw new InvalidOperationException();
                }
            }

            return result;
        }

        public static string GetString(this ICell cell)
        {
            if (null == cell)
            {
                return string.Empty;
            }
            if (CellType.String == cell.CellType)
            {
                return cell.StringCellValue;
            }
            return cell.ToString();
        }
    }
}
