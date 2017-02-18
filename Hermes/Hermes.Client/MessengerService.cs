using Hermes.Core;
using System;

namespace Hermes.Client
{
    // TODO: Doc.
    internal sealed class MessengerService
    {
        public CustomSocket Socket { get; private set; }
            = new CustomSocket("DESKTOP-VUD70BH");

        static void Main(string[] args)
        {
            MessengerService client = new MessengerService();
            client.Start();
        }

        // TODO: Criar thread para ficar buscando resposta do servidor!

        public void Start ()
        {
            this.Socket.Connect();

            while (true)
            {
                Console.Write("> ");
                string message = Console.ReadLine();
                this.Socket.Send(message);
            }
        }
    }
}
