using MagicTests.Abstractions.Attributes;
using System.Collections.Immutable;
using System.Runtime.Loader;

namespace MagicTests.Core
{
    public class TestGroupInfo
    {
        public string Title { get; init; }

        public Type Type { get; init; }

        public string? Skip { get; init; }

        public ImmutableArray<TestInfo> Tests { get; init; }

        internal void Run()
        {
            var subject = Activator.CreateInstance(Type);
            foreach (var test in Tests)
            {
                try
                {
                    test.Run(subject);
                }
                catch
                {

                }
            }
        }
    }
}
