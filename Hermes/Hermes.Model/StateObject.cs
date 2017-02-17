using System.Net.Sockets;
using System.Text;

namespace Hermes.Model
{
    // TODO: Doc.
    public sealed class StateObject
    {
        /// <summary>
        /// Size of receive buffer.
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// Client socket. 
        /// </summary>
        public Socket WorkerSocket { get; set; }
        /// <summary>
        /// Receive buffer. 
        /// </summary>
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        /// <summary>
        /// Received data string.
        /// </summary>  
        public StringBuilder StringBuilder { get; set; } = new StringBuilder();
    }
}
