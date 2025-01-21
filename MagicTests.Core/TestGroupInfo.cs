using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Interfaces;
using MagicTests.Abstractions.Results;
using System.Collections.Immutable;

namespace MagicTests.Core
{
    internal class TestGroupInfo : ITestGroup, IRunnable
    {
        public string Title { get; init; }

        public Type Type { get; init; }

        public string? Skip { get; set; }

        public ImmutableArray<ITestInfo> Tests { get; init; }

        public TestGroupResult Result { get; private set; }

        public void Run(object arg = null)
        {
            Result = new() { Start = DateTime.Now };
            var subject = Activator.CreateInstance(Type);
            foreach (var test in Tests.Where(t => t.State != TestState.Skipped))
            {
                test.OnTestStateChange += Test_OnTestStateChange;
                try
                {
                    OnBeforeTestStart?.Invoke(this, new() { Test = test });
                    if (test is IRunnable runnable)
                    {
                        runnable.Run(subject);
                    }
                    else
                    {
                        test.Skip = "Can't be run.";
                    }

                    Result = new() { Start = Result.Start, End = DateTime.Now };
                    OnAfterTestEnd?.Invoke(this, new() { Test = test });
                }
                catch (TaskCanceledException)
                {
                    throw; // interrupt all tests
                }
                catch (Exception e)
                {
                    // try to continue
                }
                finally
                {
                    test.OnTestStateChange -= Test_OnTestStateChange;
                    Result = new() { Start = Result.Start, End = DateTime.Now };
                }
            }
        }

        private void Test_OnTestStateChange(object? sender, TestStateChangeArgs e)
        {
            OnTestStateChanged?.Invoke(this, e);
        }

        public event EventHandler<TestRunEventArgs>? OnBeforeTestStart;
        public event EventHandler<TestRunEventArgs>? OnAfterTestEnd;

        public event EventHandler<TestStateChangeArgs>? OnTestStateChanged;
    }
}
