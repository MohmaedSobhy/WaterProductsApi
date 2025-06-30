using WaterProducts.data;
using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.authincations
{
    public interface IAuthincationServices
    {
        public Task<Result> LoginServices(LoginDto user);

        public Task<Result> RegisterServices(RegisterDto user,string role=userRoles.user);

        public Task<Result> generateNewToken(string refreshToken);


    }
}
