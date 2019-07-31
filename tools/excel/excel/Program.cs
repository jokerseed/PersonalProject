using excel.util;
using System;

namespace excel
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("test" + "\t" + "test1");
            //Console.WriteLine("test2");

            //Console.WriteLine(Debug.nowTime);

            //MyDebug.LogWarning("test");

            //Console.WriteLine(MyDebug.LogError("test"));

            //string mystr = string.Empty;
            //mystr.Print("1231231");

            Console.ReadKey();
        }
    }

    public static class StringExtension
    {
        public static void Print(this string str, string message)
        {
            Console.WriteLine(str + message);
        }
    }
}
