using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration; 

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            // Download chromium
            var fetcher = new BrowserFetcher();
            Console.WriteLine("Downloading chromium");
            fetcher.DownloadProgressChanged += (s, e) => Console.WriteLine($"{e.ProgressPercentage}%");
            fetcher.DownloadAsync(BrowserFetcher.DefaultRevision).Wait();
            Console.WriteLine("Chromium downloaded successfully");

            var amqpService = _serviceProvider.GetService<IAmqpService>();
            amqpService.Connect().Wait();

            var listener = _serviceProvider.GetService<LeagueSummaryListener>();
            listener.Listen().Wait();

            Console.WriteLine("Done");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(o => o.AddConsole());

            services.AddSingleton<LeagueSummaryListener>();
            services.AddSingleton<LeagueSummaryScraper>();

            var dbInfo = new DatabaseConnectionInfo();
            dbInfo.ConnectionString = _configuration.GetConnectionString("Football");
            services.AddSingleton(dbInfo);

            var amqpUri = new Uri(_configuration.GetConnectionString("AMQP"));
            services.AddSingleton(amqpUri);
            services.AddSingleton<IAmqpService, AmqpService>();
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<ICompetitionRepository, CompetitionRepository>();
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
            services.AddSingleton<IPlayerSummaryRepository, PlayerSummaryRepository>();
            services.AddSingleton<ITeamRepository, TeamRepository>();
        }
    }
}
