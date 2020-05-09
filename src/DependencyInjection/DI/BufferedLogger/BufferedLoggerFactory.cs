using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace VectronsLibrary.DI
{
    [Ignore]
    public class BufferedLoggerFactory
    {
        private readonly Dictionary<Type, IBufferedLogger> loggers = new Dictionary<Type, IBufferedLogger>();

        public ILogger<T> CreateLogger<T>()
        {
            if (!loggers.TryGetValue(typeof(T), out var logger))
            {
                logger = new BufferedLogger<T>();
                loggers.Add(typeof(T), logger);
            }

            return (ILogger<T>)logger;
        }

        public void LogAll(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
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
}