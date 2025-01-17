namespace MagicTests.Abstractions.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class TestAttribute : Attribute
    {
        public string? Title { get; }
        public string? Skip { get; }

        public TestAttribute(string? title = null, string? skip = null)
        {
            Title = title;
            Skip = skip;
        }
    }
}
