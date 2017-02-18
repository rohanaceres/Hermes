using Hermes.Core;

namespace Hermes.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            AsynListener server = new AsynListener();
            server.Start();
        }
    }
}
