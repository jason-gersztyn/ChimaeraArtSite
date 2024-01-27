using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Utils;

namespace ChimLib.Database.Classes
{
    public class SaleProduct
    {
        public decimal UnitPrice;
        public int Quantity;
    }

    public class Discount
    {
        public int DiscountID;
        public string DiscountCode;
        public DiscountType Type;
        public decimal DiscountAmount;
        public int? DiscountUsage;
        public int? DiscountLimit;
        public DateTime? ExpirationDate;
    }

    public class Product
    {
        public int ProductID { get; set; }
        public Image ImageItem { get; set; }
        public bool Available { get; set; }
        public string DesignID { get; set; }
        public ProductVariation[] ProductVariations { get; set; }
    }

    public class ProductVariation
    {
        public int ProductVariationID { get; set; }
        public ProductType Type { get; set; }
        public ProductColor Color { get; set; }
        public string ProofURL { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Available { get; set; }
    }

    public class ProductType
    {
        public int ProductTypeID { get; set; }
        public string ProductTypeDisplayName { get; set; }
        public string ProductTypeSPName { get; set; }
        public ProductGenre Genre { get; set; }
        public MaterialInfo Material { get; set; }
    }

    public class ProductColor
    {
        public int ProductColorID { get; set; }
        public string ProductColorDisplayName { get; set; }
        public string ProductColorSPName { get; set; }
    }

    public class ProductSize
    {
        public int ProductSizeID { get; set; }
        public string ProductSizeDisplayName { get; set; }
        public string ProductSizeSPName { get; set; }
    }

    public class ProductGenre
    {
        public int ProductGenreID { get; set; }
        public string GenreName { get; set; }
    }

    public class MaterialInfo
    {
        public int MaterialInfoID { get; set; }
        public string[] MaterialInfoBullets { get; set; }
    }

    public class Image
    {
        public int ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageURL { get; set; }
        public Group GroupItem { get; set; }
        public int? Page { get; set; }
    }

    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class Chapter
    {
        public Image[] ChapterPages { get; set; }
    }

    public class Comic
    {
        public string ComicID { get; set; }
        public Chapter[] ComicChapters { get; set; }
        public string ComicName { get; set; }
        public string COmicDescription { get; set; }
    }

    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class Cart
    {
        public int CartID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Address ShipAddress { get; set; }
        public string OrderToken { get; set; }
        public CartItem[] CartItems { get; set; }
    }

    public class CartItem
    {
        public int CartItemID { get; set; }
        public ProductVariation Variant { get; set; }
        public ProductSize Size { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int? UserID { get; set; }
        public DateTime DateOrdered { get; set; }
        public OrderStatus Status { get; set; }
        public Address ShipAddress { get; set; }
        public decimal ShippingPaid { get; set; }
        public string OrderToken { get; set; }
        public string SPOrderID { get; set; }
        public string PaymentID { get; set; }
        public Shipping[] Shipments { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public ProductVariation Variant { get; set; }
        public ProductSize Size { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Discount DiscountUsed { get; set; }
    }

    public class OrderStatus
    {
        public int OrderStatusID { get; set; }
        public string Status { get; set; }
    }

    public class Shipping
    {
        public int ShippingID { get; set; }
        public DateTime ShippingDate { get; set; }
        public string ShippingService { get; set; }
        public string TrackingNumber { get; set; }
    }
}