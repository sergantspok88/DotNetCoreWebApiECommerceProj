using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public class MockProductRepo : IProductRepo
    {
        private List<Product> products;

        public MockProductRepo()
        {
            products = new List<Product>(){
                new Product{
                    Id = 1,
                    Name = "Notebook",
                    Description = "IFruit notebook",
                    Price = 2000
                },
                new Product{
                    Id = 2,
                    Name = "RAM 8Gb",
                    Description = "DDR4 RAM 8GB",
                    Price = 100
                },
                new Product{
                    Id = 3,
                    Name = "RTX 3080Ti",
                    Description = "GPU card Nvidia RTX 3080Ti",
                    Price = 1200
                },
                new Product{
                    Id = 4,
                    Name = "RTX 2080Ti",
                    Description = "GPU card Nvidia RTX 2080Ti",
                    Price = 900
                },
            };
        }

        public IQueryable<Product> Products => products.AsQueryable();

        public IQueryable<Category> Categories => throw new NotImplementedException();

        public IQueryable<CartItem> CartItems => throw new NotImplementedException();

        public IQueryable<WishlistItem> WishlistItems => throw new NotImplementedException();

        public IQueryable<Order> Orders => throw new NotImplementedException();

        public IQueryable<OrderItem> OrderItems => throw new NotImplementedException();

        public void CreateCartItem(CartItem c)
        {
            throw new NotImplementedException();
        }

        public void CreateCategory(Category c)
        {
            throw new NotImplementedException();
        }

        public void CreateOrder(Order o)
        {
            throw new NotImplementedException();
        }

        public void CreateOrderItem(OrderItem o)
        {
            throw new NotImplementedException();
        }

        public void CreateProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void CreateWishlistItem(WishlistItem w)
        {
            throw new NotImplementedException();
        }

        public void DeleteCartItem(CartItem c)
        {
            throw new NotImplementedException();
        }

        public void DeleteCartItemRange(IEnumerable<CartItem> c)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(Category c)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(Order o)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrderItem(OrderItem o)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrderItemRange(IEnumerable<OrderItem> o)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void DeleteWishlistItem(WishlistItem w)
        {
            throw new NotImplementedException();
        }

        public void SaveCartItem(CartItem c)
        {
            throw new NotImplementedException();
        }

        public void SaveCategory(Category c)
        {
            throw new NotImplementedException();
        }

        public void SaveOrder(Order o)
        {
            throw new NotImplementedException();
        }

        public void SaveOrderItem(OrderItem o)
        {
            throw new NotImplementedException();
        }

        public void SaveProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void SaveWishlistItem(WishlistItem w)
        {
            throw new NotImplementedException();
        }
    }
}
