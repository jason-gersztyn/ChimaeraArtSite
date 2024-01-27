using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class UploadImageFromUrlResponse : Response
    {
        [JsonProperty("image_id")]
        public int ImageId { get; set; }
    }
}