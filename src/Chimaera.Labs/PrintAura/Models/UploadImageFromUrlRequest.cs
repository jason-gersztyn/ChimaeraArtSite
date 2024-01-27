using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class UploadImageFromUrlRequest : Request
    {
        public UploadImageFromUrlRequest() : base()
        {
            Method = "uploadimagefromurl";
        }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}