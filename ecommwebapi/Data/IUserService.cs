using System.Collections.Generic;
using Ecommwebapi.Data.Dtos;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Services
{
    public interface IUserService
    {
        UserAuthenticateReadDto Authenticate(string username, string password);
        IEnumerable<UserReadDto> GetAll();
        UserReadDto GetById(int id);
        UserReadDto Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);
        //bool SaveAll();
    }
}
