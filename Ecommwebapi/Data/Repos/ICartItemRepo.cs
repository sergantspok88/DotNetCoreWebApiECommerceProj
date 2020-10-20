using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public interface ICartItemRepo
    {
        public IQueryable<CartItem> CartItems
        {
            get;
        }

        void CreateCartItem(CartItem c);
        void DeleteCartItem(CartItem c);
        void DeleteCartItemRange(IEnumerable<CartItem> c);
        void SaveCartItem(CartItem c);
    }
}
