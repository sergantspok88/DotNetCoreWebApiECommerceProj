using Ecommwebapi.Data.Models;
using Ecommwebapi.Data.Services;
using Ecommwebapi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommwebapi.Helpers;
using Ecommwebapi.Data.Dtos;
using System.Collections.Generic;

namespace Ecommwebapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService,
            IMapper mapper
            )
        {
            this.userService = userService;
            this.mapper = mapper;
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
                return BadRequest(new ResponseError { Message = "Username or password is incorrect" });
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
                var newUser = userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError { Message = ex.Message });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAll()
        {
            var users = userService.GetAll();
            return Ok(mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(users));
        }

        /// <summary>
        /// Get user by its id.
        /// Only allow admins to access other user records.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<UserReadDto> GetById(int id)
        {
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

        /// <summary>
        /// Update user by its id.
        /// Users can update only themselves, admins can update anybody.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserUpdateWriteDto model)
        {
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
                return BadRequest(new ResponseError { Message = ex.Message });
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