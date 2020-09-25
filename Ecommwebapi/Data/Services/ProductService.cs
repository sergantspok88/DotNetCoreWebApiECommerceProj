using System.Collections.Generic;
using Ecommwebapi.Data.Models;
using System.Linq;
using Ecommwebapi.Helpers;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ecommwebapi.Data
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo repo;

        public ProductService(IProductRepo repo)
        {
            this.repo = repo;
        }

        public Product CreateProduct(Product product)
        {
            //Realistically need to check also category etc
            if (repo.Products.Any(p => p.Name == product.Name))
            {
                throw new AppException("Product name " + product.Name + " is already taken");
            }

            //autoincrement max id
            //product.Id = repo.Products.Max(p => p.Id) + 1;

            repo.CreateProduct(product);

            return product;
        }

        public void DeleteProduct(int id)
        {
            var product = repo.Products.SingleOrDefault(p => p.Id == id);

            if (product != null)
            {
                repo.DeleteProduct(product);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return repo.Products.Include(p => p.Category);
        }

        public IEnumerable<Product> GetAllProductsNameContains(string nameContains)
        {
            return repo.Products.Include(p => p.Category).Where(p => EF.Functions.Like(p.Name, $"%{nameContains}%"));
        }

        public IEnumerable<Product> GetProductsNameLikeSkipAndTakeNumber(string nameContains, int skip, int number)
        {
            //return repo.Products.Where(p => p.Name.Contains(nameContains)).Skip(0).Take(number);
            return repo.Products.Where(p => EF.Functions.Like(p.Name, $"%{nameContains}%")).Skip(0).Take(number);
        }

        public Product GetProductById(int id)
        {
            return repo.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProductsByCategoryName(string categoryName)
        {
            return repo.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryName);
        }

        public IEnumerable<Product> GetProductsSkipAndTakeNumber(int skip, int number)
        {
            return repo.Products.Include(p => p.Category).Skip(skip).Take(number);
        }

        public IEnumerable<Product> GetProductsByCategoryNameSkipAndTakeNumber(string categoryName, int skip, int number)
        {
            return repo.Products.Where(p => p.Category.Name == categoryName).Include(p => p.Category).Skip(skip).Take(number);
        }

        public void UpdateProduct(Product productParam)
        {
            var product = repo.Products.SingleOrDefault(p => p.Id == productParam.Id);

            if (product == null)
            {
                throw new AppException("Product not found");
            }

            if (!string.IsNullOrWhiteSpace(productParam.Name))
            {
                product.Name = productParam.Name;
            }

            if (!string.IsNullOrWhiteSpace(productParam.Description))
            {
                product.Description = productParam.Description;
            }

            if (productParam.Price > 0)
            {
                product.Price = productParam.Price;
            }

            //category
            {
                product.Category = productParam.Category;
            }

            repo.SaveProduct(product);
        }

        //Categories-----------------------------------
        public IEnumerable<Category> GetAllCategories()
        {
            return repo.Categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            return repo.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public Category GetCategoryByName(string categoryName)
        {
            return repo.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
        }

        public Category CreateCategory(Category category)
        {
            if (repo.Categories.Any(c => c.Name == category.Name))
            {
                throw new AppException("Category name " + category.Name + " is already taken");
            }

            //autoincrement max id
            //category.Id = repo.Categories.Max(c => c.Id) + 1;

            repo.CreateCategory(category);

            return category;
        }

        public void UpdateCategory(Category categoryParam)
        {
            var category = repo.Categories.SingleOrDefault(c => c.Id == categoryParam.Id);

            if (category == null)
            {
                throw new AppException("Category not found");
            }

            if (!string.IsNullOrWhiteSpace(categoryParam.Name))
            {
                category.Name = categoryParam.Name;
            }

            if (!string.IsNullOrWhiteSpace(categoryParam.Description))
            {
                category.Description = categoryParam.Description;
            }

            repo.SaveCategory(category);
        }

        public void DeleteCategory(string name)
        {
            var category = repo.Categories.SingleOrDefault(c => c.Name == name);

            if (category != null)
            {
                repo.DeleteCategory(category);
            }
        }

        //Cart items-----------------------------------
        public IEnumerable<CartItem> GetAllCartItems()
        {
            return repo.CartItems.Include(c => c.Product).Include(c => c.User);
        }

        //public IEnumerable<CartItem> GetAllCartItemsForUser(User u)
        //{
        //    return repo.CartItems.Where(c => c.User.Id == u.Id);
        //}
        public IEnumerable<CartItem> GetAllCartItemsForUser(int userId)
        {
            return repo.CartItems.Include(c => c.Product).Include(c => c.User).Where(c => c.User.Id == userId);
        }

        public CartItem GetCartItemById(int cartItemId)
        {
            return repo.CartItems.Include(c => c.Product).Include(c => c.User).Where(c => c.Id == cartItemId).FirstOrDefault();
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

            if (repo.CartItems.Any(c => c.User.Id == cartItem.User.Id &&
                c.Product.Id == cartItem.Product.Id))
            {
                throw new AppException("Product " + cartItem.Product.Name +
                    " is already in cart for userId: " + cartItem.User.Id);
            }

            //autoincrement max id
            //do this in db now
            //int id = 1;
            //if (repo.CartItems.Count() > 0)
            //{
            //    id = repo.CartItems.Max(c => c.Id) + 1;
            //}
            //cartItem.Id = id;

            repo.CreateCartItem(cartItem);

            return cartItem;
        }

        public void UpdateCartItem(CartItem cartItemParam)
        {
            var cartItem = repo.CartItems.SingleOrDefault(c => c.Id == cartItemParam.Id);

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

            repo.SaveCartItem(cartItem);
        }

        public void DeleteCartItem(int id)
        {
            CartItem cartItem = repo.CartItems.SingleOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                repo.DeleteCartItem(cartItem);
            }
        }

        public void DeleteCartItemRange(IEnumerable<CartItem> cartItems)
        {
            repo.DeleteCartItemRange(cartItems);
        }

        //Wishlist items-----------------------------------
        public IEnumerable<WishlistItem> GetAllWishlistItems()
        {
            return repo.WishlistItems.Include(w => w.Product).Include(w => w.User);
        }

        public IEnumerable<WishlistItem> GetAllWishlistItemsForUser(int userId)
        {
            return repo.WishlistItems.Include(w => w.Product).Include(w => w.User).Where(w => w.User.Id == userId);
        }

        public WishlistItem GetWishlistItemById(int id)
        {
            return repo.WishlistItems.Include(w => w.Product).Include(w => w.User).Where(w => w.Id == id).SingleOrDefault();
        }

        public WishlistItem CreateWishlistItem(WishlistItem wishlistItem)
        {
            if (repo.WishlistItems.Any(w => w.Product.Id == wishlistItem.Product.Id && 
                    w.User.Id == wishlistItem.User.Id))
            {
                throw new AppException("Product " + wishlistItem.Product.Name + " is already in wishlist for this user");
            }

            //autoincrement max id
            //int id = 1;
            //if (repo.WishlistItems.Count() > 0)
            //{
            //    id = repo.WishlistItems.Max(c => c.Id) + 1;
            //}
            //wishlistItem.Id = id;

            repo.CreateWishlistItem(wishlistItem);

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
            WishlistItem wishlistItem = repo.WishlistItems.SingleOrDefault(w => w.Id == id);
            if (wishlistItem != null)
            {
                repo.DeleteWishlistItem(wishlistItem);
            }
        }

        //Orders and OrderItems
        public IEnumerable<Order> GetAllOrders()
        {
            return repo.Orders;
        }

        public IEnumerable<Order> GetAllOrdersForUserId(int userId)
        {
            return repo.Orders.Where(o => o.User.Id == userId);
        }

        public Order GetOrderById(int orderId)
        {
            return repo.Orders.Where(o => o.Id == orderId).SingleOrDefault();
        }

        public Order CreateOrder(Order order)
        {
            //autoincrement max id
            //order.Id = repo.Orders.Max(o => o.Id) + 1;
            order.OrderDate = DateTime.Now;
            //Maybe smth different - like Guid etc
            order.OrderNumber = ("" + order.User.Id + order.OrderDate)
                .GetHashCode().ToString();

            repo.CreateOrder(order);

            return order;
        }

        public void DeleteOrder(int orderId)
        {
            Order order = repo.Orders.SingleOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                repo.DeleteOrder(order);
            }
        }

        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            return repo.OrderItems;
        }

        public IEnumerable<OrderItem> GetAllOrderItemsForUserId(int userId)
        {
            return repo.OrderItems.Where(o => o.Order.User.Id == userId);
        }

        public IEnumerable<OrderItem> GetAllOrderItemsForOrderId(int orderId)
        {
            return repo.OrderItems.Where(o => o.Order.Id == orderId);
        }

        public OrderItem GetOrderItemById(int orderItemId)
        {
            return repo.OrderItems.Where(o => o.Id == orderItemId).SingleOrDefault();
        }

        public OrderItem CreateOrderItem(OrderItem orderItem)
        {
            if (orderItem.Order == null)
            {
                throw new AppException("Order can not be null");
            }

            if (orderItem.Product == null)
            {
                throw new AppException("Product can not be null");
            }

            if (repo.OrderItems.Any(o => o.Product.Id == orderItem.Product.Id &&
                o.Order.Id == orderItem.Order.Id))
            {
                throw new AppException("Already have OrderItem with same Order and Product");
            }

            //autoincrement max id
            //orderItem.Id = repo.OrderItems.Max(c => c.Id) + 1;

            repo.CreateOrderItem(orderItem);

            return orderItem;
        }

        public void DeleteOrderItem(int orderItemId)
        {
            OrderItem orderItem = repo.OrderItems.SingleOrDefault(o => o.Id == orderItemId);
            if (orderItem != null)
            {
                repo.DeleteOrderItem(orderItem);
            }
        }

        public void DeleteOrderItemRange(IEnumerable<OrderItem> orderItems)
        {
            repo.DeleteOrderItemRange(orderItems);
        }
    }
}