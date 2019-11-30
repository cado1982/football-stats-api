using System.Threading.Tasks;
using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using PuppeteerSharp;

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class LeagueSummaryListener : IListener
    {
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly LeagueSummaryScraper _scraper;
        private readonly IConnectionProvider _connectionProvider;

        public LeagueSummaryListener(
            IAmqpService amqpService,
            ICompetitionRepository competitionRepository,
            LeagueSummaryScraper scraper,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _scraper = scraper;
            _connectionProvider = connectionProvider;
            _amqpService = amqpService;
        }

        public async Task Listen()
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            await _amqpService.DeclareLeagueSummaryQueue();

            await _amqpService.SubscribeToQueue<GetLeagueSummaryMessage>("getLeagueSummary", async s =>
            {
                using (var conn = _connectionProvider.GetOpenConnection())
                {
                    var competition = await _competitionRepository.GetByIdAsync(s.Competition, conn);

                    await _scraper.Run(competition.InternalName);
                }
            });
        }
    }
}