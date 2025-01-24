using MagicTests.Abstractions.Interfaces;
using MagicTests.Core;
using System.Collections.Immutable;

namespace VsTests
{
    [TestClass]
    public sealed class TestEngineTests
    {
        private class LoggerFake : ILogger
        {
            public void Log(string message, LogType logType, Exception? exception = null)
            {
            }
        }

        private readonly ILogger _logger = new LoggerFake();

        [TestMethod]
        public void CreateEngine_Success()
        {
            using var engine = new TestEngine(_logger, "MagicTests.Core.dll");

            var field = engine.GetType().GetField("_assemblies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var value = field.GetValue(engine);
            var collection = (ImmutableArray<string>)value;

            Assert.AreEqual(1, collection.Length);
        }

        [TestMethod]
        public void CreateEngine_ThrowException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TestEngine(_logger));
        }
    }
}
