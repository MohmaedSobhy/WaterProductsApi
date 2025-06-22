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
    public class ProductsController : ControllerBase
    {

        private readonly IProductServices productServices;

        public ProductsController([FromForm]IProductServices productServices) { 
             this.productServices = productServices;
        }

        [HttpGet]
        public IActionResult getAllProduct(int pagenumber=1)
        {
            GeneralResponse generalResponse = new GeneralResponse { 
                success=true,
                message="you get All Product",
                data=productServices.allProducts(pagenumber),
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
    }
}
