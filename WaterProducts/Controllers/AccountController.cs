using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services.token;
using WaterProducts.services.emial;
using WaterProducts.data;
using Azure;
using Microsoft.EntityFrameworkCore;
using WaterProducts.Filters;
using Microsoft.AspNetCore.Authorization;
using WaterProducts.services.authincations;

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private readonly UserManager<ApplicationUser> userManager;
       
        private readonly IAuthincationServices authincationServices;
       

        public AccountController(IAuthincationServices authincation, UserManager<ApplicationUser> userManager)
        {
           
            this.userManager = userManager;
         
            this.authincationServices = authincation;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto user)
        {
            
            var result = await authincationServices.LoginServices(user);
            if (result.IsSuccess) {
                GeneralResponse response = new GeneralResponse();
                response.message = "You Login Succefully";
                response.success = true;
                response.data = result.data;
                return Ok(response);
            }

            return BadRequest(result);
        }

        [HttpPost("Register")]
      
        public async Task<IActionResult> register(RegisterDto user)
        {
            GeneralResponse response = new GeneralResponse();
            var result = await authincationServices.RegisterServices(user);

            if (result.IsSuccess)
            {
                response.success = true;
                response.data= result.data;
                response.message = "You Register Succefully";
                return Ok(response);
            }
            return BadRequest(result);
        }

        [HttpPost("AddAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addAdmin(RegisterDto user)
        {

            GeneralResponse response = new GeneralResponse();
            var result = await authincationServices.RegisterServices(user, userRoles.admin);

            if (result.IsSuccess)
            {
                response.success = true;
                response.data = result.data;
                response.message = "You Register Succefully";
                return Ok(response);
            }
            return BadRequest(result);
        }

        [HttpGet("AllAdmins")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAllAdmins()
        {
            
            GeneralResponse response = new GeneralResponse();

            var admins = new List<ApplicationUser>();
            var allUsers= userManager.Users.ToList();
             foreach (var user in allUsers)
            {
                if (await userManager.IsInRoleAsync(user, "Admin"))
                {
                    admins.Add(user);
                }
            }

            var result = admins.Select(user => new { user.Id,user.name,user.Email});
            response.success = true;
            response.data = result;
            response.message = "All Admins";    

            return Ok(response);
        }


        [HttpPost("RefreshToken")]
        public async Task<IActionResult> getNewToken([FromBody] string refreshToken)
        {
            var result = await authincationServices.generateNewToken(refreshToken);
            if (result.IsSuccess)
            {
                GeneralResponse response = new GeneralResponse();
                response.success = true;
                response.data = result.data;
                response.message = "You Get New Token Succefully";
                return Ok(response);
            }
            return Unauthorized("Refresh Token Expire");
        }

    }
}
