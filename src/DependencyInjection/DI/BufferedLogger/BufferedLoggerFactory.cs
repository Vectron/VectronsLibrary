using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// Default implementation of <see cref="IBufferedLoggerFactory"/>.
    /// </summary>
    [Ignore]
    public class BufferedLoggerFactory : IBufferedLoggerFactory
    {
        private readonly Dictionary<Type, IBufferedLogger> loggers = new();

        /// <inheritdoc/>
        public ILogger<T> CreateLogger<T>()
        {
            if (!loggers.TryGetValue(typeof(T), out var logger))
            {
                logger = new BufferedLogger<T>();
                loggers.Add(typeof(T), logger);
            }

            return (ILogger<T>)logger;
        }

        /// <inheritdoc/>
        public void LogAll(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            foreach (var kv in loggers)
            {
                var generic = typeof(ILogger<>);
                var fullType = generic.MakeGenericType(kv.Key);
                var target = (ILogger)serviceProvider.GetService(fullType);
                kv.Value.WriteItems(target);
            }
        }
    }
}