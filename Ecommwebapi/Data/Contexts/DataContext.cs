using Ecommwebapi.Entities;
using Ecommwebapi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ecommwebapi.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users
        {
            get; set;
        }

        public DbSet<Product> Products
        {
            get; set;
        }

        public DbSet<Category> Categories
        {
            get; set;
        }

        public DbSet<WishlistItem> WishlistItems
        {
            get; set;
        }

        public DbSet<CartItem> CartItems
        {
            get; set;
        }

        public DbSet<Order> Orders
        {
            get; set;
        }

        public DbSet<OrderItem> OrderItems
        {
            get; set;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public bool EnsureCreated()
        {
            //return Database.EnsureCreated();
            //Migrate should do everything that EnsureCreated does
            //and additionaly handle migrations
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
            return true;
        }
    }
}