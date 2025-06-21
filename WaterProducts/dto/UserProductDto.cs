using System.ComponentModel.DataAnnotations;

namespace WaterProducts.dto
{
    public class UserProductDto
    {
        public int productId { get; set; }

        
        
        public string name { get; set; }

      
        public string description { get; set; }

        
        public double price { get; set; }

        public string imageUrl { get; set; }

    }
}
