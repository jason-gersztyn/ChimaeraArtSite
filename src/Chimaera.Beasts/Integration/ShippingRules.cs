using System.Collections.Generic;
using Chimaera.Beasts.Model;

namespace Chimaera.Beasts.Integration
{
    public static class ShippingRules
    {
        public static List<ShippingRule> First_Class = new List<ShippingRule>()
        {
            new ShippingRule(3.5M, 0M, ShippingType.First_Class, Genre.TShirt)
        };

        public static List<ShippingRule> Priority = new List<ShippingRule>()
        {
            new ShippingRule(5.5M, .75M, ShippingType.Priority, Genre.TShirt),
            new ShippingRule(8M, 1M, ShippingType.Priority, Genre.Sweatshirt)
        };

        public static List<ShippingRule> CA_First_Class = new List<ShippingRule>()
        {
            new ShippingRule(8M, 1M, ShippingType.CA_First_Class, Genre.TShirt),
            new ShippingRule(10M, 2M, ShippingType.CA_First_Class, Genre.Sweatshirt)
        };

        public static List<ShippingRule> International_First_Class = new List<ShippingRule>()
        {
            new ShippingRule(12M, 1M, ShippingType.International_First_Class, Genre.TShirt),
            new ShippingRule(15M, 2M, ShippingType.International_First_Class, Genre.Sweatshirt)
        };
    }

    public class ShippingRule
    {
        public decimal first;
        public decimal additional;
        public ShippingType type;
        public Genre genre;

        public ShippingRule(decimal _f, decimal _a, ShippingType _t, Genre _g)
        {
            first = _f;
            additional = _a;
            type = _t;
            genre = _g;
        }
    }
}