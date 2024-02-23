namespace VectronsLibrary.Logging;

/// <summary>
/// A error message that needs to be logged to a file.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ErrorMessage"/> class.
/// </remarks>
/// <param name="path">The file location to write the message to.</param>
/// <param name="msg">The message that needs to be written.</param>
internal sealed class ErrorMessage(string path, string msg)
{
    /// <summary>
    /// Gets the file location to write the message to.
    /// </summary>
    public string FilePath { get; private set; } = path;

    /// <summary>
    /// Gets the message that needs to be written.
    /// </summary>
    public string Message { get; private set; } = msg;
}
