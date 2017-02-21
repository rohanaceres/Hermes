using Hermes.Client.Option;
using Hermes.Core.Serialization;
using Hermes.Model;
using Hermes.Model.Request;
using Hermes.Model.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Client
{
    // TODO: Doc.
    internal sealed class MessengerService
    {
        // container
        public static string ClientId { get; private set; }

        // properties
        private Socket ClientSocket { get; set; }
            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Socket ReceiveSocket { get; set; }
            = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // public folks
        public void Connect()
        {
            this.ConnectSocket(this.ClientSocket);
            this.ConnectSocket(this.ReceiveSocket);
        }
        private void ConnectSocket(Socket socketToConnec)
        {
            int attempts = 1;

            do
            {
                try
                {
                    Console.WriteLine("Connection attempt {0}", attempts);

                    // Tries to connect to the remote host:
                    socketToConnec.Connect(IPAddress.Loopback,
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
                socketToConnec.RemoteEndPoint.ToString());
        }
        public void Chat ()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");

            Thread t = new Thread(() =>
            {
                do
                {
                    this.GetPendingMessages();
                    Thread.Sleep(500);
                }
                while (true);
            });
            t.IsBackground = true;
            t.Start();

            while (true)
            {
                // Send the request:
                string request = this.GetRequest();
                this.Send(request);
            }
        }
        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        public void Logoff()
        {
            LogoffRequest logoff = new LogoffRequest()
            {
                UserId = MessengerService.ClientId
            };

            this.SendRequest(logoff.SerializeToJson(), this.ClientSocket);

            string jsonLogoffResponse = this.ReceiveResponse(this.ClientSocket);

            MessengerService.ClientId = null;

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();

            Environment.Exit(0);
        }

        // private folks
        private void GetPendingMessages()
        {
            if (string.IsNullOrEmpty(MessengerService.ClientId) == true)
            {
                return;
            }

            // Send a receive request:
            ReceiveRequest receiveRequest = new ReceiveRequest()
            {
                MessageIndex = 0,
                UserId = MessengerService.ClientId
            };
            this.SendRequest(receiveRequest.SerializeToJson(), this.ReceiveSocket);

            // Get the response from the server:
            string jsonReceiveResponse = this.ReceiveResponse(this.ReceiveSocket);

            // Deserialize the response from the JSON string:
            ReceiveResponse receiveResponse = jsonReceiveResponse
                .DeserializeFromJson<ReceiveResponse>();
            
            if (receiveResponse.Data == null || 
                receiveResponse.Data.Messages == null ||
                receiveResponse.Data.Messages.Count <= 0)
            {
                return;
            }

            // Shows the response in the console:
            this.ShowMessages(receiveResponse.Data.Messages);
        }
        private void ShowMessages(List<PendingMessage> messages)
        {
            if (messages == null || messages.Count <= 0) { return; }
            
            this.RemovePreviousLine();

            foreach (PendingMessage msg in messages)
            {
                Console.WriteLine("{0} > {1}", msg.Sender, msg.Message);
            }
            Console.Write("Me > ");
        }
        private void RemovePreviousLine()
        {
            int currentCursor = Console.CursorTop;
            Console.SetCursorPosition(0, currentCursor);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentCursor);
        }

        private string GetRequest()
        {
            Console.Write("Me > ");
            string request = Console.ReadLine();

            return request;
        }
        /// <summary>
        /// Sends a string to the server with the encoding
        /// defined in <see cref="CommunicationProperties.CommunicationEncoding"/>.
        /// </summary>
        private void Send(string text)
        {
            string json = string.Empty;

            // TODO: Converts the string into a command to the server:
            string[] args = text.Split(' ');
            if (args[0] == "login")
            {
                // Read command args:
                LoginOption login = new LoginOption();
                CommandLine.Parser.Default.ParseArguments(args, login);

                // Setup request and serialize it into a string:
                LoginRequest request = new LoginRequest()
                {
                    UserId = login.UserId
                };

                // Serialize the object into a JSON:
                json = request.SerializeToJson();

                // Send the requesr as JSON:
                this.SendRequest(json, this.ClientSocket);
                
                // Receive the response:
                string jsonResponse = this.ReceiveResponse(this.ClientSocket);

                // Deserialize response from JSON to object:
                LoginResponse response = jsonResponse.DeserializeFromJson<LoginResponse>();

                // Show possible pending messages:
                this.ShowMessages(response.Data.Messages);

                MessengerService.ClientId = request.UserId;
            }
            else if (args[0] == "send")
            {
                // Read command args:
                SendOption send = new SendOption();
                CommandLine.Parser.Default.ParseArguments(args, send);

                // Setup request and serialize it into a string:
                SendRequest request = new SendRequest()
                {
                    DestinationUserId = send.DestinationUser,
                    UserId = MessengerService.ClientId,
                    Data = string.Join(" ", send.WordsInMessage)
                };

                // Serialize the object into a JSON:
                json = request.SerializeToJson();

                // Send the requesr as JSON:
                this.SendRequest(json, this.ClientSocket);

                // Receive the response:
                string jsonResponse = this.ReceiveResponse(this.ClientSocket);

                // Deserialize response from JSON to object:
                LoginResponse response = jsonResponse.DeserializeFromJson<LoginResponse>();
            }
            else if (args[0] == "logoff")
            {
                this.Logoff();
            }
            else
            {
                this.RemovePreviousLine();
            }
        }
        private void SendRequest (string json, Socket socket)
        {
            // Send the request:
            byte[] buffer = CommunicationProperties.CommunicationEncoding
                .GetBytes(json);
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        private string ReceiveResponse(Socket socket)
        {
            byte[] buffer = new byte[CommunicationProperties.PackageSize];
            int received = socket.Receive(buffer, SocketFlags.None);

            // If nothing received, returns NULL.
            if (received <= 0) { return null; }
            
            string text = CommunicationProperties.CommunicationEncoding
                .GetString(buffer, 0, received);

            return text;
        }
    }
}
