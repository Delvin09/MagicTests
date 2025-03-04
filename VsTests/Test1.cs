﻿using MagicTests.Core;
using System.Collections.Immutable;

namespace VsTests
{
    [TestClass]
    public sealed class TestEngineTests
    {
        [TestMethod]
        public void CreateEngine_Success()
        {
            using var engine = new TestEngine("MagicTests.Core.dll");

            var field = engine.GetType().GetField("_assemblies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var value = field.GetValue(engine);
            var collection = (ImmutableArray<string>)value;

            Assert.AreEqual(1, collection.Length);
        }

        [TestMethod]
        public void CreateEngine_ThrowException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TestEngine());
        }
    }
}
