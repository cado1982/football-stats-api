using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.v0.Extensions;
using FootballStatsApi.Models.v0;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public class FixtureManager : IFixtureManager
    {
        private readonly ILogger<FixtureManager> _logger;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly IConnectionProvider _connectionProvider;

        public FixtureManager(
            ILogger<FixtureManager> logger,
            IFixtureRepository fixtureRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _fixtureRepository = fixtureRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<List<FixtureBasic>> GetFixturesBasicAsync(int competitionId, int season)
        {
            try
            {
                if (competitionId < 1) throw new ArgumentException("competitionId must be provided", nameof(competitionId));
                if (season < 1) throw new ArgumentException("season must be provided", nameof(season));

                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var fixtures = await _fixtureRepository.GetFixturesByCompetitionAndSeasonAsync(competitionId, season, conn);
                    var players = await _fixtureRepository.GetFixturePlayersByCompetitionAndSeasonAsync(competitionId, season, conn);

                    var result = new List<FixtureBasic>();

                    foreach (var fixture in fixtures)
                    {
                        var playersForFixture = players.Where(p => p.FixtureId == fixture.FixtureId);

                        var fixtureModel = fixture.ToModel(playersForFixture);
                        fixtureModel.Substitutions = new List<Substitution>();

                        foreach (var player in playersForFixture.Where(p => p.ReplacedBy != null))
                        {
                            fixtureModel.Substitutions.Add(new Substitution
                            {
                                PlayerOut = player.Player.ToModel(),
                                PlayerIn = player.ReplacedBy.ToModel()
                            });
                        }

                        result.Add(fixtureModel);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get fixtures for competition {0} and season {1}", competitionId, season);
                throw;
            }
        }

        //public async Task<FixtureBasic> GetFixtureBasicAsync(int fixtureId)
        //{
        //    try
        //    {
        //        using (var conn = await _connectionProvider.GetOpenConnectionAsync())
        //        {
        //            var details = await _fixtureRepository.GetFixtureDetailsAsync(fixtureId, conn);
        //            var shots = await _fixtureRepository.GetFixtureShotsAsync(fixtureId, conn);

        //            return details.ToModel(shots);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Unable to get competitions");
        //        throw;
        //    }
        //}
    }
}
