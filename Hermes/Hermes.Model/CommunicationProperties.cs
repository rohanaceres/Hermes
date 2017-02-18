using System.Text;

namespace Hermes.Model
{
    // TODO: Doc.
    public abstract class CommunicationProperties
    {
        public const int CommunicationPort = 1234;
        public static Encoding CommunicationEncoding = Encoding.UTF8;
        public const short PackageSize = 2048;
        public const int ServerId = 0;
    }
}
