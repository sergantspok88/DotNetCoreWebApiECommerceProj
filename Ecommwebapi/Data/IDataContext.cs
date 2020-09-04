using Ecommwebapi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommwebapi.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; }
        DbSet<Product> Products
        {
            get;
        }
        DbSet<Category> Categories
        {
            get;
        }

        DbSet<WishlistItem> WishlistItems
        {
            get;
        }

        DbSet<CartItem> CartItems
        {
            get;
        }

        DbSet<Order> Orders
        {
            get;
        }

        DbSet<OrderItem> OrderItems
        {
            get;
        }

        int SaveChanges();
        bool EnsureCreated();
    }
}