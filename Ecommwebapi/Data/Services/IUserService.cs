using System.Collections.Generic;
using Ecommwebapi.Data.Dtos;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);
        //bool SaveAll();
    }
}
