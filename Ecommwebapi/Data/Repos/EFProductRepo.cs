using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class EFProductRepo : IProductRepo
    {
        private IDataContext ctx;

        public EFProductRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        //Products
        public IQueryable<Product> Products => ctx.Products;

        public void CreateProduct(Product p)
        {
            ctx.Products.Add(p);
            ctx.SaveChanges();
        }

        public void DeleteProduct(Product p)
        {
            ctx.Products.Remove(p);
            ctx.SaveChanges();
        }

        public void SaveProduct(Product p)
        {
            ctx.SaveChanges();
        }

        //Categories
        public IQueryable<Category> Categories => ctx.Categories;

        public void CreateCategory(Category c)
        {
            ctx.Categories.Add(c);
            ctx.SaveChanges();
        }

        public void DeleteCategory(Category c)
        {
            ctx.Categories.Remove(c);
            ctx.SaveChanges();
        }

        public void SaveCategory(Category c)
        {
            ctx.SaveChanges();
        }

        //Cart items
        public IQueryable<CartItem> CartItems => ctx.CartItems;

        public void CreateCartItem(CartItem c)
        {
            ctx.CartItems.Add(c);
            ctx.SaveChanges();
        }

        public void DeleteCartItem(CartItem c)
        {
            ctx.CartItems.Remove(c);
            ctx.SaveChanges();
        }

        public void SaveCartItem(CartItem c)
        {
            ctx.SaveChanges();
        }

        public void DeleteCartItemRange(IEnumerable<CartItem> c)
        {
            ctx.CartItems.RemoveRange(c);
            ctx.SaveChanges();
        }

        //Wishlists
        public IQueryable<WishlistItem> WishlistItems => ctx.WishlistItems;

        public void CreateWishlistItem(WishlistItem w)
        {
            ctx.WishlistItems.Add(w);
            ctx.SaveChanges();
        }

        public void DeleteWishlistItem(WishlistItem w)
        {
            ctx.WishlistItems.Remove(w);
            ctx.SaveChanges();
        }

        public void SaveWishlistItem(WishlistItem w)
        {
            ctx.SaveChanges();
        }

        //Order and OrderItems-------------------------
        public IQueryable<Order> Orders => ctx.Orders;
        public IQueryable<OrderItem> OrderItems => ctx.OrderItems;

        public void CreateOrder(Order o)
        {
            ctx.Orders.Add(o);
            ctx.SaveChanges();
        }

        public void CreateOrderItem(OrderItem o)
        {
            ctx.OrderItems.Add(o);
            ctx.SaveChanges();
        }

        public void DeleteOrder(Order o)
        {
            ctx.Orders.Remove(o);
            ctx.SaveChanges();
        }

        public void DeleteOrderItem(OrderItem o)
        {
            ctx.OrderItems.Remove(o);
            ctx.SaveChanges();
        }

        public void DeleteOrderItemRange(IEnumerable<OrderItem> o)
        {
            ctx.OrderItems.RemoveRange(o);
            ctx.SaveChanges();
        }

        public void SaveOrder(Order o)
        {
            ctx.SaveChanges();
        }

        public void SaveOrderItem(OrderItem o)
        {
            ctx.SaveChanges();
        }
    }
}
