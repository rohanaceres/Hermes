using Hermes.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Core
{
    // TODO: Doc.
    public sealed class CustomSocket
    {
        public string HostName { get; set; }

        // connection details:
        private IPHostEntry IpHostInfo { get; set; }
        private IPAddress IpAddress { get; set; }
        private IPEndPoint EndPoint { get; set; } 
        private Socket Socket { get; set; }

        // flow handlers:
        public ManualResetEvent OnConnectionReceived = new ManualResetEvent(false);
        private ManualResetEvent OnConnected = new ManualResetEvent(false);

        // constructor
        public CustomSocket(string hostName = null)
        {
            // Get host name, if necessary:
            if (hostName == null)
            {
                hostName = this.AskForHostName();
            }

            // Get mandatory info:
            this.IpHostInfo = Dns.GetHostEntry(hostName);
            this.IpAddress = this.IpHostInfo.AddressList[0];
            this.EndPoint = new IPEndPoint(this.IpAddress, 
                CommunicationProperties.CommunicationPort);

            // Create a TCP/IP socket.  
            this.Socket = new Socket(this.EndPoint.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
        }

        // public folks
        public void BindAndListen()
        {
            this.Socket.Bind(this.EndPoint);
            this.Socket.Listen(100);
        }
        public void WaitForConnection()
        {
            // Set the event to nonsignaled state.  
            this.OnConnectionReceived.Reset();

            // Start an asynchronous socket to listen for connections.  
            Console.WriteLine("Waiting for a connection...");
            this.Socket.BeginAccept(new AsyncCallback(this.AcceptCallback),
                this.Socket);

            // Wait until a connection is made before continuing.  
            this.OnConnectionReceived.WaitOne();
        }
        public Task Connect ()
        {
            return Task.Run(() =>
            {
                this.Socket.BeginConnect(this.EndPoint, new AsyncCallback(this.EndConnectCallback),
                    this.Socket);
                this.OnConnected.WaitOne();
            });
        }

        // private folks
        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        // callbacks...
        private void EndConnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)asyncResult.AsyncState;

                // Complete the connection.  
                client.EndConnect(asyncResult);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                this.OnConnected.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            this.OnConnectionReceived.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.WorkerSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(this.ReadCallback), state);
        }
        private void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkerSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.StringBuilder.Append(Encoding.ASCII.GetString(
                    state.Buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.StringBuilder.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.  
                    this.Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // privates...
        private string AskForHostName ()
        {
            Console.Write("HostName: ");
            return Console.ReadLine();
        }
    }
}
