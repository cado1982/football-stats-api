using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class TeamBasicStatsExtensions
    {
        public static Models.TeamBasicStats ToModel(this Entities.TeamBasicStats entity)
        {
            if (entity == null) return null;

            return new Models.TeamBasicStats
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

        public static Entities.TeamBasicStats ToEntity(this Models.TeamBasicStats model)
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

        public static IEnumerable<Models.TeamBasicStats> ToModels(this IEnumerable<Entities.TeamBasicStats> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.TeamBasicStats> ToEntities(this IEnumerable<Models.TeamBasicStats> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
