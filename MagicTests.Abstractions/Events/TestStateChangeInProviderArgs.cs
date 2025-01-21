using MagicTests.Abstractions.Interfaces;

namespace MagicTests.Abstractions.Events
{
    public class TestStateChangeInProviderArgs : TestStateChangeArgs
    {
        public ITestGroup TestGroup { get; init; }

        public TestStateChangeInProviderArgs(TestStateChangeArgs args, ITestGroup testGroup)
        {
            TestGroup = testGroup;
            OldState = args.OldState;
            NewState = args.NewState;
            Test = args.Test;
        }
    }
}
