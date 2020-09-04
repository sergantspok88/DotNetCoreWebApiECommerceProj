using System;
using System.Collections.Generic;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Data.Models
{
    public class Order
    {
        public int Id
        {
            get; set;
        }
        public DateTime OrderDate
        {
            get; set;
        }
        //kind of like id but for user - not internal representation
        public string OrderNumber
        {
            get; set;
        }
        //public StoreUser User { get; set; }
        public User User
        {
            get; set;
        }
        public ICollection<OrderItem> Items
        {
            get; set;
        }
    }
}