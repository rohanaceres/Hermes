using CommandLine;

namespace Hermes.Client.Option
{
    internal sealed class SendOption
    {
        [Option("msg", Required = true)]
        public string Message { get; set; }
        [Option("to", Required = true)]
        public string DestinationUser { get; set; }
    }
}
