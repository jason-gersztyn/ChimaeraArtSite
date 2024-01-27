using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ListSizesResponse : Response
    {
        [JsonProperty("results")]
        public List<ListSize> Sizes { get; set; }
    }

    public class ListSize
    {
        [JsonProperty("size_id")]
        public int SizeId { get; set; }

        [JsonProperty("size_name")]
        public string SizeName { get; set; }

        [JsonProperty("size_group")]
        public string SizeGroup { get; set; }

        [JsonProperty("plus_size_charge")]
        public decimal PlusSizeCharge { get; set; }
    }
}