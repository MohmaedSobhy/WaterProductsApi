using Microsoft.EntityFrameworkCore;
using WaterProducts.data;
using WaterProducts.models;

namespace WaterProducts.services.favourite
{
    public class FavouriteProductsServices : IFavouriteProducts
    {
        private readonly ApplicationData dataBase;

        public FavouriteProductsServices(ApplicationData dataBase)
        {
            this.dataBase= dataBase;
        }

        public async Task<bool> addProductToFavourite(int productId, string userId)
        {
            var existing = await dataBase.favourites
            .AnyAsync(f => f.productId == productId && f.userId == userId);

            if (existing)
                return false;
            

            await dataBase.favourites.AddAsync(new FavouriteProducts { productId=productId,userId=userId});
            await dataBase.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>>  getUserFavouriteProduct(string userId)
        {
            var favouriteProducts=  dataBase.favourites.Where(f=>f.userId == userId).Select(f=>f.productId).ToList();
            List<Product> products = dataBase.products.Where(p => favouriteProducts.Contains(p.Id)).ToList();

            return products;
        }

        public async Task<bool> removeProductFromFavourite(int productId, string userId)
        {
            var existing = await dataBase.favourites
                 .AnyAsync(f => f.productId == productId && f.userId == userId);

            if (!existing)
                return false;

            FavouriteProducts product=  dataBase.favourites.
                FirstOrDefault(f => f.productId == productId && f.userId == userId)!;

            dataBase.favourites.Remove(product);
            dataBase.SaveChanges();
            return true ;
        }
    }
}
