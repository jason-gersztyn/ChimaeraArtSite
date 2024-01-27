using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class AddOrderRequest : Request
    {
        public AddOrderRequest() : base()
        {
            Method = "addorder";
        }

        [JsonProperty("businessname")]
        public string BusinessName { get; set; }

        [JsonProperty("businesscontact")]
        public string BusinessContact { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("your_order_id")]
        public string AltOrderId { get; set; }

        [JsonProperty("returnlabel")]
        public string ReturnLabel { get; set; }

        [JsonProperty("clientname")]
        public string ClientName { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("customerphone")]
        public string CustomerPhone { get; set; }

        [JsonProperty("shipping_id")]
        public string ShippingId { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("items")]
        public string Items { get; set; }
    }

    public class AddOrderItem
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("brand_id")]
        public int BrandId { get; set; }

        [JsonProperty("color_id")]
        public int ColorId { get; set; }

        [JsonProperty("size_id")]
        public int SizeId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("front_print")]
        public int FrontPrintId { get; set; }

        [JsonProperty("front_mockup")]
        public int FrontMockupId { get; set; }
    }
}