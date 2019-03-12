using System.Net.Sockets;
using System.Text;

namespace VectronsLibrary.Ethernet
{
    // State object for receiving data from remote device.
    public sealed class StateObject
    {
        // Size of receive buffer.
        public const int BufferSize = 1024;

        public StateObject(Socket workSocket)
        {
            WorkSocket = workSocket;
        }

        // Receive buffer.
        public byte[] Buffer { get; set; } = new byte[BufferSize];

        // Received data string.
        public StringBuilder Sb { get; set; } = new StringBuilder();

        // Client socket.
        public Socket WorkSocket
        {
            get;
        }
    }
}