using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommwebapi.Data.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get; set;
        }
        public Product Product
        {
            get; set;
        }
        public Order Order
        {
            get; set;
        }
        public int Quantity
        {
            get; set;
        }
    }
}