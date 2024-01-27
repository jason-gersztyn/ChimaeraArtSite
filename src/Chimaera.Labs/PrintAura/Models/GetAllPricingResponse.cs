using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class GetAllPricingResponse : Response
    {
        [JsonProperty("results")]
        public List<GetAllPricingResult> PricingResults { get; set; }
    }

    public class GetAllPricingResult
    {
        [JsonProperty("shipping_id")]
        public int ShippingId { get; set; }

        [JsonProperty("base_price")]
        public decimal BasePrice { get; set; }

        [JsonProperty("printing_price")]
        public decimal PrintingPrice { get; set; }

        [JsonProperty("tagremoval")]
        public decimal TagRemoval { get; set; }

        [JsonProperty("tagapplication")]
        public decimal TagApplication { get; set; }

        [JsonProperty("additionalmaterial")]
        public decimal AdditionalMaterial { get; set; }

        [JsonProperty("shipping_cost")]
        public decimal ShippingCost { get; set; }

        [JsonProperty("total_charge")]
        public decimal TotalCharge { get; set; }
    }
}