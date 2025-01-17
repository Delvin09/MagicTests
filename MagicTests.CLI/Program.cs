using MagicTests.Core;

namespace MagicTests.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            usi ng var engine = new TestEngine(args);
            var providers = engine.LoadTestProviders();
            // render the tests list

            foreach (var provider in providers)
            {
                provider.Run();
            }
        }
    }
}
