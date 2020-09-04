using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public interface IProductRepo
    {
        public IQueryable<Product> Products
        {
            get;
        }

        void CreateProduct(Product p);
        void DeleteProduct(Product p);
        void SaveProduct(Product p);

        //Categories
        public IQueryable<Category> Categories
        {
            get;
        }

        void CreateCategory(Category c);
        void DeleteCategory(Category c);
        void SaveCategory(Category c);

        //Cart items
        public IQueryable<CartItem> CartItems
        {
            get;
        }

        void CreateCartItem(CartItem c);
        void DeleteCartItem(CartItem c);
        void DeleteCartItemRange(IEnumerable<CartItem> c);
        void SaveCartItem(CartItem c);

        //Wishlist items
        public IQueryable<WishlistItem> WishlistItems
        {
            get;
        }

        void CreateWishlistItem(WishlistItem w);
        void DeleteWishlistItem(WishlistItem w);
        void SaveWishlistItem(WishlistItem w);

        //Orders and OrderItems
        public IQueryable<Order> Orders
        {
            get;
        }
        void CreateOrder(Order o);
        void DeleteOrder(Order o);
        void SaveOrder(Order o);

        public IQueryable<OrderItem> OrderItems
        {
            get;
        }
        void CreateOrderItem(OrderItem o);
        void DeleteOrderItem(OrderItem o);
        void DeleteOrderItemRange(IEnumerable<OrderItem> o);
        void SaveOrderItem(OrderItem o);
    }
}
