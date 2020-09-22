using System.Collections.Generic;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Data
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsNameContains(string namePart);
        IEnumerable<Product> GetProductsByCategoryName(string categoryName);
        IEnumerable<Product> GetProductsSkipAndTakeNumber(int skip, int number);
        IEnumerable<Product> GetProductsNameLikeSkipAndTakeNumber(string nameContains, int skip, int number);
        IEnumerable<Product> GetProductsByCategoryNameSkipAndTakeNumber(string categoryName, int skip, int number);
        Product GetProductById(int id);
        Product CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

        //Categories
        IEnumerable<Category> GetAllCategories();
        Category CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(string name);

        //Cart items
        IEnumerable<CartItem> GetAllCartItems();
        //IEnumerable<CartItem> GetAllCartItemsForUser(User u);
        IEnumerable<CartItem> GetAllCartItemsForUser(int userId);
        CartItem GetCartItemById(int cartItemId);
        CartItem CreateCartItem(CartItem cartItem);
        void UpdateCartItem(CartItem cartItem);
        void DeleteCartItem(int id);
        void DeleteCartItemRange(IEnumerable<CartItem> cartItems);

        //Wishlist items
        IEnumerable<WishlistItem> GetAllWishlistItems();
        //IEnumerable<WishlistItem> GetAllWishlistItemsForUser(User u);
        IEnumerable<WishlistItem> GetAllWishlistItemsForUser(int userId);
        WishlistItem GetWishlistItemById(int id);
        WishlistItem CreateWishlistItem(WishlistItem wishlistItem);
        //void UpdateWishlistItem(WishlistItem wishlistItem);
        void DeleteWishlistItem(int id);

        //Orders and OrderItems
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetAllOrdersForUserId(int userId);
        Order GetOrderById(int orderId);
        Order CreateOrder(Order order);
        void DeleteOrder(int orderId);

        IEnumerable<OrderItem> GetAllOrderItems();
        IEnumerable<OrderItem> GetAllOrderItemsForOrderId(int orderId);
        IEnumerable<OrderItem> GetAllOrderItemsForUserId(int userId);
        OrderItem GetOrderItemById(int orderItemId);
        OrderItem CreateOrderItem(OrderItem orderItem);
        void DeleteOrderItem(int orderItemId);
        void DeleteOrderItemRange(IEnumerable<OrderItem> orderItems);
    }
}