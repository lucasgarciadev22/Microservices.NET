using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Basket.API
{
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
            //Redis Settings
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            //MediaTr Settings
            services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetType().Assembly);
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Basket.API", Version = "v1" });
            });
            services.AddHealthChecks()
                .AddRedis(Configuration["CacheSettings:ConnectionSettings"], "Redis Health", HealthStatus.Degraded);
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
            app.UseAuthorization();
        }
    }
}
