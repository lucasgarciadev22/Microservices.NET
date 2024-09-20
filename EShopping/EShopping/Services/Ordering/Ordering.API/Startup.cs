using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application.Services;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Services;

namespace Ordering.API;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddApplicationServices();
        services.AddInfraServices(Configuration);
        services.AddAutoMapper(typeof(Startup));

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
        });

        services.AddHealthChecks().Services.AddDbContext<OrderContext>();

        services.AddMassTransit(options =>
        {
            //Mark as a consumer
            options.AddConsumer<BasketOrderingConsumer>();
            options.UsingRabbitMq(
                (context, config) =>
                {
                    config.Host(Configuration["EventBusSettings:HostAddress"]);
                    //Provide the queue name with consumer settings
                    config.ReceiveEndpoint(
                        EventBusConstants.BasketCheckoutQueue,
                        options =>
                        {
                            options.ConfigureConsumer<BasketOrderingConsumer>(context);
                        }
                    );
                }
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
        }

        app.UseRouting();
        app.UseAuthorization();
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
