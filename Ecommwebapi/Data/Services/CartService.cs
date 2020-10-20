using Ecommwebapi.Data.Models;
using Ecommwebapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommwebapi.Data.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CartItem> GetAllCartItems()
        {
            return unitOfWork.CartItemRepo.CartItems.Include(c => c.Product).Include(c => c.User);
        }

        //public IEnumerable<CartItem> GetAllCartItemsForUser(User u)
        //{
        //    return repo.CartItems.Where(c => c.User.Id == u.Id);
        //}

        public IEnumerable<CartItem> GetAllCartItemsForUser(int userId)
        {
            return unitOfWork.CartItemRepo.CartItems.Include(c => c.Product).Include(c => c.User).Where(c => c.User.Id == userId);
        }

        public CartItem GetCartItemById(int cartItemId)
        {
            return unitOfWork.CartItemRepo.CartItems.Include(c => c.Product).Include(c => c.User).Where(c => c.Id == cartItemId).FirstOrDefault();
        }

        public CartItem CreateCartItem(CartItem cartItem)
        {
            if (cartItem.User == null)
            {
                throw new AppException("User can not be null");
            }

            if (cartItem.Product == null)
            {
                throw new AppException("Product can not be null");
            }

            if (unitOfWork.CartItemRepo.CartItems.Any(c => c.User.Id == cartItem.User.Id &&
                c.Product.Id == cartItem.Product.Id))
            {
                throw new AppException("Product " + cartItem.Product.Name +
                    " is already in cart for userId: " + cartItem.User.Id);
            }

            unitOfWork.CartItemRepo.CreateCartItem(cartItem);
            unitOfWork.Complete();

            return cartItem;
        }

        public void UpdateCartItem(CartItem cartItemParam)
        {
            var cartItem = unitOfWork.CartItemRepo.CartItems.SingleOrDefault(c => c.Id == cartItemParam.Id);

            if (cartItem == null)
            {
                throw new AppException("CartItem not found");
            }

            if (cartItemParam.Product != null)
            {
                cartItem.Product = cartItemParam.Product;
            }

            if (cartItemParam.User != null)
            {
                cartItem.User = cartItemParam.User;
            }

            if (cartItemParam.Quantity > 0)
            {
                cartItem.Quantity = cartItemParam.Quantity;
            }
            else
            {
                throw new AppException(@"Update cartItem.Quantity = {cartItemParam.Quantity}");
            }

            unitOfWork.Complete();
        }

        public void DeleteCartItem(int id)
        {
            CartItem cartItem = unitOfWork.CartItemRepo.CartItems.SingleOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                unitOfWork.CartItemRepo.DeleteCartItem(cartItem);
                unitOfWork.Complete();
            }
        }

        public void DeleteCartItemRange(IEnumerable<CartItem> cartItems)
        {
            unitOfWork.CartItemRepo.DeleteCartItemRange(cartItems);
            unitOfWork.Complete();
        }
    }
}
