using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class Request
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        public Request()
        {
            Key = "7e5mkJGLCMN4BW421528AvqU0Yh8Pf1m";
            Hash = "pe4r3Tay9C2H00LUhGNNoclT5VT242T2BSjyPqsXk63pV3y000ofjoBFaJee79p9";
        }
    }
}