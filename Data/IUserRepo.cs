using System.Collections.Generic;
using ecommwebapi.Data.Dtos;
using ecommwebapi.Models;

namespace ecommwebapi.Services{
    public interface IUserRepo{
        UserAuthenticateReadDto Authenticate(string username, string password);
        IEnumerable<UserReadDto> GetAll();
        UserReadDto GetById(int id);
        UserReadDto Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);
        bool SaveAll();
    }
}
