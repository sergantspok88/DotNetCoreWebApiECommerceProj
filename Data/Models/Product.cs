using System.ComponentModel.DataAnnotations;

namespace ecommwebapi.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}