﻿using System;
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
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;
        private static Browser _browser;

        public static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.ProcessExit += async (s, e) =>
                {
                    await _browser?.CloseAsync();
                    _browser?.Dispose();
                };

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

                var listeners = _serviceProvider.GetServices<IListener>();

                foreach (var listener in listeners)
                {
                    listener.Listen();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during startup");
                Console.WriteLine(ex);
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(o => o.AddConsole());

            services.AddSingleton<IListener, LeagueSummaryListener>();
            services.AddSingleton<IListener, FixtureDetailsListener>();
            services.AddSingleton<LeagueSummaryScraper>();
            services.AddSingleton<FixtureDetailsScraper>();

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
            services.AddSingleton<ITeamSummaryRepository, TeamSummaryRepository>();
            services.AddSingleton<IFixtureRepository, FixtureRepository>();

            services.AddSingleton<ILeagueSummaryManager, LeagueSummaryManager>();
            services.AddSingleton<IFixtureDetailsManager, FixtureDetailsManager>();

            _browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }).Result;
            services.AddSingleton(_browser);
            
        }
    }
}
