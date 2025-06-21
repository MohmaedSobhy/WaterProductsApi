using WaterProducts.models;

namespace WaterProducts.models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string Address { get; set; }=string.Empty;

        public string City { get; set;} = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Type {  get; set; }=string.Empty;

        public string UserId { get; set; }=string.Empty;

        public double totalPrice { get; set; }
        public ApplicationUser user { get; set; }

        public List<ProductOrder> Products { get; set; }=new List<ProductOrder>();
    }
}
