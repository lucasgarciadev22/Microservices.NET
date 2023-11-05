using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        /// <summary>
        /// Extension method for Database migration from PostgresSQL Db, create a new Host Config
        /// </summary>
        /// <param name="host">The host configurations to instantiate the migration</param>
        /// <returns></returns>
        public static IHost MigratedDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>(); //passes whatever the context as type
                try
                {
                    logger.LogInformation("Discount DB Migration Started");
                    ApplyMigration(config);
                    logger.LogInformation("Discount DB Migration Completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            return host;
        }

        private static void ApplyMigration(IConfiguration config)
        {
            using var connection = new NpgsqlConnection(
                config.GetValue<string>("DatabaseSettings:ConnectionString")
            ); //get the connection string from JSON
            connection.Open();

            //Create new command for managing the migration

            using var command = new NpgsqlCommand() { Connection = connection, };

            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            // @ operator because is a multiliner command
            command.CommandText =
                @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                    ProductName VARCHAR(500) NOT NULL,
                                    Description TEXT,
                                    Amount INT)";
            command.ExecuteNonQuery();

            //Insert seed values into the Coupon table
            command.CommandText =
                "INSERT INTO Coupon(ProductName,Description,Amount) VALUES ('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);";
            command.ExecuteNonQuery();
            command.CommandText =
                "INSERT INTO Coupon(ProductName,Description,Amount) VALUES ('Yonex VCORE Pro 100 A Tennis Racquet(270gm, Strung)', 'Racquet Discount', 700);";
            command.ExecuteNonQuery();
        }
    }
}
