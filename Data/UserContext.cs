using ecommwebapi.Entities;
using ecommwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommwebapi.Data
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

        public bool EnsureCreated(){
            return Database.EnsureCreated();
        }
    }
}