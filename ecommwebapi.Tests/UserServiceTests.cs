using System;
using System.Linq;
using Xunit;
using Ecommwebapi.Services;
using Ecommwebapi.Data;
using Ecommwebapi.Helpers;
using Ecommwebapi.Models;
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
        private readonly IUserService userService;
        private readonly Mock<IUserContext> userContext = new Mock<IUserContext>();

        private readonly ITestOutputHelper output;

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }

        public UserServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            //output.WriteLine("Hellow world");

            var user = new User
            {
                Id = 1,
                FirstName = "Jonh",
                LastName = "Malkovich",
                Username = "neo",
                Role = Role.Admin
            };
            var users = new List<User> { user };
            // Convert the IEnumerable list to an IQueryable list
            IQueryable<User> queryableList = users.AsQueryable();

            //Force DbSet to return the Iqueryable members of our converted list
            var mockSet = GetQueryableMockDbSet<User>(users);

            userContext.Setup(x => x.Users).Returns(mockSet);

            IOptions<AppSettings> options = new Mock<IOptions<AppSettings>>().Object;

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new DataProfiles()));
            IMapper mapper = new Mapper(configuration);

            userService = new UserService(userContext.Object, options, mapper);
        }

        [Fact]
        public void Test1()
        {
            var user = userService.GetById(1);

            Assert.Equal("Jonh", user.FirstName);
        }
    }
}
