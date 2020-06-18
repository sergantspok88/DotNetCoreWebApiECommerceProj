using System.Collections.Generic;
using ecommwebapi.Data;
using ecommwebapi.Data.Models;
using ecommwebapi.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ecommwebapi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDataRepo repo;
        private readonly IMapper mapper;

        public ProductsController(IDataRepo repo,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        //GET api/products
        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var products = repo.GetAllProducts();

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        //GET api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Order> GetProduct(int id)
        {
            var product = repo.GetProductById(id);
            if (product != null)
            {
                return Ok(mapper.Map<ProductReadDto>(product));
            }
            else
            {
                return NotFound();
            }
        }
    }

}