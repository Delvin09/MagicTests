using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Interfaces;
using MagicTests.CLI.Abstractions;
using MagicTests.Core;

namespace MagicTests.CLI.Elements
{
    internal class ConsoleTestElement : ConsoleElement
    {
        private readonly ITestInfo _testInfo;

        public ConsoleTestElement(ITestInfo testInfo)
        {
            _testInfo = testInfo;
            _testInfo.OnTestStateChange += TestInfo_OnTestStateChange;
        }

        public override void Dispose()
        {
            base.Dispose();
            _testInfo.OnTestStateChange -= TestInfo_OnTestStateChange;
        }

        private void TestInfo_OnTestStateChange(object? sender, TestStateChangeArgs e)
        {
            UpdateElement();
        }

        protected override void InnerUpdate()
        {
            Console.ResetColor();

            var stateName = _testInfo.State.ToString().ToUpper();
            var stateColor = GetStateColor();
            Console.Write('\t');
            Console.BackgroundColor = stateColor.Background;
            Console.ForegroundColor = stateColor.Foreground;
            Console.Write(stateName);
            Console.ResetColor();
            Console.Write("\t");
            Console.Write(_testInfo.Method.Name);

            Console.ForegroundColor = ConsoleColor.Cyan;
            var time = _testInfo.Result.End >= _testInfo.Result.Start
                ? _testInfo.Result.End - _testInfo.Result.Start
                : TimeSpan.Zero;

            Console.Write($"\t\t({time})");

            Console.ResetColor();
        }

        private (ConsoleColor Background, ConsoleColor Foreground) GetStateColor()
        {
            return _testInfo.State switch
            {
                TestState.Success => (ConsoleColor.Green, ConsoleColor.White),
                TestState.Pending => (ConsoleColor.Gray, ConsoleColor.DarkGray),
                TestState.Running => (ConsoleColor.Blue, ConsoleColor.White),
                TestState.Skipped => (ConsoleColor.DarkGray, ConsoleColor.White),
                TestState.Failed => (ConsoleColor.Red, ConsoleColor.White),
                TestState.Interrupted => (ConsoleColor.DarkRed, ConsoleColor.White),
                _ => (ConsoleColor.Gray, ConsoleColor.White)
            };
        }
    }
}
