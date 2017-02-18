using Newtonsoft.Json;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class LoginResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
