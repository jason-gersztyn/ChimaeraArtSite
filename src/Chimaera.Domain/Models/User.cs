using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain.Models
{
    public class User : Aggregate
    {
        public string Email;
        public string Password;
        public Address ShippingAddress;
        public string Phone;
        public IEnumerable<Order> Orders;
        public Cart CurrentCart;
    }

    public class Address
    {
        public string Name;
        public string Line1;
        public string Line2;
        public string City;
        public string State;
        public string CountryCode;
        public string PostalCode;
    }
}
