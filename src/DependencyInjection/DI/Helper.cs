using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace VectronsLibrary.DI
{
    public static class Helper
    {
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetEntryAssembly()?.CodeBase;
                if (string.IsNullOrEmpty(codeBase))
                {
                    codeBase = Assembly.GetExecutingAssembly().CodeBase;
                }
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static Type[] LoadTypesFromAssemblySafe(string assembly, ILogger logger = null)
        {
            try
            {
                if (File.Exists(assembly))
                {
                    return Assembly.LoadFrom(assembly).GetTypes();
                }

                return Assembly.Load(new AssemblyName(assembly)).GetTypes();
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                var builder = new StringBuilder()
                    .AppendLine($"Failed to load assembly {assembly}")
                    .AppendLine("\t" + reflectionTypeLoadException.Message);

                foreach (var item in reflectionTypeLoadException.LoaderExceptions)
                {
                    builder.AppendLine("\t\t" + item.Message);
                }

                logger?.LogWarning(builder.ToString());
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to load assembly {0}", assembly);
            }

            return new Type[0];
        }
    }
}