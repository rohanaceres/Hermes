using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class LoginResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "data")]
        public PendingMessages Data { get; set; }
    }
    public sealed class PendingMessages
    {
        public List<PendingMessage> Messages { get; set; }
    }
    public sealed class PendingMessage
    {
        [JsonProperty("src")]
        public string Sender { get; set; }
        [JsonProperty("data")]
        public string Message { get; set; }
    }
}
