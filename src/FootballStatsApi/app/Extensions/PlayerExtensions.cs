using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class PlayerExtensions
    {
        public static Models.Player ToModel(this Entities.Player entity)
        {
            if (entity == null) return null;

            return new Models.Player
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Entities.Player ToEntity(this Models.Player model)
        {
            if (model == null) return null;

            return new Entities.Player
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static IEnumerable<Models.Player> ToModels(this IEnumerable<Entities.Player> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Player> ToEntities(this IEnumerable<Models.Player> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
