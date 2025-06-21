using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using WaterProducts.models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WaterProducts.services.token
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration configuration;

        public GenerateToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> getGenerateToken(ApplicationUser user,IList<string> userRoles)
        {

            List<Claim> userClaims = new List<Claim>();

            userClaims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.name));
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email!));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach (string role in userRoles) {
                userClaims.Add(new Claim(ClaimTypes.Role,role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            SigningCredentials signing = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                claims: userClaims,
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                signingCredentials: signing,
                expires: DateTime.Now.AddDays(2)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
