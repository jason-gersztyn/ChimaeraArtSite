using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain.Models
{
    public class Order : Aggregate
    {
        public IEnumerable<OrderItem> OrderItems;
        public Shipping ShippingData;
    }

    public class OrderItem
    {

    }

    public class Shipping
    {

    }
}
