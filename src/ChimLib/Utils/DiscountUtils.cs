using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Database;
using ChimLib.Database.Classes;
using Dapper;

namespace ChimLib.Utils
{
    public static class DiscountUtils
    {
        public static bool ApplyDiscountToCart(int DiscountID, Guid CartKey)
        {
            bool added = false;

            try
            {
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CartKey", CartKey));
                p.Add(new SqlParameter("@DiscountID", DiscountID));

                int iAffected = DB.SetWithRowsAffected("DiscountCartApply", p.ToArray());
                added = iAffected > 0;
            }
            catch (Exception ex)
            {
                ex.Data.Add("DiscountID", DiscountID);
                ex.Data.Add("CartKey", CartKey);
                LoggingUtil.InsertError(ex);
            }

            return added;
        }

        public static decimal CalculateDiscountTotal(int DiscountID, Guid CartKey)
        {
            decimal Subtotal = 0M;
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@CacheID", CartKey));

            DataTable dtSub = DB.Get("CartSubtotalGet", p.ToArray());
            if (dtSub != null && dtSub.Rows.Count > 0)
            {
                string strSub = dtSub.Rows[0][0].ToString();
                decimal tryDecimal = 0M;
                if (decimal.TryParse(strSub, out tryDecimal))
                    Subtotal = tryDecimal;
            }

            Discount d = new Discount();
            using (SqlConnection conn = DB.GetConnection())
                d = conn.Query<Discount>("DiscountIDSelect", new { DiscountID = DiscountID }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            if(d.Type == DiscountType.Flat)
                Subtotal -= d.DiscountAmount;
            else if(d.Type == DiscountType.Calculated)
                Subtotal -= (Subtotal * (d.DiscountAmount / 100));

            return Subtotal;
        }

        public static decimal CalculateOrderDiscount(int DiscountID, DataTable dtSub)
        {
            decimal Subtotal = 0M;

            foreach (DataRow dr in dtSub.Rows)
            {
                decimal tryDec;
                int tryInt;

                decimal price = 0M, linetotal = 0M;
                int quantity = 0;

                if (int.TryParse(dr["Quantity"].ToString(), out tryInt))
                    quantity = tryInt;
                if (decimal.TryParse(dr["Price"].ToString(), out tryDec))
                    price = tryDec;

                linetotal = Math.Round(price * quantity, 2, MidpointRounding.AwayFromZero);

                Subtotal = Math.Round(Subtotal + linetotal, 2, MidpointRounding.AwayFromZero);
            }

            Discount d = new Discount();
            using (SqlConnection conn = DB.GetConnection())
                d = conn.Query<Discount>("DiscountIDSelect", new { DiscountID = DiscountID }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            if (d.Type == DiscountType.Flat)
                Subtotal -= d.DiscountAmount;
            else if (d.Type == DiscountType.Calculated)
                Subtotal -= (Subtotal * (d.DiscountAmount / 100));

            return Subtotal;
        }

        public static bool IsValidDiscount(string DiscountCode, out int DiscountID)
        {
            bool valid = false;
            DiscountID = 0;

            try
            {
                Discount d = new Discount();
                using (SqlConnection conn = DB.GetConnection())
                    d = conn.Query<Discount>("DiscountSelect", new { DiscountCode = DiscountCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if(d != null)
                {
                    if(d.DiscountUsage != null && d.DiscountLimit != null && d.DiscountUsage >= d.DiscountLimit)
                        return false;

                    if (d.ExpirationDate != null && d.ExpirationDate < DateTime.UtcNow)
                        return false;

                    DiscountID = d.DiscountID;
                    valid = true;
                }
            }
            catch(Exception ex)
            {
                ex.Data.Add("DiscountCode", DiscountCode);
                LoggingUtil.InsertError(ex);
            }

            return valid;
        }
    }
}
