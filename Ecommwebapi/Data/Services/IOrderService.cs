using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetAllOrdersForUserId(int userId);
        Order GetOrderById(int orderId);
        Order CreateOrder(Order order);
        Order CreateOrderFromCartItems(Order order, IEnumerable<CartItem> cartItems);
        void DeleteOrder(int orderId);

        IEnumerable<OrderItem> GetAllOrderItems();
        IEnumerable<OrderItem> GetAllOrderItemsForOrderId(int orderId);
        IEnumerable<OrderItem> GetAllOrderItemsForUserId(int userId);
        OrderItem GetOrderItemById(int orderItemId);
        OrderItem CreateOrderItem(OrderItem orderItem);
        void DeleteOrderItem(int orderItemId);
    }
}
