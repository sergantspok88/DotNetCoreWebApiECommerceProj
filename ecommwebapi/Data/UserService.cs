using System.Collections.Generic;
using System.Linq;
using Ecommwebapi.Entities;
using Ecommwebapi.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text.Json;
using Ecommwebapi.Data.Models;
using AutoMapper;
using Ecommwebapi.Data;
using Ecommwebapi.Data.Dtos;
using ecommwebapi.Data;

namespace Ecommwebapi.Services
{
    public class UserService : IUserService
    {
        //private readonly IDataContext ctx;
        private readonly IUserRepo repo;
        private readonly AppSettings appSettings;
        private readonly IMapper mapper;

        public UserService(//IDataContext ctx, 
            IUserRepo repo,
            IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            //this.ctx = ctx;
            this.repo = repo;
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
        }

        public UserAuthenticateReadDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = repo.Users.SingleOrDefault(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!RepoUtility.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            //authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                //Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return mapper.Map<UserAuthenticateReadDto>(user);
        }

        public IEnumerable<UserReadDto> GetAll()
        {
            return mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(repo.Users);
        }

        public UserReadDto GetById(int id)
        {
            var user = repo.Users.FirstOrDefault(x => x.Id == id);
            return mapper.Map<UserReadDto>(user);
        }

        public UserReadDto Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (repo.Users.Any(x => x.Username == user.Username))
            {
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            RepoUtility.CreatePasswordHashAndSalt(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //autoincrement id like in database
            user.Id = repo.Users.Max(u => u.Id) + 1;

            //default role will be user
            user.Role = Role.User;

            repo.CreateUser(user);

            //return user;
            return mapper.Map<UserReadDto>(user);
        }

        public void Update(User userParam, string password = null)
        {
            var user = repo.Users.SingleOrDefault(x => x.Id == userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            //update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                //throw error if the new username is already taken
                if (repo.Users.Any(x => x.Username == userParam.Username))
                {
                    throw new AppException("Username " + userParam.Username + " is already taken");
                }

                user.Username = userParam.Username;
            }

            //update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            {
                user.FirstName = userParam.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                user.LastName = userParam.LastName;
            }

            //update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                RepoUtility.CreatePasswordHashAndSalt(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            //ctx.SaveChanges();
            repo.SaveUser(user);
        }

        public void Delete(int id)
        {
            var user = repo.Users.SingleOrDefault(x => x.Id == id);

            if (user != null)
            {
                repo.DeleteUser(user);
            }
        }

        //public bool SaveAll()
        //{
        //    //return ctx.SaveChanges() > 0;
        //    return 
        //}
    }
}