using Hermes.Core;
using System;
using System.Net;

namespace Hermes.Server
{
    // TODO: Doc.
    internal sealed class AsynListener
    {
        public CustomSocket Socket { get; private set; }
            = new CustomSocket(Dns.GetHostName());

        static void Main(string[] args)
        {
            AsynListener server = new AsynListener();
            server.Start();
        }

        public void Start()
        {
            try
            {
                this.Socket.BindAndListen();

                Console.WriteLine("Host Name: {0}", this.Socket.HostName);

                // Main loop, waiting for requests and performing some action:
                while (true)
                {
                    this.Socket.WaitForConnection();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: <{0}>{1}{2}", e.GetType(),
                    Environment.NewLine, e.Message);
            }
        }
    }
}
