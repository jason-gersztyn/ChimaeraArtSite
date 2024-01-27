using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using Chimaera.Head.Models;
using System.Linq;
using System.Web.Mvc;

namespace Chimaera.Head.Controllers
{
    public class CartController : BaseController
    {
        // GET: /Cart
        [AllowAnonymous]
        public ActionResult Index()
        {
            CartIndexViewModel model = new CartIndexViewModel();

            model.cart = GetOrSetCart();

            return View(model);
        }

        [AllowAnonymous]
        public void CartInsert(int productId, int sizeId)
        {
            Cart cart = GetOrSetCart();
            CartItem item = cart.Items.Where(x => x.Product.ProductID == productId && x.Size.SizeID == sizeId).FirstOrDefault();
            if (item != null)
            {
                item.Quantity++;
                CartService.UpdateCartItem(item);
            }
            else
            {
                item = new CartItem();
                item.Product = ProductService.GetProducts(productId).FirstOrDefault();
                item.Size = SizeService.GetSizes(sizeId).FirstOrDefault();
                item.Quantity = 1;

                CartService.CreateCartItem(cart, item);
            }
        }

        [AllowAnonymous]
        public void CartRemove(int itemId)
        {
            CartService.DeleteCartItem(new CartItem() { CartItemID = itemId });
        }

        [AllowAnonymous]
        public JsonResult DiscountAdd(string discountCode)
        {
            try
            {
                Discount discount = DiscountService.GetDiscount(discountCode);

                if (discount != null)
                {
                    Cart cart = GetOrSetCart();
                    cart.DiscountApplied = discount;
                    CartService.UpdateCart(cart);

                    return Json(new { ok = true });
                }
            }
            catch {}
            return Json(new { ok = false });
        }
    }
}