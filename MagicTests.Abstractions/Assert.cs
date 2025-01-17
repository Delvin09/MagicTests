using MagicTests.Abstractions.Exceptions;

namespace MagicTests.Abstractions
{
    public static class Assert
    {
        public static void AreEqual<T>(T? expected, T? actual, string? message = null)
        {
            if (!object.Equals(expected, actual))
            {
                throw new AssertFailedException(expected?.ToString(), actual?.ToString());
            }
        }

        public static void AreNotEqual<T>(T? expected, T? actual, string? message = null)
        {
            if (object.Equals(expected, actual))
            {
                throw new AssertFailedException(expected?.ToString(), actual?.ToString());
            }
        }
    }
}
