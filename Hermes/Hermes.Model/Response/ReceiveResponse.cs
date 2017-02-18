using Newtonsoft.Json;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class ReceiveResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "msgNr")]
        public int MessageIndex { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
