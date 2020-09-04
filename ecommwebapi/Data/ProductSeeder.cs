using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommwebapi.Data
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

            if (!ctx.Products.Any())
            {
                ctx.Products.Add(new Product
                {
                    Id = 1,
                    Name = "Notebook",
                    Description = "IFruit notebook",
                    Price = 2000
                });

                ctx.Products.Add(new Product
                {
                    Id = 2,
                    Name = "RAM 8Gb",
                    Description = "DDR4 RAM 8GB",
                    Price = 100
                });

                ctx.Products.Add(new Product
                {
                    Id = 3,
                    Name = "RTX 3080Ti",
                    Description = "GPU card Nvidia RTX 3080Ti",
                    Price = 1200
                });

                ctx.Products.Add(new Product
                {
                    Id = 4,
                    Name = "RTX 2080Ti",
                    Description = "GPU card Nvidia RTX 2080Ti",
                    Price = 900
                });

                ctx.SaveChanges();
            };
        }
    }
}
