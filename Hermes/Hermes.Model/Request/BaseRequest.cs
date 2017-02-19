using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public class BaseRequest
    {
        [JsonProperty("cmd")]
        public string CommandName { get; set; }
        [JsonProperty("id")]
        public string UserId { get; set; }
        [JsonProperty("msgNr")]
        public int MessageIndex { get; set; }
    }
}
