using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Interfaces;
using MagicTests.CLI.Abstractions;
using MagicTests.CLI.Elements;
using MagicTests.CLI.Interfaces;
using MagicTests.Core;
using System.Collections.Immutable;

namespace MagicTests.CLI
{
    internal class ConsoleTestProgressRenderer : ConsoleContainerElement
    {
        private readonly ImmutableList<TestProvider> _testProviders;
        private readonly int _totalTestCount;

        public ConsoleTestProgressRenderer(IEnumerable<TestProvider> testProviders)
        {
            this._testProviders = testProviders.ToImmutableList();
            foreach (var provider in _testProviders)
            {
                provider.OnAfterTestGroupEnd += Provider_OnAfterTestGroupEnd;
            }

            _consoleElements = _testProviders
                .Select(tp => new ConsoleTestProviderElement(tp))
                .OfType<IConsoleElement>()
                .ToList();

            _totalTestCount = _consoleElements.OfType<ConsoleTestProviderElement>().Sum(c => c.Count);
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var provider in _testProviders)
            {
                provider.OnAfterTestGroupEnd -= Provider_OnAfterTestGroupEnd;
            }
        }

        private void Provider_OnAfterTestGroupEnd(object? sender, TestGroupRunEventArgs e)
        {
            UpdateElement();
        }

        public override void Init()
        {
            /*Console.BufferHeight = */Console.WindowHeight = Console.LargestWindowHeight;
            /*Console.BufferWidth = */Console.WindowWidth = Console.LargestWindowWidth;

            foreach (var element in _consoleElements)
                element.Init();

            _startPosition = new(Console.CursorLeft, Console.CursorTop);

            Update();

            Console.WriteLine();
        }

        protected override void InnerUpdate()
        {
            Console.ResetColor();
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.Write("Summary: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{GetProcessedTestsCount()} of {_totalTestCount}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" ({GetTotalTime()})");

            var errors = _testProviders.SelectMany(tp => tp.TestGroups)
                .SelectMany(tg => tg.Tests)
                .Where(t => t.State == TestState.Failed || t.State == TestState.Interrupted)
                .Select(t => new { t.Title, t.Result.Message });

            Console.ResetColor();

            foreach (var error in errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{error.Title}: ");
                Console.ResetColor();
                Console.WriteLine(error.Message);
            }

            Console.ResetColor();
        }

        private object GetTotalTime()
        {
            var end = _testProviders.SelectMany(tp => tp.TestGroups)
                .Max(tp => tp.Result.End);
            var start = _testProviders.SelectMany(tp => tp.TestGroups)
                .Min(tp => tp.Result.Start);
            return end - start;
        }

        private int GetProcessedTestsCount()
        {
            return _testProviders
                .SelectMany(tp => tp.TestGroups)
                .Where(tg => string.IsNullOrEmpty(tg.Skip))
                .SelectMany(tg => tg.Tests)
                .Where(t => t.State != TestState.Skipped)
                .Count(t => TestStateHelper.FinalStates.Contains(t.State));
        }
    }
}
