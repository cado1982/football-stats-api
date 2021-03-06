using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.LeagueSummary.Models;
using Microsoft.Extensions.Logging;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class FixtureDetailsManager : IFixtureDetailsManager
    {
        private readonly ILogger<FixtureDetailsManager> _logger;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public FixtureDetailsManager(
            ILogger<FixtureDetailsManager> logger,
            IFixtureRepository fixtureRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository)
        {
            _fixtureRepository = fixtureRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _logger = logger;
        }

        public async Task ProcessMatchInfo(FixtureMatchInfo matchInfo, IDbConnection conn)
        {
            var teams = new List<Entities.Team>
            {
                new Entities.Team
                {
                    Id = matchInfo.HomeTeamId,
                    Name = matchInfo.HomeTeamName,
                    ShortName = string.Empty
                },
                new Entities.Team
                {
                    Id = matchInfo.AwayTeamId,
                    Name = matchInfo.AwayTeamName,
                    ShortName = string.Empty
                }
            };
            await _teamRepository.InsertMultipleAsync(teams, conn);

            var details = new Entities.Fixture
            {
                FixtureId = matchInfo.Id,
                HomeDeep = matchInfo.HomeDeepPasses,
                AwayDeep = matchInfo.AwayDeepPasses,
                HomeWinForecast = matchInfo.HomeWinForecast,
                DrawForecast = matchInfo.DrawForecast,
                AwayWinForecast = matchInfo.AwayWinForecast,
                HomePpda = matchInfo.HomePpda,
                AwayPpda = matchInfo.AwayPpda,
                AwayExpectedGoals = matchInfo.AwayExpectedGoals,
                AwayExpectedPoints = matchInfo.AwayWinForecast * 3 + matchInfo.DrawForecast,
                AwayGoals = matchInfo.AwayGoals,
                AwayShots = matchInfo.AwayShots,
                AwayShotsOnTarget = matchInfo.AwayShotsOnTarget,
                HomeExpectedGoals = matchInfo.HomeExpectedGoals,
                HomeExpectedPoints = matchInfo.HomeWinForecast * 3 + matchInfo.DrawForecast,
                HomeGoals = matchInfo.HomeGoals,
                HomeShots = matchInfo.HomeShots,
                HomeShotsOnTarget = matchInfo.HomeShotsOnTarget
            };

            await _fixtureRepository.Update(details, conn);
        }

        public async Task ProcessRosters(FixtureRosters rosters, FixtureShots shots, FixtureMatchInfo matchInfo, IDbConnection conn)
        {
            var homePlayers = rosters.Home.Values.ToList();
            var awayPlayers = rosters.Away.Values.ToList();
            var players = homePlayers.Union(awayPlayers).ToList();
            var allShots = shots.Home.Union(shots.Away).ToList();

            var playerEntities = players.Select(p => new Entities.Player
            {
                Id = p.PlayerId,
                Name = p.PlayerName
            }).ToList();

            await _playerRepository.InsertPlayersAsync(playerEntities, conn);

            var fixturePlayers = new List<Entities.FixturePlayer>();

            foreach (var player in players)
            {
                var rosterOutPlayer = players.Find(p => p.Id == player.RosterOut);
                var rosterInPlayer = players.Find(p => p.Id == player.RosterIn);

                var fixturePlayer = new Entities.FixturePlayer
                {
                    ExpectedGoalsBuildup = player.ExpectedGoalsBuildup,
                    ExpectedGoalsChain = player.ExpectedGoalsChain,
                    Minutes = player.Time,
                    Player = new Entities.Player { Id = player.PlayerId },
                    Position = player.Position,
                    PositionOrder = player.PositionOrder,
                    RedCards = player.RedCard,
                    YellowCards = player.YellowCard,
                    Assists = player.Assists,
                    ExpectedAssists = player.ExpectedAssists,
                    ExpectedGoals = player.ExpectedGoals,
                    FixtureId = matchInfo.Id,
                    Goals = player.Goals,
                    KeyPasses = player.KeyPasses,
                    OwnGoals = player.OwnGoals,
                    Shots = player.Shots,
                    ShotsOnTarget = allShots.Count(s => s.PlayerId == player.PlayerId && (s.Result == "Goal" || s.Result == "SavedShot")),
                    Replaced = rosterOutPlayer == null ? null : new Entities.Player { Id = rosterOutPlayer.PlayerId },
                    ReplacedBy = rosterInPlayer == null ? null : new Entities.Player { Id = rosterInPlayer.PlayerId },
                    Team = new Entities.Team { Id = player.TeamId }
                };

                fixturePlayers.Add(fixturePlayer);
            }

            await _fixtureRepository.InsertFixturePlayers(fixturePlayers, conn);
        }

        public async Task ProcessShots(FixtureShots shots, FixtureRosters rosters, FixtureMatchInfo matchInfo, IDbConnection conn)
        {
            var allShots = shots.Home.Union(shots.Away).ToList();
            var homePlayers = rosters.Home.Values.ToList();
            var awayPlayers = rosters.Away.Values.ToList();

            var entities = new List<Entities.FixtureShot>();

            foreach (var shot in allShots)
            {
                var entity = new Entities.FixtureShot();

                // Try and resolve the assister if there is one
                if (shot.PlayerAssistedName != null)
                {
                    FixtureRosterEntry matchingPlayer = null;

                    if (shot.HomeOrAway == "h")
                    {
                        matchingPlayer = homePlayers.Find(v => v.PlayerName == shot.PlayerAssistedName);
                    }
                    else if (shot.HomeOrAway == "a")
                    {
                        matchingPlayer = awayPlayers.Find(p => p.PlayerName == shot.PlayerAssistedName);
                    }

                    if (matchingPlayer != null)
                    {
                        entity.Assist = new Entities.Player { Id = matchingPlayer.PlayerId };
                    }
                }

                entity.ExpectedGoal = shot.ExpectedGoal;
                entity.ShotId = shot.Id;
                entity.Player = new Entities.Player { Id = shot.PlayerId };
                entity.Minute = shot.Minute;
                entity.Result = shot.Result;
                entity.X = shot.X;
                entity.Y = shot.Y;
                entity.FixtureId = matchInfo.Id;
                entity.ExpectedGoal = shot.ExpectedGoal;
                entity.Team = new Entities.Team { Id = shot.HomeOrAway == "h" ? matchInfo.HomeTeamId : matchInfo.AwayTeamId };
                entity.Situation = shot.Situation;
                entity.Type = shot.ShotType;
                entity.LastAction = shot.LastAction;

                entities.Add(entity);
            }

            await _fixtureRepository.InsertFixtureShots(entities, conn);
        }

        public async Task ConfirmDetailsSaved(int fixtureId, IDbConnection conn)
        {
            await _fixtureRepository.UpdateDetailsSavedAsync(fixtureId, conn);
        }
    }

}