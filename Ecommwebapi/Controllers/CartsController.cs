using AutoMapper;
using Ecommwebapi.Data.Dtos;
using Ecommwebapi.Data.Models;
using Ecommwebapi.Data.Services;
using Ecommwebapi.Entities;
using Ecommwebapi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Ecommwebapi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly ICartService cartService;
        private readonly IMapper mapper;
        private readonly ILogger<CartsController> logger;

        public CartsController(IProductService productService,
            IUserService userService,
            ICartService cartService,
            IMapper mapper,
            ILogger<CartsController> logger)
        {
            this.productService = productService;
            this.userService = userService;
            this.cartService = cartService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Authorize]
        [HttpGet("cartitems")]
        public ActionResult<IEnumerable<CartItemReadDto>> GetAllCartItems()
        {
            if (User.IsInRole(Role.Admin))
            {
                //admins can get all cart items
                var cartItems = cartService.GetAllCartItems();
                return Ok(mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemReadDto>>(cartItems));
            }
            else
            {
                var currentUserId = int.Parse(User.Identity.Name);
                //non admins can get their cart items
                var cartItems = cartService.GetAllCartItemsForUser(currentUserId);
                return Ok(mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemReadDto>>(cartItems));
            }
        }

        [Authorize]
        [HttpGet("cartitems/user/{userId}")]
        public ActionResult<IEnumerable<CartItemReadDto>> GetAllCartItemsForUser(int userId)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            if (User.IsInRole(Role.Admin) || userId == currentUserId)
            {
                var cartItems = cartService.GetAllCartItemsForUser(userId);
                return Ok(mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemReadDto>>(cartItems));
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("cartitems/{id}")]
        public ActionResult<CartItemReadDto> GetCartItemsById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            var cartItem = cartService.GetCartItemById(id);

            if (User.IsInRole(Role.Admin) || (cartItem != null && cartItem.User.Id == currentUserId))
            {
                if (cartItem != null)
                {
                    return Ok(mapper.Map<CartItem, CartItemReadDto>(cartItem));
                }
                else
                {
                    return NotFound();
                }
            }

            return Forbid();
        }

        [Authorize]
        [HttpPost("cartitems")]
        public IActionResult CreateCartItem([FromBody] CartItemAddWriteDto model)
        {
            var cartitem = mapper.Map<CartItem>(model);
            try
            {
                cartitem.User = userService.GetById(model.UserId);
                int currentUserId = int.Parse(User.Identity.Name);

                //UserId in CartItemAddWriteDto is not required
                //If it is empty - add current authorized user
                //otherwise add only if authorized as admin or authorized same as in addWrite model
                if (cartitem.User == null)
                {
                    cartitem.User = userService.GetById(currentUserId);
                }

                if (User.IsInRole(Role.Admin) || currentUserId == cartitem.User.Id)
                {
                    cartitem.Product = productService.GetProductById(model.ProductId);
                    var newCartItem = cartService.CreateCartItem(cartitem);

                    return Created(new Uri($"{Request.Path}/{newCartItem.Id}", UriKind.Relative),
                        mapper.Map<CartItem, CartItemReadDto>(newCartItem));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError
                {
                    Message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpPut("cartitems/{id}")]
        public IActionResult UpdateCartItem(int id, [FromBody] CartItemAddWriteDto model)
        {
            var cartitem = mapper.Map<CartItem>(model);
            cartitem.Id = id;

            try
            {
                cartitem.User = userService.GetById(model.UserId);
                int currentUserId = int.Parse(User.Identity.Name);

                if (User.IsInRole(Role.Admin) || currentUserId == model.UserId)
                {
                    cartitem.Product = productService.GetProductById(model.ProductId);
                    cartService.UpdateCartItem(cartitem);
                    return Ok();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError
                {
                    Message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpDelete("cartitems/{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            int currentUserId = int.Parse(User.Identity.Name);
            var cartItem = cartService.GetCartItemById(id);

            if (User.IsInRole(Role.Admin) ||
                (cartItem.User != null && currentUserId == cartItem.User.Id))
            {
                cartService.DeleteCartItem(id);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }
    }
}
