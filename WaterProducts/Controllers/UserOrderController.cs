using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services;

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserOrderController : ControllerBase
    {

        private readonly IUserOrderServices orderServices;

        public UserOrderController(IUserOrderServices orderServices) { 
        
         this.orderServices = orderServices;
        
        }

        [HttpPost]
        public async Task<IActionResult> createOrder(CreateOrderDto order)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.createOrder(userIdClaim!, order);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Create Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        [HttpGet(OrderTypes.waiting)]
        
        public async Task<IActionResult> getAllWaitingOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.userOrders(userIdClaim!,OrderTypes.waiting);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Waiting Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        [HttpGet(OrderTypes.deliverd)]
      
        public async Task<IActionResult> getAllDeliverdOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.userOrders(userIdClaim!, OrderTypes.deliverd);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Deliverd Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        [HttpGet(OrderTypes.progress)]
        public async Task<IActionResult> getAllOrderInProgress()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.userOrders(userIdClaim!, OrderTypes.progress);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Progress Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        
    }
}
