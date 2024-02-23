namespace VectronsLibrary.Logging;

/// <summary>
/// A error message that needs to be logged to a file.
/// </summary>
internal sealed class ErrorMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
    /// </summary>
    /// <param name="path">The file location to write the message to.</param>
    /// <param name="msg">The message that needs to be written.</param>
    public ErrorMessage(string path, string msg)
    {
        FilePath = path;
        Message = msg;
    }

    /// <summary>
    /// Gets the file location to write the message to.
    /// </summary>
    public string FilePath
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the message that needs to be written.
    /// </summary>
    public string Message
    {
        get;
        private set;
    }
}
