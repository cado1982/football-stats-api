using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class FixturePlayerExtensions
    {
        public static Models.v0.FixturePlayerBasic ToModel(this Entities.FixturePlayer entity)
        {
            if (entity == null) return null;

            return new Models.v0.FixturePlayerBasic
            {
                Minutes = entity.Minutes,
                Player = entity.Player.ToModel(),
                Position = entity.Position,
                RedCards = entity.RedCards,
                YellowCards = entity.YellowCards,
                Assists = entity.Assists,
                Goals = entity.Goals,
                KeyPasses = entity.KeyPasses,
                OwnGoals = entity.OwnGoals,
                Shots = entity.Shots,
                ShotsOnTarget = entity.ShotsOnTarget,
                Team = entity.Team.ToModel()
            };
        }

        public static IEnumerable<Models.v0.FixturePlayerBasic> ToModels(this IEnumerable<Entities.FixturePlayer> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }
    }
}
