using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VectronsLibrary.DI.Attributes;

namespace VectronsLibrary.DI.BufferedLogging;

/// <summary>
/// Default implementation for <see cref="IBufferedLogger"/>.
/// </summary>
/// <typeparam name="T">The type who's name is used for the logger category name.</typeparam>
[Ignore]
public class BufferedLogger<T> : ILogger<T>, IBufferedLogger
{
    private readonly Queue<IBufferItem> bufferItems = new();
    private ILogger? newLogger;

    /// <summary>
    /// Interface to hide the generic parameter.
    /// </summary>
    [Ignore]
    private interface IBufferItem
    {
        /// <summary>
        /// Log item to the given logger.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> where the item needs to be written.</param>
        void Log(ILogger logger);
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => newLogger == null
            ? EmptyDisposable.Instance
            : newLogger.BeginScope(state);

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
        => newLogger == null
            || newLogger.IsEnabled(logLevel);

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (newLogger != null)
        {
            newLogger.Log(logLevel, eventId, state, exception, formatter);
            return;
        }

        bufferItems.Enqueue(new BufferItem<TState>(logLevel, eventId, state, exception, formatter));
    }

    /// <inheritdoc/>
    public void WriteItems(ILogger logger)
    {
        newLogger = logger;
        while (bufferItems.Count > 0)
        {
            bufferItems.Dequeue().Log(logger);
        }
    }

    private sealed class BufferItem<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
        : IBufferItem
    {
        public void Log(ILogger logger)
            => logger.Log(logLevel, eventId, state, exception, formatter);
    }

    private sealed class EmptyDisposable : IDisposable
    {
        // Explicit static constructor to tell C# compiler not to mark type as before field initialize
        static EmptyDisposable()
            => Instance = new EmptyDisposable();

        private EmptyDisposable()
        {
        }

        public static EmptyDisposable Instance
        {
            get;
        }

        public void Dispose()
        {
        }
    }
}
