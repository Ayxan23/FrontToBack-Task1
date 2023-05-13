using FrontToBack.Models.Common;

namespace FrontToBack.Models
{
    public class Product : BaseEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public double Price { get; set; }
        public int Raiting { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}
