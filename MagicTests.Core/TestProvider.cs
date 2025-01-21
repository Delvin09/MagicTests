using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Interfaces;
using System.Collections.Immutable;
using System.Reflection;

namespace MagicTests.Core
{
    public class TestProvider : ITestProvider, IRunnable // assembly
    {
        public TestProvider(Assembly assembly, string assemblyPath, ImmutableArray<ITestGroup> testGroups)
        {
            Assembly = assembly;
            AssemblyPath = assemblyPath;
            TestGroups = testGroups;
        }

        public Assembly Assembly { get; }

        public string AssemblyPath { get; }

        public ImmutableArray<ITestGroup> TestGroups { get; }

        public void Run(object? subject = null)
        {
            foreach (var group in TestGroups)
            {
                group.OnBeforeTestStart += Group_OnBeforeTestStart;
                group.OnAfterTestEnd += Group_OnAfterTestEnd;
                group.OnTestStateChanged += Group_OnTestStateChanged;
                try
                {
                    OnBeforeTestGroupStart?.Invoke(this, new() { TestGroup = group });
                    if (group is IRunnable runnable)
                    {
                        runnable.Run();
                    }

                    OnAfterTestGroupEnd?.Invoke(this, new() { TestGroup = group });
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    group.OnBeforeTestStart -= Group_OnBeforeTestStart;
                    group.OnAfterTestEnd -= Group_OnAfterTestEnd;
                    group.OnTestStateChanged -= Group_OnTestStateChanged;
                }
            }
        }

        private void Group_OnTestStateChanged(object? sender, TestStateChangeArgs e)
        {
            var args = new TestStateChangeInProviderArgs(e, (ITestGroup)sender);
            OnTestStateChanged?.Invoke(this, args);
        }

        private void Group_OnAfterTestEnd(object? sender, TestRunEventArgs e)
        {
            OnAfterTestEnd?.Invoke(this, new() { TestGroup = (TestGroupInfo)sender, Test = e.Test });
        }

        private void Group_OnBeforeTestStart(object? sender, TestRunEventArgs e)
        {
            OnBeforeTestStart?.Invoke(this, new() { TestGroup = (TestGroupInfo)sender, Test = e.Test });
        }

        public event EventHandler<TestRunInProviderEventArgs>? OnBeforeTestStart;
        public event EventHandler<TestRunInProviderEventArgs>? OnAfterTestEnd;

        public event EventHandler<TestGroupRunEventArgs>? OnBeforeTestGroupStart;
        public event EventHandler<TestGroupRunEventArgs>? OnAfterTestGroupEnd;

        public event EventHandler<TestStateChangeInProviderArgs>? OnTestStateChanged;
    }
}
