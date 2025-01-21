using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Results;
using System.Reflection;

namespace MagicTests.Abstractions.Interfaces
{
    public interface ITestInfo
    {
        public MethodInfo Method { get; }

        public string Title { get; }

        public string? Skip { get; set; }

        public TestResult Result { get; }

        public TestState State { get; }

        event EventHandler<TestStateChangeArgs>? OnTestStateChange;
    }
}
