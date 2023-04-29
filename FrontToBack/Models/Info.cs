using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class Info
    {
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }
        [MaxLength(100), Required]
        public string Description { get; set; }
        [MaxLength(100), Required]
        public string Image { get; set; }
    }
}
