using FootballStatsApi.Domain.Entities.Identity;
using FootballStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FootballStatsApi.Managers
{
    public interface IFixtureManager
    {
        Task<FixtureBasic> GetDetailsAsync(int fixtureId);
        //Task<FixtureBasic> GetAsync(int? competitionId, int? season, int? teamId);
    }
}
