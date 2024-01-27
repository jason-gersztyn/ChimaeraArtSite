using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ListMyImagesResponse : Response
    {
        [JsonProperty("results")]
        public List<ListImage> Images { get; set; }
    }

    public class ListImage
    {
        [JsonProperty("image_id")]
        public int ImageId { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("file_size")]
        public int FileSize { get; set; }

        [JsonProperty("date_uploaded")]
        public DateTime DateUploaded { get; set; }
    }
}