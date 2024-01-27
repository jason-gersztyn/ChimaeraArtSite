using System;

namespace Chimaera.Beasts.Model
{
    public class Order
    {
        public int OrderID;
        public Quote Quote;
        public int PrintAuraID;
        public string PaypalSaleID;
        public Status Status;
        public DateTime DateCreated;
        public DateTime DateUpdated;
    }
}
