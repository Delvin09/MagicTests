namespace MagicTests.Core
{
    public class TestProvider // assembly
    {
        public TestProvider(string assemblyPath, IEnumerable<TestGroupInfo> testGroups)
        {
            AssemblyPath = assemblyPath;
            TestGroups = testGroups;
        }

        public string AssemblyPath { get; }

        public IEnumerable<TestGroupInfo> TestGroups { get; }

        public void Run()
        {
            foreach (var group in TestGroups)
            {
                try
                {
                    group.Run();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
