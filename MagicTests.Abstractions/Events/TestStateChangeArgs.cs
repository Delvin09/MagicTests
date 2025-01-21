using MagicTests.Abstractions.Interfaces;

namespace MagicTests.Abstractions.Events
{
    public class TestStateChangeArgs : EventArgs
    {
        public TestState NewState { get; init; }

        public TestState OldState { get; init; }

        public ITestInfo Test { get; init; }
    }
}
