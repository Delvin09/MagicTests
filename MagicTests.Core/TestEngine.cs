using MagicTests.Abstractions.Attributes;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Loader;

namespace MagicTests.Core
{
    public class TestEngine : IDisposable
    {
        private ImmutableArray<string> _assemblies;
        private AssemblyLoadContext _context;

        private readonly HashSet<string> _baseDirictories;

        public TestEngine(params string[] assembliesPath)
        {
            if (assembliesPath == null || !assembliesPath.Any())
                throw new ArgumentNullException(nameof(assembliesPath));

            _assemblies = assembliesPath.Select(Path.GetFullPath)
                .Where(File.Exists)
                .ToImmutableArray();

            _baseDirictories = _assemblies
                .Select(x => Path.GetDirectoryName(x)?.ToLower())
                .Where(x => x != null)
                .ToHashSet();

            _context = new AssemblyLoadContext("TestEngine", true);
            _context.Resolving += Context_Resolving;
        }

        public void Dispose()
        {
            try
            {
                _context.Resolving -= Context_Resolving;
                _context.Unload();
            }
            catch
            {
            }
        }

        public IEnumerable<TestProvider> LoadTestProviders()
        {
            var result = new List<TestProvider>();

            foreach (var assemblyPath in _assemblies)
            {
                try
                {
                    var assembly = _context.LoadFromAssemblyPath(assemblyPath);
                    var testGroups = assembly.GetTypes()
                        .Where(t => t.GetCustomAttribute<TestGroupAttribute>() != null)
                        .Select(t => new TestGroupInfo
                        {
                            Title = t.GetCustomAttribute<TestGroupAttribute>()?.Title ?? t.Name,
                            Type = t,
                            Skip = t.GetCustomAttribute<TestGroupAttribute>()?.Skip,
                            Tests = t.GetMethods().Where(m => m.GetCustomAttribute<TestAttribute>() != null)
                                .Select(m => new TestInfo
                                {
                                    Method = m,
                                    Title = m.GetCustomAttribute<TestAttribute>()?.Title ?? m.Name,
                                    Skip = t.GetCustomAttribute<TestGroupAttribute>()?.Skip,
                                })
                                .ToImmutableArray(),
                        });

                    result.Add(new TestProvider(assemblyPath, testGroups));
                }
                catch
                {
                }
            }


            return result.ToImmutableList();
        }

        private Assembly? Context_Resolving(AssemblyLoadContext context, AssemblyName name)
        {
            foreach (var baseDic in _baseDirictories)
            {
                var path = Path.Combine(baseDic, name.Name) + ".dll";
                if (File.Exists(path))
                {
                    return context.LoadFromAssemblyPath(path);
                }
            }

            return null;
        }
    }
}
