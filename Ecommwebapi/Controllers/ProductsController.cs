using System.Collections.Generic;
using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using Ecommwebapi.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Ecommwebapi.Entities;
using Ecommwebapi.Helpers;
using Ecommwebapi.Services;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ecommwebapi.Controllers
{
    //[Route("api/products")]
    [Route("api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private ILogger<ProductsController> logger;

        public ProductsController(IProductService productService,
            IUserService userService,
            IMapper mapper,
            ILogger<ProductsController> logger)
        {
            this.mapper = mapper;
            this.productService = productService;
            this.userService = userService;
            this.logger = logger;
        }

        //GET api/products
        [HttpGet("products")]
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var products = productService.GetAllProducts();

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        //
        [HttpGet("products-like/{nameLike}/{take:int}")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsNameContains(string nameLike, int take)
        {
            var products = productService.GetProductsNameLikeSkipAndTakeNumber(nameLike, 0, take);

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        /// <summary>
        /// Return products with certain category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet("products/{category}")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsByCategory(string category)
        {
            var products = productService.GetProductsByCategoryName(category);

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        //GET api/products/{id}
        [HttpGet("products/{id:int}")]
        public ActionResult<ProductReadDto> GetProduct(int id)
        {
            var product = productService.GetProductById(id);
            if (product != null)
            {
                logger.LogInformation("categoryName: " + product.Category.Name);
                return Ok(mapper.Map<ProductReadDto>(product));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Return products. Skip and take certain number. Useful for pagination etc.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpGet("products/{skip:int}/{number:int}")]
        public ActionResult<ProductReadDto> GetProductsSkipAndTakeNumber(int skip, int number)
        {
            //logger.LogInformation("1");
            var products = productService.GetProductsSkipAndTakeNumber(skip, number);
            //var products = productService.GetAllProducts().Where(p => p.Price > 100);
            //logger.LogInformation("2");

            //foreach (var item in products)
            //{
            //    logger.LogInformation("3");
            //    break;
            //}

            return Ok(mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(products));
            //return Ok();
        }

        /// <summary>
        /// Return products of certain category. Skip and take certain number. Useful for pagination etc. 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="skip"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpGet("products/{category}/{skip:int}/{number:int}")]
        public ActionResult<ProductReadDto> GetProductsByCategorySkipAndTakeNumber(string category, int skip, int number)
        {
            var products = productService.GetProductsByCategoryNameSkipAndTakeNumber(category, skip, number);
            
            return Ok(mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(products));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("products")]
        public IActionResult AddProduct([FromBody] ProductAddWriteDto model)
        {
            var product = mapper.Map<Product>(model);
            try
            {
                //create product
                var newProduct = productService.CreateProduct(product);
                //return Ok();
                return Created(new Uri($"{Request.Path}/{newProduct.Id}", UriKind.Relative), 
                    mapper.Map<Product, ProductReadDto>(newProduct));
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("products/{id}")]
        //for now can use ProductAddWriteDto, ProductUpdateWriteModel would be the same
        public IActionResult UpdateProduct(int id, [FromBody] ProductAddWriteDto model)
        {
            var product = mapper.Map<Product>(model);
            product.Id = id;

            try
            {
                productService.UpdateProduct(product);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("products/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                productService.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.ToString()
                });
            }            
        }

        //------------------------------------------------------------
        //!!!everything below need to refactor in separate controllers

        //Categories----------------------------------------
        [HttpGet("categories")]
        public ActionResult<IEnumerable<CategoryReadDto>> GetAllCategories()
        {
            var categories = productService.GetAllCategories();

            return Ok(mapper.Map<IEnumerable<CategoryReadDto>>(categories));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("categoris")]
        public IActionResult AddCategory([FromBody] CategoryAddWriteDto model)
        {
            var category = mapper.Map<Category>(model);
            try
            {
                var newCategory = productService.CreateCategory(category);
                //return Ok();
                return Created(new Uri($"{Request.Path}/{newCategory.Id}", UriKind.Relative), 
                    mapper.Map<Category, CategoryReadDto>(newCategory));
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("categories/{id}")]
        //for now can use ProductAddWriteDto, ProductUpdateWriteModel would be the same
        public IActionResult UpdateCategory(int id, [FromBody] CategoryAddWriteDto model)
        {
            var category = mapper.Map<Category>(model);
            category.Id = id;

            try
            {
                productService.UpdateCategory(category);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("categories/{name}")]
        public IActionResult DeleteCategory(string name)
        {
            productService.DeleteCategory(name);
            return Ok();
        }

        //Cart items------------------------------------
        [Authorize]
        [HttpGet("cartitems")]
        public ActionResult<IEnumerable<CartItemReadDto>> GetAllCartItems()
        {
            if (User.IsInRole(Role.Admin))
            {
                //admins can get all cart items
                var cartItems = productService.GetAllCartItems();
                return Ok(mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemReadDto>>(cartItems));
            }
            else
            {
                var currentUserId = int.Parse(User.Identity.Name);
                //non admins can get their cart items
                var cartItems = productService.GetAllCartItemsForUser(currentUserId);
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
                var cartItems = productService.GetAllCartItemsForUser(userId);
                return Ok(mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemReadDto>>(cartItems));
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("cartitems/{id}")]
        public ActionResult<CartItemReadDto> GetCartItemsById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            var cartItem = productService.GetCartItemById(id);

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

                if (User.IsInRole(Role.Admin) || currentUserId == model.UserId)
                {
                    cartitem.Product = productService.GetProductById(model.ProductId);
                    var newCartItem = productService.CreateCartItem(cartitem);
                    //return Ok();
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
                return BadRequest(new
                {
                    message = ex.Message
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
                    productService.UpdateCartItem(cartitem);
                    return Ok();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpDelete("cartitems/{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            int currentUserId = int.Parse(User.Identity.Name);
            var cartItem = productService.GetCartItemById(id);

            if (User.IsInRole(Role.Admin) ||
                (cartItem.User != null && currentUserId == cartItem.User.Id))
            {
                productService.DeleteCartItem(id);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        //WishlistItems---------------------------------------------
        [Authorize]
        [HttpGet("wishlistitems")]
        public ActionResult<IEnumerable<WishlistItemReadDto>> GetAllWishlistItems()
        {
            if (User.IsInRole(Role.Admin))
            {
                //admins can get all wishlist items
                var wishlistItems = productService.GetAllWishlistItems();
                return Ok(mapper.Map<IEnumerable<WishlistItem>, IEnumerable<WishlistItemReadDto>>(wishlistItems));
            }
            else
            {
                var currentUserId = int.Parse(User.Identity.Name);
                //non admins can get their cart items
                var wishlistItems = productService.GetAllWishlistItemsForUser(currentUserId);
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
                var wishlistItems = productService.GetAllWishlistItemsForUser(userId);
                return Ok(mapper.Map<IEnumerable<WishlistItem>, IEnumerable<WishlistItemReadDto>>(wishlistItems));
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("wishlistitems/{id}")]
        public ActionResult<WishlistItemReadDto> GetWishlistItemById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var wishlistItem = productService.GetWishlistItemById(id);

            if (User.IsInRole(Role.Admin) || (wishlistItem != null && wishlistItem.User.Id == currentUserId))
            {
                if (wishlistItem != null)
                {
                    return Ok(mapper.Map<WishlistItem, WishlistItemReadDto>(wishlistItem));
                } else
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
                } else if (!User.IsInRole(Role.Admin) && currentUserId != wishlistItem.User.Id)
                {
                    return Forbid();
                }

                wishlistItem.Product = productService.GetProductById(model.ProductId);
                var newWishlistItem = productService.CreateWishlistItem(wishlistItem);
                //return Ok();
                return Created(new Uri($"{Request.Path}/{newWishlistItem.Id}", UriKind.Relative), 
                    mapper.Map<WishlistItem, WishlistItemReadDto>(newWishlistItem));
            }
            catch (AppException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpDelete("wishlistitems/{id}")]
        public IActionResult DeleteWishlistItem(int id)
        {
            int currentUserId = int.Parse(User.Identity.Name);
            var wishlistItem = productService.GetWishlistItemById(id);

            if (User.IsInRole(Role.Admin) ||
                (wishlistItem.User != null && currentUserId == wishlistItem.User.Id))
            {
                productService.DeleteWishlistItem(id);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        //Order and OrderItems--------------------------------
        [Authorize]
        [HttpGet("orders")]
        public ActionResult<IEnumerable<OrderReadDto>> GetAllOrders()
        {
            if (User.IsInRole(Role.Admin))
            {
                //admins can get all orders
                var orders = productService.GetAllOrders();
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDto>>(orders));
            }
            else
            {
                var currentUserId = int.Parse(User.Identity.Name);
                //non admins can get their orders
                var orders = productService.GetAllOrdersForUserId(currentUserId);
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
                var orders = productService.GetAllOrdersForUserId(userId);
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDto>>(orders));
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("orders/{id:int}")]
        public ActionResult<OrderReadDto> GetOrder(int id)
        {
            var order = productService.GetOrderById(id);
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

            var cartItems = productService.GetAllCartItemsForUser(currentUserId);
            if (cartItems.Count() > 0)
            {
                var currentUser = userService.GetById(currentUserId);

                //!!!Need to implement this logic with transaction 
                //https://docs.microsoft.com/en-us/ef/core/saving/transactions
                //- but transactions are tied to dbContext
                //call service.BeginTransaction -> repo.BeginTransaction -> if no transactions - create one and begin it
                //otherwise do nothing
                //try finally code here
                //in end of try call service.CommitTransaction -> repo.CommitTransaction -> if have transaction - commit it and null and dispose it
                //in finally call service.DisposeTransaction -> repo.DisposeTransaction -> if have transaction - dispose it which should roll it back if it was not commit
                //!!! possible problems
                //- what about multithreading/async
                //- non db repos (e.g. MockRepo) would not have this transaction logic - which might be fine

                //- create new Order
                Order newOrder = new Order();
                newOrder.User = currentUser;
                productService.CreateOrder(newOrder);

                //- create new OrderItem from each CartItem
                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem()
                    {
                        Product = cartItem.Product,
                        Order = newOrder,
                        Quantity = cartItem.Quantity
                    };
                    productService.CreateOrderItem(orderItem);
                }

                //- delete all CartItems
                productService.DeleteCartItemRange(cartItems);

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
            var order = productService.GetOrderById(id);

            if (User.IsInRole(Role.Admin) ||
                (order.User != null && currentUserId == order.User.Id))
            {
                //!!!Ideally should be implemented with transaction

                //delete order items
                var orderItems = order.Items;
                productService.DeleteOrderItemRange(orderItems);

                //delete order
                productService.DeleteOrder(id);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        //Actually orders show orderitems - so at least for now no need to implement separate order item logic
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