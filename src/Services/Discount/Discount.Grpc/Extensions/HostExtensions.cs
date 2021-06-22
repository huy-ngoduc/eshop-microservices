using System;
using Discount.Grpc.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var options = services.GetRequiredService<IOptions<DiscountDatabaseOptions>>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database.");
                    using var connection = new NpgsqlConnection(options.Value.ConnectionString);
                    connection.Open();
                    using var command = new NpgsqlCommand()
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF Exists Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";

                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO public.coupon(
	                                            productname, description, amount)
	                                        VALUES ('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO public.coupon(
	                                            productname, description, amount)
	                                        VALUES ('Samsung 10', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();
                    logger.LogInformation("Migrated postgresql database.");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "An error occured while migrating the postgresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
