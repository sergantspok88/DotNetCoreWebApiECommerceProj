using AutoMapper;
using Ecommwebapi.Data;
using Ecommwebapi.Data.Dtos;
using Ecommwebapi.Data.Models;
using Ecommwebapi.Data.Services;
using Ecommwebapi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommwebapi.Controllers
{
    [Route("api")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ICartService cartService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<OrdersController> logger;
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IOrderService orderService,
            ICartService cartService,
            IUserService userService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<OrdersController> logger)
        {
            this.mapper = mapper;
            this.orderService = orderService;
            this.cartService = cartService;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        [Authorize]
        [HttpGet("orders")]
        public ActionResult<IEnumerable<OrderReadDto>> GetAllOrders()
        {
            if (User.IsInRole(Role.Admin))
            {
                //admins can get all orders
                var orders = orderService.GetAllOrders();
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDto>>(orders));
            }
            else
            {
                var currentUserId = int.Parse(User.Identity.Name);
                //non admins can get their orders
                var orders = orderService.GetAllOrdersForUserId(currentUserId);
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDto>>(orders));
            }
        }

        [Authorize]
        [HttpGet("orders/user/{userId}")]
        public ActionResult<IEnumerable<OrderReadDto>> GetAllOrdersForUser(int userId)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            if (User.IsInRole(Role.Admin) || userId == currentUserId)
            {
                var orders = orderService.GetAllOrdersForUserId(userId);
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDto>>(orders));
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("orders/{id:int}")]
        public ActionResult<OrderReadDto> GetOrder(int id)
        {
            var order = orderService.GetOrderById(id);
            var currentUserId = int.Parse(User.Identity.Name);

            if (User.IsInRole(Role.Admin) || (order != null && order.User.Id == currentUserId))
            {
                if (order != null)
                {
                    return Ok(mapper.Map<OrderReadDto>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Forbid();
            }
        }

        [Authorize]
        [HttpPost("orders/create-from-cart")]
        public IActionResult CreateOrderFromCartItems()
        {
            var currentUserId = int.Parse(User.Identity.Name);

            var cartItems = cartService.GetAllCartItemsForUser(currentUserId);
            if (cartItems.Count() > 0)
            {
                var currentUser = userService.GetById(currentUserId);

                Order newOrder = new Order();
                newOrder.User = currentUser;
                newOrder = orderService.CreateOrderFromCartItems(newOrder, cartItems);

                return Created(new Uri($"orders/{newOrder.Id}", UriKind.Relative),
                    mapper.Map<Order, OrderReadDto>(newOrder));
            }
            else
            {
                return Ok();
            }
        }

        [Authorize]
        [HttpDelete("orders/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            int currentUserId = int.Parse(User.Identity.Name);
            var order = orderService.GetOrderById(id);

            if (User.IsInRole(Role.Admin) ||
                (order.User != null && currentUserId == order.User.Id))
            {
                orderService.DeleteOrder(id);

                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        //Orders show orderitems - so at least for now no need to implement separate order item logic
        //[Authorize]
        //[HttpGet("orderitems")]
        //public ActionResult<IEnumerable<OrderItemReadDto>> GetAllOrderItems()
        //{
        //    if (User.IsInRole(Role.Admin))
        //    {
        //        //admins can get all orders
        //        var orderItems = productService.GetAllOrderItems();
        //        return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemReadDto>>(orderItems));
        //    }
        //    else
        //    {
        //        var currentUserId = int.Parse(User.Identity.Name);
        //        //non admins can get their orders
        //        var orderItems = productService.GetAllOrderItemsForUserId(currentUserId);
        //        return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemReadDto>>(orderItems));
        //    }
        //}
    }
}
