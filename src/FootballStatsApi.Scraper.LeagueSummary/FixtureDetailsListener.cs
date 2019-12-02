using System.Threading.Tasks;
using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using PuppeteerSharp;

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class FixtureDetailsListener : IListener
    {
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly FixtureDetailsScraper _scraper;
        private readonly IConnectionProvider _connectionProvider;

        public FixtureDetailsListener(
            IAmqpService amqpService,
            ICompetitionRepository competitionRepository,
            FixtureDetailsScraper scraper,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _scraper = scraper;
            _connectionProvider = connectionProvider;
            _amqpService = amqpService;
        }

        public async Task Listen()
        {
            await _amqpService.DeclareFixtureDetailsQueue();

            await _amqpService.SubscribeToQueue<GetFixtureDetailsMessage>("getFixtureDetails", async s =>
            {
                await _scraper.Run(s.FixtureId);
            });
        }
    }
}