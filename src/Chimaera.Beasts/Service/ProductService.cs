using System.Collections.Generic;
using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Chimaera.Beasts.Service
{
    public static class ProductService
    {
        public static IEnumerable<Product> GetProducts(int? ProductID = null, int? SeriesID = null)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Product, Design, Color, Model.Type, Product>("GetProducts",
                    (product, design, color, type) =>
                    {
                        type.Genre = GetGenre(type.TypeID);
                        product.Design = design;
                        product.Color = color;
                        product.Type = type;
                        product.Sizes = SizeService.GetProductSizes(product.ProductID).ToArray();
                        return product;
                    },
                    new { ProductID = ProductID, SeriesID = SeriesID },
                    splitOn: "DesignID, ColorID, TypeID",
                    commandType: CommandType.StoredProcedure);
        }

        public static Genre GetGenre(int TypeID)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Genre>("GetGenre",
                                    new
                                    {
                                        TypeID = TypeID
                                    },
                                    commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public static Product CreateProduct(Product product)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@DesignID", product.Design.DesignID));
            Params.Add(new SqlParameter("@TypeID", product.Type.TypeID));
            Params.Add(new SqlParameter("@ColorID", product.Color.ColorID));
            Params.Add(new SqlParameter("@ProofURL", product.ProofURL));
            Params.Add(new SqlParameter("@UnitPrice", product.UnitPrice));
            Params.Add(new SqlParameter("@Available", product.Available));

            SqlParameter OutProductID = new SqlParameter("@OutProductID", SqlDbType.Int);
            OutProductID.Direction = ParameterDirection.Output;
            Params.Add(OutProductID);

            DB.Set("PostProduct", Params.ToArray());

            return GetProducts((int?)OutProductID.Value).FirstOrDefault();
        }

        public static Product UpdateProduct(Product product)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@ProductID", product.ProductID));
            Params.Add(new SqlParameter("@DesignID", product.Design.DesignID));
            Params.Add(new SqlParameter("@TypeID", product.Type.TypeID));
            Params.Add(new SqlParameter("@ColorID", product.Color.ColorID));
            Params.Add(new SqlParameter("@ProofURL", product.ProofURL));
            Params.Add(new SqlParameter("@UnitPrice", product.UnitPrice));
            Params.Add(new SqlParameter("@Available", product.Available));

            DB.Set("PutProduct", Params.ToArray());

            return GetProducts(product.ProductID).FirstOrDefault();
        }

        public static void DeleteProduct(Product product)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@ProductID", product.ProductID));

            DB.Set("DeleteProduct", Params.ToArray());
        }
    }
}
