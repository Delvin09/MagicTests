using MagicTests.Abstractions.Exceptions;
using System.Reflection;

namespace MagicTests.Core
{
    public class TestInfo
    {
        public MethodInfo Method { get; init; }

        public string Title { get; init; }

        public string? Skip { get; init; }

        internal void Run(object? subject)
        {
            try
            {
                Method.Invoke(subject, new object[] { });
            }
            catch (Exception ex) when (ex.InnerException is AssertFailedException)
            {
                // test failed by Assertion
            }
            catch
            {
                // if something goes very bad
            }
        }
    }
}
