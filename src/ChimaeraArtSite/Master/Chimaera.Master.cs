using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Utils;

namespace ChimaeraArtSite.Master
{
    public partial class Chimaera : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies.Get("cartKey") != null)
            {
                Guid cartKey = Guid.Parse(Request.Cookies["cartKey"].Value);
                bool exists = BasicUtils.IsCartAlive(cartKey);
                if (!exists)
                {
                    HttpCookie cartCookie = new HttpCookie("cartKey");
                    cartCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cartCookie);
                }
            }
        }
    }
}