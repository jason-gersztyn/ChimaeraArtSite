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
    public partial class PrizeItem : System.Web.UI.Page
    {
        public int ProductID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["ContestKey"] == null || string.IsNullOrWhiteSpace(Request.Cookies["ContestKey"].Value))
                Response.Redirect("~/Contest/KeyInvalid.aspx", false);

            string key = Request.Cookies["ContestKey"].Value;

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ContestKey", key));

            DataTable dtCode = DB.Get("ContestCodeGetValid", p.ToArray());

            if (dtCode != null && dtCode.Rows.Count > 0)
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
                        p = new List<SqlParameter>();
                        p.Add(new SqlParameter("@SeriesID", ProductID));
                        if (!string.IsNullOrWhiteSpace(dtCode.Rows[0]["GenreID"].ToString()))
                            p.Add(new SqlParameter("@GenreID", dtCode.Rows[0]["GenreID"].ToString()));

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
            else
            {
                Response.Redirect("~/Contest/KeyInvalid.aspx", false);
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
            string size = ddlSize.SelectedValue;

            string postbackURL = "~/Contest/Recipient.aspx?VariationID=" + VariationID + "&Size=" + size;
            Response.Redirect(postbackURL, false);
        }

        public string CreateClick(string ID)
        {
            return "VariantSelect(" + ID + ")";
        }
    }
}