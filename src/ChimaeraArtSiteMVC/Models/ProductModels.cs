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
    public class Product
    {
        public int ProductID { get; set; }
        public Image ImageItem { get; set; }
        public bool Available { get; set; }
        public string DesignID { get; set; }
    }

    public class ProductVariation : Product
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
}