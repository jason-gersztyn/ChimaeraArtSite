using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimaera.Beasts.Integration
{
    public static class PayPal
    {
        static Environment mode = Environment.live;

        public static string ConfirmSale(Quote quote)
        {
            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", mode.ToString());

            string AccessToken = GetPayPalAccessToken();
            APIContext AC = new APIContext(AccessToken);
            AC.Config = payPalConfig;

            Payee pe = new Payee();
            pe.merchant_id = "Q4A2XY37JY7VW";

            RedirectUrls ru = new RedirectUrls();
            ru.return_url = "https://www.chimaeraconspiracy.com/checkout/approved?Quote=" + quote.QuoteKey;
            ru.cancel_url = "https://www.chimaeraconspiracy.com/checkout/cancelled";

            Payer pyr = new Payer();
            pyr.payment_method = "paypal";

            decimal subtotal = QuoteService.CalculateSubtotal(quote);
            if (quote.Discount != null)
                subtotal -= DiscountService.CalculateDiscount(quote.Discount, subtotal);

            List<Transaction> transactions = new List<Transaction>();
            Transaction t = new Transaction();
            t.amount = new Amount();
            t.amount.currency = "USD";
            t.amount.details = new Details();
            t.amount.details.subtotal = subtotal.ToString("0.00");
            t.amount.details.tax = "0.00";
            t.amount.details.shipping = quote.ShippingCharge.Value.ToString("0.00");
            t.amount.total = Math.Round(subtotal + quote.ShippingCharge.Value, 2, MidpointRounding.AwayFromZero).ToString("0.00");
            t.description = "Chimaera Conspiracy Shop Purchase";

            Payment p = new Payment();
            p.intent = "sale";
            p.payer = pyr;
            p.redirect_urls = ru;
            p.transactions = new List<Transaction>() { t };

            Payment pResp = p.Create(AC);

            if (pResp.state.Equals("created", StringComparison.OrdinalIgnoreCase))
                return pResp.links.Where(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase)).Select(y => y.href).FirstOrDefault();

            throw new Exception("Paypal: approval_url missing");
        }

        public static string ExecuteSale(string PaymentID, string PayerID)
        {
            string finalID = "";

            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", mode.ToString());

            string AccessToken = GetPayPalAccessToken();
            APIContext AC = new APIContext(AccessToken);
            AC.Config = payPalConfig;

            PaymentExecution pe = new PaymentExecution() { payer_id = PayerID };

            Payment p = new Payment() { id = PaymentID };
            Payment pResp = p.Execute(AC, pe);

            if (pResp.state.Equals("approved", StringComparison.OrdinalIgnoreCase))
                finalID = pResp.transactions[0].related_resources[0].sale.id;

            return finalID;
        }

        public static bool CheckSale(string SaleID)
        {
            bool complete = false;

            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", mode.ToString());

            string AccessToken = GetPayPalAccessToken();
            APIContext AC = new APIContext(AccessToken);
            AC.Config = payPalConfig;

            Sale s = Sale.Get(AC, SaleID);
            complete = s.state == "completed";

            return complete;
        }

        public static string GetPayPalAccessToken()
        {
            string ClientID = (mode == Environment.sandbox ?
                "ARTy9lorp45zSyhnQtvYJKstGnkqORNlXazL_0ffl8blI164XHHBlHeVA3aex0ur4byEvuThuU33zQCW" : //SANDBOX
                "AYDMFeIOCHQNXp-coIUc_Q1VHkPEjaPUdD6Hp94teg9-VlarHtiuTQBzE2C5f0tm37Uj6kSCu9e5GyWO"); //LIVE

            string Secret = (mode == Environment.sandbox ?
                "EKv5bZFltlefyO4E2y-634orP1D7nHG_JLgOohDpW6rBRkK4PDsk5uklcDLscf0_11yyk6D4EdZ_IWJY" : //SANDBOX
                "EPbGPxMUv9-WXJb_V57bZnXBnP4p0XZgFRn4jM3mE-ZHxQVtMYkvBDx46x9Rrd3b2qCwqDDVJdSf5nA1"); //LIVE

            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", mode.ToString());
            OAuthTokenCredential otc = new OAuthTokenCredential(ClientID, Secret, payPalConfig);

            return otc.GetAccessToken();
        }

        internal enum Environment
        {
            live,
            sandbox
        }
    }
}