using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services.token;
using WaterProducts.services.emial;

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

                ApplicationUser userData = await userManager.FindByEmailAsync(user.Email);
                 IList<String> roles = await userManager.GetRolesAsync(userData!);
                string token = await generateToken.getGenerateToken(userData!,roles);

                response.message = "You Login Succefully";
                response.success = true;
                bool answe =await  emailSender.SendEmailAsync();
                if (answe == true)
                {
                    response.message = "You Login Succefully and Email Not send";
                }
                
                    response.message = "You Login Succefully but Email Not Sent";
                    response.data = new { token, name = userData.name, email = user.Email };
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

                string token = await generateToken.getGenerateToken(newUser!, []);
                response.data=new { token, newUser.name,email=newUser.Email};
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
    }
}
