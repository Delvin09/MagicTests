using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MagicTests.Abstractions.Results
{
    public readonly struct TestResult
    {
        public string Message { get; init; }

        [XmlIgnore]
        [JsonIgnore]
        public Exception? Exception { get; init; }

        public string? ExceptionMessage => Exception?.Message;

        public string? ExceptionStack => Exception?.StackTrace;

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
