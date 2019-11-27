using RabbitMQ.Client;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using RabbitMQ.Client.Events;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using PuppeteerSharp;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Scraper.LeagueSummary.Models;
using System.Data;
using FootballStatsApi.Domain.Entities;
using System.Linq;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class LeagueSummaryScraper  //: IDisposable
    {
        private readonly IAmqpService _amqpService;
        private readonly ILogger<LeagueSummaryScraper> _logger;
        private readonly Browser _browser;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IPlayerSummaryRepository _playerSummaryRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamSummaryRepository _teamSummaryRepository;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly IConnectionProvider _connectionProvider;

        public LeagueSummaryScraper(
            IAmqpService amqpService,
            ILogger<LeagueSummaryScraper> logger,
            ICompetitionRepository competitionRepository,
            IPlayerSummaryRepository playerSummaryRepository,
            IPlayerRepository playerRepository,
            ITeamRepository teamRepository,
            ITeamSummaryRepository teamSummaryRepository,
            IFixtureRepository fixtureRepository,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _playerSummaryRepository = playerSummaryRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _teamSummaryRepository = teamSummaryRepository;
            _fixtureRepository = fixtureRepository;
            _connectionProvider = connectionProvider;
            _amqpService = amqpService;
            _logger = logger;
            _browser = Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }).Result;
        }

        public async Task Run(string competitionName, int? season = null)
        {
            try
            {
                _logger.LogDebug($"Entering Run({competitionName})");
                using (var conn = _connectionProvider.GetOpenConnection())
                {
                    var competitions = await _competitionRepository.GetAsync(conn);

                    var competition = competitions.Find(c => c.InternalName == competitionName);

                    if (competition == null)
                    {
                        throw new Exception($"Competition not found '{competitionName}'");
                    }

                    var page = await _browser.NewPageAsync();

                    var url = season != null ? $"https://understat.com/league/{competition.InternalName}/{season}" :
                                               $"https://understat.com/league/{competition.InternalName}";

                    _logger.LogInformation("Loading " + url);

                    var response = await page.GoToAsync(url, new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle2 } });

                    if (!response.Ok)
                    {
                        _logger.LogError($"Unable to load {url}. Http Status {response.Status}");
                        return;
                    }

                    _logger.LogInformation("Attempting to parse season from page");
                    // Use the select dropdown on the page to decide which season it is
                    var seasonFromPage = await page.EvaluateExpressionAsync<int>("+document.querySelector('select[name=season] > option[selected]').value");
                    _logger.LogInformation($"Season {seasonFromPage} found");

                    var pd = await page.EvaluateExpressionAsync("playersData");
                    var td = await page.EvaluateExpressionAsync("teamsData");
                    var dd = await page.EvaluateExpressionAsync("datesData");

                    var players = pd.ToObject<List<Models.Player>>();
                    var teams = td.ToObject<Dictionary<string, Models.Team>>();
                    var fixtures = dd.ToObject<List<Models.Fixture>>();

                    await ProcessTeams(teams.Values.ToList(), seasonFromPage, competition, conn);
                    await ProcessPlayers(players, seasonFromPage, competition, conn);
                    await ProcessFixtures(fixtures, seasonFromPage, competition, conn);

                    await page.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running LeagueSummaryScraper");
            }
        }

        private Team GetPlayersTeam(Models.Player player, List<Team> allTeams)
        {
            var split = player.TeamTitle.Split(",");
            var teamTitle = split.LastOrDefault();
            var team = allTeams.Find(t => t.Name == teamTitle);

            return team;
        }

        private async Task ProcessFixtures(List<Models.Fixture> fixtures, int season, Competition competition, IDbConnection connection)
        {
            _logger.LogInformation($"Processing {fixtures.Count} fixtures for season {season} and competition {competition.Name}");

            var completedFixtures = fixtures.Where(f => f.IsResult).ToList();
            var futureFixtures = fixtures.Where(f => !f.IsResult).ToList();

            await ProcessCompletedFixtures(completedFixtures, connection);
            await ProcessFutureFixtures(futureFixtures, season, competition, connection);

            _logger.LogInformation($"Successfully processed {fixtures.Count} fixtures for season {season} and competition {competition.Name}");
        }

        private async Task ProcessCompletedFixtures(List<Models.Fixture> fixtures, IDbConnection connection)
        {
            foreach (var fixture in fixtures)
            {
                // 1. If fixture is already saved. Skip it.
                var isFixtureAlreadySaved = await _fixtureRepository.IsFixtureSavedAsync(fixture.Id, connection);
                if (isFixtureAlreadySaved) { continue; }

                // 2. Send AMQP message to request fixture is pulled.
                var message = new GetFixtureDetailsMessage();
                message.FixtureId = fixture.Id;
                await _amqpService.Send(message, "stats.getFixtureDetails");
            }
        }

        private async Task ProcessFutureFixtures(List<Models.Fixture> fixtures, int season, Competition competition, IDbConnection connection)
        {
            // 1. Get the distinct home and away teams to insert into db
            var teams = fixtures.Select(t => t.HomeTeam)
                .Union(fixtures.Select(t => t.AwayTeam))
                .GroupBy(k => k.Id)
                .Select(g => g.FirstOrDefault())
                .Where(t => t != null)
                .Select(t => new Entities.Team
                {
                    Id = t.Id,
                    Name = t.Title,
                    ShortName = t.ShortTitle
                })
                .ToList();

            var fixtureEntities = fixtures.Select(f => new Entities.FixtureDetails
            {
                FixtureId = f.Id,
                HomeTeam = new Entities.Team { Id = f.HomeTeam.Id },
                AwayTeam = new Entities.Team { Id = f.AwayTeam.Id },
                Season = season,
                Competition = competition,
                IsResult = f.IsResult,
                ForecastHomeWin = f.Forecast == null ? (double?)null : f.Forecast.HomeWin,
                ForecastDraw = f.Forecast == null ? (double?)null : f.Forecast.Draw,
                ForecastAwayWin = f.Forecast == null ? (double?)null : f.Forecast.AwayWin,
                DateTime = f.DateTime
            }).ToList();

            await _teamRepository.InsertMultipleAsync(teams, connection);
            await _fixtureRepository.InsertMultipleAsync(fixtureEntities, connection);
        }

        private async Task ProcessTeams(List<Models.Team> teams, int season, Competition competition, IDbConnection connection)
        {
            _logger.LogInformation($"Processing {teams.Count} teams for season {season} and competition {competition.Name}");

            var teamEntities = new List<Entities.Team>();
            var teamSummaryEntities = new List<Entities.TeamSummary>();

            foreach (var team in teams)
            {
                var teamEntity = new Entities.Team();
                teamEntity.Id = team.Id;
                teamEntity.Name = team.Name;
                teamEntity.ShortName = String.Empty;
                teamEntities.Add(teamEntity);

                var teamSummaryEntity = new Entities.TeamSummary();
                teamSummaryEntity.Team = teamEntity;
                teamSummaryEntity.Season = season;
                teamSummaryEntity.Competition = competition;
                teamSummaryEntity.Games = team.History.Count();
                teamSummaryEntity.Won = team.History.Sum(h => h.Wins);
                teamSummaryEntity.Drawn = team.History.Sum(h => h.Draws);
                teamSummaryEntity.Lost = team.History.Sum(h => h.Loses);
                teamSummaryEntity.GoalsFor = team.History.Sum(h => h.Scored);
                teamSummaryEntity.GoalsAgainst = team.History.Sum(h => h.Missed);
                teamSummaryEntity.Points = team.History.Sum(h => h.Points);
                teamSummaryEntity.ExpectedGoals = (short)team.History.Sum(h => h.ExpectedGoals);
                teamSummaryEntity.ExpectedGoalsAgainst = (short)team.History.Sum(h => h.ExpectedGoalsAgainst);
                teamSummaryEntity.ExpectedPoints = (short)team.History.Sum(h => h.ExpectedPoints);
                teamSummaryEntity.DeepPasses = team.History.Sum(h => h.Deep);

                var oppositionPasses = team.History.Sum(h => h.PPDA.PassesAllowed);
                var defensiveActions = team.History.Sum(h => h.PPDA.DefensiveActions);
                teamSummaryEntity.Ppda = (short)(oppositionPasses / defensiveActions);

                teamSummaryEntities.Add(teamSummaryEntity);
            }

            await _teamRepository.InsertMultipleAsync(teamEntities, connection);
            await _teamSummaryRepository.InsertMultipleAsync(teamSummaryEntities, connection);

            _logger.LogInformation($"Successfully processed {teams.Count} teams for season {season} and competition {competition.Name}");
        }

        private async Task ProcessPlayers(List<Models.Player> players, int season, Competition competition, IDbConnection conn)
        {
            _logger.LogInformation($"Processing {players.Count} players for season {season} and competition {competition.Name}");

            var playerEntities = new List<Entities.Player>();
            var playerSummaries = new List<Entities.PlayerSummary>();

            var allTeams = await _teamRepository.GetAllTeamsAsync(conn);

            foreach (var playerModel in players)
            {
                var playerEntity = new Entities.Player();
                playerEntity.Id = playerModel.Id;
                playerEntity.Name = playerModel.PlayerName.Replace("&#039;", "'");
                playerEntities.Add(playerEntity);

                var team = GetPlayersTeam(playerModel, allTeams);
                if (team == null)
                {
                    _logger.LogError($"Team not found for player {playerModel.PlayerName} with team title {playerModel.TeamTitle}");
                    continue;
                }

                var summary = new Entities.PlayerSummary();
                summary.Player = playerEntity;
                summary.Season = season;
                summary.Competition = competition;
                summary.Team = team;
                summary.Games = playerModel.Games;
                summary.MinutesPlayed = playerModel.Time;
                summary.Goals = playerModel.Goals;
                summary.ExpectedGoals = playerModel.ExpectedGoals;
                summary.Assists = playerModel.Assists;
                summary.ExpectedAssists = playerModel.ExpectedAssists;
                summary.Shots = playerModel.Shots;
                summary.KeyPasses = playerModel.KeyPasses;
                summary.YellowCards = playerModel.YellowCards;
                summary.RedCards = playerModel.RedCards;
                summary.Position = playerModel.Position;
                summary.NonPenaltyGoals = playerModel.NonPenaltyGoals;
                summary.NonPenaltyExpectedGoals = playerModel.NonPenaltyExpectedGoals;
                summary.ExpectedGoalsChain = playerModel.ExpectedGoalsChain;
                summary.ExpectedGoalsBuildup = playerModel.ExpectedGoalsBuildup;
                playerSummaries.Add(summary);
            }

            await _playerRepository.InsertPlayersAsync(playerEntities, conn);
            await _playerSummaryRepository.InsertPlayerSummariesAsync(playerSummaries, conn);

            _logger.LogInformation($"Successfully processed {players.Count} players for season {season} and competition {competition.Name}");
        }

        public void Dispose()
        {
            _browser?.CloseAsync().Wait();
            _browser.Dispose();
        }
    }
}