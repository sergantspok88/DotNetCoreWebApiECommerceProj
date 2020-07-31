using System.Threading.Tasks;
using ecommwebapi.Entities;
using ecommwebapi.Models;
using System;

namespace ecommwebapi.Data
{
    public class UserSeeder
    {
        private readonly IUserContext ctx;

        public UserSeeder(IUserContext ctx)
        {
            this.ctx = ctx;
        }

        public void Seed(){
            ctx.EnsureCreated();

            byte[] passwordHash;
            byte[] passwordSalt;
            RepoUtility.CreatePasswordHashAndSalt("admin", out passwordHash, out passwordSalt);
            ctx.Users.Add(new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Username = "admin",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Role.Admin
            });

            RepoUtility.CreatePasswordHashAndSalt("user", out passwordHash, out passwordSalt);
            ctx.Users.Add(new User
            {
                Id = 2,
                FirstName = "Normal",
                LastName = "User",
                Username = "user",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Role.User
            });

            ctx.SaveChanges();
        }
    }
}