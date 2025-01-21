namespace MagicTests.Abstractions.Results
{
    public readonly struct TestResult
    {
        public string Message { get; init; }

        public Exception? Exception { get; init; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public TestResult(TestResult result) : this()
        {
            Message = result.Message;
            Exception = result.Exception;
            Start = result.Start;
            End = result.End;
        }
    }
}
