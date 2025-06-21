using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WaterProducts.services.admin_orders;
using WaterProducts.services.cart;
using WaterProducts.services.emial;
using WaterProducts.services.favourite;
using WaterProducts.services.product;
using WaterProducts.services.token;
using WaterProducts.services;
using WaterProducts.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WaterProducts.models;

namespace WaterProducts.extensions
{
    public static class ServicesExtensions
    {
        public static void IdentiyConfigure(this IServiceCollection services) {
           services.AddIdentity<ApplicationUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 8;
                        options.User.RequireUniqueEmail = true;
                        options.User.AllowedUserNameCharacters = null;
                        options.SignIn.RequireConfirmedAccount = false;
                        options.SignIn.RequireConfirmedEmail = false;


                    }).
                    AddEntityFrameworkStores<ApplicationData>();

        }

        public static void JWTConfigureAuthincation(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(
                options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).
                AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });
        }

        public static void ScopeServices(this IServiceCollection services)
        {
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddScoped<IFavouriteProducts, FavouriteProductsServices>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<IUserOrderServices, UserOrderServices>();
            services.AddScoped<IAdminOrderServices, AdminOrderServices>();
            services.AddScoped<IEmailSender, EmailSender>();
        }

        public static void DataBaseConfigure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationData>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }
    }
}
