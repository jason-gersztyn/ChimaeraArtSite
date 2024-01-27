using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Beasts.Model
{
    public class Series
    {
        public int SeriesID;
        public string Name;
        public string ImageURL;
        public bool Active;
        public DateTime DateCreated;
        public Product[] Products;
    }
}
