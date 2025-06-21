using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WaterProducts.models;
using WaterProducts.services.favourite;

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {

        private readonly IFavouriteProducts favouriteProducts;

        public FavouriteController(IFavouriteProducts favourite)
        {
            this.favouriteProducts = favourite;
        }

        [HttpGet]
        [Authorize]
        public  async Task<IActionResult> getAllFavouriteProduct()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            GeneralResponse response = new GeneralResponse();

            response.success = true;
            response.message = "All Your Favourite Products";
            response.data = await favouriteProducts.getUserFavouriteProduct(userId:userIdClaim!);
            

            return Ok(response);
        }


        [HttpPost("{productId:int}")]
        public async Task<IActionResult> addProductToFavourite(int productId)
        {
            GeneralResponse response = new GeneralResponse();
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool add = await favouriteProducts.addProductToFavourite(productId, userIdClaim!);
            if (add == false)
            {
                response.success = false;
                response.message = "Failed To Add Product Favourite";
                return BadRequest(response);
            }
            response.success = true;
            response.message = "You Add Product to your List";
            return Ok(response);
        }


        [HttpDelete("{productId:int}")]
        [Authorize]
        public async Task<IActionResult> deleteFavouriteProduct(int productId)
        {
            GeneralResponse response = new GeneralResponse();
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool add = await favouriteProducts.removeProductFromFavourite(productId, userIdClaim!);
            if (add == false) {
                response.success = false;
                response.message = "Your Product Not in List";
                response.data = "No Products";
                return BadRequest(response);
            }
            response.success = true;
            response.message = "You Remove Product From your Favourite List";
            response.data = "Product Remove";
            return Ok(response);
        }



    }
}
