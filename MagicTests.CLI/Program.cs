using MagicTests.Core;

namespace MagicTests.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var engine = new TestEngine(args);
            var providers = engine.LoadTestProviders();

            using var consoleRenderer = new ConsoleTestProgressRenderer(providers);
            consoleRenderer.Init();

            foreach (var provider in providers)
            {
                provider.Run();
            }
        }
    }
}
