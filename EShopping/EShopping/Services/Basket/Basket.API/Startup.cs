using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        //Versioning Settings
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            //Enable when required
            // options.ApiVersionReader = ApiVersionReader.Combine(
            //         new HeaderApiVersionReader("X-Version"),
            //         new QueryStringApiVersionReader("api-version", "ver"),
            //         new MediaTypeApiVersionReader("ver")
            //     );
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
            services.AddApiVersioning();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    policy =>
                    {
                        //TODO read the same from settings for prod deployment
                        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    }
                );
            });
        });
        services.AddAuthorization();

        //CORS Settings
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                policy =>
                {
                    //TODO read the same from settings for prod deployment
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
            );
        });

        //Healthcheckers Settings
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

        //gRPC Settings
        services.AddScoped<DiscountGrpcService>();
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
            o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"])
        );

        //MediaTr Settings
        MediatRServiceConfiguration mediatRCfg = new();
        mediatRCfg.RegisterServicesFromAssembly(
            typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly
        );
        services.AddMediatR(mediatRCfg); //register generic handler

        //MassTransit Settings
        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq(
                (context, config) =>
                {
                    config.Host(Configuration["EventBusSettings:HostAddress"]);
                }
            );
        });
        services.AddMassTransitHostedService();
        //Identity Server changes
        // var userPolicy = new AuthorizationPolicyBuilder()
        //     .RequireAuthenticatedUser()
        //     .Build();
        //
        // services.AddControllers(config =>
        // {
        //     config.Filters.Add(new AuthorizeFilter(userPolicy));
        // });
        //
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.Authority = "https://localhost:9009";
        //         options.Audience = "Basket";
        //     });

        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Basket.API", Version = "v1" });
        });
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider
    )
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant()
                    );
                }
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

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
