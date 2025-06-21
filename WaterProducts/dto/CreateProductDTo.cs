using System.ComponentModel.DataAnnotations;

namespace WaterProducts.dto
{
    public class CreateProductDTo
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        public double price { get; set; }
        [Required]
        public int stockQuantiy { get; set; }

        [Required]
        public IFormFile imageFile { get; set; }
    }
}
