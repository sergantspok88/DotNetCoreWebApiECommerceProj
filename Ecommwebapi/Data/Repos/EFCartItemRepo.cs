using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public class EFCartItemRepo : ICartItemRepo
    {
        private IDataContext ctx;

        public EFCartItemRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<CartItem> CartItems => ctx.CartItems;

        public void CreateCartItem(CartItem c)
        {
            ctx.CartItems.Add(c);
            //ctx.SaveChanges();
        }

        public void DeleteCartItem(CartItem c)
        {
            ctx.CartItems.Remove(c);
            //ctx.SaveChanges();
        }

        public void SaveCartItem(CartItem c)
        {
            ctx.SaveChanges();
        }

        public void DeleteCartItemRange(IEnumerable<CartItem> c)
        {
            ctx.CartItems.RemoveRange(c);
            //ctx.SaveChanges();
        }
    }
}
