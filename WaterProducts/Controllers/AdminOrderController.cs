using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services.admin_orders;

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminOrderController : ControllerBase
    {
        private readonly IAdminOrderServices orderServices;

        public AdminOrderController(IAdminOrderServices adminOrderServices)
        {
            this.orderServices = adminOrderServices;
        }

        [HttpPost("UpdateOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> changeOrderType(UpdateOrderTypeDto order)
        {
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.changeOrderType(order);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Waiting Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        [HttpGet("Waiting")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllWaitingOrderInSystem()
        {
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.allOrdersByType(OrderTypes.waiting);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Waiting Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        [HttpGet("Progress")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllProgressOrderInSystem()
        {
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.allOrdersByType(OrderTypes.progress);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Waiting Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

        [HttpGet("Deliverd")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllDeliverdOrders()
        {
            GeneralResponse response = new GeneralResponse();
            Result result = await orderServices.allOrdersByType(OrderTypes.deliverd);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            response.success = true;
            response.message = "You Get All Deliverd Order Succefully";
            response.data = result.data;
            return Ok(response);
        }

    }
}
