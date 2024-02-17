using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.Application.Handlers;
using Movies.Core.Repositories;
using Movies.Core.Repositories.Base;
using Movies.Infrastructure.Data;
using Movies.Infrastructure.Repositores;
using Movies.Infrastructure.Repositores.Base;

namespace Movies.API;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();

        string c = _configuration.GetConnectionString("MoviesConnection"); //aqui estao erradas ainda

        services.AddDbContext<MovieContext>(
            options => options.UseSqlServer(_configuration.GetConnectionString("MoviesConnection")),
            ServiceLifetime.Singleton
        );

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc(
                name: "v1",
                new OpenApiInfo() { Title = "Movie Review API", Version = "v1" }
            );
        });

        services.AddAutoMapper(typeof(Startup));

        services.AddMediatR(
            config =>
                config.RegisterServicesFromAssembly(typeof(CreateMovieCommandHandler).Assembly)
        );

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IMovieRepository, MovieRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.RoutePrefix = string.Empty;
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Review API");
        });
    }
}
