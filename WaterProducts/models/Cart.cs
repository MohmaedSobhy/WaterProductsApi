using System.ComponentModel.DataAnnotations.Schema;

namespace WaterProducts.models
{
    [Table("Cart")]
    public class Cart
    {
        [ForeignKey("userId")]
        public string userId { get; set; } = default!;
        public ApplicationUser user { get; set; }
        public int CartId { get; set; }
        
        public List<ProductCart> products { get; set; }= new List<ProductCart>();
    }
}
