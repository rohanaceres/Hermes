using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public abstract class BaseRequest
    {
        [JsonProperty("cmd")]
        public string CommandName { get; set; }
        [JsonProperty("id")]
        public int UserId { get; set; }
        [JsonProperty("msgNr")]
        public int MessageIndex { get; set; }
    }
}
