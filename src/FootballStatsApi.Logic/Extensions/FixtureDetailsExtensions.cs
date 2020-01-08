using FootballStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Logic.Extensions
{
    public static class FixtureDetailsExtensions
    {
        public static Models.FixtureBasic ToModel(this Entities.FixtureDetails entity, List<Entities.FixtureShot> shots)
        {
            if (entity == null) return null;

            return new Models.FixtureBasic
            {
                HomeTeam = entity.HomeTeam.ToModel(),
                AwayTeam = entity.AwayTeam.ToModel(),
                Competition = entity.Competition.ToModel(),
                Goals = new HomeAway<int>
                {
                    Home = shots.Count(s => s.Team.Id == entity.HomeTeam.Id && s.Result == "Goal") + shots.Count(s => s.Team.Id == entity.AwayTeam.Id && s.Result == "OwnGoal"),
                    Away = shots.Count(s => s.Team.Id == entity.AwayTeam.Id && s.Result == "Goal") + shots.Count(s => s.Team.Id == entity.HomeTeam.Id && s.Result == "OwnGoal")
                },
                Shots = new HomeAway<int>
                {
                    Home = shots.Count(s => s.Team.Id == entity.HomeTeam.Id && s.Result != "OwnGoal"),
                    Away = shots.Count(s => s.Team.Id == entity.AwayTeam.Id && s.Result != "OwnGoal")
                },
                ShotsOnTarget = new HomeAway<int>
                {
                    Home = shots.Count(s => s.Team.Id == entity.HomeTeam.Id && (s.Result == "Goal" || s.Result == "SavedShot")),
                    Away = shots.Count(s => s.Team.Id == entity.AwayTeam.Id && (s.Result == "Goal" || s.Result == "SavedShot")),
                },
                DateTime = entity.DateTime,
                FixtureId = entity.FixtureId,
                Season = entity.Season
            };
        }

        public static Entities.FixtureDetails ToEntity(this Models.FixtureBasic model)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Models.FixtureBasic> ToModels(this IEnumerable<Entities.FixtureDetails> entities, List<Entities.FixtureShot> shots)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToModel(shots));
        }

        public static IEnumerable<Entities.FixtureDetails> ToEntities(this IEnumerable<Models.FixtureBasic> models)
        {
            throw new NotImplementedException();
        }
    }
}
