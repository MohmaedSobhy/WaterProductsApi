using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services.cart;

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartServices cartServices;

        public CartController(ICartServices cartServices) { 
          this.cartServices = cartServices;
        }

        [HttpGet]
        public async Task<IActionResult> getAllProductsInCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();
            
            Result result = await cartServices.getUserCart(userIdClaim!);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            Cart userCart = result.data;
            response.message = "your Cart";
            response.success = true;
                    
            List<ProductCartDto> producstCart = new List<ProductCartDto>();

            foreach (var item in userCart.products) {

                producstCart.Add(new ProductCartDto { 
                           price=item.product.price,
                           productId=item.productId, 
                           productName=item.product.Name,
                           quantity=item.productQuantity,
                });
            
            }
            response.data = producstCart;
            return Ok(response);
        }

        [HttpPost("{productId:int}")]
        public async Task<IActionResult> addProducttoCart(int productId) {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();

            Result result= await cartServices.addProductToCart(productId, userIdClaim!);

            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.data = productId;
            response.message = "The product Add To your Cart";
            return Ok(response);
        }


        [HttpPut]
        public async Task<IActionResult> updateProductQuantity(ProductQuantityDto productQuantity)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();

            Result result = await cartServices.updateProductCart(productQuantity, userIdClaim!);

            if (result.IsSuccess==false)
            {
                return BadRequest(result);
            }

            response.success= true;
            response.message = "You update Product Quantity Success";
            response.data = result.data;

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> deleteAllProductsFromCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();

            Result result = await cartServices.clearUserCart(userIdClaim!);
            if (result.IsSuccess == false) { 
            
              return BadRequest(result);
            }
            response.success= true;
            response.message = "All Product in Cart Delete";
            response.data = new List<int>();
            return Ok(response);
        }


        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> deleteProductFromCart (int productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();

            Result result = await cartServices.removeProductFromCart(productId,userIdClaim!);
            if (result.IsSuccess == false)
            {

                return BadRequest(result);
            }
            response.success = true;
            response.message = "you delete Product from Cart";
            response.data = new List<int>();
            return Ok(response);
        }




    }
}
