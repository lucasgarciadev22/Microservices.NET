using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureService(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddAuthorization();
        //Healthcheckers settings
        services
            .AddHealthChecks()
            .AddRedis(
                Configuration["CacheSettings:ConnectionSettings"],
                "Redis Health",
                HealthStatus.Degraded
            );
        //Redis Settings
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>(
                "CacheSettings:ConnectionString"
            );
        });
        //MediaTr Settings
        services.AddAutoMapper(typeof(Startup));

        var mediatRCfg = new MediatRServiceConfiguration();
        mediatRCfg.RegisterServicesFromAssembly(typeof(CreateShoppingCartCommandHandler).Assembly);
        services.AddMediatR(mediatRCfg); //register generic handler

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Basket.API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        //app.UseAuthorization();
        //Registering endpoints
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            );
        });
    }
}
