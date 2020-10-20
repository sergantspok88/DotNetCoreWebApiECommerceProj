using Ecommwebapi.Data.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public interface IUnitOfWork
    {
        IProductRepo ProductRepo
        {
            get;
        }
        ICategoryRepo CategoryRepo
        {
            get;
        }
        IWishlistRepo WishlistRepo
        {
            get;
        }
        ICartItemRepo CartItemRepo
        {
            get;
        }
        IOrderRepo OrderRepo
        {
            get;
        }
        IUserRepo UserRepo
        {
            get;
        }
        int Complete();

        public bool IsInTransaction
        {
            get;          
        }

        void BeginTransaction();

        void EndTransaction();
    }
}
