using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;
using ChimLib.Utils;

namespace ChimaeraArtSite.Shop
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DataTable dtStates = DB.GetWithQuery("SELECT StateCode, StateName FROM States");
                foreach (DataRow drState in dtStates.Rows)
                    ddlStates.Items.Add(new ListItem(drState["StateName"].ToString(), drState["StateCode"].ToString()));
                ddlStates.SelectedIndex = 0;

                DataTable dtCountries = DB.GetWithQuery("SELECT CountryCode, CountryName FROM Countries");
                foreach (DataRow drCountry in dtCountries.Rows)
                    ddlCountry.Items.Add(new ListItem(drCountry["CountryName"].ToString(), drCountry["CountryCode"].ToString()));
                ddlCountry.SelectedIndex = 0;

                Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);

                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CacheID", cartKey));
                DataTable dtAddress = DB.Get("CartAddressSelect", p.ToArray());
                if (dtAddress != null && dtAddress.Rows.Count > 0)
                {
                    DataRow drAddress = dtAddress.Rows[0];
                    shipFName.Text = drAddress["ShipFirstName"].ToString();
                    shipLName.Text = drAddress["ShipLastName"].ToString();
                    shipAddress1.Text = drAddress["ShipAddress1"].ToString();
                    shipAddress2.Text = drAddress["ShipAddress2"].ToString();
                    shipCity.Text = drAddress["ShipCity"].ToString();
                    ddlCountry.SelectedValue = drAddress["ShipCountryCode"].ToString();

                    if (ddlCountry.SelectedValue == "US")
                    {
                        ddlStates.SelectedValue = drAddress["ShipState"].ToString();
                        ddlStates.Visible = true;
                        shipState.Visible = false;
                    }
                    else
                    {
                        shipState.Text = drAddress["ShipState"].ToString();
                        shipState.Visible = true;
                        ddlStates.Visible = false;
                    }

                    shipZip.Text = drAddress["ShipZip"].ToString();
                    shipEmail.Text = drAddress["ShipEmail"].ToString();
                    shipPhone.Text = drAddress["ShipPhone"].ToString();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (checkoutCart.IsCartEmpty())
                Response.Redirect("~/Shop", false);
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            lError.Text = "";

            string CountryCode = ddlCountry.SelectedValue;
            string State = shipState.Text;

            if (CountryCode == "US")
                State = ddlStates.SelectedValue;

            if(string.IsNullOrEmpty(State))
            {
                lState.Text = "State is required";
                return;
            }

            if(!BasicUtils.IsValidEmail(shipEmail.Text))
            {
                lError.Text = "Email must be valid";
                return;
            }

            Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@CacheID", cartKey));
            p.Add(new SqlParameter("@ShipFirstName", shipFName.Text));
            p.Add(new SqlParameter("@ShipLastName", shipLName.Text));
            p.Add(new SqlParameter("@ShipAddress1", shipAddress1.Text));
            p.Add(new SqlParameter("@ShipAddress2", shipAddress2.Text));
            p.Add(new SqlParameter("@ShipCity", shipCity.Text));
            p.Add(new SqlParameter("@ShipState", State));
            p.Add(new SqlParameter("@ShipCountryCode", ddlCountry.SelectedValue));
            p.Add(new SqlParameter("@ShipZip", shipZip.Text));
            p.Add(new SqlParameter("@ShipEmail", shipEmail.Text));
            p.Add(new SqlParameter("@ShipPhone", shipPhone.Text));
            DB.Set("CartAddressUpdate", p.ToArray());

            decimal Shipping = ScalablePressUtils.GetShippingQuote(cartKey);

            string redirect_url = PayPalUtils.ConfirmSale(cartKey, Shipping);
            Response.Redirect(redirect_url, false);
        }

        protected void shipZip_TextChanged(object sender, EventArgs e)
        {
            string zipcode = shipZip.Text;
            if(!string.IsNullOrWhiteSpace(zipcode))
            {
                Local l = BasicUtils.GetLocality(zipcode);

                if(!string.IsNullOrEmpty(l.countrycode))
                    ddlCountry.SelectedValue = l.countrycode;

                switchStateBoxes();

                if (!string.IsNullOrEmpty(l.state))
                {
                    if (ddlCountry.SelectedValue == "US")
                    {
                        ListItem shipList = ddlStates.Items.FindByText(l.state);
                        if (shipList != null)
                        {
                            ddlStates.SelectedItem.Selected = false;
                            shipList.Selected = true;
                        }
                    }
                    else
                        shipState.Text = l.state;
                    shipEmail.Focus();
                }
                else
                {
                    if (ddlCountry.SelectedValue == "US")
                        ddlStates.Focus();
                    else
                        shipState.Focus();
                }

            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            switchStateBoxes();
        }

        protected void switchStateBoxes()
        {
            ddlStates.Visible = false;
            shipState.Visible = false;

            if (ddlCountry.SelectedValue == "US")
                ddlStates.Visible = true;
            else
                shipState.Visible = true;
        }
    }
}