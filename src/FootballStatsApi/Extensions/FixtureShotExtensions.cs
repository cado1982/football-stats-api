using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class FixtureShotExtensions
    {
        public static Models.FixtureShot ToModel(this Entities.FixtureShot entity)
        {
            if (entity == null) return null;

            return new Models.FixtureShot
            {
                ExpectedGoal = entity.ExpectedGoal,
                HomeOrAway = entity.HomeOrAway,
                LastAction = entity.LastAction,
                Minute = entity.Minute,
                Player = entity.Player.ToModel(),
                Assist = entity.Assist.ToModel(),
                Result = entity.Result,
                ShotId = entity.ShotId,
                Situation = entity.Situation,
                Type = entity.Type,
                X = entity.X,
                Y = entity.Y
            };
        }

        public static Entities.FixtureShot ToEntity(this Models.FixtureShot model)
        {
            if (model == null) return null;

            return new Entities.FixtureShot
            {
                ExpectedGoal = model.ExpectedGoal,
                HomeOrAway = model.HomeOrAway,
                LastAction = model.LastAction,
                Minute = model.Minute,
                Player = model.Player.ToEntity(),
                Assist = model.Assist.ToEntity(),
                Result = model.Result,
                ShotId = model.ShotId,
                Situation = model.Situation,
                Type = model.Type,
                X = model.X,
                Y = model.Y
            };
        }

        public static IEnumerable<Models.FixtureShot> ToModels(this IEnumerable<Entities.FixtureShot> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.FixtureShot> ToEntities(this IEnumerable<Models.FixtureShot> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
