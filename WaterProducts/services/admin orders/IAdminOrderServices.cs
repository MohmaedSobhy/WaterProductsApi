using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.admin_orders
{
    public interface IAdminOrderServices
    {
        public Task<Result> changeOrderType(UpdateOrderTypeDto orderUpdate);
        public Task<Result> allOrdersByType(string orderType);
    }
}
