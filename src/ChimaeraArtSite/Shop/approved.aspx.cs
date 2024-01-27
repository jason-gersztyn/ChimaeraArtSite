using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;
using ChimLib.Utils;

namespace ChimaeraArtSite.Shop
{
    public partial class approved : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lDiscount.Visible = false;
            if (!string.IsNullOrWhiteSpace(Request.Params["paymentId"].ToString()) && !string.IsNullOrWhiteSpace(Request.Params["PayerID"].ToString()))
            {
                paymentid.Value = Request.Params["paymentId"].ToString();
                payerid.Value = Request.Params["PayerID"].ToString();
            }
            else
                Response.Redirect("~/Shop", false);

            if (Request.Cookies["cartKey"] != null)
            {
                Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@GUID", cartKey));

                DataTable cartDetails = DB.Get("CartSummaryGet", p.ToArray());
                CartView.DataSource = cartDetails;
                CartView.DataBind();

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

                DataRow dr = cartDetails.Rows[0];
                decimal Discount = 0M;
                if (!string.IsNullOrEmpty(dr["DiscountID"].ToString()))
                    Discount = DiscountUtils.CalculateDiscountTotal((int)dr["DiscountID"], cartKey);

                decimal Shipping = ScalablePressUtils.GetShippingQuote(cartKey);
                if (Shipping > 0M)
                {
                    lSubTotal.Text = "Subtotal: $" + Subtotal.ToString("0.00");
                    if (Discount > 0)
                    {
                        lDiscount.Text = "Discount: -$" + Discount.ToString("0.00");
                        lDiscount.Visible = true;
                    }
                    lShipTotal.Text = "S&H: $" + Shipping.ToString("0.00");
                    lTotal.Text = "Total: $" + Math.Round(Subtotal - Discount + Shipping, 2, MidpointRounding.AwayFromZero).ToString("0.00");
                }
            }
        }

        protected void completeSale_Click(object sender, EventArgs e)
        {
            try
            {
                string PID = PayPalUtils.ExecuteSale(paymentid.Value, payerid.Value);

                Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);

                decimal ShippingTotal = ScalablePressUtils.GetShippingQuote(cartKey);

                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CacheID", cartKey));
                p.Add(new SqlParameter("@ShippingPaid", ShippingTotal));
                p.Add(new SqlParameter("@PaymentID", PID));

                SqlParameter outOrderID = new SqlParameter("@OutOrderID", SqlDbType.Int);
                outOrderID.Direction = ParameterDirection.Output;
                p.Add(outOrderID);

            FORCEREPEAT:
                try
                {
                    DB.Set("CartOrderedInsert", p.ToArray());
                }
                catch
                {
                    goto FORCEREPEAT;
                }

                string OrderID = outOrderID.Value.ToString();
                HttpCookie cartCookie = new HttpCookie("cartKey");
                cartCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cartCookie);

                bool ordered = false;
                int retry = 0;
                int maxRetry = 3;

                while (!ordered)
                {
                    try
                    {
                        ordered = ScalablePressUtils.PlaceOrder(OrderID);
                    }
                    catch (Exception ex)
                    {
                        LoggingUtil.InsertError(ex);
                        if (retry > maxRetry)
                        {
                            return;
                        }
                        else
                            retry++;
                    }
                }

                BasicUtils.SendConfirmation(OrderID);
                Response.Redirect("~/Shop/Confirm.aspx", false);
            }
            catch (Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }
        }
    }
}