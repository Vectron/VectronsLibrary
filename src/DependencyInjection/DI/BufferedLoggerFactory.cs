using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace VectronsLibrary.DI
{
    [Ignore]
    public class BufferedLoggerFactory
    {
        private readonly Dictionary<Type, BufferedLogger> loggers = new Dictionary<Type, BufferedLogger>();

        public ILogger CreateLogger<T>()
        {
            if (!loggers.TryGetValue(typeof(T), out var logger))
            {
                logger = new BufferedLogger();
                loggers.Add(typeof(T), logger);
            }

            return logger;
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