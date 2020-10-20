using Ecommwebapi.Data.Repos;
using Ecommwebapi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly IDataContext ctx;

        public IProductRepo ProductRepo
        {
            get;
        }

        public ICategoryRepo CategoryRepo
        {
            get;
        }

        public IWishlistRepo WishlistRepo
        {
            get;
        }

        public ICartItemRepo CartItemRepo
        {
            get;
        }

        public IOrderRepo OrderRepo
        {
            get;
        }

        public IUserRepo UserRepo
        {
            get;
        }

        public bool IsInTransaction
        {
            get;
            private set;
        }

        public EFUnitOfWork(IDataContext ctx, IProductRepo products,
            ICategoryRepo categories,
            IWishlistRepo wishlists,
            ICartItemRepo cartItems,
            IOrderRepo orders,
            IUserRepo users)
        {
            this.ctx = ctx;
            this.ProductRepo = products;
            this.CategoryRepo = categories;
            this.WishlistRepo = wishlists;
            this.CartItemRepo = cartItems;
            this.OrderRepo = orders;
            this.UserRepo = users;
        }

        public void BeginTransaction()
        {
            if (IsInTransaction)
            {
                throw new AppException("Already in transaction mode");
            }

            IsInTransaction = true;
        }

        public void EndTransaction()
        {
            if (IsInTransaction)
            {
                IsInTransaction = false;
                Complete();
            }
        }

        public int Complete()
        {
            if (!IsInTransaction)
            {
                return ctx.SaveChanges();
            }

            return -1;
        }
    }
}
