using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public class EFWishlistRepo : IWishlistRepo
    {
        private IDataContext ctx;

        public EFWishlistRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<WishlistItem> WishlistItems => ctx.WishlistItems;

        public void CreateWishlistItem(WishlistItem w)
        {
            ctx.WishlistItems.Add(w);
        }

        public void DeleteWishlistItem(WishlistItem w)
        {
            ctx.WishlistItems.Remove(w);
        }

        public void SaveWishlistItem(WishlistItem w)
        {
            ctx.SaveChanges();
        }
    }
}
