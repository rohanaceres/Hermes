using Newtonsoft.Json;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public abstract class BaseResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "msgNr")]
        public int MessageIndex { get; set; }
    }
}
