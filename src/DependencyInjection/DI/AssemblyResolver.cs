using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// Default implementation of <see cref="IAssemblyResolver"/>.
    /// </summary>
    [Singleton]
    public class AssemblyResolver : IAssemblyResolver
    {
        private readonly IEnumerable<string> extraDirectories;
        private readonly IEnumerable<string> ignoredAssemblies;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
        /// </summary>
        public AssemblyResolver()
            : this(Microsoft.Extensions.Logging.Abstractions.NullLogger<AssemblyResolver>.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> used for logging data.</param>
        public AssemblyResolver(ILogger<AssemblyResolver> logger)
            : this(logger, Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> used for logging data.</param>
        /// <param name="ignoredAssemblies">A <see cref="IEnumerable{T}"/> with names of assemblies to ignore when resolving.</param>
        public AssemblyResolver(ILogger<AssemblyResolver> logger, IEnumerable<string> ignoredAssemblies)
            : this(logger, ignoredAssemblies, Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> used for logging data.</param>
        /// <param name="ignoredAssemblies">A <see cref="IEnumerable{T}"/> with names of assemblies to ignore when resolving.</param>
        /// <param name="extraDirectories">A <see cref="IEnumerable{T}"/> with folders to search for the missing assembly.</param>
        public AssemblyResolver(ILogger<AssemblyResolver> logger, IEnumerable<string> ignoredAssemblies, IEnumerable<string> extraDirectories)
        {
            this.logger = logger;
            this.extraDirectories = extraDirectories;
            this.ignoredAssemblies = new List<string>(ignoredAssemblies) { "System.Reactive.Debugger" };
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
        }

        /// <summary>
        /// Implementation of <see cref="ResolveEventHandler"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event data.</param>
        /// <returns>The loaded <see cref="Assembly"/>.</returns>
        public virtual Assembly? CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var fullname = new AssemblyName(args.Name);
                var fields = args.Name.Split(',');
                var name = fields[0];
                var culture = fields.Length >= 3 ? fields[2] : string.Empty;

                // failing to ignore queries for satellite resource assemblies or using [assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
                // in AssemblyInfo.cs will crash the program on non en-US based system cultures.
                if (name.EndsWith(".XmlSerializers", StringComparison.OrdinalIgnoreCase) || (name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase) && !culture.EndsWith("neutral", StringComparison.OrdinalIgnoreCase)))
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
                Assembly? foundAssembly = null;
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

        private Assembly? TryLoadFile(string directory, string wantedDLL)
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