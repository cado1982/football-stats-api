using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Logic.Extensions
{
    public static class TeamExtensions
    {
        public static Models.Team ToModel(this Entities.Team entity)
        {
            if (entity == null) return null;

            return new Models.Team
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Entities.Team ToEntity(this Models.Team model)
        {
            if (model == null) return null;

            return new Entities.Team
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static IEnumerable<Models.Team> ToModels(this IEnumerable<Entities.Team> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Team> ToEntities(this IEnumerable<Models.Team> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
