using MagicTests.Abstractions.Interfaces;

namespace MagicTests.Abstractions.Events
{
    public class TestGroupRunEventArgs : EventArgs
    {
        public required ITestGroup TestGroup { get; init; }
    }
}
