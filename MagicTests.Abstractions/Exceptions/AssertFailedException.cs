namespace MagicTests.Abstractions.Exceptions
{
    [Serializable]
    public class AssertFailedException : Exception
    {
        public AssertFailedException(string? expected, string? actual)
            : base($"Actual: {actual}, expected: {expected}") { }

        public AssertFailedException(string? expected, string? actual, Exception inner)
            : base($"Actual: {actual}, expected: {expected}", inner) { }

        public AssertFailedException(string message) : base(message) { }

        public AssertFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
