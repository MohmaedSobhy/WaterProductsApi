
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using WaterProducts.data;
using WaterProducts.extensions;
using WaterProducts.models;
using WaterProducts.services;
using WaterProducts.services.admin_orders;
using WaterProducts.services.cart;
using WaterProducts.services.emial;
using WaterProducts.services.favourite;
using WaterProducts.services.product;
using WaterProducts.services.token;

namespace WaterProducts
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

    
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.DataBaseConfigure(builder.Configuration);
            builder.Services.ScopeServices();

            builder.Services.Configure<GmailOptions>(
               builder.Configuration.GetSection(GmailOptions.GmailOptionsKey));

            builder.Services.Configure<JwtOptions>(
              builder.Configuration.GetSection("Jwt"));

            builder.Services.IdentiyConfigure();
            builder.Services.JWTConfigureAuthincation(builder.Configuration);
            builder.Services.SetCors();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAllPolicy");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication(); 

            app.UseAuthorization();  

            


            app.MapControllers();
           
            app.Run();
        }
    }
}
