using System.Collections.Generic;
using Ecommwebapi.Data.Models;
using System.Linq;
using ecommwebapi.Data;

namespace Ecommwebapi.Data
{
    public class ProductService : IProductService
    {
        private IProductRepo repo;

        public ProductService(IProductRepo repo)
        {
            this.repo = repo;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return repo.Products;
        }

        public Product GetProductById(int id)
        {
            return repo.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}