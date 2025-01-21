using MagicTests.Abstractions.Interfaces;

namespace MagicTests.CLI.Loggers
{
    public class FileLogger : ILogger, IDisposable
    {
        private StreamWriter _writer;

        public FileLogger()
        {
            var fileName = $"{DateTime.Now.ToString().Replace(":", "_")}_TestRunLog.log";
            _writer = new StreamWriter(fileName);
        }

        public void Dispose()
        {
            _writer.Flush();
            _writer.Dispose();
        }

        public void Log(string message, LogType logType, Exception? exception = null)
        {
            _writer.WriteLine($"[{logType}] {DateTime.Now}: {message}");
            if (exception != null)
            {
                _writer.WriteLine(exception.Message);
                _writer.WriteLine(exception.StackTrace);
            }
        }
    }
}
