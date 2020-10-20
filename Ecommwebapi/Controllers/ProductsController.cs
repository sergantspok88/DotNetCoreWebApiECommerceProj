using System.Collections.Generic;
using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using Ecommwebapi.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Ecommwebapi.Entities;
using Ecommwebapi.Helpers;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Ecommwebapi.Data.Services;

namespace Ecommwebapi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductService productService,
            ICategoryService categoryService,
            IUserService userService,
            IMapper mapper,
            ILogger<ProductsController> logger)
        {
            this.mapper = mapper;
            this.productService = productService;
            this.categoryService = categoryService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("products/count")]
        public ActionResult<string> GetTest()
        {
            return Ok("Product count: " + productService.GetAllProducts().Count());
        }

        //GET api/products
        [HttpGet("products")]
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var products = productService.GetAllProducts();

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

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
            var products = productService.GetProductsSkipAndTakeNumber(skip, number);

            return Ok(mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(products));
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

            //product.Category = productService.GetCategoryById(model.CategoryId);
            product.Category = categoryService.GetCategoryByName(model.CategoryName);

            if (product.Category == null)
            {
                return BadRequest(new ResponseError
                {
                    Message = "Category is not set"
                });
            }

            try
            {
                //create product
                var newProduct = productService.CreateProduct(product);

                return Created(new Uri($"{Request.Path}/{newProduct.Id}", UriKind.Relative),
                    mapper.Map<Product, ProductReadDto>(newProduct));
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError
                {
                    Message = ex.Message
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
            product.Category = categoryService.GetCategoryByName(model.CategoryName);
            if (product.Category == null)
            {
                return BadRequest(new ResponseError
                {
                    Message = "Can not find category"
                });
            }

            try
            {
                productService.UpdateProduct(product);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError
                {
                    Message = ex.Message
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
                return StatusCode(500, new ResponseError
                {
                    Message = ex.ToString()
                });
            }
        }      
    }
}