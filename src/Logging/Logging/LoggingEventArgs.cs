using System;

namespace VectronsLibrary.Logging
{
    public sealed class LoggingEventArgs : EventArgs
    {
        public LoggingEventArgs(string text, string file)
        {
            Message = text;
            File = file;
        }

        public string File
        {
            get; private set;
        }

        public string Message
        {
            get; private set;
        }
    }
}