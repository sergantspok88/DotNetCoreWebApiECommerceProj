using System.ComponentModel.DataAnnotations;

namespace ecommwebapi.Data.Dtos
{
    public class UserRegisterWriteDto
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}