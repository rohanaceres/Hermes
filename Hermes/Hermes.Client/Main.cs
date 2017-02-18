using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.Client
{
    class Main_
    {
        static void Main()
        {
            Console.Title = "Client";

            MessengerService client = new MessengerService();

            client.Connect();
            client.Chat();
            client.Exit();
        }
    }
}
