using WaterProducts.models;

namespace WaterProducts.services.token
{
    public interface IGenerateToken
    {
        public  Task<string> getGenerateToken(ApplicationUser user,IList<string> userRoles);
    }
}
