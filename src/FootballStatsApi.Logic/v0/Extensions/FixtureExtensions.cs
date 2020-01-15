using System;
using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Models.v0;

namespace FootballStatsApi.Logic.v0.Extensions
{
    public static class FixtureExtensions
    {
        public static Models.v0.FixtureBasic ToModel(this Entities.Fixture entity, IEnumerable<Entities.FixturePlayer> players)
        {
            if (entity == null) return null;

            return new Models.v0.FixtureBasic
            {
                HomeTeam = entity.HomeTeam.ToModel(),
                AwayTeam = entity.AwayTeam.ToModel(),
                Competition = entity.Competition.ToModel(),
                HomeGoals = entity.HomeGoals,
                AwayGoals = entity.AwayGoals,
                HomeShots = entity.HomeShots,
                AwayShots = entity.AwayShots,
                HomeShotsOnTarget = entity.HomeShotsOnTarget,
                AwayShotsOnTarget = entity.AwayShotsOnTarget,
                HomePlayers = players.Where(p => p.Team.Id == entity.HomeTeam.Id).ToModels().ToList(),
                AwayPlayers = players.Where(p => p.Team.Id == entity.AwayTeam.Id).ToModels().ToList(),
                DateTime = entity.DateTime,
                FixtureId = entity.FixtureId,
                Season = entity.Season
            };
        }

        public static IEnumerable<Models.v0.FixtureBasic> ToModels(this IEnumerable<Entities.Fixture> entities, IEnumerable<Entities.FixturePlayer> players)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel(players));
        }
    }
}
