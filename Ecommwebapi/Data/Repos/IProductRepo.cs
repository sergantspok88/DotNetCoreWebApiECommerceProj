using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public interface IProductRepo
    {
        public IQueryable<Product> Products
        {
            get;
        }

        void CreateProduct(Product p);
        void DeleteProduct(Product p);
        void SaveProduct(Product p);
    }
}
