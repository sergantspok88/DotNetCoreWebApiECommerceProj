using Ecommwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommwebapi.Data
{
    public interface IUserContext
    {
        DbSet<User> Users { get; }
        int SaveChanges();
        bool EnsureCreated();
    }
}