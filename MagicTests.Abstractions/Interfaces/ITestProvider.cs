using MagicTests.Abstractions.Events;
using System.Collections.Immutable;
using System.Reflection;

namespace MagicTests.Abstractions.Interfaces
{
    public interface ITestProvider
    {
        Assembly Assembly { get; }

        string AssemblyPath { get; }

        ImmutableArray<ITestGroup> TestGroups { get; }


        event EventHandler<TestRunInProviderEventArgs>? OnBeforeTestStart;
        event EventHandler<TestRunInProviderEventArgs>? OnAfterTestEnd;

        event EventHandler<TestGroupRunEventArgs>? OnBeforeTestGroupStart;
        event EventHandler<TestGroupRunEventArgs>? OnAfterTestGroupEnd;

        event EventHandler<TestStateChangeInProviderArgs>? OnTestStateChanged;
    }
}
