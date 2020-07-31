using ecommwebapi.Entities;
using ecommwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommwebapi.Data
{
    public class MockUserContext : DbContext, IUserContext
    {
        public MockUserContext(DbContextOptions<MockUserContext> options) : base(options)
        {
            //this.user
            //add mock data here???
            //InitData();
        }

        // private void InitData()
        // {
        //     //Users = new List<User>();
        //     byte[] passwordHash;
        //     byte[] passwordSalt;
        //     RepoUtility.CreatePasswordHashAndSalt("admin", out passwordHash, out passwordSalt);
        //     Users.Add(new User
        //     {
        //         Id = 1,
        //         FirstName = "Admin",
        //         LastName = "User",
        //         Username = "admin",
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //         Role = Role.Admin
        //     });

        //     RepoUtility.CreatePasswordHashAndSalt("user", out passwordHash, out passwordSalt);
        //     Users.Add(new User
        //     {
        //         Id = 2,
        //         FirstName = "Normal",
        //         LastName = "User",
        //         Username = "user",
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //         Role = Role.User
        //     });

        //     SaveChanges();
        // }

        public DbSet<User> Users { get; set; }

        public int SaveChangesCount { get; private set; }
        public override int SaveChanges()
        {
            base.SaveChanges();
            this.SaveChangesCount++;
            return 1;
        }

        public bool EnsureCreated(){
            return Database.EnsureCreated();
        }
    }
}