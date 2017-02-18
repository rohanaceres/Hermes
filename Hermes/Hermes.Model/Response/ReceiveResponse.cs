using Newtonsoft.Json;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class ReceiveResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
