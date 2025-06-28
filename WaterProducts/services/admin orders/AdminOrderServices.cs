using Microsoft.EntityFrameworkCore;
using WaterProducts.data;
using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services.admin_orders
{
    public class AdminOrderServices:IAdminOrderServices
    {
        private readonly ApplicationData dataBase;  

        public AdminOrderServices(ApplicationData dataBase) 
        {
            this.dataBase = dataBase;
        }

        public async Task<Result> allOrdersByType(string orderType)
        {
            var waitingOrders = dataBase.orders.AsNoTracking().Include(u=>u.user).Where(order => order.Type == orderType).ToList();
            var result = waitingOrders.Select(order => new OrderDetailsDto()
            {
                address = order.Address,
                city = order.City,
                phonenumber = order.Phone,
                Id = order.OrderId,
                userEmail=order.user.Email!,
                name=order.user.name,
                totalPriced = order.totalPrice
            });
            return Result.Success(result);
        }

       
        public async Task<Result> changeOrderType(UpdateOrderTypeDto orderUpdate)
        {
            var order = await dataBase.orders.FirstOrDefaultAsync(o => orderUpdate.orderId == o.OrderId);
            if (order == null)
            {
                return Result.Failure("No Order Found with This id");
            }
            if (orderUpdate.orderType.ToLower() == OrderTypes.waiting.ToLower())
            {
                order.Type = OrderTypes.waiting;
            }
            else if (orderUpdate.orderType.ToLower() == OrderTypes.deliverd.ToLower())
            {
                order.Type = OrderTypes.deliverd;
            }
            else if (orderUpdate.orderType.ToLower() == OrderTypes.progress.ToLower())
            {
                order.Type = OrderTypes.progress;
            }
            else
            {
                return Result.Failure("Invalid Order Type");
            }
            dataBase.SaveChanges();
            return Result.Success(order);
        }

      
      
    }
}
