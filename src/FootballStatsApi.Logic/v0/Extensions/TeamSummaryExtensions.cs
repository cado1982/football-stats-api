using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class TeamSummaryExtensions
    {
        public static Models.v0.TeamSummaryBasic ToModelBasic(this Entities.TeamSummary entity)
        {
            if (entity == null) return null;

            return new Models.v0.TeamSummaryBasic
            {
                Team = entity.Team.ToModel(),
                Season = entity.Season,
                Games = entity.Games,
                Won = entity.Won,
                Drawn = entity.Drawn,
                Lost = entity.Lost,
                GoalsFor = entity.GoalsFor,
                GoalsAgainst = entity.GoalsAgainst,
                Points = entity.Points
            };
        }

        public static Models.v0.TeamSummaryAdvanced ToModelAdvanced(this Entities.TeamSummary entity)
        {
            if (entity == null) return null;

            return new Models.v0.TeamSummaryAdvanced
            {
                Team = entity.Team.ToModel(),
                Season = entity.Season,
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

        public static IEnumerable<Models.v0.TeamSummaryBasic> ToModelsBasic(this IEnumerable<Entities.TeamSummary> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModelBasic());
        }

        public static IEnumerable<Models.v0.TeamSummaryAdvanced> ToModelsAdvanced(this IEnumerable<Entities.TeamSummary> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModelAdvanced());
        }
    }
}
