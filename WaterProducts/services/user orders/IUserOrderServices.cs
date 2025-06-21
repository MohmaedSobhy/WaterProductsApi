using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services
{
    public interface IUserOrderServices
    {
        public Task<Result> createOrder(string UserId,CreateOrderDto order);
       
        public Task<Result> userOrders(string userId,string OrderType);
 
    }
}
