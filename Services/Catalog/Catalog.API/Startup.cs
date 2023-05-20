namespace Catalog.API;

public class Startup
{
  public IConfiguration Configuration { get; set; }

  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public void ConfigurationServices(IServiceCollection services)
  {
    services.AddControllers();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseRouting();
    app.UseStaticFiles();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
