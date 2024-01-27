using System;

namespace Chimaera.Beasts.Model
{
    public class Cart
    {
        public int CartID;
        public Guid CartKey;
        public DateTime DateCreated;
        public Discount DiscountApplied;
        public CartItem[] Items;
        public decimal? ShippingCharge;
    }
}
