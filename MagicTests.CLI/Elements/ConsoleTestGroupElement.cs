using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Interfaces;
using MagicTests.CLI.Abstractions;
using MagicTests.CLI.Interfaces;

namespace MagicTests.CLI.Elements
{
    internal class ConsoleTestGroupElement : ConsoleContainerElement
    {
        private readonly ITestGroup _testGroup;
        private readonly int _testCount;
        private TimeSpan _time = TimeSpan.Zero;

        public int Count => _testCount;

        public ConsoleTestGroupElement(ITestGroup testGroup)
        {
            _testGroup = testGroup;
            _consoleElements = _testGroup.Tests
                .Select(t => new ConsoleTestElement(t))
                .OfType<IConsoleElement>()
                .ToList();

            _testCount = _testGroup.Tests
                .Count(t => t.State != TestState.Skipped);

            _testGroup.OnAfterTestEnd += _testGroup_OnAfterTestEnd;
        }

        public override void Dispose()
        {
            base.Dispose();
            _testGroup.OnAfterTestEnd -= _testGroup_OnAfterTestEnd;
        }

        private void _testGroup_OnAfterTestEnd(object? sender, TestRunEventArgs e)
        {
            _time = _testGroup.Result.End - _testGroup.Result.Start;
            UpdateElement();
        }

        protected override void InnerUpdate()
        {
            Console.ResetColor();
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(_testGroup.Title);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" {GetProcessedTestsCount()} of {_testCount}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" ({_time})");
            Console.ResetColor();
            Console.Write(new string('-', Console.WindowWidth));
        }

        private int GetProcessedTestsCount()
        {
            return _testGroup.Tests
                .Where(t => t.State != TestState.Skipped)
                .Count(t => TestStateHelper.FinalStates.Contains(t.State));
        }
    }
}
