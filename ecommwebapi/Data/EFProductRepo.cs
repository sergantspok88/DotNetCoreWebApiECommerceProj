using Ecommwebapi.Data;
using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommwebapi.Data
{
    public class EFProductRepo : IProductRepo
    {
        private IDataContext ctx;
        public EFProductRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<Product> Products => ctx.Products;
    }
}
