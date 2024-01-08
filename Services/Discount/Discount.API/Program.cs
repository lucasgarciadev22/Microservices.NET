using Discount.Infrastructure.Extensions;

namespace Discount.API;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.MigratedDatabase<Program>();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        throw new NotImplementedException();
    }
}
