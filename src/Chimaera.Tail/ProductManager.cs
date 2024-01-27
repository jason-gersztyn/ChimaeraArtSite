using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using Chimaera.Labs.PrintAura;
using Chimaera.Tail.Mappings;

namespace Chimaera.Tail
{
    public class ProductManager
    {
        private PrintAuraProductsClient client;

        public ProductManager()
        {
            client = new PrintAuraProductsClient();
            ProductMappings.Configure();
        }

        public void AddColors()
        {
            var listColorsResponse = client.ListColors();

            if (listColorsResponse.Result == 1 && listColorsResponse.Colors.Count > 0)
            {
                foreach(var listColor in listColorsResponse.Colors)
                {
                    Color color = ColorService.GetColor(listColor.ColorName);

                    if (color == null || color.ColorID == 0)
                    {
                        color = new Color()
                        {
                            Name = listColor.ColorName,
                            PrintAuraID = listColor.ColorId
                        };

                        ColorService.AddColor(color);
                    }
                }
            }
        }

        public void AddProductSizes()
        {
            var myProducts = ProductService.GetProducts();

            var listProductsResponse = client.ListProducts();

            foreach(var myProduct in myProducts)
            {
                var listProduct = listProductsResponse.Products.Where(x => x.BrandId == myProduct.Type.BrandID
                                                && x.ProductId == myProduct.Type.PrintAuraID).FirstOrDefault();

                if (listProduct != null)
                {
                    var listSizes = listProduct.Colors.Where(x => x.Key == myProduct.Color.PrintAuraID).FirstOrDefault();

                    foreach (int listSize in listSizes.Value)
                    {
                        Size size = SizeService.GetSize(listSize);

                        if (size != null && !myProduct.Sizes.Any(x => x.SizeID == size.SizeID))
                            SizeService.CreateProductSize(myProduct.ProductID, size.SizeID);
                    }
                }
            }
        }

        public void AddSizes()
        {
            var listSizesResponse = client.ListSizes();

            if (listSizesResponse.Result == 1 && listSizesResponse.Sizes.Count > 0)
            {
                var mySizes = SizeService.GetSizes();

                foreach(var listSize in listSizesResponse.Sizes)
                {
                    if (listSize.SizeGroup == "Adult" && !mySizes.Where(x => x.PrintAuraID == listSize.SizeId).Any())
                    {
                        Size size = new Size()
                        {
                            Name = listSize.SizeName,
                            PrintAuraID = listSize.SizeId
                        };

                        SizeService.CreateSize(size);
                    }
                }
            }
        }
    }
}