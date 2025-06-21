using System.ComponentModel.DataAnnotations.Schema;

namespace WaterProducts.models
{
    public class FavouriteProducts
    {
        
        public int productId { get; set; }

        [ForeignKey("userId")]
        public string userId { get; set; }

        public ApplicationUser user { get; set; }
    }
}
