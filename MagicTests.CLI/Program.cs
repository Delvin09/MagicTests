using MagicTests.Abstractions.Interfaces;
using MagicTests.CLI.Loggers;
using MagicTests.Core;

namespace MagicTests.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new CombineLogger(new FileLogger()/*, new ConsoleLogger()*/);
            try
            {
                using var engine = new TestEngine(logger, args);
                var providers = engine.LoadTestProviders();

                using var consoleRenderer = new ConsoleTestProgressRenderer(providers);
                consoleRenderer.Init();

                foreach (var provider in providers)
                {
                    provider.Run();
                }
            }
            finally
            {
                if (logger is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}
