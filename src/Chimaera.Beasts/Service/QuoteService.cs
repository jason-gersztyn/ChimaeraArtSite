using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class QuoteService
    {
        public static Quote GetQuote(Guid QuoteKey)
        {
            Quote quoteObject;

            using (SqlConnection conn = DB.GetConnection())
                quoteObject = conn.Query<Quote, Address, Discount, Quote>("GetQuote",
                    (quote, address, discount) =>
                    {
                        quote.Address = address;
                        quote.Discount = discount;
                        return quote;
                    },
                    new { QuoteKey = QuoteKey },
                    splitOn: "AddressID, DiscountID",
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            quoteObject.Items = GetQuoteItems(quoteObject).ToArray();

            return quoteObject;
        }

        public static Quote CreateQuote(Cart cart)
        {
            Guid quoteKey = Guid.NewGuid();

            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartKey", cart.CartKey));
            Params.Add(new SqlParameter("@QuoteKey", quoteKey));
            if(cart.DiscountApplied != null)
                Params.Add(new SqlParameter("@DiscountID", cart.DiscountApplied.DiscountID));
            Params.Add(new SqlParameter("@ShippingCharge", cart.ShippingCharge));

            DB.Set("PostQuote", Params.ToArray());

            Quote quoteObject = GetQuote(quoteKey);

            foreach (CartItem item in cart.Items)
                CreateQuoteItem(quoteObject, item);

            quoteObject.Items = GetQuoteItems(quoteObject).ToArray();

            return quoteObject;
        }

        public static void UpdateQuote(Quote quote)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@QuoteID", quote.QuoteID));
            Params.Add(new SqlParameter("@ShippingCharge", quote.ShippingCharge));

            DB.Set("PutQuote", Params.ToArray());
        }

        public static void DeleteQuote(Guid QuoteKey)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@QuoteKey", QuoteKey));
            DB.Set("DeleteQuote", Params.ToArray());
        }

        public static IEnumerable<QuoteItem> GetQuoteItems(Quote quote)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<QuoteItem, Product, Size, QuoteItem>("GetQuoteItems",
                    (item, product, size) =>
                    {
                        product = ProductService.GetProducts(product.ProductID).FirstOrDefault();
                        item.Product = product;
                        item.Size = size;
                        return item;
                    },
                    new { QuoteID = quote.QuoteID },
                    splitOn: "ProductID, SizeID",
                    commandType: CommandType.StoredProcedure);
        }

        public static void CreateQuoteItem(Quote quote, CartItem item)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@QuoteID", quote.QuoteID));
            Params.Add(new SqlParameter("@ProductID", item.Product.ProductID));
            Params.Add(new SqlParameter("@SizeID", item.Size.SizeID));
            Params.Add(new SqlParameter("@Quantity", item.Quantity));
            DB.Set("PostQuoteItem", Params.ToArray());
        }

        public static decimal CalculateSubtotal(Quote quote)
        {
            decimal subtotal = 0M;

            foreach (QuoteItem item in quote.Items)
                subtotal += (item.Product.UnitPrice * item.Quantity);

            return subtotal;
        }
    }
}