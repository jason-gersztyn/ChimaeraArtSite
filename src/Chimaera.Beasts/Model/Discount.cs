using System;

namespace Chimaera.Beasts.Model
{
    public class Discount
    {
        public int DiscountID { get; set; }
        public string Code { get; set; }
        public DiscountType DiscountTypeID { get; set; }
        public decimal Amount { get; set; }
        public int? Usage { get; set; }
        public int? Limit { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public void UpdateUsage()
        {
            if (!Usage.HasValue)
                Usage = 1;
            else
                Usage++;
        }
    }

    public enum DiscountType
    {
        Calculated = 1,
        Flat = 2
    }
}
