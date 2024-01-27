using System.Web.Mvc;
using Chimaera.Beasts.Service;
using Chimaera.Head.Models;
using System.Linq;
using Chimaera.Beasts.Model;
using System.Collections.Generic;
using System;

namespace Chimaera.Head.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            var seriesBag = SeriesService.GetSeries();
            var products = new List<Product>();

            foreach (var series in seriesBag)
                products.AddRange(ProductService.GetProducts(SeriesID: series.SeriesID));

            Random rnd = new Random();
            model.ProductBag = products.OrderBy(x => rnd.Next()).Take(5).ToArray();

            return View(model);
        }

        public ActionResult FAQs()
        {
            return View();
        }
    }
}