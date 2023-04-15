using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static void MigrateDatabase<TContext>(this WebApplication? host, int? retry = 0)
        {
            var retryForAvailability = retry!.Value;

            using var scope = host!.Services.CreateScope();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>()!;
            var logger = scope.ServiceProvider.GetService<ILogger<TContext>>()!;

            try
            {
                logger.LogInformation("Migrating PostgreSQL database.");

                using var connection = new NpgsqlConnection
                    (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();

                using var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();
                
                command.CommandText = @"CREATE TABLE Coupon(ID SERIAL PRIMARY KEY NOT NULL,
	                                                        ProductName VARCHAR(24) NOT NULL,
	                                                        Description TEXT,
	                                                        Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrated PostgreSQL database.");
            }
            catch (NpgsqlException ex) // PostgreSQL db container may not be ready when migration is requested so we try a few times.
            {
                logger.LogError(ex, "An error occurred migrating PostgreSQL database.");

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(retryForAvailability);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }
            }
        }
    }
}
