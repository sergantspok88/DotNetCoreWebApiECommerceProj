using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommwebapi.Data
{
    public interface IUserRepo
    {
        public IQueryable<User> Users
        {
            get;
        }

        void SaveUser(User u);
        void CreateUser(User u);
        void DeleteUser(User u);
    }
}
