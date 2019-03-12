using System.Net.Sockets;

namespace VectronsLibrary.Ethernet
{
    public sealed class ReceivedData
    {
        private readonly string message;
        private readonly Socket socket;

        public ReceivedData(string text, Socket socket)
        {
            message = text;
            this.socket = socket;
        }

        public string GetMessage()
        {
            return message;
        }

        public Socket GetSender()
        {
            return socket;
        }
    }
}