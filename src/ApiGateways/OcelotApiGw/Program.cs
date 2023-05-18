using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

            builder.Logging.AddConsole().AddDebug();

            builder.Services.AddOcelot()
                .AddCacheManager(settings => settings.WithDictionaryHandle());

            var app = builder.Build();

            app.UseOcelot().Wait();

            app.MapGet("/", () => "Hello World!");            

            app.Run();
        }
    }
}