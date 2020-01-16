using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class PlayerSummaryExtensions
    {
        public static Models.v0.PlayerSummaryBasic ToModel(this Entities.PlayerSummary entity)
        {
            if (entity == null) return null;

            return new Models.v0.PlayerSummaryBasic
            {
                Player = entity.Player.ToModel(),
                Games = entity.Games,
                MinutesPlayed = entity.MinutesPlayed,
                Goals = entity.Goals,
                Assists = entity.Assists,
                Shots = entity.Shots,
                KeyPasses = entity.KeyPasses,
                YellowCards = entity.YellowCards,
                RedCards = entity.RedCards,
                Position = entity.Position,
                Team = entity.Team.ToModel(),
                NonPenaltyGoals = entity.NonPenaltyGoals,
            };
        }

        public static Entities.PlayerSummary ToEntity(this Models.v0.PlayerSummaryBasic model)
        {
            if (model == null) return null;

            return new Entities.PlayerSummary
            {
                Player = model.Player.ToEntity(),
                Games = model.Games,
                MinutesPlayed = model.MinutesPlayed,
                Goals = model.Goals,
                Assists = model.Assists,
                Shots = model.Shots,
                KeyPasses = model.KeyPasses,
                YellowCards = model.YellowCards,
                RedCards = model.RedCards,
                Position = model.Position,
                Team = model.Team.ToEntity(),
                NonPenaltyGoals = model.NonPenaltyGoals,
            };
        }

        public static IEnumerable<Models.v0.PlayerSummaryBasic> ToModels(this IEnumerable<Entities.PlayerSummary> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.PlayerSummary> ToEntities(this IEnumerable<Models.v0.PlayerSummaryBasic> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
