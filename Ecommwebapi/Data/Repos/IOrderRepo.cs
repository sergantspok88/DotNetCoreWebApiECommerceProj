using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public interface IOrderRepo
    {
        public IQueryable<Order> Orders
        {
            get;
        }
        void CreateOrder(Order o);
        void DeleteOrder(Order o);
        void SaveOrder(Order o);

        public IQueryable<OrderItem> OrderItems
        {
            get;
        }
        void CreateOrderItem(OrderItem o);
        void DeleteOrderItem(OrderItem o);
        void DeleteOrderItemRange(IEnumerable<OrderItem> o);
        void SaveOrderItem(OrderItem o);
    }
}
