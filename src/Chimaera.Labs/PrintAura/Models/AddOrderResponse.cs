using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class AddOrderResponse : Response
    {
        [JsonProperty("results")]
        public AddedOrder Order { get; set; }
    }

    public class AddedOrder
    {
        [JsonProperty("order_id")]
        public int OrderId { get; set; }
    }
}