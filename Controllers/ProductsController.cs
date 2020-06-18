using System.Collections.Generic;
using ecommwebapi.Data;
using ecommwebapi.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommwebapi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDataRepo repo;

        public ProductsController(IDataRepo repo)
        {
            this.repo = repo;
        }

        //GET api/products
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllProducts()
        {
            //!!!need to implement dto for product
            //- and also profiles for automapper
            return Ok(repo.GetAllProducts());
        }

        //GET api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Order> GetProduct(int id)
        {
            var product = repo.GetProductById(id);
            if (product != null)
            {
                return Ok(repo.GetProductById(id));
            }
            else
            {
                return NotFound();
            }
        }
    }

}