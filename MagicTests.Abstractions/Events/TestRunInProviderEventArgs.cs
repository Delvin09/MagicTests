using MagicTests.Abstractions.Interfaces;

namespace MagicTests.Abstractions.Events
{
    public class TestRunInProviderEventArgs : TestRunEventArgs
    {
        public required ITestGroup TestGroup { get; init; }
    }
}
