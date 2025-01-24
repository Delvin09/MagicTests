using MagicTests.Abstractions.Events;
using MagicTests.Abstractions.Exceptions;
using MagicTests.Abstractions.Interfaces;
using MagicTests.Abstractions.Results;
using System.Reflection;

namespace MagicTests.Core
{
    internal class TestInfo : ITestInfo, IRunnable
    {
        private TestState _state = TestState.Pending;
        private ILogger _logger;

        public TestInfo(ILogger logger)
        {
            this._logger = logger;
        }

        public MethodInfo Method { get; init; }

        public string Title { get; init; }

        public string? Skip { get; set; }

        public TestResult Result { get; private set; }

        public TestState State
        {
            get
            {
                if (!string.IsNullOrEmpty(Skip))
                    return _state = TestState.Skipped;

                return _state;
            }
            private set
            {
                if (_state != value && !TestStateHelper.FinalStates.Contains(_state))
                {
                    var oldState = _state;
                    _state = value;

                    try
                    {
                        OnTestStateChange?.Invoke(this, new() { OldState = oldState, NewState = _state, Test = this });
                    }
                    catch { }
                }
            }
        }

        public void Run(object? subject)
        {
            _logger.Info($"Start excec test '{Title}'");

            Result = new() { Start = DateTime.Now };
            try
            {
                State = TestState.Running;
                Method.Invoke(subject, new object[] { });
                Result = new(Result) { End = DateTime.Now };
                State = TestState.Success;
            }
            catch (Exception ex) when (ex.InnerException is AssertFailedException)
            // test failed by Assertion
            {
                Result = new(Result) { Exception = ex.InnerException, Message = ex.InnerException.Message, End = DateTime.Now };
                State = TestState.Failed;

                _logger.Warn($"Test '{Title}' failed");
            }
            catch (Exception ex) // if something goes very bad
            {
                Result = new(Result) { Exception = ex, Message = ex.Message, End = DateTime.Now };
                State = TestState.Interrupted;

                _logger.Error($"Test '{Title}' failed with exception.", ex);
                throw;
            }
            finally
            {
                Result = new(Result) { End = DateTime.Now };
                _logger.Info($"End excec test '{Title}'");
            }
        }

        internal void Interrupt()
        {
            State = TestState.Interrupted;
        }

        public event EventHandler<TestStateChangeArgs>? OnTestStateChange;
    }
}
