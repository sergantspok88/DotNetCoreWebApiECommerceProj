using System.Collections.Generic;
using ecommwebapi.Entities;

namespace ecommwebapi.Services{
    public interface IUserService{
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
