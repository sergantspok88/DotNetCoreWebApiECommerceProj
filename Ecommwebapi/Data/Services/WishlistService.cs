using Ecommwebapi.Data.Models;
using Ecommwebapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork unitOfWork;

        public WishlistService(IUnitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<WishlistItem> GetAllWishlistItems()
        {
            return unitOfWork.WishlistRepo.WishlistItems.Include(w => w.Product).Include(w => w.User);
        }

        public IEnumerable<WishlistItem> GetAllWishlistItemsForUser(int userId)
        {
            return unitOfWork.WishlistRepo.WishlistItems.Include(w => w.Product).Include(w => w.User).Where(w => w.User.Id == userId);
        }

        public WishlistItem GetWishlistItemById(int id)
        {
            return unitOfWork.WishlistRepo.WishlistItems.Include(w => w.Product).Include(w => w.User).Where(w => w.Id == id).SingleOrDefault();
        }

        public WishlistItem CreateWishlistItem(WishlistItem wishlistItem)
        {
            if (unitOfWork.WishlistRepo.WishlistItems.Any(w => w.Product.Id == wishlistItem.Product.Id &&
                    w.User.Id == wishlistItem.User.Id))
            {
                throw new AppException("Product " + wishlistItem.Product.Name + " is already in wishlist for this user");
            }

            unitOfWork.WishlistRepo.CreateWishlistItem(wishlistItem);
            unitOfWork.Complete();

            return wishlistItem;
        }

        //public void UpdateWishlistItem(WishlistItem wishlistItemParam)
        //{
        //    var wishlistItem = repo.WishlistItems.SingleOrDefault(w => w.Id == wishlistItemParam.Id);

        //    if (wishlistItem == null)
        //    {
        //        throw new AppException("WishlistItem not found");
        //    }

        //    if (wishlistItemParam.Product != null)
        //    {
        //        wishlistItem.Product = wishlistItemParam.Product;
        //    }

        //    if (wishlistItemParam.User != null)
        //    {
        //        wishlistItem.User = wishlistItemParam.User;
        //    }

        //    repo.SaveWishlistItem(wishlistItem);
        //}

        public void DeleteWishlistItem(int id)
        {
            WishlistItem wishlistItem = unitOfWork.WishlistRepo.WishlistItems.SingleOrDefault(w => w.Id == id);
            if (wishlistItem != null)
            {
                unitOfWork.WishlistRepo.DeleteWishlistItem(wishlistItem);
                unitOfWork.Complete();
            }
        }
    }
}
