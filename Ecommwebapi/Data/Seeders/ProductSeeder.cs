using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class ProductSeeder
    {
        private readonly IDataContext ctx;

        public ProductSeeder(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public void Seed()
        {
            ctx.EnsureCreated();

            Category categoryVideocards = new Category
            {
                Id = 1,
                Name = "Videocards",
                Description = "videocards category"
            };
            Category categoryNotebooks = new Category
            {
                Id = 2,
                Name = "Notebooks",
                Description = "notebooks category"
            };
            Category categoryRest = new Category
            {
                Id = 3,
                Name = "Rest",
                Description = "rest category"
            };

            var categories = new List<Category>()
            {
                categoryVideocards,
                categoryNotebooks,
                categoryRest
            };

            var product1 = new Product
            {
                Id = 1,
                Name = "Notebook",
                Description = "IFruit notebook",
                Price = 2000,
                Category = categoryRest
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "RAM 8Gb",
                Description = "DDR4 RAM 8GB",
                Price = 100,
                Category = categoryRest
            };
            var product3 = new Product
            {
                Id = 3,
                Name = "RTX 3080Ti",
                Description = "GPU card Nvidia RTX 3080Ti",
                Price = 1200,
                Category = categoryVideocards
            };
            var product4 = new Product
            {
                Id = 4,
                Name = "RTX 2080Ti",
                Description = "GPU card Nvidia RTX 2080Ti",
                Price = 900,
                Category = categoryVideocards
            };

            if (!ctx.Products.Any())
            {
                ctx.Products.Add(product1);
                ctx.Products.Add(product2);
                ctx.Products.Add(product3);
                ctx.Products.Add(product4);

                Random rand = new Random();

                //add bunch or random products
                int count = 100;
                for (int i = 0; i < count; i++)
                {
                    ctx.Products.Add(new Product()
                    {
                        Id = (5 + i),
                        Name = "Product" + (5 + i),
                        Description = "product" + (5 + i) + " description",
                        Price = rand.Next(50, 3000),
                        Category = categories[rand.Next(0, categories.Count)]
                    });
                }

                ctx.SaveChanges();
            };


        }
    }
}
