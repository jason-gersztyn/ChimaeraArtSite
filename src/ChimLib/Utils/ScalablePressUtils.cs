using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Database;
using Newtonsoft.Json;

namespace ChimLib.Utils
{
    #region Static methods
    public static class ScalablePressUtils
    {
        //TEST KEY = test_G8KWYQvgK8-m6LfEdzpUBQ
        //LIVE KEY = live_FPq6i-kNzL4HtC6QZyb3kw
        public static string APIKEY = "live_FPq6i-kNzL4HtC6QZyb3kw";

        public static List<string> GetProductSizes(string productType, string color)
        {
            List<string> ProductSizes = new List<string>();

            string URL = "https://api.scalablepress.com/v2/products/" + productType + "/availability";
            string Method = "GET";

            try
            {
                string result = ScalablePressAPICall(URL, Method, string.Empty);
                var sizesByColor = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(result);

                if (sizesByColor.ContainsKey(color))
                {
                    var colorSizes = sizesByColor[color];
                    foreach(var size in colorSizes)
                    {
                        if (size.Value > 0)
                            ProductSizes.Add(size.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }

            return ProductSizes;
        }

        public static string GetDesignID(string VariantID)
        {
            List<SqlParameter> vp = new List<SqlParameter>();
            vp.Add(new SqlParameter("@VariantID", VariantID));
            DataTable dtProduct = DB.Get("ProductDesignSelect", vp.ToArray());
            string designID = string.Empty;

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                DataRow drProduct = dtProduct.Rows[0];
                if (!string.IsNullOrWhiteSpace(drProduct["DesignID"].ToString()))
                    designID = drProduct["DesignID"].ToString();

                else
                {
                    string URL = "https://api.scalablepress.com/v2/design";
                    string Method = "POST";

                    /*var bodyCapsule = new { name = designName, type = "dtg", sides = new { front = new { artwork = artworkLoc, dimensions = new { width = 14 }, position = new { horizontal = "C", offset = new { top = 1.5 } } } } };

                    string Body = JsonConvert.SerializeObject(bodyCapsule).ToString();*/

                    string Body = "name=" + drProduct["Name"].ToString() + "&type=dtg&sides[front][artwork]=" + drProduct["Design"].ToString() 
                        //+ "&sides[front][proof]=" + drProduct["Proof"].ToString()
                        + "&sides[front][dimensions][width]=" + drProduct["Width"].ToString() + "&sides[front][position][horizontal]=C&sides[front][position][offset][top]=" + drProduct["InchesFromTop"].ToString();

                    try
                    {
                        string result = ScalablePressAPICall(URL, Method, Body);

                        Design designObject = JsonConvert.DeserializeObject<Design>(result);

                        designID = designObject.DesignID;

                        vp.Add(new SqlParameter("@DesignID", designID));
                        DB.Set("ProductDesignIDPut", vp.ToArray());
                    }
                    catch (Exception ex)
                    {
                        LoggingUtil.InsertError(ex);
                    }
                }
            }

            return designID;
        }

        public static decimal GetShippingQuote(Guid CacheID)
        {
            string BulkQuoteURL = "https://api.scalablepress.com/v2/quote/bulk";
            string Method = "POST";
            string Body = string.Empty;

            decimal ShipTotal = 0M;

            try
            {
                List<SqlParameter> p = new List<SqlParameter>();
                p.Add(new SqlParameter("@CacheID", CacheID));

                DataTable dtCartAddress = DB.Get("CartAddressSelect", p.ToArray());
                DataTable dtCart = DB.Get("CartDetailedSelect", p.ToArray());
                DataRow drAddress = dtCartAddress.Rows[0];

                int i = 0;
                foreach (DataRow drCart in dtCart.Rows)
                {
                    if (i > 0)
                        Body += "&";

                    string DesignID = drCart["DesignID"].ToString();

                    if(string.IsNullOrWhiteSpace(DesignID))
                        DesignID = GetDesignID(drCart["VariationID"].ToString());

                    Body += "items[" + i + "][type]=dtg";
                    Body += "&items[" + i + "][designId]=" + DesignID;

                    Body += "&items[" + i + "][address][name]=" + drAddress["ShipFirstName"].ToString() + " " + drAddress["ShipLastName"].ToString();
                    Body += "&items[" + i + "][address][address1]=" + drAddress["ShipAddress1"].ToString();
                    if (!string.IsNullOrWhiteSpace(drAddress["ShipAddress2"].ToString()))
                        Body += "&items[" + i + "][address][address2]=" + drAddress["ShipAddress2"].ToString();
                    Body += "&items[" + i + "][address][city]=" + drAddress["ShipCity"].ToString();
                    Body += "&items[" + i + "][address][state]=" + drAddress["ShipState"].ToString();
                    Body += "&items[" + i + "][address][country]=" + drAddress["ShipCountryCode"].ToString();
                    Body += "&items[" + i + "][address][zip]=" + drAddress["ShipZip"].ToString();

                    Body += "&items[" + i + "][products][0][id]=" + drCart["Type"].ToString();
                    Body += "&items[" + i + "][products][0][color]=" + drCart["Color"].ToString();
                    Body += "&items[" + i + "][products][0][quantity]=" + drCart["Quantity"].ToString();
                    Body += "&items[" + i + "][products][0][size]=" + drCart["Size"].ToString();

                    i++;
                }

                string result = ScalablePressAPICall(BulkQuoteURL, Method, Body);

                Quote quoteObject = JsonConvert.DeserializeObject<Quote>(result);

                string OrderToken = quoteObject.OrderToken;

                p.Add(new SqlParameter("@OrderToken", OrderToken));
                int RowsAffected = DB.SetWithRowsAffected("CartOrderTokenPost", p.ToArray());
                if (RowsAffected <= 0)
                    throw new Exception("Order token not saved");

                ShipTotal = quoteObject.Shipping;
            }
            catch (Exception ex)
            {
                ex.Data.Add("CacheID", CacheID);
                LoggingUtil.InsertError(ex);
            }

            return ShipTotal;
        }

        public static bool PlaceOrder(string OrderID)
        {
            bool completed = false;

            try
            {
                string OrderToken = "";

                List<SqlParameter> op = new List<SqlParameter>();
                op.Add(new SqlParameter("@OrderID", OrderID));

                DataTable dtToken = DB.Get("OrderTokenSelect", op.ToArray());
                if (dtToken != null && dtToken.Rows.Count > 0)
                {
                    OrderToken = dtToken.Rows[0][0].ToString();
                }
                else 
                    return completed;

                string URL = "https://api.scalablepress.com/v2/order";
                string Method = "POST";
                string Body = "orderToken=" + OrderToken;

                string result = ScalablePressAPICall(URL, Method, Body);

                Order orderObject = JsonConvert.DeserializeObject<Order>(result);

                if (orderObject.Events.Where(x => x.Name.Equals("order", StringComparison.OrdinalIgnoreCase)).Any())
                {
                    completed = true;

                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@OrderID", OrderID));
                    p.Add(new SqlParameter("@SPOrderID", orderObject.OrderID));

                    int RowsAffected = DB.SetWithRowsAffected("OrderIDSet", p.ToArray());
                }
                else
                    throw new Exception("Order " + OrderToken + " was not completed successfully");
            }
            catch (Exception ex)
            {
                ex.Data.Add("OrderID", OrderID);
                LoggingUtil.InsertError(ex);
            }

            return completed;
        }

        public static string ContestQuote(int VariantID, string name, string address1, string address2, string city, string state, string country, string zip, string size)
        {
            string DesignID = GetDesignID(VariantID.ToString());
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@VariationID", VariantID));

            DataTable dtVariant = DB.Get("ProductVariationSelect", p.ToArray());

            if (dtVariant != null && dtVariant.Rows.Count > 0)
            {
                DataRow drVariant = dtVariant.Rows[0];
                string QuoteURL = "https://api.scalablepress.com/v2/quote";
                string Method = "POST";
                string Body = string.Empty;

                Body += "type=dtg";
                Body += "&designId=" + DesignID;

                Body += "&address[name]=" + name;
                Body += "&address[address1]=" + address1;
                if (!string.IsNullOrWhiteSpace(address2))
                    Body += "&address[address2]=" + address2;
                Body += "&address[city]=" + city;
                Body += "&address[state]=" + state;
                Body += "&address[country]=" + country;
                Body += "&address[zip]=" + zip;

                Body += "&products[0][id]=" + drVariant["Type"].ToString();
                Body += "&products[0][color]=" + drVariant["Color"].ToString();
                Body += "&products[0][quantity]=" + 1;
                Body += "&products[0][size]=" + size;

                string result = ScalablePressAPICall(QuoteURL, Method, Body);

                Quote quoteObject = JsonConvert.DeserializeObject<Quote>(result);

                return quoteObject.OrderToken;
            }

            return "";
        }

        public static Status UpdateOrderStatus(string OrderID)
        {
            Status OrderStatus = Status.Error;

            try
            {
                Order orderObject = GetOrder(OrderID);

                Event eventObject = GetOrderShipping(orderObject);

                if (eventObject != null && eventObject.Shipping != null)
                {
                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@OrderID", OrderID));
                    p.Add(new SqlParameter("@ShippingDate", eventObject.CreatedAt));
                    p.Add(new SqlParameter("@Tracking", eventObject.Shipping.Tracking));
                    p.Add(new SqlParameter("@Service", eventObject.Shipping.Service));

                    int RowsAffected = DB.SetWithRowsAffected("OrderShippingInsert", p.ToArray());
                    if (RowsAffected <= 0)
                        OrderStatus = Status.Error;
                    else
                        OrderStatus = Status.Shipped;
                }
                else
                {
                    OrderStatus = GetOrderStatus(orderObject);

                    List<SqlParameter> p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@OrderID", OrderID));
                    p.Add(new SqlParameter("@StatusID", (int)OrderStatus));

                    int RowsAffected = DB.SetWithRowsAffected("OrderStatusUpdate", p.ToArray());
                }
            }
            catch(Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }

            if (OrderStatus == Status.Shipped)
                BasicUtils.SendUpdate(OrderID);

            return OrderStatus;
        }

        public static Status GetOrderStatus(Order orderObject)
        {
            Status OrderStatus = Status.Error;

            try
            {
                Event eventObject = orderObject.Events.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                if (eventObject != null)
                {
                    switch (eventObject.Name.ToUpper())
                    {
                        case "QUOTE":
                            OrderStatus = Status.Pending;
                            break;
                        case "ORDER":
                        case "PAID":
                        case "UNPAID":
                        case "HOLD":
                            OrderStatus = Status.Paid;
                            break;
                        case "PRINTING":
                            OrderStatus = Status.Printing;
                            break;
                        case "SHIPPED":
                            OrderStatus = Status.Shipped;
                            break;
                        case "CANCELLED":
                            OrderStatus = Status.Canceled;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }

            return OrderStatus;
        }

        public static Event GetOrderShipping(Order orderObject)
        {
            Event eventObject = null;

            try
            {
                eventObject = orderObject.Events.Where(x => x.Name.Equals("shipped", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }

            return eventObject;
        }

        public static Order GetOrder(string OrderID)
        {
            Order orderObject = null;

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@OrderID", OrderID));

            DataTable dtOrder = DB.Get("OrderSPIDSelect", p.ToArray());

            if (dtOrder != null && dtOrder.Rows.Count > 0)
            {
                DataRow drOrder = dtOrder.Rows[0];
                string SPOrderID = drOrder["SPOrderID"].ToString();

                if (string.IsNullOrWhiteSpace(SPOrderID))
                {
                    throw new Exception("SPOrderID found to be blank or missing");
                }
                else
                {
                    string URL = "https://api.scalablepress.com/v2/order/" + SPOrderID;
                    string Method = "GET";

                    string result = ScalablePressAPICall(URL, Method, string.Empty);

                    orderObject = JsonConvert.DeserializeObject<Order>(result);
                }
            }
            
            if (orderObject == null)
                throw new Exception("Order object was not found");

            return orderObject;
        }

        public static string ScalablePressAPICall(string URL, string Method, string Body)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = Method;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "*/*";
                CredentialCache mycache = new CredentialCache();
                mycache.Add(new Uri(URL), "Basic", new NetworkCredential("", APIKEY));
                request.Credentials = mycache;
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(":" + APIKEY)));

                if (!string.IsNullOrWhiteSpace(Body))
                {
                    byte[] myShinyMetalAss = System.Text.ASCIIEncoding.Default.GetBytes(Body);
                    request.ContentLength = myShinyMetalAss.Length;

                    using (Stream postStream = request.GetRequestStream())
                        postStream.Write(myShinyMetalAss, 0, myShinyMetalAss.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);

                string result = reader.ReadToEnd();

                reader.Close();
                responseStream.Close();
                return result;
            }
            catch(WebException ex)
            {
                LoggingUtil.InsertError(ex);

                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                string body = sr.ReadToEnd();
                sr.Close();

                throw new Exception(body);
            }
        }
    }
    #endregion

    #region JSON contracts
    [DataContract]
    public class Design
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "designId")]
        public string DesignID { get; set; }

        [DataMember(Name = "mode")]
        public string Mode { get; set; }
    }

    [DataContract]
    public class Quote
    {
        [DataMember(Name = "total")]
        public decimal Total { get; set; }

        [DataMember(Name = "shipping")]
        public decimal Shipping { get; set; }

        [DataMember(Name = "subtotal")]
        public decimal Subtotal { get; set; }

        [DataMember(Name = "fees")]
        public decimal Fees { get; set; }

        [DataMember(Name = "orderToken")]
        public string OrderToken { get; set; }

        [DataMember(Name = "mode")]
        public string Mode { get; set; }
    }

    [DataContract]
    public class Order
    {
        [DataMember(Name = "orderToken")]
        public string OrderToken { get; set; }

        [DataMember(Name = "events")]
        public Event[] Events { get; set; }

        [DataMember(Name = "orderId")]
        public string OrderID { get; set; }

        [DataMember(Name = "mode")]
        public string Mode { get; set; }
    }

    [DataContract]
    public class Event
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "meta")]
        public Shipping Shipping { get; set; }
    }

    [DataContract]
    public class Shipping
    {
        [DataMember(Name = "item")]
        public int Item { get; set; }

        [DataMember(Name = "itemName")]
        public string ItemNme { get; set; }

        [DataMember(Name = "tracking")]
        public string Tracking { get; set; }

        [DataMember(Name = "service")]
        public string Service { get; set; }
    }
    #endregion
}
