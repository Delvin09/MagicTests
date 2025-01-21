using MagicTests.Abstractions.Interfaces;

namespace MagicTests.Abstractions.Events
{
    public class TestRunEventArgs : EventArgs
    {
        public required ITestInfo Test { get; init; }
    }
}
