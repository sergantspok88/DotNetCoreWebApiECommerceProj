using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Models
{
    public class WishlistItem
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
        //public DateTime DateTimeOfWishlist
        //{
        //    get; set;
        //}
    }
}
