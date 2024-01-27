using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Database;
using ChimLib.Database.Classes;
using PayPal.Api;

namespace ChimLib.Utils
{
    public static class PayPalUtils
    {
        public static string mode = "live";

        public static string ProcessSale(string firstName, string lastName, string billAddress1, string billAddress2, string billCity, string billState, string billCountry, string billZip, 
                                        List<SaleProduct> CartItems, string CardNo, string cvv2, int expYear, int expMonth, decimal shippingCharge)
        {
            string PaymentID = "";
            decimal totalPrice = 0M;
            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", mode);

            try
            {
                string AccessToken = GetPayPalAccessToken();
                APIContext AC = new APIContext(AccessToken);
                AC.Config = payPalConfig;

                Payee pe = new Payee();
                pe.merchant_id = "Q4A2XY37JY7VW";

                PayPal.Api.Address billingAddress = new PayPal.Api.Address();
                billingAddress.city = billCity;
                billingAddress.line1 = billAddress1;
                if (!string.IsNullOrWhiteSpace(billAddress2))
                    billingAddress.line2 = billAddress2;
                billingAddress.state = billState;
                billingAddress.country_code = billCountry;
                billingAddress.postal_code = billZip;

                CreditCard cc = new CreditCard();
                cc.billing_address = billingAddress;
                cc.number = CardNo;
                cc.cvv2 = cvv2;
                cc.type = CCUtils.CreditCardType(CardNo).ToLower();
                cc.first_name = firstName;
                cc.last_name = lastName;
                cc.expire_month = expMonth;
                cc.expire_year = expYear;

                FundingInstrument fi = new FundingInstrument();
                fi.credit_card = cc;

                Payer py = new Payer();
                py.payment_method = "credit_card";
                py.funding_instruments = new List<FundingInstrument>() { fi };

                Transaction t = new Transaction();
                t.amount = new Amount();

                foreach (SaleProduct item in CartItems)
                    totalPrice = Math.Round(totalPrice + (item.UnitPrice * item.Quantity), 2, MidpointRounding.AwayFromZero);

                t.amount.currency = "USD";
                t.amount.details = new Details();
                t.amount.details.subtotal = totalPrice.ToString("0.00");
                t.amount.details.tax = "0.00";
                t.amount.details.shipping = shippingCharge.ToString("0.00");
                t.amount.total = Math.Round(totalPrice + shippingCharge, 2, MidpointRounding.AwayFromZero).ToString("0.00");
                t.description = "Chimaera Conspiracy Store Purchase";

                Payment p = new Payment();
                p.intent = "sale";
                p.transactions = new List<Transaction>() { t };
                p.payer = py;

                Payment pResp = p.Create(AC);

                if(pResp.state.Equals("approved", StringComparison.OrdinalIgnoreCase))
                    PaymentID = pResp.id;
            }
            catch (PayPal.PayPalException ppex)
            {
                LoggingUtil.InsertError(ppex);
            }
            catch (WebException ex)
            {
                LoggingUtil.InsertError(ex);
                WebResponse wr = ex.Response;
                if(wr != null)
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream());
                    string ss = sr.ReadToEnd();
                    throw new Exception(ss);
                }
            }
            catch(Exception ex)
            {
                LoggingUtil.InsertError(ex);
            }

            return PaymentID;
        }

        public static string ConfirmSale(Guid cartKey, decimal ShippingCharge)
        {
            try
            {
                string payment_url = "";

                decimal totalPrice = 0M;
                Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
                payPalConfig.Add("mode", mode);

                string AccessToken = GetPayPalAccessToken();
                APIContext AC = new APIContext(AccessToken);
                AC.Config = payPalConfig;

                Payee pe = new Payee();
                pe.merchant_id = "Q4A2XY37JY7VW";

                RedirectUrls ru = new RedirectUrls();
                ru.return_url = "https://www.chimaeraconspiracy.com/shop/approved.aspx";
                ru.cancel_url = "https://www.chimaeraconspiracy.com/shop/cancelled.aspx";

                Payer pyr = new Payer();
                pyr.payment_method = "paypal";

                List<Transaction> transactions = new List<Transaction>();
                Transaction t = new Transaction();
                t.amount = new Amount();

                List<SqlParameter> sqlp = new List<SqlParameter>();
                sqlp.Add(new SqlParameter("@CacheID", cartKey));

                DataTable dtDiscount = DB.Get("CartDiscountSelect", sqlp.ToArray());
                if(dtDiscount != null && dtDiscount.Rows.Count > 0)
                {
                    int DiscountID = (int)(dtDiscount.Rows[0][0]);
                    totalPrice = DiscountUtils.CalculateDiscountTotal(DiscountID, cartKey);
                }
                else
                {
                    DataTable dtSub = DB.Get("CartSubtotalGet", sqlp.ToArray());
                    if (dtSub != null && dtSub.Rows.Count > 0)
                    {
                        string strSub = dtSub.Rows[0][0].ToString();
                        decimal tryDecimal = 0M;
                        if (decimal.TryParse(strSub, out tryDecimal))
                            totalPrice = tryDecimal;
                        else
                            throw new Exception("You done mucked up good");
                    }
                    else
                    {
                        throw new Exception("Subtotal could not be gathered");
                    }
                }

                t.amount.currency = "USD";
                t.amount.details = new Details();
                t.amount.details.subtotal = totalPrice.ToString("0.00");
                t.amount.details.tax = "0.00";
                t.amount.details.shipping = ShippingCharge.ToString("0.00");
                t.amount.total = Math.Round(totalPrice + ShippingCharge, 2, MidpointRounding.AwayFromZero).ToString("0.00");
                t.description = "Chimaera Conspiracy Shop Purchase";

                Payment p = new Payment();
                p.intent = "sale";
                p.payer = pyr;
                p.redirect_urls = ru;
                p.transactions = new List<Transaction>() { t };

                Payment pResp = p.Create(AC);

                if (pResp.state.Equals("created", StringComparison.OrdinalIgnoreCase))
                    payment_url = pResp.links.Where(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase)).Select(y => y.href).FirstOrDefault();

                return payment_url;
            }
            catch(PayPal.PayPalException ppe)
            {
                throw ppe;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static string ExecuteSale(string PaymentID, string PayerID)
        {
            try
            {
                string finalID = "";

                Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
                payPalConfig.Add("mode", mode);

                string AccessToken = GetPayPalAccessToken();
                APIContext AC = new APIContext(AccessToken);
                AC.Config = payPalConfig;

                PaymentExecution pe = new PaymentExecution();
                pe.payer_id = PayerID;

                Payment p = new Payment();
                p.id = PaymentID;
                Payment pResp = p.Execute(AC, pe);

                if (pResp.state.Equals("approved", StringComparison.OrdinalIgnoreCase))
                    finalID = pResp.id;

                return finalID;
            }
            catch(PayPal.PayPalException ppe)
            {
                Console.WriteLine(ppe);
                throw ppe;
            }
        }

        public static string GetPayPalAccessToken()
        {
            string ClientID = (mode == "sandbox" ? 
                "ARTy9lorp45zSyhnQtvYJKstGnkqORNlXazL_0ffl8blI164XHHBlHeVA3aex0ur4byEvuThuU33zQCW" :
                "AYDMFeIOCHQNXp-coIUc_Q1VHkPEjaPUdD6Hp94teg9-VlarHtiuTQBzE2C5f0tm37Uj6kSCu9e5GyWO");
            //SANDBOX
            //"ARTy9lorp45zSyhnQtvYJKstGnkqORNlXazL_0ffl8blI164XHHBlHeVA3aex0ur4byEvuThuU33zQCW";
            //LIVE
            //"AYDMFeIOCHQNXp-coIUc_Q1VHkPEjaPUdD6Hp94teg9-VlarHtiuTQBzE2C5f0tm37Uj6kSCu9e5GyWO";
            string Secret = (mode == "sandbox" ?
                "EKv5bZFltlefyO4E2y-634orP1D7nHG_JLgOohDpW6rBRkK4PDsk5uklcDLscf0_11yyk6D4EdZ_IWJY" :
                "EPbGPxMUv9-WXJb_V57bZnXBnP4p0XZgFRn4jM3mE-ZHxQVtMYkvBDx46x9Rrd3b2qCwqDDVJdSf5nA1");
            //SANDBOX
            //"EKv5bZFltlefyO4E2y-634orP1D7nHG_JLgOohDpW6rBRkK4PDsk5uklcDLscf0_11yyk6D4EdZ_IWJY";
            //LIVE
            //"EPbGPxMUv9-WXJb_V57bZnXBnP4p0XZgFRn4jM3mE-ZHxQVtMYkvBDx46x9Rrd3b2qCwqDDVJdSf5nA1";

            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", mode);
            OAuthTokenCredential otc = new OAuthTokenCredential(ClientID, Secret, payPalConfig);

            return otc.GetAccessToken();
        }
    }
}
