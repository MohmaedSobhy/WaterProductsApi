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

namespace WaterProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenerateToken generateToken;
        private readonly services.emial.IEmailSender emailSender;

        public AccountController(SignInManager<ApplicationUser> signInManager,services.emial.IEmailSender email, UserManager<ApplicationUser> userManager,IGenerateToken generateToken)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.generateToken = generateToken;
            this.emailSender = email;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto user)
        {
            GeneralResponse response = new GeneralResponse();
            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
            if (result.Succeeded) {

                ApplicationUser? userData = await userManager.FindByEmailAsync(user.Email);
                if (userData == null)
                {
                    return BadRequest(Result.Failure("Email or password wrong"));
                }
                 IList<string> roles = await userManager.GetRolesAsync(userData!);
                string token = await generateToken.getGenerateToken(userData!,roles);

                response.message = "You Login Succefully";
                response.success = true;
                response.data = new { token, name = userData.name, email = user.Email, role = roles[0]};
                return Ok(response);
            }
            response.success = false;
            response.message = "Email or password wrong";
            return BadRequest(response);
        }

        [HttpPost("Register")]
      
        public async Task<IActionResult> register(RegisterDto user)
        {
            GeneralResponse response = new GeneralResponse();

            ApplicationUser newUser=new ApplicationUser();
            newUser.Email = user.Email;
            newUser.name = user.userName;
            newUser.UserName = user.Email;
           
            var result = await userManager.CreateAsync(newUser, user.Password);
           
            if (result.Succeeded) {

                string token = await generateToken.getGenerateToken(newUser!, [userRoles.user]);
                userManager.AddToRoleAsync(newUser, userRoles.user).Wait();
                response.data=new { token, newUser.name,email=newUser.Email,role="user"};
                response.success = true;
                response.message = "You Register Scussfully";
                return Ok(response); 
            }
            var  errors = result.Errors.Where(e => !e.Code.Contains("UserName"));
            response.message = "Failed To Register";
            response.success = false;
            response.data = errors;
            return BadRequest(response);
            
        }

        [HttpPost("AddAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addAdmin(RegisterDto user)
        {
            GeneralResponse response = new GeneralResponse();
            ApplicationUser newUser = new ApplicationUser();
            newUser.Email = user.Email;
            newUser.name = user.userName;
            newUser.UserName = user.Email;

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {

                string token = await generateToken.getGenerateToken(newUser!, [userRoles.user]);
                userManager.AddToRoleAsync(newUser, userRoles.admin).Wait();
                response.data = new { token, newUser.name, email = newUser.Email };
                response.success = true;
                response.message = "New Admin Add Scussfully";
                return Ok(response);
            }
            var errors = result.Errors.Where(e => !e.Code.Contains("UserName"));
            response.message = "Failed To Add new Admin";
            response.success = false;
            response.data = errors;
            return BadRequest(response);
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

    }
}
