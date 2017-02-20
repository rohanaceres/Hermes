using Hermes.Client.Option;
using Hermes.Core.Serialization;
using Hermes.Model;
using Hermes.Model.Request;
using Hermes.Model.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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

            Task.Run(() =>
            {
                Task.Delay(5000);
                do { this.GetPendingMessages(); } while (true);
            });

            while (true)
            {
                // Send the request:
                string request = this.GetRequest();
                this.Send(request);
                
                // TODO: Tirar daqui.
                this.GetPendingMessages();
            }
        }

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
            this.Send(receiveRequest.SerializeToJson());

            // Get the response from the server:
            string jsonReceiveResponse = this.ReceiveResponse();

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
            foreach (PendingMessage msg in messages)
            {
                Console.WriteLine("{0} > {1}", msg.Sender, msg.Message);
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
                this.SendRequest(json);
                
                // Receive the response:
                string jsonResponse = this.ReceiveResponse();

                // Deserialize response from JSON to object:
                LoginResponse response = jsonResponse.DeserializeFromJson<LoginResponse>();

                // Show possible pending messages:
                this.ShowMessages(response.Data.Messages);
            }
            
        }
        private void SendRequest (string json)
        {
            // Send the request:
            byte[] buffer = CommunicationProperties.CommunicationEncoding
                .GetBytes(json);
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
