using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public sealed class SendRequest
    {
        [JsonProperty(PropertyName = "cmd")]
        public string CommandName { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "msgNr")]
        public int MessageIndex { get; set; }
        [JsonProperty(PropertyName = "dst")]
        public int DestinationUserId { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
