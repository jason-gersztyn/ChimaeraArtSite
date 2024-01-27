using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MoreLinq;

namespace Chimaera.Beasts.Service
{
    public static class SizeService
    {
        public static Size GetSize(int PrintAuraID)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Size>("GetSize", new { PrintAuraID = PrintAuraID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public static IEnumerable<Size> GetSizes(int? SizeID = null)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Size>("GetSizes", new { SizeID = SizeID }, commandType: CommandType.StoredProcedure).DistinctBy(x => x.SizeID);
        }

        public static IEnumerable<Size> GetProductSizes(int ProductID)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Size>("GetProductSizes", new { ProductID = ProductID }, commandType: CommandType.StoredProcedure).DistinctBy(x => x.SizeID);
        }

        public static void CreateSize(Size size)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@DisplayName", size.Name));
            Params.Add(new SqlParameter("@PrintAuraID", size.PrintAuraID));

            DB.Set("PostSize", Params.ToArray());
        }

        public static void CreateProductSize(int ProductID, int SizeID)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@ProductID", ProductID));
            Params.Add(new SqlParameter("@SizeID", SizeID));

            DB.Set("PostProductSizes", Params.ToArray());
        }

        public static Size UpdateSize(Size size)
        {
            throw new Exception("Unimplemented");
        }

        public static void DeleteSize(Size size)
        {
            throw new Exception("Unimplemented");
        }
    }
}