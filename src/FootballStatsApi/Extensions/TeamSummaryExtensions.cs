using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class TeamSummaryExtensions
    {
        public static Models.TeamSummary ToModel(this Entities.TeamSummary entity)
        {
            if (entity == null) return null;

            return new Models.TeamSummary
            {
                Team = entity.Team.ToModel(),
                Games = entity.Games,
                Won = entity.Won,
                Drawn = entity.Drawn,
                Lost = entity.Lost,
                GoalsFor = entity.GoalsFor,
                GoalsAgainst = entity.GoalsAgainst,
                Points = entity.Points,
                ExpectedGoals = entity.ExpectedGoals,
                ExpectedGoalsAgainst = entity.ExpectedGoalsAgainst,
                ExpectedPoints = entity.ExpectedPoints,
                Ppda = entity.Ppda,
                DeepPasses = entity.DeepPasses
            };
        }

        public static Entities.TeamSummary ToEntity(this Models.TeamSummary model)
        {
            if (model == null) return null;

            return new Entities.TeamSummary
            {
                Team = model.Team.ToEntity(),
                Games = model.Games,
                Won = model.Won,
                Drawn = model.Drawn,
                Lost = model.Lost,
                GoalsFor = model.GoalsFor,
                GoalsAgainst = model.GoalsAgainst,
                Points = model.Points,
                ExpectedGoals = model.ExpectedGoals,
                ExpectedGoalsAgainst = model.ExpectedGoalsAgainst,
                ExpectedPoints = model.ExpectedPoints,
                Ppda = model.Ppda,
                DeepPasses = model.DeepPasses
            };
        }

        public static IEnumerable<Models.TeamSummary> ToModels(this IEnumerable<Entities.TeamSummary> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.TeamSummary> ToEntities(this IEnumerable<Models.TeamSummary> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
