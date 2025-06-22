using WaterProducts.models;

namespace WaterProducts.services.favourite
{
    public interface IFavouriteProducts
    {
        public Task<List<Product>> getUserFavouriteProduct(string userId);

        public Task<Result> addProductToFavourite(int productId,string userId);

        public Task<Result> removeProductFromFavourite(int productId,string userId);
    }
}
