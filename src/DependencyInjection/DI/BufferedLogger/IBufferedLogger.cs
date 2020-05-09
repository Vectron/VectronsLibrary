using Microsoft.Extensions.Logging;

namespace VectronsLibrary.DI
{
    [Ignore]
    public interface IBufferedLogger
    {
        void WriteItems(ILogger logger);
    }
}