using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Dtos
{
    public class OrderReadDto
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
        public int UserId
        {
            get; set;
        }
        public ICollection<OrderItemReadDto> Items
        {
            get; set;
        }
    }
}
