using System;

namespace Chimaera.Beasts.Model
{
    public class Quote
    {
        public int QuoteID { get; set; }
        public Guid CartKey { get; set; }
        public Guid QuoteKey { get; set; }
        public DateTime DateCreated { get; set; }
        public Address Address { get; set; }
        public Discount Discount { get; set; }
        public QuoteItem[] Items { get; set; }
        public decimal? ShippingCharge { get; set; }
    }
}