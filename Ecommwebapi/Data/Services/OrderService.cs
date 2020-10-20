using Ecommwebapi.Data.Models;
using Ecommwebapi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICartService cartService;

        public OrderService(IUnitOfWork unitOfWork,
            ICartService cartService)
        {
            this.unitOfWork = unitOfWork;
            this.cartService = cartService;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return unitOfWork.OrderRepo.Orders;
        }

        public IEnumerable<Order> GetAllOrdersForUserId(int userId)
        {
            return unitOfWork.OrderRepo.Orders.Where(o => o.User.Id == userId);
        }

        public Order GetOrderById(int orderId)
        {
            return unitOfWork.OrderRepo.Orders.Where(o => o.Id == orderId).SingleOrDefault();
        }

        public Order CreateOrder(Order order)
        {
            order.OrderDate = DateTime.Now;
            //Maybe smth different - like Guid etc
            order.OrderNumber = ("" + order.User.Id + order.OrderDate)
                .GetHashCode().ToString();

            unitOfWork.OrderRepo.CreateOrder(order);
            unitOfWork.Complete();

            return order;
        }

        public Order CreateOrderFromCartItems(Order order, IEnumerable<CartItem> cartItems)
        {
            unitOfWork.BeginTransaction();

            order = CreateOrder(order);

            //- create new OrderItem from each CartItem
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem()
                {
                    Product = cartItem.Product,
                    Order = order,
                    Quantity = cartItem.Quantity
                };
                CreateOrderItem(orderItem);
            }

            //- delete all CartItems
            cartService.DeleteCartItemRange(cartItems);

            unitOfWork.EndTransaction();

            return order;
        }

        public void DeleteOrder(int orderId)
        {
            Order order = unitOfWork.OrderRepo.Orders.SingleOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                if (order.Items != null)
                {
                    DeleteOrderItemRange(order.Items);
                }
                
                unitOfWork.OrderRepo.DeleteOrder(order);

                unitOfWork.Complete();
            }            
        }

        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            return unitOfWork.OrderRepo.OrderItems;
        }

        public IEnumerable<OrderItem> GetAllOrderItemsForUserId(int userId)
        {
            return unitOfWork.OrderRepo.OrderItems.Where(o => o.Order.User.Id == userId);
        }

        public IEnumerable<OrderItem> GetAllOrderItemsForOrderId(int orderId)
        {
            return unitOfWork.OrderRepo.OrderItems.Where(o => o.Order.Id == orderId);
        }

        public OrderItem GetOrderItemById(int orderItemId)
        {
            return unitOfWork.OrderRepo.OrderItems.Where(o => o.Id == orderItemId).SingleOrDefault();
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

            if (unitOfWork.OrderRepo.OrderItems.Any(o => o.Product.Id == orderItem.Product.Id &&
                o.Order.Id == orderItem.Order.Id))
            {
                throw new AppException("Already have OrderItem with same Order and Product");
            }

            unitOfWork.OrderRepo.CreateOrderItem(orderItem);

            unitOfWork.Complete();

            return orderItem;
        }

        public void DeleteOrderItem(int orderItemId)
        {
            OrderItem orderItem = unitOfWork.OrderRepo.OrderItems.SingleOrDefault(o => o.Id == orderItemId);
            if (orderItem != null)
            {
                unitOfWork.OrderRepo.DeleteOrderItem(orderItem);
                unitOfWork.Complete();
            }
        }

        private void DeleteOrderItemRange(IEnumerable<OrderItem> orderItems)
        {
            unitOfWork.OrderRepo.DeleteOrderItemRange(orderItems);
            unitOfWork.Complete();
        }
    }
}
