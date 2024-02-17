using dotenv.net;
using Movies.API;
using Movies.Infrastructure.Data;

IHost host = CreateHostBuilder(args).Build();

await CreateAndSeedDb(host);

host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            DotEnv.Load();

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();
        })
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

static async Task CreateAndSeedDb(IHost host)
{
    using IServiceScope scope = host.Services.CreateScope();

    IServiceProvider services = scope.ServiceProvider;
    ILoggerFactory loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        MovieContext moviesContext = services.GetRequiredService<MovieContext>();

        await MovieContextSeed.SeedAsync(moviesContext, loggerFactory);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

        logger.LogError("Exception occured in API while starting:{Message}", ex.Message);
    }
}
