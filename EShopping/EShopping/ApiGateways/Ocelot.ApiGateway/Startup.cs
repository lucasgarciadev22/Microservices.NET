using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Ocelot.ApiGateway;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
            );
        });

        //Identity Server changes
        string authScheme = "EShoppingGatewayAuthScheme";
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                authScheme,
                options =>
                {
                    options.Authority = "https://localhost:9010";
                    options.Audience = "EShoppingGateway";
                }
            );

        services.AddOcelot().AddCacheManager(o => o.WithDictionaryHandle());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet(
                "/",
                async context =>
                {
                    await context.Response.WriteAsync("Hello Ocelot");
                }
            );
        });
        app.UseOcelot().Wait();
    }
}
