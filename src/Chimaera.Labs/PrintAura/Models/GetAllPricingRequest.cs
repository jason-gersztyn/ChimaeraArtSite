using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class GetAllPricingRequest : Request
    {
        public GetAllPricingRequest() : base()
        {
            Method = "getallpricing";
        }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("brand_id")]
        public int BrandId { get; set; }

        [JsonProperty("color_id")]
        public int ColorId { get; set; }

        [JsonProperty("size_id")]
        public int SizeId { get; set; }

        [JsonProperty("front_print")]
        public bool FrontPrint { get; set; }

        [JsonProperty("back_print")]
        public bool BackPrint { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}