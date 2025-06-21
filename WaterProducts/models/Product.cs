using System.ComponentModel.DataAnnotations;

namespace WaterProducts.models
{
    public class Product
    {
        public int Id { get; set; }

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
        public string imageUrl { get; set; } = string.Empty;
    }
}
