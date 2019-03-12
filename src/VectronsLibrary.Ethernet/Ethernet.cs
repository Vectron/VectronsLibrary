using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace VectronsLibrary.Ethernet
{
    public abstract class Ethernet : IEthernet
    {
        protected readonly BehaviorSubject<IConnected<Socket>> connectionState = new BehaviorSubject<IConnected<Socket>>(Connected.No<Socket>(null));
        protected readonly ILogger logger;
        private ISubject<ReceivedData> DataReceived = new Subject<ReceivedData>();

        public Ethernet(ILogger<Ethernet> logger)
        {
            this.logger = logger;
        }

        public IObservable<ReceivedData> ReceivedDataStream => DataReceived.AsObservable();
        public IObservable<IConnected<Socket>> SessionStream => connectionState.AsObservable();

        public abstract void Dispose();

        public virtual void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            logger.LogDebug($"Sending: {data}  - To: {handler.RemoteEndPoint.ToString()}");
        }

        protected virtual void ReceiveCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the client socket
            // from the asynchronous state object.
            var state = (StateObject)ar.AsyncState;
            Socket socket = state.WorkSocket;
            try
            {
                // Read data from the remote device.
                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

                    if (socket.Available == 0)
                    {
                        DataReceived.OnNext(new ReceivedData(state.Sb.ToString(), socket));
                        logger.LogDebug($"Received: {state.Sb.ToString()} - From: {socket.RemoteEndPoint.ToString()}");
                        state.Sb.Clear();
                    }

                    // Get the rest of the data.
                    socket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    logger.LogInformation($"{socket.RemoteEndPoint} requested a shutdown");
                    Shutdown(socket);
                }
            }
            catch (ObjectDisposedException ex)
            {
                if (socket.Connected)
                {
                    logger.LogError($"{ex.Message}, Failed receiving data from {socket.RemoteEndPoint}");
                    connectionState.OnNext(Connected.No(socket));
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}, Failed receiving data from {socket.RemoteEndPoint}");
                connectionState.OnNext(Connected.No(socket));
            }
        }

        protected virtual void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            var client = (Socket)ar.AsyncState;
            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}, Failed sending data to {client.RemoteEndPoint.ToString()}");
                connectionState.OnNext(Connected.No(client));
            }
        }

        protected virtual void Shutdown(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }

            socket.Close();
            connectionState.OnNext(Connected.No(socket));
        }
    }
}