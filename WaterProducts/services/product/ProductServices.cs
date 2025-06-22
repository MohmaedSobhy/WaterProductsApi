using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using WaterProducts.data;
using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.product
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationData dataBase;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly string imagePath;
        private readonly IHttpContextAccessor httpContext;
        


        public ProductServices(ApplicationData dataBase, IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor)
        {
            this.dataBase = dataBase;
            this.hostEnvironment = host;
            this.imagePath = $"{ hostEnvironment.WebRootPath}/images";
            this.httpContext= httpContextAccessor;
           
        }

        public async Task<Result>  addProduct(CreateProductDTo product)
        {
            try
            {
                var coverName = $"{Guid.NewGuid()}{Path.GetExtension(product.imageFile.FileName)}";
                var path = Path.Combine(imagePath, coverName);

               using var stream = File.Create(path);
                await product.imageFile.CopyToAsync(stream);
                stream.Dispose();
                var newProduct = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    price = product.price,
                    imageUrl = coverName,
                    stockQuantiy = product.stockQuantiy

                };
                await dataBase.products.AddAsync(newProduct);
                dataBase.SaveChanges();

                return Result.Success(newProduct);
            }
            catch (Exception ex)
            {
                return Result.Failure("Check Product Details follow requirments");
            }
            
        }

        public List<Product> allProductsForAdmin(int page)
        {
            var results = dataBase.products.Where(p => p.stockQuantiy > 0).Skip((page - 1) * 5).Take(page * 5)
               .ToList();
            return results;
        }

        public List<UserProductDto> allProductsForUsers(int page)
        {
            var results = dataBase.products.Where(p=>p.stockQuantiy>0).Skip((page - 1) * 5).Take(page * 5)
                .Select(product => new UserProductDto
                {
                    productId = product.Id,
                    name = product.Name,
                    price = product.price,
                    description = product.Description,
                    imageUrl = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/images/{product.imageUrl}"
                }).ToList();
            return results ;
        }

        public async Task<Result> deleteProductById(int id)
        {
           Product? product=  getProductById(id);
            if (product != null) {
                dataBase.products.Remove(product);
                await dataBase.SaveChangesAsync();
                return Result.Success($"You Delete Product {id} success");
            }
            return Result.Failure("No Product Founds with This Id");
        }

        public Product? getProductById(int id)
        {
            return dataBase.products.FirstOrDefault(product => id == product.Id);
        }

        public Product? getProductByName(string name)
        {
            return dataBase.products.FirstOrDefault(product => name == product.Name);
        }

        public async Task<Result> getProductsOutOfStock()
        {
            var results = await dataBase.products.AsNoTracking().Where(p => p.stockQuantiy == 0)
                 .ToListAsync();

            return Result.Success(results);

        }

        public List<UserProductDto> searchForProducts(string searchKey)
        {
            var results = dataBase.products.Where(p => p.Name.Contains(searchKey))
                .Select(product => new UserProductDto
                {
                    productId = product.Id,
                    name = product.Name,
                    price = product.price,
                    description = product.Description,
                    imageUrl = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/images/{product.imageUrl}"
                }).ToList();
            return results;
        }

        public async Task<Result> updateProductById(int id,Product product)
        {
            if (id != product.Id)
            {
                return Result.Failure("Product Id Not Match");
            }
            dataBase.Entry(product).State = EntityState.Modified;
            await dataBase.SaveChangesAsync();
            return Result.Success(product);
        }

        
    }
}
