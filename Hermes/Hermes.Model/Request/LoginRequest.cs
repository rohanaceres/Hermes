using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public sealed class LoginRequest : BaseRequest
    {
        public LoginRequest()
        {
            this.CommandName = "login";
        }
    }
}
