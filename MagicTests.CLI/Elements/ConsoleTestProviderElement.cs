using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Interfaces;
using MagicTests.CLI.Abstractions;
using MagicTests.CLI.Interfaces;
using MagicTests.Core;

namespace MagicTests.CLI.Elements
{
    internal class ConsoleTestProviderElement : ConsoleContainerElement
    {
        private readonly TestProvider _testProvider;
        private readonly int _testCount;

        public int Count => _testCount;

        public ConsoleTestProviderElement(TestProvider testProvider)
        {
            _testProvider = testProvider;
            _consoleElements = _testProvider.TestGroups
                .Select(tg => new ConsoleTestGroupElement(tg))
                .OfType<IConsoleElement>()
                .ToList();

            _testCount = _testProvider.TestGroups
                .Where(tg => string.IsNullOrEmpty(tg.Skip))
                .SelectMany(tg => tg.Tests)
                .Count(t => t.State != TestState.Skipped);

            _testProvider.OnAfterTestEnd += _testProvider_OnAfterTestEnd;
        }

        private void _testProvider_OnAfterTestEnd(object? sender, TestRunInProviderEventArgs e)
        {
            UpdateElement();
        }

        public override void Dispose()
        {
            base.Dispose();
            _testProvider.OnAfterTestEnd -= _testProvider_OnAfterTestEnd;
        }

        protected override void InnerUpdate()
        {
            Console.ResetColor();
            Console.WriteLine(new string('=', Console.WindowWidth));

            Console.Write(_testProvider.Assembly.FullName);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" {GetProcessedTestsCount()} of {_testCount}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" ({GetTotalTime()})");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(_testProvider.AssemblyPath);
            Console.ResetColor();
        }

        private TimeSpan GetTotalTime()
        {
            return _testProvider.TestGroups
                .Where(tg => string.IsNullOrEmpty(tg.Skip))
                .Max(tg => tg.Result.End)
            -
            _testProvider.TestGroups
                .Where(tg => string.IsNullOrEmpty(tg.Skip))
                .Min(tg => tg.Result.Start);
        }

        private int GetProcessedTestsCount()
        {
            return _testProvider.TestGroups
                .Where(tg => string.IsNullOrEmpty(tg.Skip))
                .SelectMany(tg => tg.Tests)
                .Where(t => t.State != TestState.Skipped)
                .Count(t => TestStateHelper.FinalStates.Contains(t.State));
        }
    }
}
