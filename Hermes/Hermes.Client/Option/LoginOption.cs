using CommandLine;

namespace Hermes.Client.Option
{
    // TODO: Doc.
    internal sealed class LoginOption
    {
        [Option("username", Required = true)]
        public string UserId { get; set; }
    }
}
