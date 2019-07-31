using System;

namespace excel.util
{
    static class MyDebug
    {
        private static string nowTime => $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";

        public static void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{nowTime} {message}");
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{nowTime} {message}");
            Console.ResetColor();
        }

        public static int LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{nowTime} {message}");
            Console.ResetColor();
            return -1;
        }
    }
}
