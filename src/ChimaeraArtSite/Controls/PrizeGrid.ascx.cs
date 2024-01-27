using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;

namespace ChimaeraArtSite.Controls
{
    public partial class PrizeGrid : System.Web.UI.UserControl
    {
        public int GenreID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            if (GenreID > 0)
                parms.Add(new SqlParameter("@GenreID", GenreID));

            ShopListView.DataSource = DB.Get("dbo.ShopSeriesSelect", parms.ToArray());
            ShopListView.DataBind();
        }
    }
}