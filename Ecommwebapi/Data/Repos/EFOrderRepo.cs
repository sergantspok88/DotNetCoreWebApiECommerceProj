using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public class EFOrderRepo : IOrderRepo
    {
        private IDataContext ctx;

        public EFOrderRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<Order> Orders => ctx.Orders;
        public IQueryable<OrderItem> OrderItems => ctx.OrderItems;

        public void CreateOrder(Order o)
        {
            ctx.Orders.Add(o);
        }

        public void CreateOrderItem(OrderItem o)
        {
            ctx.OrderItems.Add(o);
        }

        public void DeleteOrder(Order o)
        {
            ctx.Orders.Remove(o);
        }

        public void DeleteOrderItem(OrderItem o)
        {
            ctx.OrderItems.Remove(o);
        }

        public void DeleteOrderItemRange(IEnumerable<OrderItem> o)
        {
            ctx.OrderItems.RemoveRange(o);
        }

        public void SaveOrder(Order o)
        {
            ctx.SaveChanges();
        }

        public void SaveOrderItem(OrderItem o)
        {
            ctx.SaveChanges();
        }
    }
}
