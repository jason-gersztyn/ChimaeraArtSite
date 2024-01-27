using System;

namespace Chimaera.Beasts.Model
{
    public class Shipment
    {
        public int ShipmentID;
        public int OrderID;
        public DateTime ShipDate;
        public string Service;
        public string Tracking;
    }

    public enum ShippingType
    {
        First_Class,
        Priority,
        CA_First_Class,
        International_First_Class
    }
}