using System;
using System.IO;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public static class Program
    {   
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();

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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .UseSerilog();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<AppService>();
            services.AddSingleton<FixtureDetailsListener>();
            services.AddSingleton<LeagueSummaryListener>();
            services.AddSingleton<LeagueSummaryScraper>();
            services.AddSingleton<FixtureDetailsScraper>();

            var chromeSettings = new ChromeSettings
            {
                Host = Configuration["ChromeRemoteDebuggingHost"],
                Port = Configuration["ChromeRemoteDebuggingPort"]
            };
            services.AddSingleton(chromeSettings);
            services.AddSingleton<ChromeHelper>();

            var dbInfo = new DatabaseConnectionInfo
            {
                ConnectionString = Configuration.GetConnectionString("Football")
            };
            services.AddSingleton(dbInfo);

            var amqpUri = new Uri(Configuration.GetConnectionString("AMQP"));
            services.AddSingleton(amqpUri);
            services.AddSingleton<IAmqpService, AmqpService>();
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<ICompetitionRepository, CompetitionRepository>();
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
            services.AddSingleton<IPlayerSummaryRepository, PlayerSummaryRepository>();
            services.AddSingleton<ITeamRepository, TeamRepository>();
            services.AddSingleton<ITeamSummaryRepository, TeamSummaryRepository>();
            services.AddSingleton<IFixtureRepository, FixtureRepository>();

            services.AddSingleton<ILeagueSummaryManager, LeagueSummaryManager>();
            services.AddSingleton<IFixtureDetailsManager, FixtureDetailsManager>();
        }
    }
}
