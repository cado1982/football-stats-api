using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class PlayerExtensions
    {
        public static Models.v0.Player ToModel(this Entities.Player entity)
        {
            if (entity == null) return null;

            return new Models.v0.Player
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Entities.Player ToEntity(this Models.v0.Player model)
        {
            if (model == null) return null;

            return new Entities.Player
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static IEnumerable<Models.v0.Player> ToModels(this IEnumerable<Entities.Player> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Player> ToEntities(this IEnumerable<Models.v0.Player> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
