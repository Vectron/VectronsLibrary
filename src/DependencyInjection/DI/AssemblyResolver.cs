using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VectronsLibrary.DI.Attributes;

namespace VectronsLibrary.DI;

/// <summary>
/// Default implementation of <see cref="IAssemblyResolver"/>.
/// </summary>
[Singleton]
public partial class AssemblyResolver : IAssemblyResolver, IDisposable
{
    private readonly IEnumerable<string> extraDirectories;
    private readonly IEnumerable<string> ignoredAssemblies;
    private readonly ILogger logger;
    private readonly ConcurrentDictionary<string, Assembly?> resolvedAssemblies = new(StringComparer.Ordinal);
    private bool disposedValue;

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
        : this(logger, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used for logging data.</param>
    /// <param name="ignoredAssemblies">A <see cref="IEnumerable{T}"/> with names of assemblies to ignore when resolving.</param>
    public AssemblyResolver(ILogger<AssemblyResolver> logger, IEnumerable<string> ignoredAssemblies)
        : this(logger, ignoredAssemblies, [])
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
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.extraDirectories = extraDirectories ?? throw new ArgumentNullException(nameof(extraDirectories));
        this.ignoredAssemblies = ignoredAssemblies ?? throw new ArgumentNullException(nameof(ignoredAssemblies));

        System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += Default_Resolving;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected implementation of Dispose pattern.
    /// </summary>
    /// <param name="disposing">Value indicating if we need to cleanup managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                System.Runtime.Loader.AssemblyLoadContext.Default.Resolving -= Default_Resolving;
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Verifies that found assembly name matches requested to avoid security issues.
    /// Looks only at PublicKeyToken and Version, empty matches anything.
    /// </summary>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    private static bool RequestedAssemblyNameMatchesFound(AssemblyName requestedName, AssemblyName foundName)
    {
        var requestedPublicKey = requestedName.GetPublicKeyToken();
        if (requestedPublicKey != null)
        {
            var foundPublicKey = foundName.GetPublicKeyToken();
            if (foundPublicKey == null)
            {
                return false;
            }

            for (var index = 0; index < requestedPublicKey.Length; ++index)
            {
                if (requestedPublicKey[index] != foundPublicKey[index])
                {
                    return false;
                }
            }
        }

        if (requestedName.Version != null)
        {
            return requestedName.Version.Equals(foundName.Version);
        }

        return true;
    }

    private Assembly? Default_Resolving(System.Runtime.Loader.AssemblyLoadContext loadContext, AssemblyName assemblyName)
    {
        if (assemblyName == null || string.IsNullOrEmpty(assemblyName.Name))
        {
            LogInvalidAssemblyName();
            return null;
        }

        using var scope = logger.BeginScope(assemblyName.Name);
        LogResolvingAssembly(assemblyName.Name);

        if (resolvedAssemblies.TryGetValue(assemblyName.Name, out var assembly))
        {
            LogResolvedCache(assemblyName.Name);
            return assembly;
        }

        // skip resource files
        // skip xml Serializers
        // skip ignored assemblies
        if (assemblyName.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase)
            || assemblyName.Name.EndsWith(".XmlSerializers", StringComparison.OrdinalIgnoreCase)
            || ignoredAssemblies.Contains(assemblyName.Name, StringComparer.OrdinalIgnoreCase))
        {
            LogSkipped(assemblyName.Name);
            resolvedAssemblies[assemblyName.Name] = null;
            return null;
        }

        assembly = ResolveFromDisk(loadContext, assemblyName);
        resolvedAssemblies[assemblyName.Name] = assembly;
        return assembly;
    }

    private Assembly? ResolveFromDisk(System.Runtime.Loader.AssemblyLoadContext loadContext, AssemblyName assemblyName)
    {
        if (assemblyName == null || string.IsNullOrEmpty(assemblyName.Name))
        {
            return null;
        }

        var wantedDLL = assemblyName.Name + ".dll";
        var rootDir = AssemblyTypeLoader.AssemblyDirectory;
        var searchDirectories = new List<string>(extraDirectories) { rootDir };
        searchDirectories.AddRange(Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories));
        foreach (var dir in searchDirectories)
        {
            if (string.IsNullOrEmpty(dir))
            {
                continue;
            }

            LogSearchDirectory(assemblyName.Name, dir);
            var assemblyPath = Path.Combine(dir, wantedDLL);
            assemblyPath = Environment.ExpandEnvironmentVariables(assemblyPath);

            try
            {
                if (!File.Exists(assemblyPath))
                {
                    LogInvalidPath(assemblyName.Name, assemblyPath);
                    continue;
                }

                var foundName = AssemblyName.GetAssemblyName(assemblyPath);
                if (!RequestedAssemblyNameMatchesFound(assemblyName, foundName))
                {
                    LogWrongVersion(assemblyName.Name);
                    continue;
                }

                LogLoading(assemblyName.Name, assemblyPath);

                var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
                resolvedAssemblies[assemblyName.Name] = assembly;
                LogResolved(assemblyName.Name, assemblyPath);
                return assembly;
            }
            catch (FileLoadException ex)
            {
                LogLoadFailed(ex, assemblyName.Name);

                // Re-throw FileLoadException, because this exception means that the assembly was found, but could not be loaded.
                // This will allow us to report a more specific error message to the user for things like access denied.
                throw;
            }
            catch (Exception ex)
            {
                // For all other exceptions, try the next extension.
                LogLoadFailedContinue(ex, assemblyName.Name);
            }
        }

        LogNotFound(assemblyName.Name);
        return null;
    }
}
