using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class TeamBasicStatsExtensions
    {
        public static Models.v0.TeamBasicStats ToModel(this Entities.TeamBasicStats entity)
        {
            if (entity == null) return null;

            return new Models.v0.TeamBasicStats
            {
                Team = entity.Team.ToModel(),
                Drawn = entity.Drawn,
                GoalDifference = entity.GoalDifference,
                Goals = entity.Goals,
                GoalsAgainst = entity.GoalsAgainst,
                Lost = entity.Lost,
                Played = entity.Played,
                Points = entity.Points,
                Position = entity.Position,
                Won = entity.Won
            };
        }

        public static Entities.TeamBasicStats ToEntity(this Models.v0.TeamBasicStats model)
        {
            if (model == null) return null;

            return new Entities.TeamBasicStats
            {
                Team = model.Team.ToEntity(),
                Drawn = model.Drawn,
                GoalDifference = model.GoalDifference,
                Goals = model.Goals,
                GoalsAgainst = model.GoalsAgainst,
                Lost = model.Lost,
                Played = model.Played,
                Points = model.Points,
                Position = model.Position,
                Won = model.Won
            };
        }

        public static IEnumerable<Models.v0.TeamBasicStats> ToModels(this IEnumerable<Entities.TeamBasicStats> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.TeamBasicStats> ToEntities(this IEnumerable<Models.v0.TeamBasicStats> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
