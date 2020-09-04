using System.Collections.Generic;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Data
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
    }
}