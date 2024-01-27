using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ChimLib.Database;

namespace ChimLib.Utils
{
    public static class BasicUtils
    {
        public static Dictionary<string, string> GetSizeDictionary(List<string> Sizes)
        {
            Dictionary<string, string> DicSize = new Dictionary<string, string>();

            foreach (string size in Sizes)
            {
                string key = size;

                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@ProductSize", size));

                DataTable dtSize = DB.Get("ProductSizeDisplayNameSelect", p.ToArray());
                if (dtSize != null && dtSize.Rows.Count > 0)
                    key = dtSize.Rows[0][0].ToString();

                DicSize.Add(key, size);
            }

            return DicSize;
        }

        public static bool IsCartAlive(Guid CacheKey)
        {
            bool exist = false;

            try
            {
                DataTable dtCart = DB.GetWithQuery("SELECT TOP 1 1 FROM Cart WITH(NOLOCK) WHERE CacheID = '" + CacheKey + "'");
                if (dtCart != null && dtCart.Rows.Count > 0)
                    exist = true;
            }
            catch(Exception ex)
            {
                ex.Data.Add("CacheKey", CacheKey);
                LoggingUtil.InsertError(ex);
            }

            return exist;
        }

        public static bool IsValidEmail(string strIn)
        {
            bool invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                bool invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static void SendConfirmation(string OrderID)
        {
            List<SqlParameter> op = new List<SqlParameter>();
            op.Add(new SqlParameter("@OrderID", OrderID));
            DataTable dtOrder = DB.Get("OrderSummarySelect", op.ToArray());
            DataTable dtCustomer = DB.GetWithQuery("SELECT * FROM Orders WITH(NOLOCK) WHERE OrderID = " + OrderID);

            if (dtOrder != null && dtOrder.Rows.Count > 0 && dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                DataRow drCustomer = dtCustomer.Rows[0];

                string address = "";
                address += "<b>" + drCustomer["ShipName"].ToString() + "</b><br />";
                address += drCustomer["ShipAddress1"].ToString() + "<br />";
                if (!string.IsNullOrWhiteSpace(drCustomer["ShipAddress2"].ToString()))
                    address += drCustomer["ShipAddress2"].ToString() + "<br />";
                address += drCustomer["ShipCity"].ToString() + ", " + drCustomer["ShipState"].ToString() + " " 
                    + drCustomer["ShipZip"].ToString() + " " + drCustomer["ShipCountryCode"].ToString();

                decimal subtotal = 0M;
                StringBuilder itemrows = new StringBuilder();
                foreach (DataRow dr in dtOrder.Rows)
                {
                    decimal tryDec;
                    int tryInt;

                    decimal price = 0M, linetotal = 0M;
                    int quantity = 0;

                    if (decimal.TryParse(dr["Price"].ToString(), out tryDec))
                        price = tryDec;
                    if (int.TryParse(dr["Quantity"].ToString(), out tryInt))
                        quantity = tryInt;

                    linetotal = Math.Round(price * quantity, 2, MidpointRounding.AwayFromZero);

                    itemrows.AppendLine("<tr>");
                    itemrows.AppendLine("<td style=\"padding-right:8px;\">" + dr["Title"].ToString() + "</td>");
                    itemrows.AppendLine("<td style=\"padding-right:8px;\">" + dr["Color"].ToString() + "</td>");
                    itemrows.AppendLine("<td style=\"padding-right:8px;\">" + dr["Size"].ToString() + "</td>");
                    itemrows.AppendLine("<td style=\"padding-right:8px;\">$" + price.ToString("0.00") + "</td>");
                    itemrows.AppendLine("<td style=\"padding-right:8px;\">" + quantity + "</td>");
                    itemrows.AppendLine("<td style=\"padding-right:8px;\">$" + linetotal.ToString("0.00") + "</td>");
                    itemrows.AppendLine("</tr>");

                    subtotal = Math.Round(subtotal + linetotal, 2, MidpointRounding.AwayFromZero);
                }

                decimal Discount = 0M;
                int DiscountID = 0;
                if(!string.IsNullOrEmpty(drCustomer["DiscountID"].ToString()) && int.TryParse(drCustomer["DiscountID"].ToString(), out DiscountID))
                    Discount = DiscountUtils.CalculateOrderDiscount(DiscountID, dtOrder);

                string body = Properties.Resources.OrderConfirmation;

                body = body.Replace("$$USER$$", drCustomer["ShipName"].ToString());
                body = body.Replace("$$ORDER$$", OrderID);
                body = body.Replace("$$ADDRESS$$", address);
                body = body.Replace("$$ITEMROWS$$", itemrows.ToString());
                body = body.Replace("$$SUBTOTAL$$", "$" + subtotal.ToString("0.00"));
                body = body.Replace("$$DISCOUNT$$", "$" + (subtotal - Discount).ToString("0.00"));
                decimal shipping = 0M;
                if(decimal.TryParse(drCustomer["ShippingPaid"].ToString(), out shipping))
                {
                    body = body.Replace("$$SHIPPING$$", "$" + shipping.ToString("0.00"));
                }
                body = body.Replace("$$TOTAL$$", "$" + Math.Round((subtotal - (subtotal - Discount)) + shipping, 2, MidpointRounding.AwayFromZero).ToString("0.00"));

                SendEmail(drCustomer["ShipEmail"].ToString(), "Your Order #" + OrderID, body);
            }
            else
            {
                return;
            }
        }

        public static void SendUpdate(string OrderID)
        {
            List<SqlParameter> op = new List<SqlParameter>();
            op.Add(new SqlParameter("@OrderID", OrderID));
            DataTable dtOrder = DB.Get("OrderShippingSelect", op.ToArray());

            if(dtOrder != null && dtOrder.Rows.Count > 0)
            {
                DataRow drName = dtOrder.Rows[0];
                StringBuilder sb = new StringBuilder();

                foreach(DataRow dr in dtOrder.Rows)
                {
                    DateTime dateShipped = DateTime.Parse(dr["ShippingDate"].ToString());

                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td style=\"padding-right:8px;\">" + dr["ShippingService"].ToString() + "</td>");
                    sb.AppendLine("<td style=\"padding-right:8px;\">" + dr["TrackingNumber"].ToString() + "</td>");
                    sb.AppendLine("<td style=\"padding-right:8px;\">" + System.Net.WebUtility.HtmlEncode(dateShipped.ToString("g")) + " UTC</td>");
                    sb.AppendLine("</tr>");
                }

                string body = Properties.Resources.StatusUpdate;

                body = body.Replace("$$USER$$", drName["ShipName"].ToString());
                body = body.Replace("$$ORDER$$", OrderID);
                body = body.Replace("$$ITEMROWS$$", sb.ToString());

                SendEmail(drName["ShipEmail"].ToString(), "Order #" + OrderID + " Shipped!", body);
            }
            else
            {
                return;
            }
        }

        public static void SendEmail(string msgTo, string msgTitle, string msgBody)
        {
            SmtpClient client = new SmtpClient("smtp.zoho.com", 587)
            {
                Credentials = new NetworkCredential("noreply@chimaeraconspiracy.com", "chuXU4pa"),
                EnableSsl = true
            };

            MailMessage msg = new MailMessage("noreply@chimaeraconspiracy.com", msgTo, msgTitle, msgBody);
            msg.IsBodyHtml = true;

            client.Send(msg);
        }

       public static Local GetLocality(string zipcode)
        {
            Local l = new Local();

            if (!string.IsNullOrEmpty(zipcode))
            {
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@Zipcode", zipcode));

                DataTable dtZip = DB.Get("ZipDataSelect", p.ToArray());
                if (dtZip != null && dtZip.Rows.Count > 0)
                {
                    DataRow drZip = dtZip.Rows[0];

                    l.state = drZip["State"].ToString();
                    l.countrycode = drZip["CountryCode"].ToString();
                }
                else
                {
                    string response = string.Empty;

                    string queryURL = "https://maps.googleapis.com/maps/api/geocode/xml?address=" + zipcode + "&sensor=true";
                    HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(queryURL);
                    hwr.Method = "POST";
                    hwr.ContentType = "application/xml";
                    hwr.ContentLength = 0;

                    using (HttpWebResponse hwresp = (HttpWebResponse)hwr.GetResponse())
                    {
                        Stream webStream = hwresp.GetResponseStream();
                        StreamReader sr = new StreamReader(webStream);
                        response = sr.ReadToEnd();
                        Thread.Sleep(500);
                        sr.Close();
                        webStream.Close();
                    }

                    if (!string.IsNullOrEmpty(response))
                    {
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(response);
                        XmlNode xnr = xd.DocumentElement.SelectSingleNode("result");
                        if (xnr != null)
                        {
                            XmlNodeList xnl = xnr.SelectNodes("address_component");

                            if (xnl != null)
                            {

                                foreach (XmlNode xn in xnl)
                                {
                                    XmlNodeList typeList = xn.SelectNodes("type");
                                    foreach (XmlNode type in typeList)
                                    {
                                        if (type.InnerText.ToLower() == "country")
                                        {
                                            l.countrycode = xn.SelectSingleNode("short_name").InnerText;
                                        }
                                        else if (type.InnerText.ToLower() == "administrative_area_level_1")
                                        {
                                            l.state = xn.SelectSingleNode("long_name").InnerText;
                                        }
                                    }
                                }

                                p.Add(new SqlParameter("@State", l.state));
                                p.Add(new SqlParameter("@CountryCode", l.countrycode));

                                DB.Set("ZipDataInsert", p.ToArray());
                            }
                        }
                    }
                }
            }

            return l;
        }
    }

    public class Local
    {
        public string state;
        public string countrycode;
    }
}
