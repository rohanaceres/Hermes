using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public sealed class SendRequest : BaseRequest
    {
        [JsonProperty("dst")]
        public string DestinationUserId { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }

        public SendRequest()
        {
            this.CommandName = "enviar";
        }
    }
}
