using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ListProductsResponse : Response
    {
        [JsonProperty("results")]
        public List<ListProduct> Products { get; set; }
    }

    public class ListProduct
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("brand_id")]
        public int BrandId { get; set; }

        [JsonProperty("colors")]
        public Dictionary<int, int[]> Colors { get; set; }
    }
}