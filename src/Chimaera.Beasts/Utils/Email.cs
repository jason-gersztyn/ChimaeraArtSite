using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Chimaera.Beasts.Utils
{
    public static class Email
    {
        public static void SendConfirmation(Order order)
        {
            Address orderAddress = order.Quote.Address;
            string address = "";
            address += "<b>" + orderAddress.Name + "</b><br />";
            address += orderAddress.Street1 + "<br />";
            if (!string.IsNullOrWhiteSpace(orderAddress.Street2))
                address += orderAddress.Street2 + "<br />";
            address += orderAddress.City + ", " + orderAddress.State + " "
                + orderAddress.Zip + "<br />" + orderAddress.Country;

            order.Quote.Items = QuoteService.GetQuoteItems(order.Quote).ToArray();

            decimal subtotal = 0M;
            StringBuilder itemrows = new StringBuilder();
            foreach (QuoteItem item in order.Quote.Items)
            {
                decimal linetotal = 0M;

                linetotal = Math.Round(item.Product.UnitPrice * item.Quantity, 2, MidpointRounding.AwayFromZero);

                itemrows.AppendLine("<tr>");
                itemrows.AppendLine("<td style=\"padding-right:8px;\">" + item.Product.Design.Name + "</td>");
                itemrows.AppendLine("<td style=\"padding-right:8px;\">" + item.Product.Color.Name + "</td>");
                itemrows.AppendLine("<td style=\"padding-right:8px;\">" + item.Size.Name + "</td>");
                itemrows.AppendLine("<td style=\"padding-right:8px;\">$" + item.Product.UnitPrice.ToString("0.00") + "</td>");
                itemrows.AppendLine("<td style=\"padding-right:8px;\">" + item.Quantity + "</td>");
                itemrows.AppendLine("<td style=\"padding-right:8px;\">$" + linetotal.ToString("0.00") + "</td>");
                itemrows.AppendLine("</tr>");

                subtotal = Math.Round(subtotal + linetotal, 2, MidpointRounding.AwayFromZero);
            }

            decimal discount = DiscountService.CalculateDiscount(order.Quote.Discount, subtotal);

            string body = Properties.Resources.OrderConfirmation;

            decimal grandtotal = Math.Round(subtotal - discount + order.Quote.ShippingCharge.Value, 2, MidpointRounding.AwayFromZero);

            body = body.Replace("$$USER$$", orderAddress.Name);
            body = body.Replace("$$ORDER$$", order.OrderID.ToString());
            body = body.Replace("$$ADDRESS$$", address);
            body = body.Replace("$$ITEMROWS$$", itemrows.ToString());
            body = body.Replace("$$SUBTOTAL$$", "$" + subtotal.ToString("0.00"));
            if (discount != 0M)
                body = body.Replace("$$DISCOUNT$$", "$" + discount.ToString("0.00"));
            else
                body = body.Replace("$$DISCOUNT$$", string.Empty);
            body = body.Replace("$$SHIPPING$$", "$" + order.Quote.ShippingCharge.Value.ToString("0.00"));
            body = body.Replace("$$TOTAL$$", "$" + grandtotal.ToString("0.00"));
            body = body.Replace("$$QUOTEKEY$$", order.Quote.QuoteKey.ToString());

            SendEmail(orderAddress.Email, "Order Purchase Confirmation", body);
        }

        public static void SendTracking(Order order)
        {
            Shipment shipment = ShipmentService.GetShipment(order.OrderID);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr>");
            sb.AppendLine("<td style=\"padding-right:8px;\">" + shipment.Service + "</td>");
            sb.AppendLine("<td style=\"padding-right:8px;\">" + shipment.Tracking + "</td>");
            sb.AppendLine("<td style=\"padding-right:8px;\">" + System.Net.WebUtility.HtmlEncode(shipment.ShipDate.ToString("g")) + " UTC</td>");
            sb.AppendLine("</tr>");

            string body = Properties.Resources.StatusUpdate;

            body = body.Replace("$$USER$$", order.Quote.Address.Name);
            body = body.Replace("$$ORDER$$", order.OrderID.ToString());
            body = body.Replace("$$ITEMROWS$$", sb.ToString());

            SendEmail(order.Quote.Address.Email, "Order Shipping Confirmation", body);
        }

        private static void SendEmail(string msgTo, string msgTitle, string msgBody)
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
    }
}