using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Data.Dtos
{
    public class ProductReadDto
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string CategoryName
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
