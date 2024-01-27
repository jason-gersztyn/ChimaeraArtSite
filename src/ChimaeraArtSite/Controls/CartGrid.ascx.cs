using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;
using ChimLib.Database.Classes;
using ChimLib.Utils;

namespace ChimaeraArtSite.Controls
{
    public partial class CartGrid : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                BindGrid();
        }

        public void BindGrid()
        {
            try
            {
                cartSub.Style.Remove("text-decoration");
                discSub.Text = "";

                if (Request.Cookies.Get("cartKey") != null)
                {
                    Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);

                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@GUID", cartKey));

                    DataTable cartDetails = DB.Get("CartSummaryGet", p.ToArray());
                    CartView.DataSource = cartDetails;

                    if (cartDetails != null && cartDetails.Rows.Count > 0)
                    {
                        p = new List<SqlParameter>();
                        p.Add(new SqlParameter("@CacheID", cartKey));

                        decimal Subtotal = 0M;
                        DataTable dtSub = DB.Get("CartSubtotalGet", p.ToArray());
                        if (dtSub != null && dtSub.Rows.Count > 0)
                        {
                            string strSub = dtSub.Rows[0][0].ToString();
                            decimal tryDecimal = 0M;
                            if (decimal.TryParse(strSub, out tryDecimal))
                                Subtotal = tryDecimal;
                        }

                        cartSub.Text = "$" + Subtotal.ToString("0.00");

                        DataRow dr = cartDetails.Rows[0];
                        if (!string.IsNullOrEmpty(dr["DiscountID"].ToString()))
                        {
                            decimal CartTotal = DiscountUtils.CalculateDiscountTotal((int)dr["DiscountID"], cartKey);
                            cartSub.Style.Add("text-decoration", "line-through");
                            discSub.Text = "W/ Discount: $" + CartTotal.ToString("0.00");
                        }
                    }
                    else
                        cartSub.Text = "$0.00";
                }
                else
                {
                    CartView.DataSource = string.Empty;
                }
                CartView.DataBind();
                
            }
            catch(Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }
        }

        public List<SaleProduct> GetSalesCart()
        {
            List<SaleProduct> sp = new List<SaleProduct>();

            try
            {
                if (Request.Cookies.Get("cartKey") != null)
                {
                    Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);

                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@CacheID", cartKey));

                    DataTable dtCart = DB.Get("CartSalesProducts", p.ToArray());
                    foreach (DataRow dr in dtCart.Rows)
                        sp.Add(new SaleProduct() { Quantity = (int)dr["Quantity"], UnitPrice = (decimal)dr["UnitPrice"] });
                }
            }
            catch(Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }

            return sp;
        }

        protected void cartRemove_Click(object sender, EventArgs e)
        {
            var argument = ((LinkButton)sender).CommandArgument;
            if (!string.IsNullOrEmpty(argument.ToString()))
            {
                int CartItemID = int.Parse(argument);

                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CartItemID", CartItemID));

                DB.Set("CartItemRemove", p.ToArray());

                BindGrid();
            }
        }

        public bool IsCartEmpty()
        {
            bool empty = true;

            if (Request.Cookies.Get("cartKey") != null)
            {
                Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);
                DataTable dtCart = DB.GetWithQuery("SELECT TOP 1 1 FROM CartItems ci WITH(NOLOCK) JOIN Cart c WITH(NOLOCK) ON ci.CartID = c.CartID WHERE c.CacheID = '" + cartKey.ToString() + "'");
                empty = dtCart == null || dtCart.Rows.Count < 1;
            }

            return empty;
        }
    }
}