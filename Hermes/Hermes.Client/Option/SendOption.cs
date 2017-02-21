using CommandLine;

namespace Hermes.Client.Option
{
    internal sealed class SendOption
    {
        [OptionArray("msg", Required = true)]
        public string[] WordsInMessage { get; set; }
        [Option("to", Required = true)]
        public string DestinationUser { get; set; }
    }
}
