using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// Helper class for getting assembly data.
    /// </summary>
    public static class Helper
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
                var builder = new StringBuilder()
                    .AppendLine($"Failed to load assembly {assembly}")
                    .AppendLine("\t" + reflectionTypeLoadException.Message);

                foreach (var item in reflectionTypeLoadException.LoaderExceptions)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    _ = builder.AppendLine("\t\t" + item.Message);
                }

                logger?.LogWarning(builder.ToString());
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to load assembly {0}", assembly);
            }

            return Array.Empty<Type>();
        }
    }
}