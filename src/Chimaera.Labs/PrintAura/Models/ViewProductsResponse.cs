using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ViewProductsResponse : Response
    {
        [JsonProperty("results")]
        public List<ViewProduct> Products { get; set; }
    }

    public class ViewProduct
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }
    }
}