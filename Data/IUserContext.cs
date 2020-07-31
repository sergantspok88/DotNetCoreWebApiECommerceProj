using ecommwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommwebapi.Data
{
    public interface IUserContext
    {
        DbSet<User> Users { get; }
        int SaveChanges();
        bool EnsureCreated();
    }
}