using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(
        this IHost host,
        Action<TContext, IServiceProvider> seeder
    )
        where TContext : DbContext
    {
        using (IServiceScope scope = host.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
            TContext? context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Started Db Migration: {ContextName}", typeof(TContext).Name);

                Polly.Retry.RetryPolicy retry = Policy
                    .Handle<SqlException>()
                    .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, span, cont) =>
                        {
                            logger.LogError(
                                "Retrying because of {Exception} {Retry}",
                                exception,
                                span
                            );
                        }
                    );

                retry.Execute(() => CallSeeder(seeder, context, services));

                logger.LogInformation("Migration Completed: {ContextName}", typeof(TContext).Name);
            }
            catch (SqlException e)
            {
                logger.LogError(
                    e,
                    "An error occurred while migrating db: {ContextName}",
                    typeof(TContext).Name
                );
            }
        }

        return host;
    }

    private static void CallSeeder<TContext>(
        Action<TContext, IServiceProvider> seeder,
        TContext context,
        IServiceProvider services
    )
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
