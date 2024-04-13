using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.Data;

public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
{
    public OrderContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<OrderContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer("Data Source=OrderDb");

        return new OrderContext(optionsBuilder.Options);
    }
}
