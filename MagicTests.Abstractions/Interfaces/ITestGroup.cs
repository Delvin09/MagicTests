using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Results;
using System.Collections.Immutable;

namespace MagicTests.Abstractions.Interfaces
{
    public interface ITestGroup
    {
        public string Title { get; }

        public Type Type { get; }

        public string? Skip { get; set; }

        public ImmutableArray<ITestInfo> Tests { get; }

        public TestGroupResult Result { get; }

        public event EventHandler<TestRunEventArgs>? OnBeforeTestStart;
        public event EventHandler<TestRunEventArgs>? OnAfterTestEnd;
        public event EventHandler<TestStateChangeArgs>? OnTestStateChanged;
    }
}
