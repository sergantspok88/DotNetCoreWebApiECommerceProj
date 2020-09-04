using Ecommwebapi.Data.Models;
using Ecommwebapi.Services;
using Ecommwebapi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommwebapi.Helpers;
using Ecommwebapi.Data.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommwebapi.Controllers
{
    [Authorize]
    [ApiController]
    //[Route("[controller]")]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private readonly IMapper mapper;
        //private UserManager<User> userManager;

        public UsersController(IUserService userService,
            IMapper mapper
            //,UserManager<User> userManager
            )
        {
            this.userService = userService;
            this.mapper = mapper;
            //this.userManager = userManager;
        }

        /// <summary>
        /// Check role of current user
        /// </summary>
        /// <returns>
        /// Not authorized
        /// User
        /// Admin
        /// </returns>
        [AllowAnonymous]
        [HttpGet("role")]
        public string CheckRole()
        {
            if (User.Identity.Name == null)
            {
                return "Not authorized";
            }

            var currentUserId = int.Parse(User.Identity.Name);
            var user = userService.GetById(currentUserId);

            return user.Role;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<UserAuthenticateReadDto> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(mapper.Map<UserAuthenticateReadDto>(user));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterWriteDto model)
        {
            var user = mapper.Map<User>(model);

            try
            {
                //create user
                var newUser = userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAll()
        {
            var users = userService.GetAll();
            //return Ok(users);
            return Ok(mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(users));
        }

        [HttpGet("{id}")]
        public ActionResult<UserReadDto> GetById(int id)
        {
            //only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            var user = userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<User, UserReadDto>(user));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserUpdateWriteDto model)
        {
            //users can update only themselves, admins can anybody
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            var user = mapper.Map<User>(model);
            user.Id = id;

            try
            {
                userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            userService.Delete(id);
            return Ok();
        }
    }
}