using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Logic.Extensions
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
                DeepPasses = entity.DeepPasses,
                OppositionPpda = entity.OppositionPpda,
                OppositionDeepPasses = entity.OppositionDeepPasses,
                NonPenaltyExpectedGoals = entity.NonPenaltyExpectedGoals,
                NonPenaltyExpectedGoalsAgainst = entity.NonPenaltyExpectedGoalsAgainst
            };
        }

        public static IEnumerable<Models.TeamSummary> ToModels(this IEnumerable<Entities.TeamSummary> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }
    }
}
