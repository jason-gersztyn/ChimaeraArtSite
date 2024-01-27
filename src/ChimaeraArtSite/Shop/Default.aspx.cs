using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChimaeraArtSite.Shop
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.Params["GenreID"] != null)
            {
                int tryInt = 0;

                if(int.TryParse(Request.Params["GenreID"].ToString(), out tryInt))
                    shopGrid.GenreID = tryInt;
            }
        }
    }
}