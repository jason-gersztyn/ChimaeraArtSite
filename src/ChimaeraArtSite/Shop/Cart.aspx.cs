using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Utils;

namespace ChimaeraArtSite.Shop
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (cgCurrent.IsCartEmpty())
                btCheckout.Visible = false;
            else
                btCheckout.Visible = true;
        }

        protected void btCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Shop/Checkout.aspx");
        }

        protected void btDiscount_Click(object sender, EventArgs e)
        {
            lDiscount.Text = "";
            if (Request.Cookies.Get("cartKey") != null)
            {
                Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);

                string discountcode = tbCode.Text;
                int DiscountID = 0;
                if (!string.IsNullOrWhiteSpace(discountcode))
                {
                    if (DiscountUtils.IsValidDiscount(discountcode, out DiscountID))
                    {
                        if (DiscountUtils.ApplyDiscountToCart(DiscountID, cartKey))
                        {
                            lDiscount.Text = "Discount code applied!";
                            cgCurrent.BindGrid();
                        }
                        else
                            lDiscount.Text = "Unknown error has occured";
                    }
                    else
                    {
                        lDiscount.Text = "Discount code is not valid";
                    }
                }
                else
                {
                    lDiscount.Text = "Discount code is empty";
                }
            }
            else
            {
                lDiscount.Text = "Cart cannot be empty";
            }            
        }
    }
}