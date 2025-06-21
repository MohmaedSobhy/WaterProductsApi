using System.ComponentModel.DataAnnotations.Schema;

namespace WaterProducts.models
{
    public class ProductCart
    {
        public int Id { get; set; }

        [ForeignKey("ProductId")]
        public int productId { get; set; }

        public Product product { get; set; }

        public double totalPrice { get; set; }
        public int productQuantity { get; set; }

        [ForeignKey("CartId")]
        public int CartId { get; set; }
    }
}
