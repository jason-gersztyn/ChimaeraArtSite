using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChimLib.Database;
using ChimLib.Utils;

namespace ChimaeraArtSiteMVC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(string ID, string Size)
        {
            HttpStatusCodeResult result = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            Guid cartKey;
            if (Request.Cookies.Get("cartKey") == null)
            {
                cartKey = Guid.NewGuid();

                List<SqlParameter> gp = new List<SqlParameter>();
                gp.Add(new SqlParameter("@GUID", cartKey));
                DB.Set("CartCreate", gp.ToArray());

                Response.Cookies["cartKey"].Value = cartKey.ToString();
                Response.Cookies["cartKey"].Expires = DateTime.Now.AddMonths(1);
            }
            else
            {
                cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);
            }

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@GUID", cartKey));

            DataTable dtCart = DB.Get("CartIDSelect", p.ToArray());
            if (dtCart != null && dtCart.Rows.Count > 0)
            {
                int CartID = (int)dtCart.Rows[0][0];

                List<SqlParameter> cp = new List<SqlParameter>();
                cp.Add(new SqlParameter("@CartID", CartID));
                cp.Add(new SqlParameter("@VariationID", ID));

                DataTable dtCartItems = DB.Get("CartItemSelect", cp.ToArray());
                if (dtCartItems == null || dtCartItems.Rows.Count <= 0)
                {
                    string DesignID = ScalablePressUtils.GetDesignID(ID);
                    if (string.IsNullOrWhiteSpace(DesignID))
                    {
                        Exception ex = new Exception("Design ID not found for variation ID " + ID);
                        LoggingUtil.InsertError(ex);
                    }
                    else
                    {
                        cp.Add(new SqlParameter("@DesignID", DesignID));
                    }
                }

                cp.Add(new SqlParameter("@ProductSize", Size));
                cp.Add(new SqlParameter("@Quantity", 1));

                try
                {
                    int iAffected = DB.SetWithRowsAffected("CartItemInsert", cp.ToArray());

                    if (iAffected > 0)
                        result = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);

                }
                catch (Exception ex)
                {
                    LoggingUtil.InsertError(ex);
                }
            }

            return result;
        }

        public ActionResult Remove(string ID)
        {
            try
            {
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CartItemID", ID));

                DB.Set("CartItemRemove", p.ToArray());
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                LoggingUtil.InsertError(ex);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}