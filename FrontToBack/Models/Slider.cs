﻿using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }
        [MaxLength(100), Required]
        public string Description { get; set; }
        [Required]
        public int Offer { get; set; }

        public string Image { get; set; }
    }
}
