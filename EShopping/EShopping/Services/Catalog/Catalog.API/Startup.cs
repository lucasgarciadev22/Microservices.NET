using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning();
        services.AddAuthorization();

        //Healthcheckers settings
        services
            .AddHealthChecks()
            .AddMongoDb(
                Configuration["DatabaseSettings:ConnectionString"],
                "Catalog  Mongo Db Health Check",
                HealthStatus.Degraded
            );

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
        });

        services.AddAutoMapper(typeof(Startup));

        //MediaTR settings
        services.AddMediatR(
            cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(CreateProductHandler).GetTypeInfo().Assembly
                )
        ); //register generic handler

        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, ProductRepository>();
        services.AddScoped<ITypeRepository, ProductRepository>();

        //Identity Server
        AuthorizationPolicy userPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(userPolicy)));
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:9009";
                options.Audience = "Catalog";
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthorization();

        //Registering endpoints
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks(
                "/health",
                new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            );
        });
    }
}
