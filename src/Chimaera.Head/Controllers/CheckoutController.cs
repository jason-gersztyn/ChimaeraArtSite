using AutoMapper;
using Chimaera.Beasts.Integration;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using Chimaera.Head.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Web;
using Chimaera.Beasts.Extensions;
using Chimaera.Beasts.Utils;

namespace Chimaera.Head.Controllers
{
    public class CheckoutController : BaseController
    {
        // GET: /Checkout
        [AllowAnonymous]
        public ActionResult Index()
        {
            CheckoutShippingViewModel model = new CheckoutShippingViewModel();

            Cart cart = GetOrSetCart();

            if (cart.Items == null || cart.Items.Length == 0)
                return RedirectToAction("Index", "Cart");

            model.quote = QuoteService.CreateQuote(cart);

            model.quote.Address = new Address();

            model.Countries = CountryCodeService.GetCountries().Select(x => new SelectListItem()
            {
                Value = x.Code,
                Text = x.Name,
                Selected = x.Code == model.quote.Address.Country
            });

            HttpCookie newQuoteCookie = new HttpCookie("QuoteKey");
            newQuoteCookie.Value = model.quote.QuoteKey.ToString();
            newQuoteCookie.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Set(newQuoteCookie);

            return View(model);
        }

        // POST: /Checkout
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(CheckoutShippingViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.quote = GetQuote();

                Mapper.CreateMap<CheckoutShippingViewModel, Address>();

                if (model.quote.Address == null)
                    AddressService.CreateAddress(model.quote.QuoteID, Mapper.Map<Address>(model));
                else
                {
                    Address UpdatedAddress = Mapper.Map<Address>(model);
                    UpdatedAddress.AddressID = model.quote.Address.AddressID;
                    AddressService.UpdateAddress(UpdatedAddress);
                }

                Quote quote = GetQuote();
                quote.ShippingCharge = quote.CalculateShipping();
                QuoteService.UpdateQuote(quote);

                string URL = PayPal.ConfirmSale(GetQuote());

                return Redirect(URL);
            }

            return Index();
        }

        // GET: /Checkout/Approved
        [AllowAnonymous]
        public ActionResult Approved()
        {
            CheckoutApprovedViewModel model = new CheckoutApprovedViewModel();

            if (Request.Params.AllKeys.Contains("QuoteKey") 
                && Request.Params.AllKeys.Contains("paymentId") 
                && Request.Params.AllKeys.Contains("PayerID"))
            {
                Guid quoteKey;
                if (Guid.TryParse(Request.Params["QuoteKey"], out quoteKey))
                {
                    model.quote = QuoteService.GetQuote(quoteKey);
                    model.PaymentID = Request.Params["paymentId"].ToString();
                    model.PayerID = Request.Params["PayerID"].ToString();

                    return View(model);
                }
            }

            return RedirectToAction("Exploit");
        }

        // POST: /Checkout/Approved
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Approved(CheckoutApprovedPostModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    Order order = new Order();
            //    order.Quote = QuoteService.GetQuote(Guid.Parse(model.QuoteKey));
            //    order.Status = Status.Pending;
            //    order.PaypalSaleID = PayPal.ExecuteSale(model.PaymentID, model.PayerID);

            //    if (string.IsNullOrEmpty(order.PaypalSaleID))
            //        return RedirectToAction("Exploit");

            //    order = OrderService.CreateOrder(order);

            //    Email.SendConfirmation(order);

            //    if (order.Quote.Discount != null)
            //    {
            //        order.Quote.Discount.UpdateUsage();
            //        DiscountService.UpdateDiscount(order.Quote.Discount);
            //    }

            //    return RedirectToAction("Complete", new { id = order.Quote.QuoteKey });
            //}
            return RedirectToAction("Index");
        }

        // GET: /Checkout/Cancelled
        [AllowAnonymous]
        public ActionResult Cancelled()
        {
            return View();
        }

        // GET: /Checkout/Complete/{0}
        [AllowAnonymous]
        public ActionResult Complete(Guid id)
        {
            CheckoutCompleteViewModel model = new CheckoutCompleteViewModel();
            model.order = OrderService.GetOrders(QuoteKey: id).FirstOrDefault();

            HttpCookie quoteCookie = ControllerContext.HttpContext.Request.Cookies["QuoteKey"];
            if (quoteCookie != null)
            {
                quoteCookie.Expires = DateTime.Now.AddDays(-1);
                ControllerContext.HttpContext.Response.Cookies.Set(quoteCookie);
            }

            HttpCookie cartCookie = ControllerContext.HttpContext.Request.Cookies["CartKey"];
            if (cartCookie != null)
            {
                cartCookie.Expires = DateTime.Now.AddDays(-1);
                ControllerContext.HttpContext.Response.Cookies.Set(cartCookie);
            }            

            return View(model);
        }

        // GET: /Checkout/History/{0}
        [AllowAnonymous]
        public ActionResult History(Guid id)
        {
            CheckoutHistoryViewModel model = new CheckoutHistoryViewModel();
            model.order = OrderService.GetOrders(QuoteKey: id).FirstOrDefault();
            if (model.order != null)
                model.shipment = ShipmentService.GetShipment(model.order.OrderID);

            return View(model);
        }

        // GET: /Checkout/Exploit
        [AllowAnonymous]
        public ActionResult Exploit()
        {
            return View();
        }
    }
}