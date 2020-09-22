using System.Threading.Tasks;
using Ecommwebapi.Entities;
using Ecommwebapi.Data.Models;
using System;
using System.Linq;

namespace Ecommwebapi.Data
{
    public class UserSeeder
    {
        private readonly IDataContext ctx;

        public UserSeeder(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public void Seed()
        {
            ctx.EnsureCreated();

            if (!ctx.Users.Any())
            {
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
}