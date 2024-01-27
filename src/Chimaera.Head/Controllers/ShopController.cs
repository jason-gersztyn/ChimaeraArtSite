using System.Linq;
using System.Web.Mvc;
using Chimaera.Head.Models;
using Chimaera.Beasts.Service;

namespace Chimaera.Head.Controllers
{
    public class ShopController : BaseController
    {
        // GET: /Shop/Index
        [AllowAnonymous]
        public ActionResult Index()
        {
            ShopIndexViewModel model = new ShopIndexViewModel();
            model.series = SeriesService.GetSeries().ToArray();
            return View(model);
        }

        // GET: /Shop/Product/{0}
        [AllowAnonymous]
        public ActionResult Product(int id)
        {
            ShopProductViewModel model = new ShopProductViewModel();
            model.series = SeriesService.GetSeries(id).FirstOrDefault();
            if (model.series != null)
                model.series.Products = ProductService.GetProducts(SeriesID: id).ToArray();

            return View(model);
        }
    }
}