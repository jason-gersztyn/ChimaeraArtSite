using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Beasts.Model
{
    public class CartItem
    {
        public int CartItemID;
        public int CartID;
        public Product Product;
        public Size Size;
        public int Quantity;
    }
}
