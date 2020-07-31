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
        //private List<User> _users;
        private readonly IUserContext ctx;

        private readonly AppSettings _appSettings;
        private readonly IMapper mapper;

        //!!! with new ctx should maybe rename from MockUserRepo to just UserRepo
        //- because can now switch context to mock and this repo is on higher abstraction level
        public MockUserRepo(IUserContext ctx, IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            //Console.WriteLine("MockUserRepo ctor called");
            this.ctx = ctx;

            _appSettings = appSettings.Value;

            //InitData();
            this.mapper = mapper;
        }

        // private void InitData(){
        //     _users = new List<User>();
        //     byte[] passwordHash;
        //     byte[] passwordSalt;
        //     CreatePasswordHashAndSalt("admin", out passwordHash, out passwordSalt);
        //     _users.Add(new User{
        //         Id = 1, 
        //         FirstName = "Admin",
        //         LastName = "User",
        //         Username = "admin",
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //         Role = Role.Admin
        //     });

        //     CreatePasswordHashAndSalt("user", out passwordHash, out passwordSalt);
        //     _users.Add(new User
        //     {
        //         Id = 2,
        //         FirstName = "Normal",
        //         LastName = "User",
        //         Username = "user",
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //         Role = Role.User
        //     });
        // }

        public UserAuthenticateReadDto Authenticate(string username, string password)
        {
            // Console.WriteLine($"username: {username} password: {password}");
            // Console.WriteLine("All users:");
            // foreach (var item in ctx.Users)
            // {
            //     Console.WriteLine($"username {item.Username}  password {item.PasswordHash}");
            // }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = ctx.Users.SingleOrDefault(x => x.Username == username);

            //Console.WriteLine(JsonSerializer.Serialize(user));

            //return null if user not found
            if (user == null)
            {
                return null;
            }

            if(!RepoUtility.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
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
            return mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(ctx.Users);
        }

        public UserReadDto GetById(int id)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            //return user.WithoutPasswordData();
            return mapper.Map<UserReadDto>(user);
        }

        public UserReadDto Create(User user, string password){
            if(string.IsNullOrWhiteSpace(password)){
                throw new AppException("Password is required");
            }

            if(ctx.Users.Any(x => x.Username == user.Username)){
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            RepoUtility.CreatePasswordHashAndSalt(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //autoincrement id like in database
            user.Id = ctx.Users.Max(u => u.Id) + 1;
            
            //default role will be user
            user.Role = Role.User;

            ctx.Users.Add(user);

            //return user;
            return mapper.Map<UserReadDto>(user);
        }

        public void Update(User userParam, string password = null){
            var user = ctx.Users.SingleOrDefault(x => x.Id == userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            //update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                //throw error if the new username is already taken
                if(ctx.Users.Any(x => x.Username == userParam.Username)){
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
        }

        public void Delete(int id){
            var user = ctx.Users.SingleOrDefault(x => x.Id == id);

            if (user != null)
            {
                ctx.Users.Remove(user);
            }
        }

        public bool SaveAll(){
            return ctx.SaveChanges() > 0;
        }
    }
}