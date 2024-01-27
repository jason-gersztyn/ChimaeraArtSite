using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class Response
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}