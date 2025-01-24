using MagicTests.Abstractions.Interfaces;

namespace MagicTests.CLI.Loggers
{
    class CombineLogger : ILogger, IDisposable
    {
        private readonly ILogger[] _loggers;

        public CombineLogger(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public void Dispose()
        {
            foreach (var logger in _loggers.OfType<IDisposable>())
            {
                logger.Dispose();
            }
        }

        public void Log(string message, LogType logType, Exception? exception = null)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(message, logType, exception);
            }
        }
    }
}
