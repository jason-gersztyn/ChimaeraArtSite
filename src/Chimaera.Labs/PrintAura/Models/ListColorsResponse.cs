using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ListColorsResponse : Response
    {
        [JsonProperty("results")]
        public List<ListColor> Colors { get; set; }
    }

    public class ListColor
    {
        [JsonProperty("color_id")]
        public int ColorId { get; set; }

        [JsonProperty("color_name")]
        public string ColorName { get; set; }

        [JsonProperty("color_code")]
        public string ColorCode { get; set; }

        [JsonProperty("color_group")]
        public string ColorGroup { get; set; }
    }
}