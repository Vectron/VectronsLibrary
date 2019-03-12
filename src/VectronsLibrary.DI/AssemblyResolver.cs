using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace VectronsLibrary.DI
{
    [Singleton]
    public class AssemblyResolver : IAssemblyResolver
    {
        private readonly IEnumerable<string> extraDirectories;
        private readonly IEnumerable<string> ignoredAssemblies;
        private readonly ILogger logger;

        public AssemblyResolver()
            : this(Microsoft.Extensions.Logging.Abstractions.NullLogger<AssemblyResolver>.Instance) { }

        public AssemblyResolver(ILogger<AssemblyResolver> logger)
            : this(logger, new string[0]) { }

        public AssemblyResolver(ILogger<AssemblyResolver> logger, IEnumerable<string> ignoredAssemblies)
            : this(logger, ignoredAssemblies, new string[0]) { }

        public AssemblyResolver(ILogger<AssemblyResolver> logger, IEnumerable<string> ignoredAssemblies, IEnumerable<string> extraDirectories)
        {
            this.logger = logger;
            this.extraDirectories = extraDirectories;
            this.ignoredAssemblies = new List<string>(ignoredAssemblies) { "System.Reactive.Debugger" };
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var fullname = new AssemblyName(args.Name);
                var fields = args.Name.Split(',');
                var name = fields[0];
                var culture = fields.Length >= 3 ? fields[2] : string.Empty;

                // failing to ignore queries for satellite resource assemblies or using [assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
                // in AssemblyInfo.cs will crash the program on non en-US based system cultures.
                if (name.EndsWith(".XmlSerializers") || (name.EndsWith(".resources") && !culture.EndsWith("neutral")))
                {
                    return null;
                }

                if (ignoredAssemblies.Contains(name))
                {
                    return null;
                }

                logger.LogDebug("Resolving Assembly: " + fullname);
                var wantedDLL = fullname.Name + ".dll";
                var rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var directoriesToSearch = new List<string>(extraDirectories) { rootDir };
                directoriesToSearch.AddRange(Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories));
                Assembly foundAssembly = null;
                foreach (var dir in directoriesToSearch)
                {
                    foundAssembly = TryLoadFile(dir, wantedDLL);

                    if (foundAssembly != null)
                    {
                        logger.LogDebug($"Resolved {fullname} in {foundAssembly.Location}");
                        break;
                    }
                }

                if (foundAssembly == null)
                {
                    logger.LogError($"Failed to resolve assembly for {args.Name}");
                }

                return foundAssembly;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to resolve assembly for {args.Name}");
                return null;
            }
        }

        private Assembly TryLoadFile(string directory, string wantedDLL)
        {
            try
            {
                var dllPath = Path.Combine(directory, wantedDLL);
                dllPath = Environment.ExpandEnvironmentVariables(dllPath);

                if (File.Exists(dllPath))
                {
                    return Assembly.LoadFile(dllPath);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Assembly resolve failed for {wantedDLL} in {directory}");
            }

            return null;
        }
    }
}