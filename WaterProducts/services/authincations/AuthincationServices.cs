using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WaterProducts.data;
using WaterProducts.dto;
using WaterProducts.models;
using WaterProducts.services.token;

namespace WaterProducts.services.authincations
{
    public class AuthincationServices : IAuthincationServices
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenServices tokenServices;
        private readonly ApplicationData database;

        public AuthincationServices(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITokenServices generateToken, ApplicationData database)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenServices = generateToken;
            this.database = database;
        }
        public async Task<Result> LoginServices(LoginDto user)
        {
            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
            if (!result.Succeeded)
            {
                return Result.Failure("User Name or Password Wrong");
            }

            ApplicationUser? userData = await userManager.FindByEmailAsync(user.Email);
            if (userData==null)
                return Result.Failure("User Name or Password Wrong");

            IList<string> roles = await userManager.GetRolesAsync(userData!);
            string token = await tokenServices.getGenerateToken(userData!, roles);
            string refreshToken = await tokenServices.getNewRefreshToken(userData.Id);


            return Result.Success(new { token, name = userData.name, email = user.Email, role = roles[0],refreshToken });


        }

        public async Task<Result> RegisterServices(RegisterDto user, string role = userRoles.user)
        {
            ApplicationUser newUser = new ApplicationUser();
            newUser.Email = user.Email;
            newUser.name = user.userName;
            newUser.UserName = user.Email;

            var result = await userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                string token = await tokenServices.getGenerateToken(newUser!, [userRoles.user]);
                string refreshToken = await tokenServices.getNewRefreshToken(newUser.Id);
                userManager.AddToRoleAsync(newUser, userRoles.user).Wait();
                return Result.Success(new { token, newUser.name, email = newUser.Email, role ,refreshToken});
            }

            return Result.Failure(result.Errors.Where(e => !e.Code.Contains("UserName")));
        }

        public async Task<Result> generateNewToken(string refreshToken)
        {
            var refreshTokenData = await database.refreshTokens.FirstOrDefaultAsync(token=>token.Token==refreshToken);

            if(refreshTokenData == null)
               return Result.Failure("Refresh Token Not Found");

            if (refreshTokenData.Expiration < DateTime.UtcNow)
                return Result.Failure("Refresh Token Expired");

            var user = await userManager.FindByIdAsync(refreshTokenData.UserId);
            var roles= await userManager.GetRolesAsync(user!);
            string token = await tokenServices.getGenerateToken(user!,roles);

            return Result.Success(token);

        }
    }
}
