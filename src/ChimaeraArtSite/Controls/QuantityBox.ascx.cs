using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChimaeraArtSite.Controls
{
    public partial class QuantityBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                tbQuantity.Text = "1";
        }

        public int GetQuantity()
        {
            return int.Parse(tbQuantity.Text);
        }
    }
}