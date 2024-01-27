using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using System;
using System.Web;
using System.Web.Mvc;

namespace Chimaera.Head.Controllers
{
    public class BaseController : Controller
    {

        #region Helper methods
        public Cart GetOrSetCart()
        {
            Guid cartKey;
            Cart cart = null;
            HttpCookie cartCookie = ControllerContext.HttpContext.Request.Cookies["CartKey"];
            if (cartCookie != null && Guid.TryParse(cartCookie.Value, out cartKey))
            {
                cart = CartService.GetCart(cartKey);
            }

            if (cart == null)
            {
                cart = new Cart();
                cart.CartKey = Guid.NewGuid();
                CartService.CreateCart(cart);
                cart = CartService.GetCart(cart.CartKey);
            }

            HttpCookie newCartCookie = new HttpCookie("CartKey");
            newCartCookie.Value = cart.CartKey.ToString();
            newCartCookie.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Set(newCartCookie);

            return cart;
        }

        public Quote GetQuote()
        {
            Guid quoteKey;
            Quote quote = new Quote();
            HttpCookie quoteCookie = ControllerContext.HttpContext.Request.Cookies["QuoteKey"];
            if (quoteCookie != null && Guid.TryParse(quoteCookie.Value, out quoteKey))
                quote = QuoteService.GetQuote(quoteKey);

            return quote;
        }
        #endregion
    }
}