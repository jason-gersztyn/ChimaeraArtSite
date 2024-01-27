using Chimaera.Head.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chimaera.Head.Controllers
{
    public class ComicController : Controller
    {
        // GET: Comic
        public ActionResult Index()
        {
            ComicIndexViewModel model = new ComicIndexViewModel();

            return View(model);
        }

        // GET: Comic/Series
        public ActionResult Series(int id)
        {
            ComicSeriesViewModel model = new ComicSeriesViewModel();

            return View(model);
        }

        // GET: Comic/Chapter
        public ActionResult Chapter(int id)
        {
            ComicChapterViewModel model = new ComicChapterViewModel();

            return View(model);
        }

        // GET: Comic/Page
        public ActionResult Page(int id)
        {
            ComicPageViewModel model = new ComicPageViewModel();

            return View(model);
        }
    }
}