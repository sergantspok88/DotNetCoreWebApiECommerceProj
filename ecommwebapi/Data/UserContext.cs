using Ecommwebapi.Entities;
using Ecommwebapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ecommwebapi.Data
{
    public class UserContext : DbContext, IUserContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        //public int SaveChangesCount { get; private set; }
        public override int SaveChanges()
        {
            //base.SaveChanges();
            //this.SaveChangesCount++;
            //return 1;
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