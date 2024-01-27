using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;

namespace ChimaeraArtSite.Controls
{
    public partial class ImageGrid : System.Web.UI.UserControl
    {
        private string virtualPath;
        private string physicalPath;

        public string GridTitle { get; set; }

        public int GroupID { get; set; }

        public int SeriesID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PNUM", 20));
            ImageListView.DataSource = DB.Get("dbo.ImageItemsSelect", parms.ToArray());
            ImageListView.DataBind();
        }

        protected void titleLabel_Load(object sender, EventArgs e)
        {

        }
    }
}