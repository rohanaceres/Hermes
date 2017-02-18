using Hermes.Core;
using System;
using System.Net;

namespace Hermes.Server
{
    // TODO: Doc.
    internal sealed class AsynListener
    {
        public CustomSocket Socket { get; set; } 

        public AsynListener()
        {
            string hostName = Dns.GetHostName();
            this.Socket = new CustomSocket(hostName);
        }

        public void Start()
        {
            try
            {
                this.Socket.BindAndListen();

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
