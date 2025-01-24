using MagicTests.Abstractions.Interfaces;
using MagicTests.Abstractions.Results;
using MagicTests.CLI.Loggers;
using MagicTests.Core;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MagicTests.CLI
{
    public class TestInfo
    {
        public TestInfo()
        {
        }

        public TestInfo(ITestInfo testInfo)
        {
            Method = testInfo.Method;
            Title = testInfo.Title;
            Skip = testInfo.Skip;
            Result = testInfo.Result;
            State = testInfo.State;
        }

        [XmlIgnore]
        [JsonIgnore]
        public MethodInfo Method { get; set; }

        public string Title { get; set; }

        public string? Skip { get; set; }

        public TestResult Result { get; set; }

        public TestState State { get; set; }
    }

    public class TestGroupInfo
    {
        public TestGroupInfo()
        {
        }

        public TestGroupInfo(ITestGroup testGroup)
        {
            Title = testGroup.Title;
            Type = testGroup.Type;
            SkipReason = testGroup.Skip;
            Tests = testGroup.Tests.Select(t => new TestInfo(t)).ToArray();
            Result = testGroup.Result;
        }

        public string Title { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Type Type { get; set; }

        public string? SkipReason { get; set; }

        public TestInfo[] Tests { get; set; }

        public TestGroupResult Result { get; set; }
    }

    public class TestProviderInfo //DTO|POCO
    {
        public TestProviderInfo()
        {
        }

        public TestProviderInfo(TestProvider testProvider)
        {
            FullName = testProvider.Assembly.FullName;
            AssemblyPath = testProvider.AssemblyPath;
            TestGroups = testProvider.TestGroups.Select(tg => new TestGroupInfo(tg)).ToArray();
        }

        [XmlAttribute(AttributeName = "Name")]
        public string? FullName { get; set; }

        [XmlAttribute(AttributeName = "Path")]
        public string AssemblyPath { get; set; }

        public TestGroupInfo[] TestGroups { get; set; }

        public TestProviderInfo TestInfo { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new CombineLogger(new FileLogger()/*, new ConsoleLogger()*/);
            try
            {
                using var engine = new TestEngine(logger, args);
                var providers = engine.LoadTestProviders();

                using var consoleRenderer = new ConsoleTestProgressRenderer(providers);
                consoleRenderer.Init();

                foreach (var provider in providers)
                {
                    provider.Run();
                }
            }
            finally
            {
                if (logger is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}
