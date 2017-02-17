using Hermes.Model;
using System;
using System.Net;
using System.Net.Sockets;
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
        private ManualResetEvent OnConnected = new ManualResetEvent(false);

        public CustomSocket(string hostName = null)
        {
            // Get host name, if necessary:
            if (HostName == null)
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

        public Task Connect ()
        {
            return Task.Run(() =>
            {
                this.Socket.BeginConnect(this.EndPoint, new AsyncCallback(ConnectCallback),
                    this.Socket);
                this.OnConnected.WaitOne();
            });
        }

        // callbacks...
        private void ConnectCallback(IAsyncResult asyncResult)
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

        // privates...
        private string AskForHostName ()
        {
            Console.Write("HostName: ");
            return Console.ReadLine();
        }
    }
}
