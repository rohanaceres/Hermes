using Hermes.Core.Serialization;
using Hermes.Model;
using Hermes.Model.Request;
using Hermes.Model.Response;
using Hermes.Server.Command;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Hermes.Server
{
    // TODO: Doc.
    internal sealed class AsyncListener
    {
        // containers (ew!)
        public static List<string> ConnectedClientIds { get; set; }
            = new List<string>();
        public static List<Message> PendingMessages { get; set; }
            = new List<Message>();
        public static List<TocTocJoke> PendingJokes { get; set; }
            = new List<TocTocJoke>();

        // properties...
        public List<Socket> ConnectedSockets { get; set; } 
            = new List<Socket>();

        // public folks...
        public void Initialize()
        {
            Console.Title = string.Format("Hermes Server - {0}",
                Dns.GetHostName());
            // TODO: Retirar mock daqui!
            this.MockStuff();
            Console.WriteLine("Setting up server...");

            this.ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 
                CommunicationProperties.CommunicationPort));
            this.ServerSocket.Listen(0);
            this.ServerSocket.BeginAccept(AcceptCallback, null);

            Console.WriteLine("Server setup complete");
        }

        private Socket ServerSocket { get; set; } 
            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private byte[] Buffer { get; set; }
            = new byte[CommunicationProperties.PackageSize];

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server 
        /// socket as its connections are already closed with the clients).
        /// </summary>
        public void CloseAllSockets()
        {
            foreach (Socket currentClient in this.ConnectedSockets)
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

            this.ConnectedSockets.Add(socket);

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
                this.ConnectedSockets.Remove(current);
                return;
            }

            // Get request as string:
            byte[] recBuf = new byte[received];
            Array.Copy(Buffer, recBuf, received);
            string text = CommunicationProperties.CommunicationEncoding
                .GetString(recBuf);
            Console.WriteLine("Request: <{0}>", text);

            // Get request as serialized object:
            BaseRequest request = this.GetRequest(text);

            if (request == null)
            {
                // TODO: Retornar mensagem de erro.
            }
            else
            {
                // Perform some asynchronous action based on the request received:
                BaseResponse response = CommandFactory.Build(request);

                // Create the corresponding response, based on the action
                // performed previously:
                current.Send(this.GetResponseData(response));

                current.BeginReceive(Buffer, 0, CommunicationProperties.PackageSize,
                    SocketFlags.None, ReceiveCallback, current);
            }
       }

        private BaseRequest GetRequest(string json)
        {
            JsonSerializerMotherfucka serializer = new JsonSerializerMotherfucka();
            BaseRequest request = serializer.Deserialize<BaseRequest>(json);

            if (request.CommandName == "login")
            {
                request = serializer.Deserialize<LoginRequest>(json);
            }
            else if (request.CommandName == "receber")
            {
                request = serializer.Deserialize<ReceiveRequest>(json);
            }
            else if (request.CommandName == "enviar")
            {
                request = serializer.Deserialize<SendRequest>(json);
            }

            return request;
        }
        private byte[] GetResponseData(BaseResponse response)
        {
            string serializedResponse;

            if (response is LoginResponse)
            {
                serializedResponse = (response as LoginResponse).SerializeToJson<LoginResponse>();
            }
            else if (response is ReceiveResponse)
            {
                serializedResponse = (response as ReceiveResponse).SerializeToJson<ReceiveResponse>();
            }
            else
            {
                serializedResponse = (response as SendResponse).SerializeToJson<SendResponse>();
            }

            Console.WriteLine("Response: <{0}>", serializedResponse);
            return CommunicationProperties.CommunicationEncoding
                .GetBytes(serializedResponse);
        }

        private void MockStuff()
        {
            AsyncListener.PendingMessages.Add(new Message
            {
                Data = "hola que tal!",
                DestinationUserId = "maria",
                SourceUserId = "ceres"
            });
        }
    }
}
