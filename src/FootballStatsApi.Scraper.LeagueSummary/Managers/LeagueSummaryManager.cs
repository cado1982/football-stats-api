using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Entities = FootballStatsApi.Domain.Entities;
using System.Data;
using FootballStatsApi.Domain.Entities;
using System.Linq;
using FootballStatsApi.Domain.Repositories;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class LeagueSummaryManager : ILeagueSummaryManager
    {
        private readonly ILogger<LeagueSummaryManager> _logger;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly ITeamSummaryRepository _teamSummaryRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerSummaryRepository _playerSummaryRepository;
        private readonly ITeamRepository _teamRepository;

        public LeagueSummaryManager(
            ILogger<LeagueSummaryManager> logger,
            ITeamRepository teamRepository,
            IFixtureRepository fixtureRepository,
            ITeamSummaryRepository teamSummaryRepository,
            IPlayerRepository playerRepository,
            IPlayerSummaryRepository playerSummaryRepository)
        {
            _teamRepository = teamRepository;
            _logger = logger;
            _fixtureRepository = fixtureRepository;
            _teamSummaryRepository = teamSummaryRepository;
            _playerRepository = playerRepository;
            _playerSummaryRepository = playerSummaryRepository;
        }

        public async Task ProcessFixtures(List<Models.Fixture> fixtures, int season, Competition competition, IDbConnection connection)
        {
            _logger.LogInformation($"Processing {fixtures.Count} fixtures for season {season} and competition {competition.Name}");

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

            // 2. Insert the pre-match stats into the db. Any advanced stats will be requested by the orchestration project.
            var fixtureEntities = fixtures.Select(f => new Entities.FixtureDetails
            {
                FixtureId = f.Id,
                HomeTeam = new Entities.Team { Id = f.HomeTeam.Id },
                AwayTeam = new Entities.Team { Id = f.AwayTeam.Id },
                Season = season,
                Competition = competition,
                IsResult = f.IsResult,
                DateTime = f.DateTime
            }).ToList();

            await _teamRepository.InsertMultipleAsync(teams, connection);
            await _fixtureRepository.InsertMultipleAsync(fixtureEntities, connection);

            _logger.LogInformation($"Successfully processed {fixtures.Count} fixtures for season {season} and competition {competition.Name}");
        }

        public async Task ProcessTeams(List<Models.Team> teams, int season, Competition competition, IDbConnection connection)
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

        public async Task ProcessPlayers(List<Models.Player> players, int season, Competition competition, IDbConnection conn)
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

        private Team GetPlayersTeam(Models.Player player, List<Team> allTeams)
        {
            var split = player.TeamTitle.Split(",");
            var teamTitle = split.LastOrDefault();
            var team = allTeams.Find(t => t.Name == teamTitle);

            return team;
        }
    }
}