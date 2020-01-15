using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class TeamExtensions
    {
        public static Models.v0.Team ToModel(this Entities.Team entity)
        {
            if (entity == null) return null;

            return new Models.v0.Team
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Entities.Team ToEntity(this Models.v0.Team model)
        {
            if (model == null) return null;

            return new Entities.Team
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static IEnumerable<Models.v0.Team> ToModels(this IEnumerable<Entities.Team> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Team> ToEntities(this IEnumerable<Models.v0.Team> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
