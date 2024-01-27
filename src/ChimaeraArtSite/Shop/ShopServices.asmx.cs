using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using ChimLib.Database;
using ChimLib.Utils;
using Dapper;

namespace ChimaeraArtSite.Shop
{
    /// <summary>
    /// Summary description for ShopServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ShopServices : System.Web.Services.WebService
    {
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public Variation FetchVariationData(int ID)
        {
            Variation v = new Variation();

            using(SqlConnection conn = DB.GetConnection())
            {
                v = conn.Query<Variation>("ProductVariationSelect", new { VariationID = ID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            if(v != null)
            {
                List<string> Sizes = ScalablePressUtils.GetProductSizes(v.Type, v.Color);
                v.Sizes = BasicUtils.GetSizeDictionary(Sizes);
            }

            return v;
        }

        public class Variation
        {
            public string Name = "";
            public string ProofURL = "";
            public string SizeChartURL = "";
            public string VariationID = "";
            public string Type = "";
            public string Color = "";
            public decimal UnitPrice = 0M;
            public string MaterialInfo = "";
            public Dictionary<string, string> Sizes = new Dictionary<string, string>();
        }
    }
}
