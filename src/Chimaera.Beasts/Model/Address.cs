using System;

namespace Chimaera.Beasts.Model
{
    public class Address
    {
        public int AddressID {get; set; }
        public string Email {get; set; }
        public string Name {get; set; }
        public string Street1 {get; set; }
        public string Street2 {get; set; }
        public string City {get; set; }
        public string State {get; set; }
        public string Country {get; set; }
        public string Zip {get; set; }
        public string Phone {get; set; }
        public DateTime DateCreated {get; set; }
        public DateTime DateUpdated {get; set; }
    }
}
