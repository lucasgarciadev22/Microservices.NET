using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetOrders());
            await orderContext.SaveChangesAsync();

            logger.LogInformation($"Ordering Database: {nameof(OrderContext)} seeded.");
        }
    }

    private static IEnumerable<Order> GetOrders()
    {
        return
        [
            new()
            {
                UserName = "lucas",
                FirstName = "Lucas",
                LastName = "Garcia",
                EmailAddress = "lucasgarcia@eshop.net",
                AddressLine = "Santa Catarina",
                Country = "Brasil",
                TotalPrice = 750,
                State = "KA",
                ZipCode = "560001",
                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "lucas",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "lucas",
                LastModifiedDate = new DateTime(),
            }
        ];
    }
}
