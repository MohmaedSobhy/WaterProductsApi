using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services.product;

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {

        private readonly IProductServices productServices;

        public ProductsController([FromForm]IProductServices productServices) { 
             this.productServices = productServices;
        }

        [HttpGet("user")]
        [AllowAnonymous]
        public IActionResult getAllUserProduct(int pagenumber=1)
        {
            GeneralResponse generalResponse = new GeneralResponse { 
                success=true,
                message="you get All Product",
                data=productServices.allProductsForUsers(pagenumber),
            };
           
            return Ok(generalResponse);

        }

        [HttpGet("Admin")]
        public IActionResult getAllProductForAdmin(int pagenumber = 1)
        {
            GeneralResponse generalResponse = new GeneralResponse
            {
                success = true,
                message = "you get All Product",
                data = productServices.allProductsForAdmin(pagenumber),
            };

            return Ok(generalResponse);

        }

        [HttpPost]
        public async Task<IActionResult> addProduct(CreateProductDTo product) {

          var result = await productServices.addProduct(product);
            if (result.IsSuccess)
            {
                GeneralResponse generalResponse = new GeneralResponse
                {
                    success = true,
                    message = "you create product Successfully",
                    data = result.data,
                };

                return Ok(generalResponse);
            }
            
            return BadRequest(result);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deleteProduct(int id) {

            var result = await productServices.deleteProductById(id);
            
            if (result.IsSuccess == false)
            {
                
                return BadRequest(result);
            }
            

            return Ok(result);
        }

        [HttpGet("{searchKey:alpha}")]
        public IActionResult searchForProducts(string searchKey) {

            var results =  productServices.searchForProducts(searchKey);
            GeneralResponse response = new GeneralResponse
            {
                success = true,
                message = $"you get All Product for {searchKey}",
                data = results,
            };
            return Ok(response);
        }

       

        [HttpGet("ProductsOutStock")]
        public async Task<IActionResult> productsOutOfStock()
        {
            GeneralResponse response = new GeneralResponse();
            var result = await productServices.getProductsOutOfStock();
            if (result.IsSuccess)
            {
                response.success = true;
                response.message = "You Get All Products Out Of Stock";
                response.data = result.data;
                return Ok(response);
            }


            return BadRequest(result);

        }


       
    }
}
