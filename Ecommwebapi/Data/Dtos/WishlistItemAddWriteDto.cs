using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Dtos
{
    public class WishlistItemAddWriteDto
    {
        //public int Id
        //{
        //    get; set;
        //}
        [Required]
        public int ProductId
        {
            get; set;
        }
        //[Required]
        public int UserId
        {
            get; set;
        }
    }
}
