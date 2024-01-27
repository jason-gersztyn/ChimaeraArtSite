using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class DiscountService
    {
        public static Discount GetDiscount(string Code)
        {
            Discount discount;

            using (SqlConnection conn = DB.GetConnection())
                discount = conn.Query<Discount>("GetDiscount", new { Code = Code }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return discount;
        }

        public static Discount CreateDiscount(Discount discount)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@Code", discount.Code));
            Params.Add(new SqlParameter("@DiscountTypeID", (int)discount.DiscountTypeID));
            Params.Add(new SqlParameter("@Amount", discount.Amount));
            Params.Add(new SqlParameter("@Usage", discount.Usage));
            Params.Add(new SqlParameter("@Limit", discount.Limit));
            Params.Add(new SqlParameter("@ExpirationDate", discount.ExpirationDate));

            DB.Set("PostDiscount", Params.ToArray());

            return GetDiscount(discount.Code);
        }

        public static Discount UpdateDiscount(Discount discount)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@DiscountID", discount.DiscountID));
            Params.Add(new SqlParameter("@Usage", discount.Usage));

            DB.Set("PutDiscount", Params.ToArray());

            return GetDiscount(discount.Code);
        }

        public static void DeleteDiscount(Discount discount)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@DiscountID", discount.DiscountID));

            DB.Set("DeleteDiscount", Params.ToArray());
        }

        public static decimal CalculateDiscount(Discount discount, decimal subtotal)
        {
            if (discount == null)
                return 0M;
            if (discount.DiscountTypeID == DiscountType.Flat)
                return discount.Amount;
            else if (discount.DiscountTypeID == DiscountType.Calculated)
                return (subtotal * (discount.Amount / 100));
            else
                return 0M;
        }
    }
}