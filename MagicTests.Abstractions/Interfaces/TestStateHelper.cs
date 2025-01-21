namespace MagicTests.Abstractions.Interfaces
{
    public static class TestStateHelper
    {
        public static readonly HashSet<TestState> FinalStates = new HashSet<TestState>
        {
            TestState.Success,
            TestState.Skipped,
            TestState.Failed,
            TestState.Interrupted,
        };
    }
}
