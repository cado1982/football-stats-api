using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class CompetitionExtensions
    {
        public static Models.Competition ToModel(this Entities.Competition entity)
        {
            if (entity == null) return null;

            return new Models.Competition
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Entities.Competition ToEntity(this Models.Competition model)
        {
            if (model == null) return null;

            return new Entities.Competition
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static IEnumerable<Models.Competition> ToModels(this IEnumerable<Entities.Competition> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Competition> ToEntities(this IEnumerable<Models.Competition> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
