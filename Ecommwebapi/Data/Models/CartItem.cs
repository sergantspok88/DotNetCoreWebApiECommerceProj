using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Models
{
    public class CartItem
    {
        public int Id
        {
            get; set;
        }
        public Product Product
        {
            get; set;
        }
        public User User
        {
            get; set;
        }
        public int Quantity
        {
            get; set;
        }
    }
}
