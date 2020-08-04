using System.Collections.Generic;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Data
{
    public interface IDataService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
    }
}