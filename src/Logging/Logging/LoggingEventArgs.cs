using System;

namespace VectronsLibrary.Logging;

/// <summary>
/// Event data when a item is logged.
/// </summary>
public sealed class LoggingEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingEventArgs"/> class.
    /// </summary>
    /// <param name="text">The message that was logged.</param>
    /// <param name="file">The file path the message will be logged to.</param>
    public LoggingEventArgs(string text, string file)
    {
        Message = text;
        File = file;
    }

    /// <summary>
    /// Gets the file path the message will be logged to.
    /// </summary>
    public string File
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the message that was logged.
    /// </summary>
    public string Message
    {
        get;
        private set;
    }
}
