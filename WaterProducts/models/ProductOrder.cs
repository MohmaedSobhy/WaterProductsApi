using System.ComponentModel.DataAnnotations.Schema;

namespace WaterProducts.models
{
    public class ProductOrder
    {
        public int Id { get; set; }

        [ForeignKey("ProductId")]
        public int productId { get; set; }

        public Product product { get; set; }

        public int productQuantity { get; set; }

        public double price { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
    }
}
