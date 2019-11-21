using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class FixturePlayerExtensions
    {
        public static Models.FixturePlayer ToModel(this Entities.FixturePlayer entity)
        {
            if (entity == null) return null;

            return new Models.FixturePlayer
            {
                ExpectedGoalsBuildup = entity.ExpectedGoalsBuildup,
                ExpectedGoalsChain = entity.ExpectedGoalsChain,
                Minutes = entity.Minutes,
                Player = entity.Player.ToModel(),
                Position = entity.Position,
                RedCards = entity.RedCards,
                YellowCards = entity.YellowCards,
                Replaced = entity.Replaced.ToModel(),
                ReplacedBy = entity.ReplacedBy.ToModel()
            };
        }

        public static Entities.FixturePlayer ToEntity(this Models.FixturePlayer model)
        {
            if (model == null) return null;

            return new Entities.FixturePlayer
            {
                ExpectedGoalsBuildup = model.ExpectedGoalsBuildup,
                ExpectedGoalsChain = model.ExpectedGoalsChain,
                Minutes = model.Minutes,
                Player = model.Player.ToEntity(),
                Position = model.Position,
                RedCards = model.RedCards,
                YellowCards = model.YellowCards,
                Replaced = model.Replaced.ToEntity(),
                ReplacedBy = model.ReplacedBy.ToEntity()
            };
        }

        public static IEnumerable<Models.FixturePlayer> ToModels(this IEnumerable<Entities.FixturePlayer> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.FixturePlayer> ToEntities(this IEnumerable<Models.FixturePlayer> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
