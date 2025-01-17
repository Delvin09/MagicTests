namespace MagicTests.Abstractions.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class TestGroupAttribute : Attribute
    {
        public string? Title { get; }
        public string? Skip { get; }

        public TestGroupAttribute(string? title = null, string? skip = null)
        {
            Title = title;
            Skip = skip;
        }
    }
}
