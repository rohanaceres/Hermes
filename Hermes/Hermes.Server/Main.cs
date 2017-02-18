using System;

namespace Hermes.Server
{
    class Main_
    {
        static void Main()
        {
            AsyncListener server = new AsyncListener();

            server.Initialize();
            Console.ReadKey();
            server.CloseAllSockets();
        }
    }
}
