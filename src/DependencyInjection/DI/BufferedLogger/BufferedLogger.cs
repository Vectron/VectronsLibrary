using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace VectronsLibrary.DI
{
    [Ignore]
    public class BufferedLogger<T> : ILogger<T>, IBufferedLogger
    {
        private readonly Queue<IBufferItem> bufferItems = new Queue<IBufferItem>();
        private ILogger newLogger = null;

        [Ignore]
        private interface IBufferItem
        {
            void Log(ILogger logger);
        }

        public IDisposable BeginScope<TState>(TState state)
            => newLogger == null
            ? EmptyDisposable.Instance
            : newLogger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel)
            => newLogger == null
            ? true
            : newLogger.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (newLogger != null)
            {
                newLogger.Log(logLevel, eventId, state, exception, formatter);
                return;
            }

            bufferItems.Enqueue(new BufferItem<TState>(logLevel, eventId, state, exception, formatter));
        }

        public void WriteItems(ILogger logger)
        {
            newLogger = logger;
            while (bufferItems.Count > 0)
            {
                bufferItems.Dequeue().Log(logger);
            }
        }

        private class BufferItem<TState> : IBufferItem
        {
            private readonly EventId eventId;
            private readonly Exception exception;
            private readonly Func<TState, Exception, string> formatter;
            private readonly LogLevel logLevel;
            private readonly TState state;

            public BufferItem(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                this.logLevel = logLevel;
                this.eventId = eventId;
                this.state = state;
                this.exception = exception;
                this.formatter = formatter;
            }

            public void Log(ILogger logger)
                => logger.Log(logLevel, eventId, state, exception, formatter);
        }

        private class EmptyDisposable : IDisposable
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static EmptyDisposable()
            {
                Instance = new EmptyDisposable();
            }

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
}