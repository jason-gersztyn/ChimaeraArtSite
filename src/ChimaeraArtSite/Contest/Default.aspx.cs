using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChimLib.Database;

namespace ChimaeraArtSite.Contest
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["ContestKey"] == null || string.IsNullOrWhiteSpace(Request.Cookies["ContestKey"].Value))
                return;

            string key = Request.Cookies["ContestKey"].Value;

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ContestKey", key));

            DataTable dtValid = DB.Get("ContestCodeGetValid", p.ToArray());

            if (dtValid != null && dtValid.Rows.Count > 0)
                Response.Redirect("~/Contest/Prize.aspx", false);
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            string key = tbContestKey.Text.Trim();
            lbResult.Text = "";
            if(!string.IsNullOrWhiteSpace(key))
            {
                Guid theGuid;
                if (Guid.TryParse(key, out theGuid))
                {
                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@ContestKey", theGuid));

                    DataTable dtCode = DB.Get("ContestCodeGetValid", p.ToArray());

                    if (dtCode != null && dtCode.Rows.Count > 0)
                    {
                        Response.Cookies["ContestKey"].Value = key;
                        Response.Cookies["ContestKey"].Expires = DateTime.Now.AddMonths(1);

                        Response.Redirect("~/Contest/Prize.aspx", false);
                    }
                    else
                    {
                        lbResult.Text = "Contest key not valid";
                    }
                }
                else
                {
                    lbResult.Text = "Contest key not valid";
                }
            }
            else
            {
                lbResult.Text = "No contest key detected";
            }
        }
    }
}