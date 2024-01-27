using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;
using ChimLib.Utils;
using Dapper;

namespace ChimaeraArtSite.Shop
{
    public partial class ShopItem : System.Web.UI.Page
    {
        public int ProductID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.Params["SeriesID"] != null)
                designID.Value = Request.Params["SeriesID"].ToString();

            int tryID;
            if (int.TryParse(designID.Value, out tryID))
                ProductID = tryID;
            else
                ProductID = 0;

            if (!IsPostBack)
            {
                if (ProductID > 0)
                {
                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@SeriesID", ProductID));

                    DataTable dtShop = DB.Get("ShopSeriesProductsSelect", p.ToArray());

                    if (dtShop != null && dtShop.Rows.Count > 0)
                    {
                        VariationView.DataSource = dtShop;
                        VariationView.DataBind();

                        DataRow drShop = dtShop.Rows[0];
                        int VariationID = int.Parse(drShop["ProductVariationID"].ToString());
                        SetVariantInfo(VariationID);
                    }
                }
            }
        }

        protected void SetVariantInfo(int VariationID)
        {
            hdnVariation.Value = VariationID.ToString();

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ProductVariationID", VariationID));

            DataTable dtVariants = DB.Get("ShopItemDetailed", p.ToArray());

            if (dtVariants != null && dtVariants.Rows.Count > 0)
            {
                DataRow drVariant = dtVariants.Rows[0];

                lName.Text = drVariant["ImageName"].ToString() + " - " + drVariant["ColorDisplayName"].ToString();
                iDesign.ImageUrl = drVariant["ProofURL"].ToString();
                iSizeChart.ImageUrl = drVariant["SizeChartURL"].ToString();
                hlSizeChart.NavigateUrl = drVariant["SizeChartURL"].ToString();
                lPrice.Text = "$" + ((decimal)drVariant["UnitPrice"]).ToString("0.00");
                lDescription.Text = drVariant["ImageDescription"].ToString();

                blMaterial.Items.Clear();
                string materialInfo = drVariant["MaterialInfo"].ToString();
                if (!string.IsNullOrWhiteSpace(materialInfo))
                {
                    string[] materialBullets = materialInfo.Split(';');
                    foreach (string bullet in materialBullets)
                        blMaterial.Items.Add(new ListItem(bullet));
                }

                List<string> Sizes = ScalablePressUtils.GetProductSizes(drVariant["Type"].ToString(), drVariant["Color"].ToString());
                Dictionary<string, string> DicSize = BasicUtils.GetSizeDictionary(Sizes);

                ddlSize.DataSource = DicSize;
                ddlSize.DataTextField = "Key";
                ddlSize.DataValueField = "Value";
                ddlSize.DataBind();
            }
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            int VariationID = int.Parse(hdnVariation.Value);
            lMessage.Text = "";
            int qty = 1;// qb.GetQuantity();
            if (qty > 0)
            {
                Guid cartKey;
                if (Request.Cookies.Get("cartKey") == null)
                {
                    cartKey = Guid.NewGuid();

                    List<SqlParameter> gp = new List<SqlParameter>();
                    gp.Add(new SqlParameter("@GUID", cartKey));
                    DB.Set("CartCreate", gp.ToArray());

                    Response.Cookies["cartKey"].Value = cartKey.ToString();
                    Response.Cookies["cartKey"].Expires = DateTime.Now.AddDays(7);
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
                    cp.Add(new SqlParameter("@VariationID", VariationID));

                    DataTable dtCartItems = DB.Get("CartItemSelect", cp.ToArray());
                    if(dtCartItems == null || dtCartItems.Rows.Count <= 0)
                    {
                        string DesignID = ScalablePressUtils.GetDesignID(VariationID.ToString());
                        if (string.IsNullOrWhiteSpace(DesignID))
                        {
                            Exception ex = new Exception("Design ID not found for variation ID " + VariationID);
                            LoggingUtil.InsertError(ex);
                            lMessage.Text = "An error has occured with this product and our men in black have been notified.";
                        }
                        else
                        {
                            cp.Add(new SqlParameter("@DesignID", DesignID));
                        }
                    }

                    cp.Add(new SqlParameter("@ProductSize", ddlSize.SelectedValue));
                    cp.Add(new SqlParameter("@Quantity", qty));

                    try
                    {
                        int iAffected = DB.SetWithRowsAffected("CartItemInsert", cp.ToArray());

                        if (!(iAffected > 0))
                        {
                            lMessage.Text = "An unknown error has occured and our men in black have been notified.";
                        }
                        else
                        {
                            Response.Redirect("~/Shop/Cart.aspx", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingUtil.InsertError(ex);
                    }
                }
                else
                {
                    lMessage.Text = "An unknown error has occured and our men in black have been notified.";
                }
            }
            else
            {
                lMessage.Text = "Quantity orderable needs to be greater than 0.";
            }
        }

        public string CreateClick(string ID)
        {
            return "VariantSelect(" + ID + ")";
        }
    }
}