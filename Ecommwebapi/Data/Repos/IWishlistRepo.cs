using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public interface IWishlistRepo
    {
        public IQueryable<WishlistItem> WishlistItems
        {
            get;
        }
        void CreateWishlistItem(WishlistItem w);
        void DeleteWishlistItem(WishlistItem w);
        void SaveWishlistItem(WishlistItem w);
    }
}
