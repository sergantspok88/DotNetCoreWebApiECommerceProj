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
        int SaveChanges();
        bool EnsureCreated();
    }
}