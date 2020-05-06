namespace VectronsLibrary.Logging
{
    internal sealed class ErrorMessage
    {
        public ErrorMessage(string path, string msg)
        {
            FilePath = path;
            Message = msg;
        }

        public string FilePath
        {
            get; private set;
        }

        public string Message
        {
            get; private set;
        }
    }
}