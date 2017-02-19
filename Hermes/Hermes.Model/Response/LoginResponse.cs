using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hermes.Model.Response
{
    // TODO: Doc.
    public sealed class LoginResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "data")]
        public PendingMessages Data { get; set; }
    }
}
