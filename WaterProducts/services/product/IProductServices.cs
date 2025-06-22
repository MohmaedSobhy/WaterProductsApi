using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.product
{
    public interface IProductServices
    {
        public List<UserProductDto> allProductsForUsers(int page);
        public List<Product> allProductsForAdmin(int page);
        public Product? getProductById(int id);

        public Product? getProductByName(string name);

        public Task<Result> deleteProductById(int id);

        public Task<Result> updateProductById(int id,Product product);

        public Task<Result> addProduct(CreateProductDTo product);

        public List<UserProductDto> searchForProducts(string searchKey);

        public Task<Result> getProductsOutOfStock();
    }
}
