using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Logic.Extensions
{
    public static class FixturePlayerExtensions
    {
        public static Models.FixturePlayerBasic ToModel(this Entities.FixturePlayer entity)
        {
            if (entity == null) return null;

            return new Models.FixturePlayerBasic
            {
                Minutes = entity.Minutes,
                Player = entity.Player.ToModel(),
                Position = entity.Position,
                RedCards = entity.RedCards,
                YellowCards = entity.YellowCards,
                Replaced = entity.Replaced.ToModel(),
                ReplacedBy = entity.ReplacedBy.ToModel(),
                Assists = entity.Assists,
                Goals = entity.Goals,
                KeyPasses = entity.KeyPasses,
                OwnGoals = entity.OwnGoals,
                Shots = entity.Shots,
                ShotsOnTarget = entity.ShotsOnTarget,
                Team = entity.Team.ToModel()
            };
        }

        public static IEnumerable<Models.FixturePlayerBasic> ToModels(this IEnumerable<Entities.FixturePlayer> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }
    }
}
