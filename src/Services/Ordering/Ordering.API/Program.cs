using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // For SQL Server
            app.MigrateDatabase<OrderContext>((context, services) =>
            {
                // This code block is represented by the "seeder" parameter of the "MigrateDatabase" method.
                var logger = services.GetService<ILogger<OrderContextSeed>>();
                OrderContextSeed.SeedAsync(context, logger!).Wait();
            });
            //app.MigrateDatabase<OrderContext>(Seed, 0);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        //private static void Seed(OrderContext context, IServiceProvider services)
        //{
        //    var logger = services.GetService<ILogger<OrderContextSeed>>();
        //    OrderContextSeed.SeedAsync(context, logger!).Wait();
        //}
    }
}