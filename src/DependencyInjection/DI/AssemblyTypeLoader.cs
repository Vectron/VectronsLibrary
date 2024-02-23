using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace VectronsLibrary.DI;

/// <summary>
/// Helper class for getting assembly data.
/// </summary>
public static partial class AssemblyTypeLoader
{
    /// <summary>
    /// Gets the current assembly directory.
    /// </summary>
    public static string AssemblyDirectory
    {
        get
        {
            var codeBase = Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(codeBase))
            {
                codeBase = Assembly.GetExecutingAssembly().Location;
            }

            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path) ?? string.Empty;
        }
    }

    /// <summary>
    /// Load all types from an assembly and return them.
    /// </summary>
    /// <param name="assembly">The assembly name to load types from.</param>
    /// <returns>A collection of all the types found in the assembly.</returns>
    public static Type[] LoadTypesFromAssemblySafe(string assembly)
        => LoadTypesFromAssemblySafe(assembly, NullLogger.Instance);

    /// <inheritdoc cref="LoadTypesFromAssemblySafe(string)"/>
    /// /// <param name="assembly">The assembly name to load types from.</param>
    /// <param name="logger">An <see cref="ILogger"/> instance used for logging.</param>
    public static Type[] LoadTypesFromAssemblySafe(string assembly, ILogger logger)
    {
        try
        {
            return File.Exists(assembly)
                ? Assembly.LoadFrom(assembly).GetTypes()
                : Assembly.Load(new AssemblyName(assembly)).GetTypes();
        }
        catch (ReflectionTypeLoadException reflectionTypeLoadException)
        {
            var builder = new StringBuilder();
            foreach (var item in reflectionTypeLoadException.LoaderExceptions)
            {
                if (item == null)
                {
                    continue;
                }

                _ = builder.AppendLine("\t\t" + item.Message);
            }

            logger?.LogFailedToLoadAssembly(reflectionTypeLoadException, assembly, reflectionTypeLoadException.Message, builder.ToString());
        }
        catch (Exception ex)
        {
            logger?.LogFailedToLoadAssembly(ex, assembly, ex.Message, ex.InnerException?.Message);
        }

        return [];
    }

    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Warning,
        Message = "Failed to load assembly {Assembly}\t{Message}{InnerMessages}")]
    private static partial void LogFailedToLoadAssembly(this ILogger logger, Exception exception, string assembly, string message, string? innerMessages);
}
