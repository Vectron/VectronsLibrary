using System;
using Microsoft.Extensions.Logging;
using VectronsLibrary.DI.Attributes;

namespace VectronsLibrary.DI.BufferedLogging;

/// <summary>
/// A buffered logger factory to create multiple log buffers to dump to Ilogger later.
/// </summary>
[Ignore]
public interface IBufferedLoggerFactory
{
    /// <summary>
    /// Creates a <see cref="ILogger"/> for a certain type.
    /// </summary>
    /// <typeparam name="T">The type who's name is used for the logger category name.</typeparam>
    /// <returns>A <see cref="ILogger{TCategoryName}"/>.</returns>
    ILogger<T> CreateLogger<T>();

    /// <summary>
    /// Log all values to the Logger setup in the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to get the real loggers from.</param>
    void LogAll(IServiceProvider serviceProvider);
}
