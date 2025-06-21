namespace WaterProducts.dto
{
    public class ProductCartDto
    {
        public int productId {  get; set; }
        public string productName { get; set; } = string.Empty;

        public double price { get; set; }

        public int quantity { get; set; }

    }
}
