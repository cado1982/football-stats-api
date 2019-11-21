using System;
using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class FixtureDetailedExtensions
    {
        public static Models.FixtureDetails ToModel(this Entities.FixtureDetails entity)
        {
            if (entity == null) return null;

            return new Models.FixtureDetails
            {
                HomeTeam = entity.HomeTeam.ToModel(),
                AwayTeam = entity.AwayTeam.ToModel(),
                Competition = entity.Competition.ToModel(),
                AwayDeepPasses = entity.AwayDeepPasses,
                AwayPpda = entity.AwayPpda,
                DateTime = entity.DateTime,
                FixtureId = entity.FixtureId,
                ForecastAwayWin = entity.ForecastAwayWin,
                ForecastDraw = entity.ForecastDraw,
                ForecastHomeWin = entity.ForecastHomeWin,
                HomeDeepPasses = entity.HomeDeepPasses,
                HomePpda = entity.HomePpda,
                Season = entity.Season
            };
        }

        public static Entities.FixtureDetails ToEntity(this Models.FixtureDetails model)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Models.FixtureDetails> ToModels(this IEnumerable<Entities.FixtureDetails> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.FixtureDetails> ToEntities(this IEnumerable<Models.FixtureDetails> models)
        {
            throw new NotImplementedException();
        }
    }
}
