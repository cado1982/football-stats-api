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
        private readonly IConnectionProvider _connectionProvider;

        public LeagueSummaryScraper(
            IAmqpService amqpService,
            ILogger<LeagueSummaryScraper> logger,
            ICompetitionRepository competitionRepository,
            IPlayerSummaryRepository playerSummaryRepository,
            IPlayerRepository playerRepository,
            ITeamRepository teamRepository,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _playerSummaryRepository = playerSummaryRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _connectionProvider = connectionProvider;
            _amqpService = amqpService;
            _logger = logger;
            _browser = Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }).Result;
        }


        public async Task Run(string competitionName, int? season = null)
        {
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
                
                var players = pd.ToObject<List<Models.Player>>();
                var teams = td.ToObject<Dictionary<string, Models.Team>>();

                //await ProcessTeams(teams, seasonFromPage, competition, conn);
                //await ProcessPlayers(players, seasonFromPage, competition, conn);
            }

            
            // const dd: any = await page.evaluate(() => datesData);

            // const players = pd;
            // const fixtures = dd as IFixture[];
            // const teams = td as ITeams;

            // await repository.saveTeams(teams, season, competitionId, pool);
            // await repository.savePlayers(players, season, competitionId, pool);
            // await processFixtures(fixtures, seasonFromPage, competitionId, pool, repository);

            // // Clean up
            // await pool.end();
            // await browser.close();
        }

        private Team GetPlayersTeam(Models.Player player, List<Team> allTeams)
        {
            var split = player.TeamTitle.Split(",");
            var teamTitle = split.LastOrDefault();
            var team = allTeams.Find(t => t.Name == teamTitle);

            return team;
        }

        private async Task ProcessTeams(List<Models.Team> teams, int season, Competition competition, IDbConnection connection)
        {
        //     console.log(`Saving team summaries for season ${season}`);
        
        // let teamSummaryValues = '';
        // let teamValues = '';

        // for (const teamId in teams) {
        //     if (teams.hasOwnProperty(teamId)) {
        //         const team = teams[teamId];

        //         const games = team.history.length;
        //         const won = team.history.reduce((total, current) => total + current.wins, 0);
        //         const drawn = team.history.reduce((total, current) => total + current.draws, 0);
        //         const lost = team.history.reduce((total, current) => total + current.loses, 0);
        //         const goalsFor = team.history.reduce((total, current) => total + current.scored, 0);
        //         const goalsAgainst = team.history.reduce((total, current) => total + current.missed, 0);
        //         const points = team.history.reduce((total, current) => total + current.pts, 0);
        //         const expectedGoals = team.history.reduce((total, current) => total + current.xG, 0);
        //         const expectedGoalsAgainst = team.history.reduce((total, current) => total + current.xGA, 0);
        //         const expectedPoints = team.history.reduce((total, current) => total + current.xpts, 0);

        //         const oppositionPasses = team.history.reduce((total, current) => total + current.ppda.att, 0);
        //         const defensiveEvents = team.history.reduce((total, current) => total + current.ppda.def, 0);
        //         const ppda = oppositionPasses / defensiveEvents;
        //         const deepPasses = team.history.reduce((total, current) => total + current.deep, 0);

        //         teamValues = teamValues.concat(`(${team.id},'${team.title}',''),`);
        //         teamSummaryValues = teamSummaryValues.concat(`(${team.id},${season},${competitionId},${games},${won},${drawn},${lost},${goalsFor},${goalsAgainst},${points},${expectedGoals},${expectedGoalsAgainst},${expectedPoints},'${ppda}','${deepPasses}'),`);
        //     }
        // }

        // // Remove the final commas
        // teamSummaryValues = teamSummaryValues.slice(0, teamSummaryValues.length - 1);
        // teamValues = teamValues.slice(0, teamValues.length - 1);

        // let insertTeamsQuery = `INSERT INTO "stats"."team" (id, name, short_name) VALUES ${teamValues} ON CONFLICT(id) DO NOTHING`;
        // const insertTeamSummariesQuery = `INSERT INTO "stats"."team_summary" (team_id, season_id, competition_id, games, won, drawn,
        //     lost, goals_for, goals_against, points, expected_goals, expected_goals_against,
        //     expected_points, ppda, deep_passes) VALUES ${teamSummaryValues}
        // ON CONFLICT(team_id, season_id, competition_id) DO UPDATE SET games = EXCLUDED.games, won = EXCLUDED.won,
        // drawn = EXCLUDED.drawn, lost = EXCLUDED.lost, goals_for = EXCLUDED.goals_for,
        // goals_against = EXCLUDED.goals_against, points = EXCLUDED.points,
        // expected_goals = EXCLUDED.expected_goals, expected_goals_against = EXCLUDED.expected_goals_against,
        // expected_points = EXCLUDED.expected_points, ppda = EXCLUDED.ppda, deep_passes = EXCLUDED.deep_passes;`;

        // insertTeamsQuery = insertTeamsQuery.replace('&#039;', "''");

        // await pool.query(insertTeamsQuery);
        // await pool.query(insertTeamSummariesQuery);
        }

        private async Task ProcessPlayers(List<Models.Player> players, int season, Competition competition, IDbConnection conn)
        {
            // 1. Insert players
            var playerEntities = new List<Entities.Player>();
            var playerSummaries = new List<Entities.PlayerSummary>();

            var allTeams = await _teamRepository.GetAllTeamsAsync(conn);

            foreach (var playerModel in players)
            {
                var entity = new Entities.Player();
                entity.Id = playerModel.Id;
                entity.Name = playerModel.PlayerName;
                playerEntities.Add(entity);

                var team = GetPlayersTeam(playerModel, allTeams);
                if (team == null)
                {
                    _logger.LogError($"Team not found for player {playerModel.PlayerName} with team title {playerModel.TeamTitle}");
                    continue;
                }

                var summary = new Entities.PlayerSummary();
                summary.Player = new Entities.Player { Id = playerModel.Id };
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
        }

        public void Dispose()
        {
            _browser?.CloseAsync().Wait();
            _browser.Dispose();
        }
    }
}