using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ListOrdersResponse : Response
    {
        [JsonProperty("results")]
        public List<ListOrder> Orders { get; set; }
    }

    public class ListOrder
    {
        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("businessname")]
        public string BusinessName { get; set; }

        [JsonProperty("businesscontract")]
        public string BusinessContact { get; set; }

        [JsonProperty("youremail")]
        public string Email { get; set; }

        [JsonProperty("returnlabel")]
        public string ReturnLabel { get; set; }

        [JsonProperty("your_order_id")]
        public string AltOrderId { get; set; }

        [JsonProperty("shipping_id")]
        public int ShippingId { get; set; }

        [JsonProperty("shippingaddress")]
        public string ShippingAddress { get; set; }

        [JsonProperty("customerphone")]
        public string CustomerPhone { get; set; }

        [JsonProperty("packingslip")]
        public string PackingSlip { get; set; }

        [JsonProperty("tagremoval")]
        public Active TagRemoval { get; set; }

        [JsonProperty("tagapplication")]
        public Active TagApplication { get; set; }

        [JsonProperty("additionalmaterial")]
        public Active AdditionalMaterial { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("order_total")]
        public decimal OrderTotal { get; set; }

        [JsonProperty("shipping_price")]
        public decimal ShippingPrice { get; set; }

        [JsonProperty("shopify_id")]
        public int ShopifyId { get; set; }

        [JsonProperty("storenvy_id")]
        public int StorenvyId { get; set; }

        [JsonProperty("tagremovalprice")]
        public decimal TagRemovalPrice { get; set; }

        [JsonProperty("tagapplicationprice")]
        public decimal TagApplicationPrice { get; set; }

        [JsonProperty("additionalmaterialprice")]
        public decimal AdditionalMaterialPrice { get; set; }

        [JsonProperty("products_price")]
        public decimal ProductsPrice { get; set; }

        [JsonProperty("printing_price")]
        public decimal PrintingPrice { get; set; }

        [JsonProperty("items")]
        public List<ListOrderItem> Items { get; set; }
    }

    public class ListOrderItem
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

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
        public string FrontPrint { get; set; }

        [JsonProperty("front_mockup")]
        public string FrontMockup { get; set; }

        [JsonProperty("back_print")]
        public string BackPrint { get; set; }

        [JsonProperty("back_mockup")]
        public string BackMockup { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("shipping_id")]
        public int ShippingId { get; set; }

        [JsonProperty("tracking")]
        public string Tracking { get; set; }

        [JsonProperty("shipped_date")]
        public string ShippedDate { get; set; }

        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [JsonProperty("printing_price")]
        public decimal PrintingPrice { get; set; }
    }
}