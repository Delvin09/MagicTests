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

        public static void True(bool condition)
        {
            if (!condition)
            {
                throw new AssertFailedException(true.ToString(), condition.ToString());
            }
        }

        public static void False(bool condition)
        {
            if (condition)
            {
                throw new AssertFailedException(false.ToString(), condition.ToString());
            }
        }

        public static void Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex) when (ex is T)
            {
            }
            catch (Exception ex) when (ex is not T)
            {
                throw new AssertFailedException(typeof(T).FullName, ex.GetType().FullName, ex);
            }
        }

        public static void Throws<T, TResult>(Func<TResult> action) where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex) when (ex is not T)
            {
                throw new AssertFailedException(typeof(T).FullName, ex.GetType().FullName, ex);
            }
        }

        public static void AreSequenceEqual<T>(IEnumerable<T?> expected, IEnumerable<T?> actual)
        {
            if (!expected.SequenceEqual(actual))
                throw new AssertFailedException(
                    $"[{string.Join(',', expected.Select(t => t?.ToString() ?? "null"))}]",
                    $"[{string.Join(',', actual.Select(t => t?.ToString() ?? "null"))}]");
        }

        public static void Empty<T>(IEnumerable<T?>? sequence)
        {
            if (sequence == null || sequence.Any())
            {
                throw new AssertFailedException("sequence should be empty");
            }
        }

        public static void Null<T>(T? value)
        {
            if (value != null) throw new AssertFailedException("null", value.ToString());
        }
    }
}
