using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.product
{
    public interface IProductServices
    {
        public List<UserProductDto> allProducts(int page);

        public Product? getProductById(int id);

        public Product? getProductByName(string name);

        public Task<Result> deleteProductById(int id);

        public bool updateProductById(Product product);

        public Task<Result> addProduct(CreateProductDTo product);

        public List<UserProductDto> searchForProducts(string searchKey);
    }
}
