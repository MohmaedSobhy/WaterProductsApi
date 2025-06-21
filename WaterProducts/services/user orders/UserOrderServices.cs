using Microsoft.EntityFrameworkCore;
using WaterProducts.data;
using WaterProducts.dto;
using WaterProducts.models;

namespace WaterProducts.services
{
    public class UserOrderServices : IUserOrderServices
    {
        private readonly ApplicationData dataBase;
        public UserOrderServices(ApplicationData dataBase)
        {
            this.dataBase = dataBase;
        }

       

        public async Task<Result> createOrder(string UserId,CreateOrderDto createOrder)
        {
            var cart= dataBase.carts.FirstOrDefault(c=>c.userId==UserId);
            if (cart == null)
                return Result.Failure("Create your cart first");

            await dataBase.Entry(cart).Collection(c=>c.products).LoadAsync();

            if (cart.products.Count()==0)
                return Result.Failure("Add Product To your Cart");


            Order order = new Order();
            order.UserId = UserId;
            order.Address = createOrder.Address;
            order.City = createOrder.City;
            order.Phone = createOrder.Phone;
            order.Type = OrderTypes.waiting;
            order.totalPrice= cart.products.Sum(c=>c.totalPrice);

            

            foreach (ProductCart product in cart.products) {
               
                order.Products.Add(new ProductOrder {
                    productId=product.productId,
                    productQuantity=product.productQuantity,
                    price=product.totalPrice,
                });
            }

            dataBase.orders.Add(order);
            await dataBase.SaveChangesAsync();
            cart.products.Clear();
            dataBase.carts.Update(cart);
            await dataBase.SaveChangesAsync();

            return Result.Success(new
            {
                order.OrderId,
                order.Products,
            });
        }

        

        public async Task<Result> userOrders(string UserId, string OrderType )
        {
            var user = dataBase.users.FirstOrDefault(c => c.Id == UserId);
            if (user == null)
                return Result.Failure("No Authried");

            await dataBase.Entry(user).Collection(u => u.orders).LoadAsync();
            var orders = user.orders;
            if (orders == null || orders.Count()==0)
                return Result.Failure("Start Creating Orders");

            List<Order> waitingOrders = orders.Where(o=>o.Type==OrderType).ToList();

            var result = waitingOrders.Select(order => new OrderDetailsDto()
            {
                address = order.Address,
                city = order.City,
                phonenumber = order.Phone,
                Id = order.OrderId,
                userEmail = user.UserName!,
                totalPriced = order.totalPrice
            });

            return Result.Success(result);

        }


    }
}
