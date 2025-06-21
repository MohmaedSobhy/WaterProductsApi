using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.cart
{
    public interface ICartServices
    {
        public Task<Result> getUserCart(string UserId);

        public Task<Result> addProductToCart(int ProductId, string UserId);

        public Task<Result> updateProductCart(ProductQuantityDto product, string UserId);

        public Task<Result> removeProductFromCart(int ProductId, string UserId);

        public Task<Result> clearUserCart(string UserId);
    }
}
