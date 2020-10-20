using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class EFUserRepo : IUserRepo
    {
        private IDataContext ctx;

        public EFUserRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<User> Users => ctx.Users;

        public void CreateUser(User u)
        {
            ctx.Users.Add(u);
            //ctx.SaveChanges();
        }

        public void DeleteUser(User u)
        {
            ctx.Users.Remove(u);
            //ctx.SaveChanges();
        }

        public void SaveUser(User u)
        {
            ctx.SaveChanges();
        }
    }
}
