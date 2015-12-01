using System;

namespace ScriptMigrator
{
    public class Logger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void LogInfo(string messageFormat, params object[] messageParams)
        {
            Console.WriteLine(messageFormat, messageParams);
        }

        public void LogError(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + message);
            Console.ForegroundColor = originalColor;
        }

        public void LogError(string messageFormat, params object[] messageParams)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + messageFormat, messageParams);
            Console.ForegroundColor = originalColor;
        }
    }
}