using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription descr in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(descr.GroupName, ProvideApiInfo(descr));
        }
    }

    private static OpenApiInfo ProvideApiInfo(ApiVersionDescription descr)
    {
        OpenApiInfo info =
            new()
            {
                Title = "Basket API Microservice",
                Version = descr.ApiVersion.ToString(),
                Description = "Fetches details about Basket",
                Contact = new OpenApiContact() { Name = "Rahul Sahay", Email = "rahul@xyz.com" },
                License = new OpenApiLicense()
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

        if (descr.IsDeprecated)
        {
            info.Description += " API Version has been deprecated!!!";
        }

        return info;
    }
}
