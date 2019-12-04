using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.Orchestration.Requesters;
using FootballStatsApi.Scraper.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Scraper.Orchestration
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

            var amqpService = _serviceProvider.GetService<IAmqpService>();
            amqpService.Connect().Wait();
            amqpService.Declare();

            var leagueSummaryRequester = _serviceProvider.GetService<LeagueSummaryRequester>();
            var fixtureDetailsRequester = _serviceProvider.GetService<FixtureDetailsRequester>();

            Task.WhenAll(leagueSummaryRequester.Run(), fixtureDetailsRequester.Run()).Wait();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(o => o.AddConsole());

            services.AddSingleton<LeagueSummaryRequester>();
            services.AddSingleton<FixtureDetailsRequester>();

            var dbInfo = new DatabaseConnectionInfo();
            dbInfo.ConnectionString = _configuration.GetConnectionString("Football");
            services.AddSingleton(dbInfo);

            var amqpUri = new Uri(_configuration.GetConnectionString("AMQP"));
            services.AddSingleton(amqpUri);
            services.AddSingleton<IAmqpService, AmqpService>();
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<ICompetitionRepository, CompetitionRepository>();
            services.AddSingleton<IFixtureRepository, FixtureRepository>();
        }
    }
}
