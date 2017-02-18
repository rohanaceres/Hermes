using Newtonsoft.Json;

namespace Hermes.Model.Request
{
    // TODO: Doc.
    public sealed class LoginRequest
    {
        [JsonProperty(PropertyName = "cmd")]
        public string CommandName { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "msgNr")]
        public int MessageIndex { get; set; }
    }
}
