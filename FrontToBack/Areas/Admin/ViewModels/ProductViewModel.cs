using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace FrontToBack.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [Range(1, 5)]
        public int Raiting { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
    }
}
