using FootballStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Extensions
{
    public static class FixtureDetailsExtensions
    {
        public static Models.FixtureDetails ToModel(this Entities.FixtureDetails entity)
        {
            if (entity == null) return null;

            return new Models.FixtureDetails
            {
                HomeTeam = entity.HomeTeam.ToModel(),
                AwayTeam = entity.AwayTeam.ToModel(),
                Competition = entity.Competition.ToModel(),
                DeepPasses = new HomeAway<int>
                {
                    Home = entity.HomeDeepPasses,
                    Away = entity.AwayDeepPasses
                },
                Ppda = new HomeAway<double>
                {
                    Home = entity.HomeDefensiveActions == 0 ? 0 : entity.AwayPasses / entity.HomeDefensiveActions,
                    Away = entity.AwayDefensiveActions == 0 ? 0 : entity.HomePasses / entity.AwayDefensiveActions
                },
                Forecast = new FixtureForecast
                {
                    HomeWin = entity.ForecastHomeWin,
                    Draw = entity.ForecastDraw,
                    AwayWin = entity.ForecastAwayWin
                },
                ExpectedPoints = new HomeAway<double>
                {
                    Home = (entity.ForecastHomeWin * 3) + (entity.ForecastDraw * 1),
                    Away = (entity.ForecastAwayWin * 3) + (entity.ForecastDraw * 1)
                },
                ExpectedGoals = new HomeAway<double>
                {
                    Home = entity.HomeExpectedGoals,
                    Away = entity.AwayExpectedGoals
                },
                Goals = new HomeAway<int>
                {
                    Home = entity.HomeGoals,
                    Away = entity.AwayGoals
                },
                Shots = new HomeAway<int>
                {
                    Home = entity.HomeShots,
                    Away = entity.AwayShots
                },
                ShotsOnTarget = new HomeAway<int>
                {
                    Home = entity.HomeShotsOnTarget,
                    Away = entity.AwayShotsOnTarget
                },
                DateTime = entity.DateTime,
                FixtureId = entity.FixtureId,
                Season = entity.Season
            };
        }

        public static Entities.FixtureDetails ToEntity(this Models.FixtureDetails model)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Models.FixtureDetails> ToModels(this IEnumerable<Entities.FixtureDetails> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel());
        }

        public static IEnumerable<Entities.FixtureDetails> ToEntities(this IEnumerable<Models.FixtureDetails> models)
        {
            throw new NotImplementedException();
        }
    }
}
