using Basket.API.GrpcServices;
using Basket.API.Repositories;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddStackExchangeRedisCache(options =>
                options.Configuration = builder.Configuration["CacheSettings:ConnectionString"]);

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddGrpcClient<DiscountProtoServiceClient>(options =>
                options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!));
            builder.Services.AddScoped<DiscountGrpcService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
    }
}