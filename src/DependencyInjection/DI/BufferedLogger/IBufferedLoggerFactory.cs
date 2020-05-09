using Microsoft.Extensions.Logging;
using System;

namespace VectronsLibrary.DI
{
    [Ignore]
    public interface IBufferedLoggerFactory
    {
        ILogger<T> CreateLogger<T>();

        void LogAll(IServiceProvider serviceProvider);
    }
}