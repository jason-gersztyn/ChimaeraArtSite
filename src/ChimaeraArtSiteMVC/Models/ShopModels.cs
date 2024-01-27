using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ChimLib.Database;
using Dapper;

namespace ChimaeraArtSiteMVC.Models
{
    public class ProductSeries
    {
        public int ProductSeriesID { get; set; }
        public string ProductSeriesName { get; set; }
        public string ProductSeriesImageURL { get; set; }
        public List<ProductVariation> Products { get; set; }

        public void GetProducts()
        {
            var p = new { SeriesID = this.ProductSeriesID };

            using (SqlConnection conn = DB.GetConnection())
                Products = conn.Query<ProductVariation, Image, Group, ProductType, ProductColor, ProductGenre, ProductVariation>("MVCProductSeriesSelect",
                    (product, image, group, type, color, genre) =>
                    {
                        image.GroupItem = group;
                        product.ImageItem = image;
                        type.Genre = genre;
                        product.Type = type;
                        product.Color = color;
                        return product;
                    },
                    p,
                    splitOn: "ImageID, GroupID, ProductTypeID, ProductColorID, ProductGenreID",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();

        }
    }

    public class Address
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class Cart
    {
        public int CartID { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }
        [Display(Name = "Ship Address")]
        public Address ShipAddress { get; set; }
        [Display(Name = "Order Token")]
        public string OrderToken { get; set; }
    }

    public class CartItem
    {
        public int CartItemID { get; set; }
        [Display(Name = "Product")]
        public ProductVariation Variant { get; set; }
        public ProductSize Size { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int? UserID { get; set; }
        [Display(Name = "Date Ordered")]
        public DateTime DateOrdered { get; set; }
        public OrderStatus Status { get; set; }
        [Display(Name = "Ship Address")]
        public Address ShipAddress { get; set; }
        [Display(Name = "Shipping Paid")]
        public decimal ShippingPaid { get; set; }
        [Display(Name = "Order Token")]
        public string OrderToken { get; set; }
        public string SPOrderID { get; set; }
        public string PaymentID { get; set; }
        public Shipping[] Shipments { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemID { get; set; }
        [Display(Name = "Product")]
        public ProductVariation Variant { get; set; }
        public ProductSize Size { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        [Display(Name = "Discount Used")]
        public Discount DiscountUsed { get; set; }
    }

    public class OrderStatus
    {
        public int OrderStatusID { get; set; }
        [Display(Name = "Order Status")]
        public string Status { get; set; }
    }

    public class Shipping
    {
        public int ShippingID { get; set; }
        [Display(Name = "Shipping Date")]
        public DateTime ShippingDate { get; set; }
        [Display(Name = "Shipping Service")]
        public string ShippingService { get; set; }
        [Display(Name = "Tracking Number")]
        public string TrackingNumber { get; set; }
    }

    public class Discount
    {
        public int DiscountID { get; set; }
    }
}