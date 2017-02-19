using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hermes.Model.Response
{
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
