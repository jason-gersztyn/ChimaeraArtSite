using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ViewProductsRequest : Request
    {
        public ViewProductsRequest() : base()
        {
            Method = "viewproducts";
        }

        [JsonProperty("paproduct_id")]
        public int PrintAuraProductId { get; set; }
    }
}