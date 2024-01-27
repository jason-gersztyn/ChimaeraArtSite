using System.Collections.Generic;
using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Integration;

namespace Chimaera.Beasts.Extensions
{
    public static class QuoteExtensions
    {
        public static decimal CalculateShipping(this Quote quote)
        {
            decimal result = 0;
            List<ShippingRule> selectedRule = new List<ShippingRule>();

            if (quote.Address.Country == "US")
            {
                if (quote.Items.Length == 1 
                        && quote.Items[0].Quantity == 1 
                        && ShippingRules.First_Class.Any(x => x.genre == quote.Items[0].Product.Type.Genre))
                    return ShippingRules.First_Class.Where(x => x.genre == quote.Items[0].Product.Type.Genre).Select(y => y.first).First();
                else
                    selectedRule = ShippingRules.Priority;
            }
            else if(quote.Address.Country == "CA")
                selectedRule = ShippingRules.CA_First_Class;
            else
                selectedRule = ShippingRules.International_First_Class;

            QuoteItem firstItem = (from items in quote.Items
                                 join rules in selectedRule on items.Product.Type.Genre equals rules.genre
                                 orderby rules.first descending
                                 select items).First();

            result = selectedRule.Where(x => x.genre == firstItem.Product.Type.Genre).Select(y => y.first).First();
            
            foreach(QuoteItem item in quote.Items)
            {
                if (item == firstItem)
                {
                    if (item.Quantity == 1)
                        continue;
                    else
                        result += selectedRule.Where(x => x.genre == firstItem.Product.Type.Genre)
                                              .Select(y => y.additional * (item.Quantity - 1)).First();
                }
                else
                    result += selectedRule.Where(x => x.genre == firstItem.Product.Type.Genre)
                                          .Select(y => y.additional * item.Quantity).First();
            }

            return result;
        }

        public static int GetShippingId(this Quote quote)
        {
            List<ShippingRule> selectedRule = new List<ShippingRule>();

            if (quote.Address.Country == "US")
            {
                if (quote.Items.Length == 1
                        && quote.Items[0].Quantity == 1
                        && ShippingRules.First_Class.Any(x => x.genre == quote.Items[0].Product.Type.Genre))
                    return 4;
                else
                    return 1;
            }
            else if (quote.Address.Country == "CA")
                return 2;
            else
                return 3;
        }
    }
}