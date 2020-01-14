using System;
using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Logic.Extensions
{
    public static class FixtureExtensions
    {
        public static Models.FixtureBasic ToModel(this Entities.Fixture entity, IEnumerable<Entities.FixturePlayer> players)
        {
            if (entity == null) return null;

            return new Models.FixtureBasic
            {
                HomeTeam = entity.HomeTeam.ToModel(),
                AwayTeam = entity.AwayTeam.ToModel(),
                Competition = entity.Competition.ToModel(),
                Goals = new Models.HomeAway<int>
                {
                    Home = entity.HomeGoals,
                    Away = entity.AwayGoals
                },
                Shots = new Models.HomeAway<int>
                {
                    Home = entity.HomeShots,
                    Away = entity.AwayShots
                },
                ShotsOnTarget = new Models.HomeAway<int>
                {
                    Home = entity.HomeShotsOnTarget,
                    Away = entity.AwayShotsOnTarget
                },
                Players = players.ToModels().ToList(),
                DateTime = entity.DateTime,
                FixtureId = entity.FixtureId,
                Season = entity.Season
            };
        }

        public static IEnumerable<Models.FixtureBasic> ToModels(this IEnumerable<Entities.Fixture> entities, IEnumerable<Entities.FixturePlayer> players)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel(players));
        }
    }
}
