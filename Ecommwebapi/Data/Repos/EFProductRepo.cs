using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class EFProductRepo : IProductRepo
    {
        private IDataContext ctx;

        public EFProductRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<Product> Products => ctx.Products;

        public void CreateProduct(Product p)
        {
            ctx.Products.Add(p);
        }

        public void DeleteProduct(Product p)
        {
            ctx.Products.Remove(p);
        }

        public void SaveProduct(Product p)
        {
            ctx.SaveChanges();
        }
    }
}
