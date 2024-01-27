using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain.Models
{
    public class Cart : Aggregate
    {
        public IEnumerable<CartItem> CartItems;
    }

    public class CartItem
    {
        
    }
}
