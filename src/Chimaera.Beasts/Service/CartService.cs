using Chimaera.Beasts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using Chimaera.Beasts.Utils;
using System.Data;

namespace Chimaera.Beasts.Service
{
    public static class CartService
    {
        public static Cart GetCart(Guid CartKey)
        {
            Cart cartObject;

            using (SqlConnection conn = DB.GetConnection())
                cartObject = conn.Query<Cart, Discount, Cart>("GetCart",
                    (cart, discount) =>
                    {
                        cart.DiscountApplied = discount;
                        return cart;
                    },
                    new { CartKey = CartKey },
                    splitOn: "DiscountID",
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            cartObject.Items = GetCartItems(cartObject).ToArray();

            return cartObject;
        }

        public static void CreateCart(Cart cart)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartKey", cart.CartKey));

            DB.Set("PostCart", Params.ToArray());
        }

        public static void UpdateCart(Cart cart)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartID", cart.CartID));
            Params.Add(new SqlParameter("@DiscountID", cart.DiscountApplied.DiscountID));

            DB.Set("PutCart", Params.ToArray());
        }

        public static void DeleteCart(Guid CartKey)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartKey", CartKey));
            DB.Set("DeleteCart", Params.ToArray());
        }

        public static IEnumerable<CartItem> GetCartItems(Cart cart)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<CartItem, Product, Size, CartItem>("GetCartItems",
                    (item, product, size) =>
                    {
                        product = ProductService.GetProducts(product.ProductID).FirstOrDefault();
                        item.Product = product;
                        item.Size = size;
                        return item;
                    },
                    new { CartID = cart.CartID },
                    splitOn: "ProductID, SizeID",
                    commandType: CommandType.StoredProcedure);
        }

        public static void CreateCartItem(Cart cart, CartItem item)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartID", cart.CartID));
            Params.Add(new SqlParameter("@ProductID", item.Product.ProductID));
            Params.Add(new SqlParameter("@SizeID", item.Size.SizeID));
            Params.Add(new SqlParameter("@Quantity", item.Quantity));
            DB.Set("PostCartItem", Params.ToArray());
        }

        public static void UpdateCartItem(CartItem item)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartItemID", item.CartItemID));
            Params.Add(new SqlParameter("@Quantity", item.Quantity));
            DB.Set("PutCartItem", Params.ToArray());
        }

        public static void DeleteCartItem(CartItem item)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@CartItemID", item.CartItemID));
            DB.Set("DeleteCartItem", Params.ToArray());
        }


        public static decimal CalculateSubtotal(Cart cart)
        {
            decimal subtotal = 0M;

            foreach (CartItem item in cart.Items)
                subtotal += (item.Product.UnitPrice * item.Quantity);

            return subtotal;
        }
    }
}