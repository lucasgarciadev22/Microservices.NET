namespace Ocelot.ApiGateway;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(
                (env, config) =>
                {
                    config.AddJsonFile($"ocelot.{env.HostingEnvironment.EnvironmentName}.json");
                    config.AddEnvironmentVariables();
                }
            )
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
