using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain.Models
{
    public class Series : Aggregate
    {
        public IEnumerable<Product> Products;
    }

    public class Product
    {
        public int Id;
        public string SPDesignId;
        public decimal InchesFromTop;
        public decimal Width;
        public IEnumerable<ProductVariation> Variations;
        public bool Available;
    }

    public class ProductVariation
    {
        public int Id;
        public string Name;
        public ProductColor Color;
        public ProductType Type;
        public Uri Proof;
        public decimal Price;
        public bool Available;
    }

    public class ProductSize : Entity
    {

    }

    public class ProductType : Entity
    {
        public ProductGenre Genre;
        public MaterialInformation Material;
        public Uri SizeChart;
    }

    public class ProductColor : Entity
    {

    }

    public class ProductGenre
    {
        public int Id;
        public string GenreName;
    }

    public class MaterialInformation
    {
        public int Id;
        public IEnumerable<string> MaterialInfo;
    }
}
