using System.Collections.Generic;
using System.Linq;
using ecommwebapi.Entities;
using ecommwebapi.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text.Json;
using ecommwebapi.Models;
using AutoMapper;
using ecommwebapi.Data;
using ecommwebapi.Data.Dtos;

namespace ecommwebapi.Services
{
    public class MockUserRepo : IUserRepo
    {
        private List<User> _users;

        private readonly AppSettings _appSettings;
        private readonly IMapper mapper;

        public MockUserRepo(IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            //Console.WriteLine("MockUserRepo ctor called");

            _appSettings = appSettings.Value;

            InitData();
            this.mapper = mapper;
        }

        private void InitData(){
            _users = new List<User>();
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHashAndSalt("admin", out passwordHash, out passwordSalt);
            _users.Add(new User{
                Id = 1, 
                FirstName = "Admin",
                LastName = "User",
                Username = "admin",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Role.Admin
            });

            CreatePasswordHashAndSalt("user", out passwordHash, out passwordSalt);
            _users.Add(new User
            {
                Id = 2,
                FirstName = "Normal",
                LastName = "User",
                Username = "user",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Role.User
            });
        }

        public UserAuthenticateReadDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _users.SingleOrDefault(x => x.Username == username);

            //Console.WriteLine(JsonSerializer.Serialize(user));

            //return null if user not found
            if (user == null)
            {
                return null;
            }

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
                return null;
            }

            //authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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

            //return user.WithoutPasswordData();
            return mapper.Map<UserAuthenticateReadDto>(user);
        }

        public IEnumerable<UserReadDto> GetAll()
        {
            //return _users.WithoutPasswordData();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(_users);
        }

        public UserReadDto GetById(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            //return user.WithoutPasswordData();
            return mapper.Map<UserReadDto>(user);
        }

        public UserReadDto Create(User user, string password){
            if(string.IsNullOrWhiteSpace(password)){
                throw new AppException("Password is required");
            }

            if(_users.Any(x => x.Username == user.Username)){
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHashAndSalt(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _users.Add(user);

            //return user;
            return mapper.Map<UserReadDto>(user);
        }

        public void Update(User userParam, string password = null){
            var user = _users.Find(x => x.Id == userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            //update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                //throw error if the new username is already taken
                if(_users.Any(x => x.Username == userParam.Username)){
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
                CreatePasswordHashAndSalt(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
        }

        public void Delete(int id){
            var user = _users.Find(x => x.Id == id);

            if (user != null)
            {
                _users.Remove(user);
            }
        }

        //private helper methods

        private static void CreatePasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }
            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHas");
            }
            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected)", "passwordSalt");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}