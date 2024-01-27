using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using ChimaeraArtSiteMVC.Models;
using ChimLib.Database;

namespace ChimaeraArtSiteMVC.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            ProductSeries[] prods;

            using (SqlConnection conn = DB.GetConnection())
                prods = conn.Query<ProductSeries>("MVCProductSeries",
                    commandType: System.Data.CommandType.StoredProcedure).ToArray();

            return View(prods);
        }

        public ActionResult Product(int id)
        {
            ProductSeries ps = new ProductSeries();
            using (SqlConnection conn = DB.GetConnection())
                ps = conn.Query<ProductSeries>("MVCProductSeries",
                    new { SeriesID = id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            ps.GetProducts();
            return View(ps);
        }
    }
}