using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public interface ICartService
    {
        IEnumerable<CartItem> GetAllCartItems();
        //IEnumerable<CartItem> GetAllCartItemsForUser(User u);
        IEnumerable<CartItem> GetAllCartItemsForUser(int userId);
        CartItem GetCartItemById(int cartItemId);
        CartItem CreateCartItem(CartItem cartItem);
        void UpdateCartItem(CartItem cartItem);
        void DeleteCartItem(int id);
        void DeleteCartItemRange(IEnumerable<CartItem> cartItems);
    }
}
