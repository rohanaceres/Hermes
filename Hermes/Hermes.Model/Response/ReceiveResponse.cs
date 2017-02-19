using Newtonsoft.Json;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class ReceiveResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "data")]
        public PendingMessages Data { get; set; }
    }
}
