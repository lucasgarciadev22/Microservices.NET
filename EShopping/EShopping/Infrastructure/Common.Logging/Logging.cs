using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, loggerConfig) =>
        {
            IHostEnvironment env = context.HostingEnvironment;
            //enrich logs with env infos
            loggerConfig.MinimumLevel
                .Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                .WriteTo.Console();

            //development env overrides
            if (env.IsDevelopment())
            {
                loggerConfig.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                loggerConfig.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                loggerConfig.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                loggerConfig.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
            }

            //elastic search configs
            string? elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            if (!string.IsNullOrEmpty(elasticUrl))
            {
                loggerConfig.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = "EShopping-Logs-{0:yyyy.MM.dd}",
                        MinimumLogEventLevel = LogEventLevel.Debug
                    }
                );
            }
        };
}
