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
    public class WishlistsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IWishlistService wishlistService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<WishlistsController> logger;

        public WishlistsController(IProductService productService,
            IWishlistService wishlistService,
            IUserService userService,
            IMapper mapper,
            ILogger<WishlistsController> logger)
        {
            this.mapper = mapper;
            this.productService = productService;
            this.wishlistService = wishlistService;
            this.userService = userService;
            this.logger = logger;
        }

        [Authorize]
        [HttpGet("wishlistitems")]
        public ActionResult<IEnumerable<WishlistItemReadDto>> GetAllWishlistItems()
        {
            if (User.IsInRole(Role.Admin))
            {
                //admins can get all wishlist items
                var wishlistItems = wishlistService.GetAllWishlistItems();
                return Ok(mapper.Map<IEnumerable<WishlistItem>, IEnumerable<WishlistItemReadDto>>(wishlistItems));
            }
            else
            {
                var currentUserId = int.Parse(User.Identity.Name);
                //non admins can get only their wishlist items
                var wishlistItems = wishlistService.GetAllWishlistItemsForUser(currentUserId);
                return Ok(mapper.Map<IEnumerable<WishlistItem>, IEnumerable<WishlistItemReadDto>>(wishlistItems));
            }
        }

        [Authorize]
        [HttpGet("wishlistitems/user/{userId}")]
        public ActionResult<IEnumerable<WishlistItemReadDto>> GetAllWishlistItemsForUser(int userId)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            if (User.IsInRole(Role.Admin) || userId == currentUserId)
            {
                var wishlistItems = wishlistService.GetAllWishlistItemsForUser(userId);
                return Ok(mapper.Map<IEnumerable<WishlistItem>, IEnumerable<WishlistItemReadDto>>(wishlistItems));
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("wishlistitems/{id}")]
        public ActionResult<WishlistItemReadDto> GetWishlistItemById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var wishlistItem = wishlistService.GetWishlistItemById(id);

            if (User.IsInRole(Role.Admin) || (wishlistItem != null && wishlistItem.User.Id == currentUserId))
            {
                if (wishlistItem != null)
                {
                    return Ok(mapper.Map<WishlistItem, WishlistItemReadDto>(wishlistItem));
                }
                else
                {
                    return NotFound();
                }
            }

            return Forbid();
        }

        [Authorize]
        [HttpPost("wishlistitems")]
        public IActionResult CreateWishlistItem([FromBody] WishlistItemAddWriteDto model)
        {
            var wishlistItem = mapper.Map<WishlistItem>(model);
            try
            {
                wishlistItem.User = userService.GetById(model.UserId);
                int currentUserId = int.Parse(User.Identity.Name);

                //UserId in WishlistItemAddWriteDto is not required
                //If it is empty - add current authorized user
                //otherwise add only if authorized as admin or authorized same as in addWrite model
                if (wishlistItem.User == null)
                {
                    wishlistItem.User = userService.GetById(currentUserId);
                }
                else if (!User.IsInRole(Role.Admin) && currentUserId != wishlistItem.User.Id)
                {
                    return Forbid();
                }

                wishlistItem.Product = productService.GetProductById(model.ProductId);
                var newWishlistItem = wishlistService.CreateWishlistItem(wishlistItem);

                return Created(new Uri($"{Request.Path}/{newWishlistItem.Id}", UriKind.Relative),
                    mapper.Map<WishlistItem, WishlistItemReadDto>(newWishlistItem));
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
        [HttpDelete("wishlistitems/{id}")]
        public IActionResult DeleteWishlistItem(int id)
        {
            int currentUserId = int.Parse(User.Identity.Name);
            var wishlistItem = wishlistService.GetWishlistItemById(id);

            if (User.IsInRole(Role.Admin) ||
                (wishlistItem.User != null && currentUserId == wishlistItem.User.Id))
            {
                wishlistService.DeleteWishlistItem(id);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }
    }
}
