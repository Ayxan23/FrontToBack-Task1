using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Areas.Admin.ViewModels
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }
        [MaxLength(100), Required]
        public string Description { get; set; }
        [Required]
        public int Offer { get; set; }

        public IFormFile? Image { get; set; }
    }
}
