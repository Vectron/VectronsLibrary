using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VectronsLibrary.DI.Attributes;

namespace VectronsLibrary.DI.BufferedLogger;

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
        var generic = typeof(ILogger<>);
        foreach (var kv in loggers)
        {
            var fullType = generic.MakeGenericType(kv.Key);
            if (serviceProvider.GetService(fullType) is not ILogger target)
            {
                // No logger was added to the service collection;
                return;
            }

            kv.Value.WriteItems(target);
        }
    }
}
