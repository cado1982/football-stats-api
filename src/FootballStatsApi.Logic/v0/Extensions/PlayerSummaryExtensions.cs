using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class PlayerSummaryExtensions
    {
        public static Models.v0.PlayerSummary ToModel(this Entities.PlayerSummary entity)
        {
            if (entity == null) return null;

            return new Models.v0.PlayerSummary
            {
                Player = entity.Player.ToModel(),
                Games = entity.Games,
                MinutesPlayed = entity.MinutesPlayed,
                Goals = entity.Goals,
                ExpectedGoals = entity.ExpectedGoals,
                Assists = entity.Assists,
                ExpectedAssists = entity.ExpectedAssists,
                Shots = entity.Shots,
                KeyPasses = entity.KeyPasses,
                YellowCards = entity.YellowCards,
                RedCards = entity.RedCards,
                Position = entity.Position,
                Team = entity.Team.ToModel(),
                NonPenaltyGoals = entity.NonPenaltyGoals,
                NonPenaltyExpectedGoals = entity.NonPenaltyExpectedGoals,
                ExpectedGoalsChain = entity.ExpectedGoalsChain,
                ExpectedGoalsBuildup = entity.ExpectedGoalsBuildup
            };
        }

        public static Entities.PlayerSummary ToEntity(this Models.v0.PlayerSummary model)
        {
            if (model == null) return null;

            return new Entities.PlayerSummary
            {
                Player = model.Player.ToEntity(),
                Games = model.Games,
                MinutesPlayed = model.MinutesPlayed,
                Goals = model.Goals,
                ExpectedGoals = model.ExpectedGoals,
                Assists = model.Assists,
                ExpectedAssists = model.ExpectedAssists,
                Shots = model.Shots,
                KeyPasses = model.KeyPasses,
                YellowCards = model.YellowCards,
                RedCards = model.RedCards,
                Position = model.Position,
                Team = model.Team.ToEntity(),
                NonPenaltyGoals = model.NonPenaltyGoals,
                NonPenaltyExpectedGoals = model.NonPenaltyExpectedGoals,
                ExpectedGoalsChain = model.ExpectedGoalsChain,
                ExpectedGoalsBuildup = model.ExpectedGoalsBuildup
            };
        }

        public static IEnumerable<Models.v0.PlayerSummary> ToModels(this IEnumerable<Entities.PlayerSummary> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.PlayerSummary> ToEntities(this IEnumerable<Models.v0.PlayerSummary> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
