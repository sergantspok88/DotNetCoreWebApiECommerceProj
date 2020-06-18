using System.Collections.Generic;
using ecommwebapi.Data.Models;

namespace ecommwebapi.Data
{
    public interface IDataRepo
    {
         IEnumerable<Product> GetAllProducts();
         Product GetProductById(int id);
    }
}