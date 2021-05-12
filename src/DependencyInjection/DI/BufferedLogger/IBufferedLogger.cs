using Microsoft.Extensions.Logging;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// A logger that buffers the messages until they can be writen to the final logger setup.
    /// </summary>
    [Ignore]
    public interface IBufferedLogger
    {
        /// <summary>
        /// Write all items to the given <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">An <see cref="ILogger"/> instance used for logging.</param>
        void WriteItems(ILogger logger);
    }
}