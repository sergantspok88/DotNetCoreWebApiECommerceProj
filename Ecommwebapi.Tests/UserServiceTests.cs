using System;
using System.Linq;
using Xunit;
using Ecommwebapi.Services;
using Ecommwebapi.Data;
using Ecommwebapi.Helpers;
using Ecommwebapi.Data.Models;
using Ecommwebapi.Entities;
using Ecommwebapi.Profiles;
using Moq;
using Microsoft.Extensions.Options;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests
{
    public class UserServiceTests
    {
        private readonly ITestOutputHelper output;
        private IOptions<AppSettings> options;
        private Mapper mapper;

        public UserServiceTests(ITestOutputHelper output)
        {
            //Can use this output to write to console
            this.output = output;

            options = new Mock<IOptions<AppSettings>>().Object;

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new DataProfiles()));
            mapper = new Mapper(configuration);
        }

        [Fact]
        public void GetById_Correct_Data_Mapping()
        {
            var user1 = new User
            {
                Id = 1,
                FirstName = "Jonh",
                LastName = "Malkovich",
                Username = "neo",
                Role = Role.Admin
            };
            var users = new List<User> { user1 };

            var userRepo = new Mock<IUserRepo>();
            userRepo.Setup(x => x.Users).Returns(users.AsQueryable());

            //IUserService userService = new UserService(userRepo.Object, options, mapper);
            IUserService userService = new UserService(userRepo.Object, options);

            var user2 = userService.GetById(1);
            Assert.Equal(user1.Id, user2.Id);
            Assert.Equal(user1.FirstName, user2.FirstName);
            Assert.Equal(user1.LastName, user2.LastName);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("neo", null)]
        [InlineData(null, "password")]
        public void Authenticate_Returns_Null_On_Empty_Input(string username, string password)
        {
            var user1 = new User
            {
                Id = 1,
                FirstName = "Jonh",
                LastName = "Malkovich",
                Username = "neo",
                Role = Role.Admin
            };
            var users = new List<User> { user1 };

            var userRepo = new Mock<IUserRepo>();
            userRepo.Setup(x => x.Users).Returns(users.AsQueryable());

            //IUserService userService = new UserService(userRepo.Object, options, mapper);
            IUserService userService = new UserService(userRepo.Object, options);

            Assert.Null(userService.Authenticate(username, password));
            userRepo.Verify(x => x.Users, Times.Never);
        }
    }
}
