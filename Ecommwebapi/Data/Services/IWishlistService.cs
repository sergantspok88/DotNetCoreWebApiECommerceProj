using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public interface IWishlistService
    {
        IEnumerable<WishlistItem> GetAllWishlistItems();
        //IEnumerable<WishlistItem> GetAllWishlistItemsForUser(User u);
        IEnumerable<WishlistItem> GetAllWishlistItemsForUser(int userId);
        WishlistItem GetWishlistItemById(int id);
        WishlistItem CreateWishlistItem(WishlistItem wishlistItem);
        //void UpdateWishlistItem(WishlistItem wishlistItem);
        void DeleteWishlistItem(int id);
    }
}
