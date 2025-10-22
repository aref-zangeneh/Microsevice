using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!await orderContext.Orders.AnyAsync())
            {
                await orderContext.Orders.AddRangeAsync(GetPreConfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("data seen section configured");
            }
        }


        public static IEnumerable<Order> GetPreConfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    FirstName = "Aref",
                    LastName = "Zangeneh",
                    Username = "aref",
                    Email = "aref@gmail.com",
                    City = "Tehran",
                    Country = "Iran",
                    TotalPrice = 10000
                }
            };
        }
    }
}
