﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace VectronsLibrary.DI
{
    [Ignore]
    public class LogBuffer : ILogger
    {
        private readonly Queue<IBufferItem> bufferItems = new Queue<IBufferItem>();

        [Ignore]
        private interface IBufferItem
        {
            void Log(ILogger logger);
        }

        public IDisposable BeginScope<TState>(TState state)
            => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => bufferItems.Enqueue(new BufferItem<TState>(logLevel, eventId, state, exception, formatter));

        public void WriteItems(ILogger logger)
        {
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
    }
}