using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommwebapi.Data
{
    public interface IProductRepo
    {
        public IQueryable<Product> Products
        {
            get;
        }
    }
}
