using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChimaeraArtSite.Controls
{
    public partial class DatePicker : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    ddlMonth.Items.Add(new ListItem(month.ToString("00") + " " + monthName, month.ToString().PadLeft(2, '0')));
                }

                int year = DateTime.Now.Year;
                int expireYear = year + 20;

                for (int i = year; i <= expireYear; i++)
                    ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        public DateTime GetDate()
        {
            int month = int.Parse(ddlMonth.SelectedValue);
            int year = int.Parse(ddlYear.SelectedValue);

            return new DateTime(year, month, 1);
        }
    }
}