using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class SubscriptionExtensions
    {
        public static Models.v0.Subscription ToModel(this Entities.Subscription entity)
        {
            if (entity == null) return null;

            return new Models.v0.Subscription
            {
                Id = entity.Id,
                InternalName = entity.InternalName,
                DisplayName = entity.DisplayName,
                HourlyCallLimit = entity.HourlyCallLimit,
                Cost = entity.Cost,
                IsActive = entity.IsActive,
                IsInternal = entity.IsInternal,
                Features = entity.Features?.Select(f => f.DisplayName)?.ToList()
            };
        }

        public static Entities.Subscription ToEntity(this Models.v0.Subscription model)
        {
            if (model == null) return null;

            return new Entities.Subscription
            {
                Id = model.Id,
                InternalName = model.InternalName,
                DisplayName = model.DisplayName,
                HourlyCallLimit = model.HourlyCallLimit,
                Cost = model.Cost,
                IsActive = model.IsActive,
                IsInternal = model.IsInternal,
                Features = model.Features?.Select(f => new Entities.SubscriptionFeature {  DisplayName = f })?.ToList()
            };
        }

        public static IEnumerable<Models.v0.Subscription> ToModels(this IEnumerable<Entities.Subscription> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.Subscription> ToEntities(this IEnumerable<Models.v0.Subscription> models)
        {
            if (models == null) return null;

            return models.Select(m => m.ToEntity());
        }
    }
}
