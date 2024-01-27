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

namespace ChimaeraArtSite.Contest
{
    public partial class Recipient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["ContestKey"] == null || string.IsNullOrWhiteSpace(Request.Cookies["ContestKey"].Value))
                    Response.Redirect("~/Contest/KeyInvalid.aspx", false);

                if (Request.Params["VariationID"] == null || Request.Params["Size"] == null)
                    Response.Redirect("~/Contest/Prize.aspx", false);

                string key = Request.Cookies["ContestKey"].Value;

                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@ContestKey", key));

                DataTable dtCode = DB.Get("ContestCodeGetValid", p.ToArray());

                if (dtCode != null && dtCode.Rows.Count > 0)
                {
                    DataTable dtStates = DB.GetWithQuery("SELECT StateCode, StateName FROM States");
                    foreach (DataRow drState in dtStates.Rows)
                        ddlStates.Items.Add(new ListItem(drState["StateName"].ToString(), drState["StateCode"].ToString()));
                    ddlStates.SelectedIndex = 0;

                    DataTable dtCountries = DB.GetWithQuery("SELECT CountryCode, CountryName FROM Countries");
                    foreach (DataRow drCountry in dtCountries.Rows)
                        ddlCountry.Items.Add(new ListItem(drCountry["CountryName"].ToString(), drCountry["CountryCode"].ToString()));
                    ddlCountry.SelectedIndex = 0;
                }
                else
                {
                    Response.Redirect("~/Contest/KeyInvalid.aspx", false);
                }
            }
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["ContestKey"] == null || string.IsNullOrWhiteSpace(Request.Cookies["ContestKey"].Value))
                Response.Redirect("~/Contest/KeyInvalid.aspx", false);

            string key = Request.Cookies["ContestKey"].Value;
              List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ContestKey", key));

            DataTable dtCode = DB.Get("ContestCodeGetValid", p.ToArray());

            if (dtCode != null && dtCode.Rows.Count > 0)
            {
                string CountryCode = ddlCountry.SelectedValue;
                string State = shipState.Text;

                if (CountryCode == "US")
                    State = ddlStates.SelectedValue;

                if (string.IsNullOrEmpty(State))
                {
                    lState.Text = "State is required";
                    return;
                }

                if (!BasicUtils.IsValidEmail(shipEmail.Text))
                {
                    lError.Text = "Email must be valid";
                    return;
                }

                int variationID = 0;
                if(!int.TryParse(Request.Params["VariationID"].ToString(), out variationID))
                    Response.Redirect("~/Contest/Prize.aspx", false);

                string OrderToken = ScalablePressUtils.ContestQuote(variationID, shipFName.Text + " " + shipLName.Text, shipAddress1.Text, shipAddress2.Text, shipCity.Text, State,
                                                                    ddlCountry.SelectedValue, shipZip.Text, Request.Params["Size"].ToString());

                if (!string.IsNullOrWhiteSpace(OrderToken))
                {
                    p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@ContestKey", key));
                    p.Add(new SqlParameter("@ProductVariationID", variationID));
                    p.Add(new SqlParameter("@ProductSize", Request.Params["Size"].ToString()));
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
                    p.Add(new SqlParameter("@OrderToken", OrderToken));

                    SqlParameter outOrderID = new SqlParameter("@OutOrderID", SqlDbType.Int);
                    outOrderID.Direction = ParameterDirection.Output;
                    p.Add(outOrderID);

                    int iRowsAffected = DB.SetWithRowsAffected("ContestOrderInsert", p.ToArray());
                    if (iRowsAffected > 0)
                    {
                        if (ScalablePressUtils.PlaceOrder(outOrderID.Value.ToString()))
                        {
                            Response.Cookies.Remove("ContestKey");
                            Response.Redirect("~/Shop/Confirm.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/Contest/KeyInvalid.aspx", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Contest/KeyInvalid.aspx", false);
                    }
                }
            }
            else
            {
                Response.Redirect("~/Contest/KeyInvalid.aspx", false);
            }
        }

        protected void shipZip_TextChanged(object sender, EventArgs e)
        {
            string zipcode = shipZip.Text;
            if (!string.IsNullOrWhiteSpace(zipcode))
            {
                Local l = BasicUtils.GetLocality(zipcode);

                if (!string.IsNullOrEmpty(l.countrycode))
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