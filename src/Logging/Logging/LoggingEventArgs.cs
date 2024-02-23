using System;

namespace VectronsLibrary.Logging;

/// <summary>
/// Event data when a item is logged.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LoggingEventArgs"/> class.
/// </remarks>
/// <param name="text">The message that was logged.</param>
/// <param name="file">The file path the message will be logged to.</param>
public sealed class LoggingEventArgs(string text, string file) : EventArgs
{
    /// <summary>
    /// Gets the file path the message will be logged to.
    /// </summary>
    public string File { get; private set; } = file;

    /// <summary>
    /// Gets the message that was logged.
    /// </summary>
    public string Message { get; private set; } = text;
}
