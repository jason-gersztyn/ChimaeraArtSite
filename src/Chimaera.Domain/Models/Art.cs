using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain.Models
{
    public class Comic : Aggregate
    {
        public IEnumerable<Art> ComicPages;
    }

    public class Art : Aggregate
    {
        public Uri ImageURL;
    }
}
