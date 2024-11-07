using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

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
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override(
                    "Microsoft.Hosting.Lifetime",
                    Serilog.Events.LogEventLevel.Warning
                )
                .WriteTo.Console();

            //development env overrides
            if (env.IsDevelopment())
            {
                loggerConfig.MinimumLevel.Override("Basket", Serilog.Events.LogEventLevel.Debug);
                loggerConfig.MinimumLevel.Override("Catalog", Serilog.Events.LogEventLevel.Debug);
                loggerConfig.MinimumLevel.Override("Discount", Serilog.Events.LogEventLevel.Debug);
                loggerConfig.MinimumLevel.Override("Ordering", Serilog.Events.LogEventLevel.Debug);
            }
        };
}
