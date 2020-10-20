using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataContext ctx;

        public IProductRepo ProductRepo { get; }

        public IUserRepo UserRepo { get; }

        public UnitOfWork(IDataContext ctx , IProductRepo products, IUserRepo users)
        {
            this.ctx = ctx;
            this.ProductRepo = products;
            this.UserRepo = users;
        }

        public int Complete()
        {
            return ctx.SaveChanges();
        }
    }
}
