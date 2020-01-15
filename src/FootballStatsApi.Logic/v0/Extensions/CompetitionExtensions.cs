using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class CompetitionExtensions
    {
        public static Models.v0.Competition ToModel(this Entities.Competition entity)
        {
            if (entity == null) return null;

            return new Models.v0.Competition
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Entities.Competition ToEntity(this Models.v0.Competition model)
        {
            if (model == null) return null;

            return new Entities.Competition
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static IEnumerable<Models.v0.Competition> ToModels(this IEnumerable<Entities.Competition> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Competition> ToEntities(this IEnumerable<Models.v0.Competition> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
