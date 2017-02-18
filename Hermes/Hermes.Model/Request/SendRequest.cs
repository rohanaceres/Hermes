using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public class SendRequest : BaseRequest
    {
        [JsonProperty(PropertyName = "dst")]
        public int DestinationUserId { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
