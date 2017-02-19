namespace Hermes.Model.Request
{
    // TODO: Doc.
    // TODO: Add testes de serialização para ele.
    public sealed class LogoffRequest : BaseRequest
    {
        public LogoffRequest()
        {
            this.CommandName = "logoff";
        }
    }
}
