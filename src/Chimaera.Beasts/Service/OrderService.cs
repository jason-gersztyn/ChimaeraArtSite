using System.Collections.Generic;
using System.Linq;
using Dapper;
using Chimaera.Beasts.Model;
using System.Data.SqlClient;
using Chimaera.Beasts.Utils;
using System.Data;
using System;

namespace Chimaera.Beasts.Service
{
    public static class OrderService
    {
        public static IEnumerable<Order> GetOrders(int? OrderID = null, Guid? QuoteKey = null, int? StatusID = null)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Order, Quote, Order>("GetOrder",
                                                    (order, quote) =>
                                                    {
                                                        quote = QuoteService.GetQuote(quote.QuoteKey);
                                                        order.Quote = quote;
                                                        return order;
                                                    },
                                                    new { OrderID = OrderID, QuoteKey = QuoteKey, StatusID = StatusID },
                                                    splitOn: "QuoteID",
                                                    commandType: CommandType.StoredProcedure);
        }

        public static Order CreateOrder(Order order)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@QuoteID", order.Quote.QuoteID));
            Params.Add(new SqlParameter("@PaypalID", order.PaypalSaleID));
            Params.Add(new SqlParameter("@StatusID", (int)order.Status));

            DB.Set("PostOrder", Params.ToArray());

            return GetOrders(QuoteKey: order.Quote.QuoteKey).FirstOrDefault();
        }

        public static Order UpdateOrder(Order order)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@OrderID", order.OrderID));
            Params.Add(new SqlParameter("@PrintAuraID", order.PrintAuraID));
            Params.Add(new SqlParameter("@StatusID", (int)order.Status));

            DB.Set("PutOrder", Params.ToArray());

            return GetOrders(OrderID: order.OrderID).FirstOrDefault();
        }

        public static void DeleteOrder(Order order)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@OrderID", order.OrderID));

            DB.Set("DeleteOrder", Params.ToArray());
        }
    }
}