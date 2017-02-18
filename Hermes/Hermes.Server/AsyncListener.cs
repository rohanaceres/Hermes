using Hermes.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Hermes.Server
{
    // TODO: Doc.
    internal sealed class AsyncListener
    {
        private Socket ServerSocket { get; set; } 
            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<Socket> ConnectedClients { get; private set; } 
            = new List<Socket>();

        private byte[] Buffer 
            = new byte[CommunicationProperties.PackageSize];

        

        // public folks...
        public void Initialize()
        {
            Console.Title = string.Format("Hermes Server - {0}",
                Dns.GetHostName());
            Console.WriteLine("Setting up server...");

            this.ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 
                CommunicationProperties.CommunicationPort));
            this.ServerSocket.Listen(0);
            this.ServerSocket.BeginAccept(AcceptCallback, null);

            Console.WriteLine("Server setup complete");
        }
        /// <summary>
        /// Close all connected client (we do not need to shutdown the server 
        /// socket as its connections are already closed with the clients).
        /// </summary>
        public void CloseAllSockets()
        {
            foreach (Socket currentClient in this.ConnectedClients)
            {
                currentClient.Shutdown(SocketShutdown.Both);
                currentClient.Close();
            }

            this.ServerSocket.Close();
        }

        // callbacks...
        private void AcceptCallback(IAsyncResult results)
        {
            Socket socket;

            try
            {
                socket = ServerSocket.EndAccept(results);
            }
            catch (ObjectDisposedException) 
            {
                // I cannot seem to avoid this (on exit when properly closing sockets)
                return;
            }

            this.ConnectedClients.Add(socket);

            socket.BeginReceive(this.Buffer, 0, CommunicationProperties.PackageSize, 
                SocketFlags.None, this.ReceiveCallback, socket);

            Console.WriteLine("Client connected, waiting for request...");

            this.ServerSocket.BeginAccept(AcceptCallback, null);
        }
        private void ReceiveCallback(IAsyncResult results)
        {
            Socket current = results.AsyncState as Socket;
            int received;

            try
            {
                received = current.EndReceive(results);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");

                // Don't shutdown because the socket may be disposed and 
                // its disconnected anyway:
                current.Close();
                this.ConnectedClients.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(Buffer, recBuf, received);
            string text = CommunicationProperties.CommunicationEncoding
                .GetString(recBuf);
            Console.WriteLine("Request: <{0}>", text);

            // TODO: Implementar comandos!
            string response = "I'm not fully implemented yet. But I like you. Keed texting.";
            Console.WriteLine("Response: <{0}>", response);
            byte[] data = CommunicationProperties.CommunicationEncoding
                    .GetBytes(response);
            current.Send(data);

            current.BeginReceive(Buffer, 0, CommunicationProperties.PackageSize, 
                SocketFlags.None, ReceiveCallback, current);
        }
    }
}
