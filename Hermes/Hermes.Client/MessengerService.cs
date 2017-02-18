using Hermes.Model;
using System;
using System.Net;
using System.Net.Sockets;

namespace Hermes.Client
{
    // TODO: Doc.
    internal sealed class MessengerService
    {
        // properties
        private Socket ClientSocket { get; set; }
            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // public folks
        public void Connect()
        {
            int attempts = 1;

            do
            {
                try
                {
                    Console.WriteLine("Connection attempt {0}", attempts);

                    // Tries to connect to the remote host:
                    this.ClientSocket.Connect(IPAddress.Loopback,
                        CommunicationProperties.CommunicationPort);
                }
                catch (SocketException)
                {
                    // If an exception occurr, clear the console to print
                    // the number of tries.
                    Console.Clear();
                }
                finally
                {
                    attempts++;
                }
            }
            while (this.ClientSocket.Connected == false);

            Console.Clear();
            Console.WriteLine("Successfully connected to {0}!", 
                this.ClientSocket.RemoteEndPoint.ToString());
        }
        public void Chat ()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");

            while (true)
            {
                string request = this.GetRequest();
                this.Send(request);
                string response = this.ReceiveResponse();

                Console.WriteLine("Hermes > {0}", response);
            }
        }
        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        public void Exit()
        {
            Send("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        // private folks
        private string GetRequest()
        {
            Console.Write("Me > ");
            string request = Console.ReadLine();

            // TODO: Implementação mais inteligente de comandos?
            if (request.ToLower() == "exit")
            {
                this.Exit();
            }

            return request;
        }
        /// <summary>
        /// Sends a string to the server with the encoding
        /// defined in <see cref="CommunicationProperties.CommunicationEncoding"/>.
        /// </summary>
        private void Send(string text)
        {
            byte[] buffer = CommunicationProperties.CommunicationEncoding
                .GetBytes(text);
            this.ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        private string ReceiveResponse()
        {
            byte[] buffer = new byte[CommunicationProperties.PackageSize];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);

            // If nothing received, returns NULL.
            if (received <= 0) { return null; }
            
            string text = CommunicationProperties.CommunicationEncoding
                .GetString(buffer, 0, received);

            return text;
        }
    }
}
