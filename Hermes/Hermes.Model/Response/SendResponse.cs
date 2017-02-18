using Newtonsoft.Json;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class SendResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "msgNr")]
        public int MessageIndex { get; set; }
    }
}
