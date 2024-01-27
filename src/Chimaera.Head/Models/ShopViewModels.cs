using Chimaera.Beasts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chimaera.Head.Models
{
    public class ShopIndexViewModel
    {
        public Series[] series { get; set; }
    }

    public class ShopProductViewModel
    {
        public Series series { get; set; }
        public Size[] sizes { get; set; }
    }
}