using WaterProducts.models;

namespace WaterProducts.services.favourite
{
    public interface IFavouriteProducts
    {
        public Task<List<Product>> getUserFavouriteProduct(string userId);

        public Task<bool> addProductToFavourite(int productId,string userId);

        public Task<bool> removeProductFromFavourite(int productId,string userId);
    }
}
