using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Dtos
{
    public class CartItemReadDto
    {
        public int Id
        {
            get; set;
        }
        public ProductReadDto Product
        {
            get; set;
        }
        //public int ProductId
        //{
        //    get; set;
        //}
        //public UserReadDto User
        //{
        //    get; set;
        //}
        public int UserId
        {
            get; set;
        }
        public int Quantity
        {
            get; set;
        }
    }
}
