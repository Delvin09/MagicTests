using MagicTests.Abstractions.Attributes;
using MagicTests.Abstractions.Interfaces;
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
        private readonly ILogger _logger;

        public TestEngine(ILogger logger, params string[] assembliesPath)
        {
            if (assembliesPath == null || !assembliesPath.Any())
                throw new ArgumentNullException(nameof(assembliesPath));

            _assemblies = assembliesPath.Select(Path.GetFullPath)
                .Where(File.Exists)
                .ToImmutableArray();

            _baseDirictories = _assemblies
                .Select(x => Path.GetDirectoryName(x)?.ToLower()!)
                .Where(x => x is not null)
                .ToHashSet();

            _context = new AssemblyLoadContext("TestEngine", true);
            _context.Resolving += Context_Resolving;
            this._logger = logger;
        }

        public void Dispose()
        {
            _logger.Info("Engine dispose start");
            try
            {
                _context.Resolving -= Context_Resolving;
                _context.Unload();
            }
            catch (Exception ex)
            {
                _logger.Error("Engine dispose exception", ex);
            }
            finally
            {
                _logger.Info("Engine dispose end");
            }
        }

        public IEnumerable<TestProvider> LoadTestProviders()
        {
            var result = new List<TestProvider>();
            _logger.Info("Start load providers.");

            foreach (var assemblyPath in _assemblies)
            {
                _logger.Info($"Start load `{assemblyPath}`.");
                try
                {
                    var assembly = _context.LoadFromAssemblyPath(assemblyPath);
                    var testGroups = assembly.GetTypes()
                        .Where(t => t.GetCustomAttribute<TestGroupAttribute>() != null)
                        .Select(t => new TestGroupInfo(_logger)
                        {
                            Title = t.GetCustomAttribute<TestGroupAttribute>()?.Title ?? t.Name,
                            Type = t,
                            Skip = t.GetCustomAttribute<TestGroupAttribute>()?.Skip,
                            Tests = t.GetMethods().Where(m => m.GetCustomAttribute<TestAttribute>() != null)
                                .Select(m => new TestInfo(_logger)
                                {
                                    Method = m,
                                    Title = m.GetCustomAttribute<TestAttribute>()?.Title ?? m.Name,
                                    Skip = t.GetCustomAttribute<TestGroupAttribute>()?.Skip,
                                })
                                .Cast<ITestInfo>()
                                .ToImmutableArray(),
                        })
                        .Cast<ITestGroup>()
                        .ToImmutableArray();

                    result.Add(new TestProvider(assembly, assemblyPath, testGroups, _logger));
                }
                catch (Exception ex)
                {
                    _logger.Error($"Exception when `{assemblyPath}` load.", ex);
                }
                finally
                {
                    _logger.Info($"End load `{assemblyPath}`.");
                }
            }

            return result.ToImmutableList();
        }

        private Assembly? Context_Resolving(AssemblyLoadContext context, AssemblyName name)
        {
            foreach (var baseDic in _baseDirictories)
            {
                try
                {
                    // AssemblyName.GetAssemblyName()
                    var path = Path.Combine(baseDic, name.Name) + ".dll";
                    if (File.Exists(path))
                    {
                        return context.LoadFromAssemblyPath(path);
                    }
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
