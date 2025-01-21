namespace MagicTests.Abstractions.Interfaces
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    public static class LoggerExtensions
    {
        public static void Info(this ILogger logger, string message)
        {
            logger.Log(message, LogType.Info);
        }

        public static void Warn(this ILogger logger, string message)
        {
            logger.Log(message, LogType.Warning);
        }

        public static void Error(this ILogger logger, string message, Exception exception)
        {
            logger.Log(message, LogType.Error, exception);
        }
    }

    public interface ILogger
    {
        void Log(string message, LogType logType, Exception? exception = null);
    }
}
