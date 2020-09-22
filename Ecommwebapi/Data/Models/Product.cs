using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommwebapi.Data.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get; set;
        }
        [Required]
        public string Name
        {
            get; set;
        }
        public Category Category
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public float Price
        {
            get; set;
        }
    }
}