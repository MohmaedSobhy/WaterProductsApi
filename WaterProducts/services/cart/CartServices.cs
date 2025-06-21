using Microsoft.EntityFrameworkCore;
using WaterProducts.data;
using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.cart
{
    public class CartServices : ICartServices
    {
        private readonly ApplicationData dataBase;

        public CartServices(ApplicationData dataBase) { 
            this.dataBase = dataBase;
        }

        public async Task<Result> addProductToCart(int ProductId, string UserId)
        {
            var cart = await dataBase.carts.FirstOrDefaultAsync(c => c.userId == UserId);

            if (cart == null)
            {
                cart = new Cart { userId = UserId };
                dataBase.carts.Add(cart!);
                await dataBase.SaveChangesAsync();
            }

            
            var productExist = cart.products.Any(p=>p.productId == ProductId);
            if (productExist)
            {
                return Result.Failure("The Product Already Exist in Your Cart List");
            }
            
            var product= dataBase.products.FirstOrDefault(p=>p.Id == ProductId);
            if (product == null) {
                return Result.Failure("No Products Found With this Id");
            }
            cart.products.Add(new ProductCart
            {
                productId = ProductId,
                productQuantity=1,
                totalPrice=product.price,
            });

            await dataBase.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> clearUserCart(string UserId)
        {
            var user = await dataBase.users
                   .Include(u => u.cart)
                   .ThenInclude(c => c.products)
                   .FirstOrDefaultAsync(u => u.Id == UserId)!;

            if (user == null)
                 return Result.Failure("you not Authrized");
            
            user.cart.products.Clear();
           
            await dataBase.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> getUserCart(string UserId)
        {
            var cart = await dataBase.carts.FirstOrDefaultAsync(c => c.userId == UserId);

            if (cart == null)
            {
                cart = new Cart { userId = UserId };
                dataBase.carts.Add(cart!);
                await dataBase.SaveChangesAsync();
                return Result.Success(cart);
            }
           
            var user = await dataBase.users
                   .Include(u => u.cart)
                   .ThenInclude(c => c.products)       
                   .ThenInclude(ci => ci.product)     
                   .FirstOrDefaultAsync(u => u.Id == UserId)!;


            return Result.Success(user.cart);
        }

        public async Task<Result> removeProductFromCart(int productId, string UserId)
        {
            var cart = await dataBase.carts.FirstOrDefaultAsync(c => c.userId == UserId);

            if (cart == null)
                return Result.Failure("Create Cart First");
          
            await dataBase.Entry(cart).Collection(c => c.products).LoadAsync();

            var productDb = cart.products.FirstOrDefault(p=>p.productId == productId);

            if (productDb == null)
                return Result.Failure("No Product Found in Your Cart");

            cart.products.Remove(productDb);
            await dataBase.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> updateProductCart(ProductQuantityDto product, string UserId)
        {
           

            var productDb= await dataBase.products.FirstOrDefaultAsync(p=>p.Id==product.productId);

            if (productDb == null)
              return Result.Failure("No Product Found");

            if (product.quantity <= 0)
                return Result.Failure("The Quantity Not Valid");

           var Cart=  await dataBase.carts
                    .FirstOrDefaultAsync(u => u.userId==UserId);

            if (Cart == null)
                return Result.Failure("Create Cart First");

            dataBase.Entry(Cart).Collection(c => c.products).Load();
            var cartProduct= Cart.products.FirstOrDefault(p=>p.productId==product.productId);



            if (cartProduct == null)
                return Result.Failure("should Add Product To your List");

            cartProduct.productQuantity=product.quantity;
            cartProduct.totalPrice = product.quantity * productDb.price;
            await dataBase.SaveChangesAsync();
            ProductCartDto productDto = new ProductCartDto
            {
                quantity = cartProduct.productQuantity,
                productName = productDb.Name,
                productId = productDb.Id,
                price = productDb.price
            };
            return Result.Success(productDto);
        }

        
    }
}
