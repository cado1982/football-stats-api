using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore;
using Serilog;

namespace FootballStatsApi
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(Configuration)
                            .Filter.ByExcluding("RequestPath = '/healthcheck' and StatusCode < 400")
                            .Enrich.FromLogContext()
                            .CreateLogger();

            try
            {
                CreateWebHostBuilder(args).Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .SuppressStatusMessages(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                .UseConfiguration(Configuration)
                .UseSerilog()
                .UseStartup<Startup>();
        }
    }
}
