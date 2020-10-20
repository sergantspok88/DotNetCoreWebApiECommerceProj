using System.Collections.Generic;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Data.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsNameContains(string namePart);
        IEnumerable<Product> GetProductsByCategoryName(string categoryName);
        IEnumerable<Product> GetProductsSkipAndTakeNumber(int skip, int number);
        IEnumerable<Product> GetProductsNameLikeSkipAndTakeNumber(string nameContains, int skip, int number);
        IEnumerable<Product> GetProductsByCategoryNameSkipAndTakeNumber(string categoryName, int skip, int number);
        Product GetProductById(int id);
        Product CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}