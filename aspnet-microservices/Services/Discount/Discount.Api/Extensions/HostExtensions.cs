using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Threading;

namespace Discount.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var conf = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                // migrate db
                try
                {
                    logger.LogInformation("migrating postgresql db!");

                    using var connection = new NpgsqlConnection(conf.GetValue<string>("DatabaseSettings:ConnectionString"));

                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(200) NOT NULL, Description TEXT, Amount INT)";
                    command.ExecuteNonQuery();

                    // seeding data
                    command.CommandText =
                        "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Description', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Description', 250);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migration has been completed.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError("an error has been occured!");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }
            return host;
        }
    }
}
