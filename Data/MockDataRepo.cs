using System.Collections.Generic;
using ecommwebapi.Data.Models;
using System.Linq;

namespace ecommwebapi.Data
{
    public class MockDataRepo : IDataRepo
    {
        private List<Product> products;

        public MockDataRepo()
        {
            products = new List<Product>(){
                new Product{
                    Id = 1,
                    Name = "Notebook",
                    Description = "IFruit notebook",
                    Price = 2000
                },
                new Product{
                    Id = 2,
                    Name = "RAM 8Gb",
                    Description = "DDR4 RAM 8GB",
                    Price = 100
                },
                new Product{
                    Id = 3,
                    Name = "RTX 3080Ti",
                    Description = "GPU card Nvidia RTX 3080Ti",
                    Price = 1200
                },
                new Product{
                    Id = 4,
                    Name = "RTX 2080Ti",
                    Description = "GPU card Nvidia RTX 2080Ti",
                    Price = 900
                },
            };
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public Product GetProductById(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}