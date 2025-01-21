using MagicTests.Abstractions.Interfaces;

namespace MagicTests.CLI.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, LogType logType, Exception? exception = null)
        {
            Console.ResetColor();
            switch (logType)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            var msg = $"[{logType}] {DateTime.Now}: {message}";

            if (exception != null)
            {
                Console.Error.WriteLine(msg);
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
            else
            {
                Console.WriteLine(msg);
            }

            Console.ResetColor();
        }
    }
}
